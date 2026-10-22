using System.Diagnostics;
using System.Timers;

namespace AISoftCraftDuck.Services;

public interface IGitAutoCommitService
{
    void StartMonitoring(string projectPath, string provider = "None", bool isLocal = true);
    void StopMonitoring();
    event Action<string>? OnGitAction;
}

/// <summary>Periodic local Git commits. SVN and remote repositories are deliberately not modified automatically.</summary>
public sealed class GitAutoCommitService : IGitAutoCommitService, IDisposable
{
    private System.Timers.Timer? _timer;
    private string? _projectPath;
    public event Action<string>? OnGitAction;

    public void StartMonitoring(string projectPath, string provider = "None", bool isLocal = true)
    {
        StopMonitoring();
        if (!isLocal || !string.Equals(provider, "Git", StringComparison.OrdinalIgnoreCase))
        {
            OnGitAction?.Invoke(provider == "None" ? "Контроль версий отключён" : $"{provider}: автоматические локальные коммиты отключены");
            return;
        }

        _projectPath = projectPath;
        InitializeGit(projectPath);
        _timer = new System.Timers.Timer(30000) { AutoReset = true };
        _timer.Elapsed += OnTimerElapsed;
        _timer.Start();
        OnGitAction?.Invoke("Git: локальные авто-коммиты включены");
    }

    public void StopMonitoring() { _timer?.Stop(); _timer?.Dispose(); _timer = null; }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (string.IsNullOrEmpty(_projectPath)) return;
        try
        {
            if (string.IsNullOrWhiteSpace(RunGitCommand(_projectPath, "status --porcelain"))) return;
            RunGitCommand(_projectPath, "add .");
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            RunGitCommand(_projectPath, $"commit -m \"AI changes {timestamp}\"");
            OnGitAction?.Invoke($"Git: создан локальный коммит ({timestamp})");
        }
        catch (Exception ex) { OnGitAction?.Invoke($"Git: {ex.Message}"); }
    }

    private void InitializeGit(string projectPath)
    {
        try
        {
            if (Directory.Exists(Path.Combine(projectPath, ".git"))) { OnGitAction?.Invoke("Git: найден локальный репозиторий"); return; }
            RunGitCommand(projectPath, "init");
            OnGitAction?.Invoke("Git: инициализирован локальный репозиторий");
        }
        catch (Exception ex) { OnGitAction?.Invoke($"Git: не удалось инициализировать репозиторий: {ex.Message}"); }
    }

    private static string RunGitCommand(string workingDirectory, string arguments)
    {
        using var process = new Process { StartInfo = new ProcessStartInfo("git", arguments) { WorkingDirectory = workingDirectory, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true } };
        process.Start(); var output = process.StandardOutput.ReadToEnd(); var error = process.StandardError.ReadToEnd(); process.WaitForExit();
        if (process.ExitCode != 0) throw new Exception(string.IsNullOrWhiteSpace(error) ? "Команда git завершилась с ошибкой" : error.Trim());
        return output;
    }
    public void Dispose() => StopMonitoring();
}
