from pathlib import Path
p=Path('src/Infrastructure/Services/RemoteProjectService.cs')
s=p.read_text(encoding='utf-8-sig'); a=s.index('    public async Task<RemoteOperationResult> UpdateAsync'); b=s.index('    static void Require',a)
s=s[:a]+'''    public Task<RemoteOperationResult> UpdateAsync(string localRoot, RemoteProject remote, CancellationToken ct = default) => Task.Run(() =>
    {
        if (remote.OperatingSystem != RemoteOperatingSystem.Windows || remote.AppManager == WindowsAppManager.NotConfigured)
            return new RemoteOperationResult(false, "Настройте Windows Service или IIS Application Pool.", 0, 0);
        if (!remote.DatabaseExclusionsReviewed) return new RemoteOperationResult(false, "Проверьте исключения базы данных в настройках.", 0, 0);
        var root = NormalizeRemotePath(remote.RemotePath).TrimEnd('/');
        if (!System.Text.RegularExpressions.Regex.IsMatch(root, @"^[A-Za-z]:/[^.].+") || root.Split('/').Any(x => x is "." or ".."))
            return new RemoteOperationResult(false, "Нужна абсолютная папка Windows, не корень диска.", 0, 0);
        var identity = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(Path.GetFullPath(localRoot).ToUpperInvariant() + "|" + remote.Id + "|" + remote.Host + "|" + remote.Port + "|" + remote.User + "|" + root.ToUpperInvariant())));
        var state = string.IsNullOrWhiteSpace(remote.StatePath) ? root + "/.coretech-core/" + identity : NormalizeRemotePath(remote.StatePath).TrimEnd('/') + "/" + identity;
        if (!string.IsNullOrWhiteSpace(remote.StatePath) && state.StartsWith(root + "/", StringComparison.OrdinalIgnoreCase))
            return new RemoteOperationResult(false, "Пользовательская папка копий должна находиться вне папки приложения.", 0, 0);
        try
        {
            using var client = Connect(remote); client.Connect();
            using var sftp = ConnectSftp(remote); sftp.Connect();
            EnsureRemoteDirectory(sftp, state);
            if (sftp.Exists(state + "/pending.json")) return new RemoteOperationResult(false, "Есть незавершённое обновление. Проверьте журнал и восстановите сервер вручную.", 0, 0);
            var previous = ReadRemote(sftp, state + "/manifest.json") ?? "";
            var old = ParseManifest(previous.Length == 0 ? null : previous);
            var patterns = Patterns(remote); var current = EnumerateLocal(localRoot, patterns);
            var upload = current.Where(x => !old.TryGetValue(x.Path, out var e) || e.Size != x.Size || e.LastWriteUtc != x.LastWriteUtc).ToList();
            var deleted = old.Keys.Except(current.Select(x => x.Path), StringComparer.OrdinalIgnoreCase).Where(x => !IsExcluded(x, patterns)).ToList();
            if (upload.Count == 0 && deleted.Count == 0) return new RemoteOperationResult(true, "Изменений нет.", 0, current.Count);
            var operation = DateTime.UtcNow.ToString("yyyyMMddHHmmssfffffff") + "-" + Guid.NewGuid().ToString("N");
            var directory = state + "/operation-" + operation;
            EnsureRemoteDirectory(sftp, directory);
            foreach (var item in upload)
            {
                ct.ThrowIfCancellationRequested(); ValidateRelative(item.Path);
                var target = directory + "/files/" + item.Path; EnsureRemoteDirectory(sftp, target[..target.LastIndexOf('/')]);
                using var input = File.OpenRead(Path.Combine(localRoot, item.Path)); sftp.UploadFile(input, target, true);
                var info = new FileInfo(Path.Combine(localRoot, item.Path));
                if (info.Length != item.Size || info.LastWriteTimeUtc != item.LastWriteUtc) throw new IOException("Файл изменился при подготовке обновления.");
            }
            var plan = new { Root = root.Replace('/', '\\\\'), State = state.Replace('/', '\\\\'), Operation = operation,
                Manager = remote.AppManager.ToString(), Application = remote.ApplicationName, Timeout = Math.Clamp(remote.OperationTimeoutSeconds, 10, 3600),
                Exclusions = patterns, Upload = upload.Select(x => x.Path).ToArray(), Delete = deleted.ToArray(), PreviousManifest = previous,
                NextManifest = JsonSerializer.Serialize(current, JsonOptions) };
            WriteRemote(sftp, directory + "/plan.json", JsonSerializer.Serialize(plan));
            var assembly = typeof(RemoteProjectService).Assembly;
            using var resource = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().Single(x => x.EndsWith("WindowsDeployment.ps1")))!;
            using var reader = new StreamReader(resource);
            WriteRemote(sftp, directory + "/deploy.ps1", "\\uFEFF" + reader.ReadToEnd());
            ct.ThrowIfCancellationRequested();
            using var command = client.CreateCommand(ToPowerShellCommand($"& '{Ps(directory)}/deploy.ps1' -OperationDirectory '{Ps(directory)}'"));
            command.CommandTimeout = TimeSpan.FromHours(2);
            command.Execute();
            var log = ReadRemote(sftp, directory + "/journal.log") ?? "Журнал недоступен";
            return new RemoteOperationResult(command.ExitStatus == 0, log, upload.Count, current.Count - upload.Count);
        }
        catch (Exception ex) { return new RemoteOperationResult(false, "SSH: " + ex.GetType().Name + ". Если серверный этап начался, проверьте pending.json и журнал перед повтором.", 0, 0); }
    }, ct);

    public static void ValidateRelative(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || path.Contains('\\\\') || path.Contains(':') || path.Split('/').Any(x => x is "" or "." or ".." || x.EndsWith('.') || x.EndsWith(' ')))
            throw new InvalidDataException("Небезопасный относительный путь.");
    }

''' +s[b:]
s=s.replace('"Write-Output connected"', 'ToPowerShellCommand("Write-Output connected")')
a=s.index('    static Dictionary<string,RemoteSyncEntry> ParseManifest'); b=s.index('    static string? ReadRemote',a)
s=s[:a]+'''    static Dictionary<string,RemoteSyncEntry> ParseManifest(string? text)
    {
        var entries = JsonSerializer.Deserialize<List<RemoteSyncEntry>>(text ?? "[]", JsonOptions) ?? throw new InvalidDataException("Пустой манифест");
        foreach (var entry in entries) ValidateRelative(entry.Path);
        return entries.ToDictionary(x => x.Path, StringComparer.OrdinalIgnoreCase);
    }
''' +s[b:]
s=s.replace('SearchOption.AllDirectories).Select', 'new EnumerationOptions { RecurseSubdirectories = true, AttributesToSkip = FileAttributes.ReparsePoint, IgnoreInaccessible = false }).Select')
p.write_text(s,encoding='utf-8-sig')
