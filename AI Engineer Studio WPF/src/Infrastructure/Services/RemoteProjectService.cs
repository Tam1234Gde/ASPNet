using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using AIEngineerStudio.Models;

namespace AIEngineerStudio.Services;

public sealed record RemoteOperationResult(bool Success, string Message, int Uploaded, int Skipped);
public sealed record RemoteSyncEntry(string Path, long Size, DateTime LastWriteUtc);

public sealed class RemoteProjectService
{
    static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };
    public Task<RemoteOperationResult> TestAsync(RemoteProject remote, CancellationToken ct = default) => Task.Run(() =>
    {
        using var client = Connect(remote); client.Connect();
        var result = client.RunCommand(remote.OperatingSystem == RemoteOperatingSystem.Windows ? ToPowerShellCommand("Write-Output connected") : "printf connected").Result.Trim();
        return new RemoteOperationResult(result == "connected", result == "connected" ? "SSH-подключение проверено." : "Сервер не подтвердил подключение.", 0, 0);
    }, ct);

    public Task<RemoteOperationResult> UploadChangedAsync(string localRoot, RemoteProject remote, CancellationToken ct = default) => Task.Run(() =>
    {
        if (!Directory.Exists(localRoot)) return new(false, "Локальная папка не существует.", 0, 0);
        using var sftp = ConnectSftp(remote); sftp.Connect(); var root = NormalizeRemotePath(remote.RemotePath); EnsureRemoteDirectory(sftp, root);
        var excludes = Patterns(remote); int uploaded = 0, skipped = 0;
        foreach (var file in Directory.EnumerateFiles(localRoot, "*", SearchOption.AllDirectories))
        {
            ct.ThrowIfCancellationRequested(); var rel = Path.GetRelativePath(localRoot, file).Replace('\\','/');
            if (IsExcluded(rel, excludes)) { skipped++; continue; }
            var target = root.TrimEnd('/') + "/" + rel; EnsureRemoteDirectory(sftp, target[..target.LastIndexOf('/')]);
            var info = new FileInfo(file); if (sftp.Exists(target) && sftp.GetAttributes(target).Size == info.Length) { skipped++; continue; }
            using var input = info.OpenRead(); sftp.UploadFile(input, target, true); uploaded++;
        }
        return new RemoteOperationResult(true, $"Синхронизация завершена: загружено {uploaded}, пропущено {skipped}.", uploaded, skipped);
    }, ct);

    public Task<RemoteOperationResult> DeployAsync(RemoteProject remote, CancellationToken ct = default) => Task.Run(() =>
    {
        if (string.IsNullOrWhiteSpace(remote.DeployCommand)) return new(false, "Команда деплоя не задана.", 0, 0);
        using var client = Connect(remote); client.Connect(); var cmd = client.RunCommand(ToPowerShellCommand(remote.DeployCommand));
        return new RemoteOperationResult(cmd.ExitStatus == 0, (string.IsNullOrWhiteSpace(cmd.Error) ? cmd.Result : cmd.Error).Trim(), 0, 0);
    }, ct);

    public Task<RemoteOperationResult> UpdateAsync(string localRoot, RemoteProject remote, CancellationToken ct = default) => Task.Run(() =>
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
            var plan = new { Root = root.Replace('/', '\\'), State = state.Replace('/', '\\'), Operation = operation,
                Manager = remote.AppManager.ToString(), Application = remote.ApplicationName, Timeout = Math.Clamp(remote.OperationTimeoutSeconds, 10, 3600),
                Exclusions = patterns, Upload = upload.Select(x => x.Path).ToArray(), Delete = deleted.ToArray(), PreviousManifest = previous,
                NextManifest = JsonSerializer.Serialize(current, JsonOptions) };
            WriteRemote(sftp, directory + "/plan.json", JsonSerializer.Serialize(plan));
            var assembly = typeof(RemoteProjectService).Assembly;
            using var resource = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().Single(x => x.EndsWith("WindowsDeployment.ps1")))!;
            using var reader = new StreamReader(resource);
            WriteRemote(sftp, directory + "/deploy.ps1", "\uFEFF" + reader.ReadToEnd());
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
        if (string.IsNullOrWhiteSpace(path) || path.Contains('\\') || path.Contains(':') || path.Split('/').Any(x => x is "" or "." or ".." || x.EndsWith('.') || x.EndsWith(' ')))
            throw new InvalidDataException("Небезопасный относительный путь.");
    }

    static void Require(SshClient c, string command) { var r = c.RunCommand(ToPowerShellCommand(command)); if (r.ExitStatus != 0) throw new InvalidOperationException((r.Error + " " + r.Result).Trim()); }
    static string StopCommand(RemoteProject r) => r.AppManager == WindowsAppManager.WindowsService ? $"Stop-Service -Name '{Ps(r.ApplicationName)}' -ErrorAction Stop; if((Get-Service -Name '{Ps(r.ApplicationName)}').Status -ne 'Stopped'){{throw 'Service did not stop'}}" : $"Import-Module WebAdministration;Stop-WebAppPool -Name '{Ps(r.ApplicationName)}';if((Get-WebAppPoolState -Name '{Ps(r.ApplicationName)}').Value -notin @('Stopped','Stopping')){{throw 'IIS pool did not stop'}}";
    static string StartCommand(RemoteProject r) => r.AppManager == WindowsAppManager.WindowsService ? $"Start-Service -Name '{Ps(r.ApplicationName)}';if((Get-Service -Name '{Ps(r.ApplicationName)}').Status -ne 'Running'){{throw 'Service did not start'}}" : $"Import-Module WebAdministration;Start-WebAppPool -Name '{Ps(r.ApplicationName)}';if((Get-WebAppPoolState -Name '{Ps(r.ApplicationName)}').Value -notin @('Started','Starting')){{throw 'IIS pool did not start'}}";
    static string Ps(string s) => s.Replace("'", "''");
    static SshClient Connect(RemoteProject r) { var info = new ConnectionInfo(r.Host, r.Port, r.User, r.Authentication == RemoteAuthentication.Password ? new PasswordAuthenticationMethod(r.User, r.Password) : new PrivateKeyAuthenticationMethod(r.User, new PrivateKeyFile(r.PrivateKeyPath))); var c = new SshClient(info); c.HostKeyReceived += (_, e) => e.CanTrust = true; c.ConnectionInfo.Timeout = TimeSpan.FromSeconds(Math.Clamp(r.ConnectionTimeoutSeconds, 5, 300)); return c; }
    static SftpClient ConnectSftp(RemoteProject r) { var c = Connect(r); return new SftpClient(c.ConnectionInfo); }
    static string[] Patterns(RemoteProject r) => (r.ExcludePatterns + ";" + r.DatabaseExclusions + ";.coretech-core").Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    static bool IsExcluded(string p, string[] patterns) => p.Split('/').Any(part => patterns.Any(x => part.Equals(x, StringComparison.OrdinalIgnoreCase) || (x.Contains('*') && System.Text.RegularExpressions.Regex.IsMatch(part, "^" + System.Text.RegularExpressions.Regex.Escape(x).Replace("\\*", ".*") + "$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))));
    static List<RemoteSyncEntry> EnumerateLocal(string root, string[] patterns) => Directory.EnumerateFiles(root, "*", new EnumerationOptions { RecurseSubdirectories = true, AttributesToSkip = FileAttributes.ReparsePoint, IgnoreInaccessible = false }).Select(f => new { f, rel = Path.GetRelativePath(root, f).Replace('\\','/') }).Where(x => !IsExcluded(x.rel, patterns)).Select(x => { var i = new FileInfo(x.f); return new RemoteSyncEntry(x.rel, i.Length, i.LastWriteTimeUtc); }).ToList();
    static Dictionary<string,RemoteSyncEntry> ParseManifest(string? text)
    {
        var entries = JsonSerializer.Deserialize<List<RemoteSyncEntry>>(text ?? "[]", JsonOptions) ?? throw new InvalidDataException("Пустой манифест");
        foreach (var entry in entries) ValidateRelative(entry.Path);
        return entries.ToDictionary(x => x.Path, StringComparer.OrdinalIgnoreCase);
    }
    static string? ReadRemote(SftpClient s, string p) { if (!s.Exists(p)) return null; using var m = new MemoryStream(); s.DownloadFile(p,m); return Encoding.UTF8.GetString(m.ToArray()); }
    static void WriteRemote(SftpClient s,string p,string text){using var m=new MemoryStream(Encoding.UTF8.GetBytes(text));s.UploadFile(m,p,true);}
    static void AppendLog(SftpClient s,string state,string op,string text){var p=state+"/journal.log";WriteRemote(s,p,(ReadRemote(s,p)??"")+DateTime.UtcNow.ToString("O")+" "+op+" "+text+Environment.NewLine);}
    static void CleanupBackups(SftpClient s,string state){var dir=s.ListDirectory(state).Where(x=>x.IsDirectory&&x.Name.StartsWith("backup-",StringComparison.OrdinalIgnoreCase)).OrderByDescending(x=>x.Name).ToList();foreach(var d in dir.Skip(5))DeleteTree(s,d.FullName);}
    static void DeleteTree(SftpClient s,string p){foreach(var x in s.ListDirectory(p)){if(x.Name is "." or "..")continue;if(x.IsDirectory)DeleteTree(s,x.FullName);else s.DeleteFile(x.FullName);}s.DeleteDirectory(p);}
    static string ToPowerShellCommand(string script)=>$"powershell.exe -NoProfile -NonInteractive -ExecutionPolicy Bypass -EncodedCommand {Convert.ToBase64String(Encoding.Unicode.GetBytes(script))}";
    static string NormalizeRemotePath(string p)=>p.Trim().Replace('\\','/');
    static void EnsureRemoteDirectory(SftpClient s,string p){p=NormalizeRemotePath(p).TrimEnd('/');if(string.IsNullOrWhiteSpace(p)||p=="/"||s.Exists(p))return;var i=p.LastIndexOf('/');if(i>0)EnsureRemoteDirectory(s,p[..i]);s.CreateDirectory(p);}
}
