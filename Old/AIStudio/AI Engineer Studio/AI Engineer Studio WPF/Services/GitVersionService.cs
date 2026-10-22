using System.Diagnostics;
using System.IO;

namespace AIEngineerStudio.Services;

public static class GitVersionService
{
    public static string CommitRequest(string directory, string message)
    {
        if (!Directory.Exists(directory)) return "Папка проекта не найдена";
        Run(directory, "git init");
        // Локальная идентичность проекта не зависит от глобальных настроек Git.
        Run(directory, "git config user.name \"AI Engineer Studio\"");
        Run(directory, "git config user.email \"ai-engineer-studio@localhost\"");
        Run(directory, "git add -A");
        var status = Run(directory, "git status --porcelain");
        if (string.IsNullOrWhiteSpace(status)) return "Изменений нет";
        var safeMessage = message.Replace("\"", "'").Replace("\r", " ").Replace("\n", " ");
        var result = Run(directory, $"git commit -m \"{safeMessage}\"");
        return result.Trim();
    }

    static string Run(string cwd, string args)
    {
        using var p = Process.Start(new ProcessStartInfo("git", args)
        {
            WorkingDirectory = cwd, UseShellExecute = false,
            RedirectStandardOutput = true, RedirectStandardError = true,
            CreateNoWindow = true
        });
        if (p == null) return "git не запущен";
        var output = p.StandardOutput.ReadToEnd();
        var error = p.StandardError.ReadToEnd();
        p.WaitForExit();
        return string.IsNullOrWhiteSpace(output) ? error : output;
    }
}
