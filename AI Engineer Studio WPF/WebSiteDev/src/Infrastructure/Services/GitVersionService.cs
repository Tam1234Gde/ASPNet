using System.Diagnostics;
using System.IO;
using System.Text;

namespace AIEngineerStudio.Services;

public record GitChange(string Code, string Path) { public string Label => $"{Code}  {Path}"; }
public static class GitVersionService
{
    public static async Task<string> RunAsync(string directory, params string[] args)
    {
        var psi = new ProcessStartInfo("git") { WorkingDirectory = directory, UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true, StandardOutputEncoding = Encoding.UTF8, StandardErrorEncoding = Encoding.UTF8 };
        psi.Environment["GIT_TERMINAL_PROMPT"] = "0";
        psi.ArgumentList.Add("-c"); psi.ArgumentList.Add("core.quotepath=false");
        foreach (var arg in args) psi.ArgumentList.Add(arg);
        using var process = Process.Start(psi) ?? throw new InvalidOperationException("Не удалось запустить Git. Установите Git for Windows.");
        var output = process.StandardOutput.ReadToEndAsync(); var error = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        try { await process.WaitForExitAsync(timeout.Token); }
        catch (OperationCanceledException) { process.Kill(true); throw new TimeoutException("Git: превышено время ожидания."); }
        var text = await output; var failure = await error;
        if (process.ExitCode != 0) throw new InvalidOperationException(failure.Trim());
        return text;
    }
    public static async Task<bool> IsRepositoryAsync(string directory)
    {
        // Не захватываем репозиторий родительского каталога.
        if (!Directory.Exists(Path.Combine(directory, ".git")) && !File.Exists(Path.Combine(directory, ".git"))) return false;
        return (await RunAsync(directory, "rev-parse", "--is-inside-work-tree")).Trim() == "true";
    }
    public static async Task InitializeAsync(string directory)
    {
        if (await IsRepositoryAsync(directory)) return;
        await RunAsync(directory, "init");
        var ignore = Path.Combine(directory, ".gitignore");
        const string defaults = "\n# AI Engineer Studio\n.vs/\nbin/\nobj/\nnode_modules/\n__pycache__/\n.env\n.env.*\n*.pem\n*.key\nartifacts/\nbackups/\n";
        await File.AppendAllTextAsync(ignore, defaults);
    }
    public static async Task<List<GitChange>> StatusAsync(string directory)
    {
        var raw = await RunAsync(directory, "status", "--porcelain=v1", "-z", "--untracked-files=all", "--no-renames");
        return raw.Split('\0', StringSplitOptions.RemoveEmptyEntries).Where(x => x.Length > 3).Select(x => new GitChange(x[..2], x[3..])).ToList();
    }
    public static async Task<string> DiffAsync(string directory, GitChange change)
    {
        if (change.Code == "??") {
            var file = Path.Combine(directory, change.Path);
            if (new FileInfo(file).Length > 512000) return "Новый файл слишком большой для предпросмотра.";
            return "Новый файл: " + change.Path + "\n\n" + await File.ReadAllTextAsync(file);
        }
        var staged = await RunAsync(directory, "diff", "--cached", "--no-ext-diff", "--", change.Path);
        var working = await RunAsync(directory, "diff", "--no-ext-diff", "--", change.Path);
        return "ИНДЕКС\n" + staged + "\nРАБОЧАЯ ПАПКА\n" + working;
    }
    public static async Task<string> CommitAsync(string directory, string message)
    {
        if (string.IsNullOrWhiteSpace(message)) throw new InvalidOperationException("Добавьте описание изменений.");
        var changes = await StatusAsync(directory);
        if (changes.Any(x => x.Code.Contains('U') || x.Code is "AA" or "DD")) throw new InvalidOperationException("Сначала разрешите конфликты Git.");
        foreach (var marker in new[] { "MERGE_HEAD", "CHERRY_PICK_HEAD", "REVERT_HEAD", "rebase-merge", "rebase-apply" }) {
            var path = (await RunAsync(directory, "rev-parse", "--git-path", marker)).Trim();
            path = Path.IsPathRooted(path) ? path : Path.Combine(directory, path);
            if (File.Exists(path) || Directory.Exists(path)) throw new InvalidOperationException("Завершите текущую операцию merge/rebase в Git.");
        }
        if (changes.Count == 0) return "Изменений нет — коммит не требуется.";
        await RunAsync(directory, "add", "-A", "--", ".");
        var summary = string.Join("\n", changes.Select(x => x.Label));
        return (await RunAsync(directory, "-c", "user.name=AI Engineer Studio", "-c", "user.email=ai-engineer-studio@localhost", "commit", "-m", message.Trim() + "\n\nИзменённые файлы:\n" + summary)).Trim();
    }
    public static string[] ReadFileFromHead(string directory, string relativePath)
    {
        try { return Task.Run(() => RunAsync(directory, "show", "HEAD:" + relativePath.Replace('\\', '/'))).GetAwaiter().GetResult().Replace("\r", "").Split('\n'); }
        catch { return Array.Empty<string>(); }
    }
}
