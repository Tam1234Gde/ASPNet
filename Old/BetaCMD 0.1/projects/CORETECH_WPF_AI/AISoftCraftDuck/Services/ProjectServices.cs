using AISoftCraftDuck.Models;
using System.Diagnostics;
using System.Timers;

namespace AISoftCraftDuck.Services;

//public interface IGitAutoCommitService
//{
//    void StartMonitoring(string projectPath);
//    void StopMonitoring();
//    event Action<string>? OnGitAction;
//}

//public sealed class GitAutoCommitService : IGitAutoCommitService, IDisposable
//{
//    private System.Timers.Timer? _timer;
//    private string? _projectPath;

//    public event Action<string>? OnGitAction;

//    public void StartMonitoring(string projectPath)
//    {
//        StopMonitoring();
        
//        _projectPath = projectPath;
        
//        // Initialize git if not already initialized
//        InitializeGit(projectPath);
        
//        // Create timer for auto-commits every 30 seconds
//        _timer = new System.Timers.Timer(30000); // 30 seconds
//        _timer.Elapsed += OnTimerElapsed;
//        _timer.AutoReset = true;
//        _timer.Start();
        
//        OnGitAction?.Invoke("🔧 Git auto-commit enabled");
//    }

//    public void StopMonitoring()
//    {
//        _timer?.Stop();
//        _timer?.Dispose();
//        _timer = null;
//    }

//    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
//    {
//        if (string.IsNullOrEmpty(_projectPath)) return;

//        try
//        {
//            // Check if there are any changes
//            var status = RunGitCommand(_projectPath, "status --porcelain");
            
//            if (string.IsNullOrWhiteSpace(status))
//            {
//                // No changes to commit
//                return;
//            }

//            // Add all changes
//            RunGitCommand(_projectPath, "add .");
//            OnGitAction?.Invoke("📝 Git: Added all changes");

//            // Commit with timestamp
//            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//            var commitMsg = $"Auto-commit: AI changes at {timestamp}";
//            RunGitCommand(_projectPath, $"commit -m \"{commitMsg}\"");
//            OnGitAction?.Invoke($"✅ Git: Committed changes at {timestamp}");
//        }
//        catch (Exception ex)
//        {
//            OnGitAction?.Invoke($"⚠️ Git error: {ex.Message}");
//        }
//    }

//    private void InitializeGit(string projectPath)
//    {
//        try
//        {
//            // Check if .git directory exists
//            var gitDir = Path.Combine(projectPath, ".git");
            
//            if (!Directory.Exists(gitDir))
//            {
//                // Initialize new repository
//                RunGitCommand(projectPath, "init");
//                OnGitAction?.Invoke("🎉 Git: Initialized new repository");
                
//                // Create initial commit
//                RunGitCommand(projectPath, "add .");
//                RunGitCommand(projectPath, "commit -m \"Initial commit\"");
//                OnGitAction?.Invoke("✅ Git: Created initial commit");
//            }
//            else
//            {
//                OnGitAction?.Invoke("✅ Git: Repository already initialized");
//            }
//        }
//        catch (Exception ex)
//        {
//            OnGitAction?.Invoke($"⚠️ Git initialization error: {ex.Message}");
//        }
//    }

//    private string RunGitCommand(string workingDirectory, string arguments)
//    {
//        using var process = new Process
//        {
//            StartInfo = new ProcessStartInfo
//            {
//                FileName = "git",
//                Arguments = arguments,
//                WorkingDirectory = workingDirectory,
//                RedirectStandardOutput = true,
//                RedirectStandardError = true,
//                UseShellExecute = false,
//                CreateNoWindow = true
//            }
//        };

//        process.Start();
//        var output = process.StandardOutput.ReadToEnd();
//        var error = process.StandardError.ReadToEnd();
//        process.WaitForExit();

//        if (process.ExitCode != 0 && !string.IsNullOrEmpty(error))
//        {
//            throw new Exception(error);
//        }

//        return output;
//    }

//    public void Dispose()
//    {
//        StopMonitoring();
//    }
//}

public sealed class DockerService
{
    // Placeholder for Docker functionality
}

public sealed class FileDirectiveService
{
    public List<FileDirective> Parse(string content)
    {
        var files = new List<FileDirective>();
        
        // Parse file blocks like:
        // ```filename.ext
        // content
        // ```
        
        var lines = content.Split('\n');
        string? currentFile = null;
        var currentContent = new System.Text.StringBuilder();
        bool inCodeBlock = false;
        
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].TrimEnd();
            
            if (line.StartsWith("```") && !inCodeBlock)
            {
                // Start of code block
                var filename = line.Substring(3).Trim();
                if (!string.IsNullOrEmpty(filename) && !filename.Equals("json", StringComparison.OrdinalIgnoreCase) 
                    && !filename.Equals("csharp", StringComparison.OrdinalIgnoreCase)
                    && !filename.Equals("xml", StringComparison.OrdinalIgnoreCase)
                    && filename.Contains("."))
                {
                    currentFile = filename;
                    currentContent.Clear();
                    inCodeBlock = true;
                }
            }
            else if (line.StartsWith("```") && inCodeBlock)
            {
                // End of code block
                if (currentFile != null)
                {
                    files.Add(new FileDirective(currentFile, currentContent.ToString()));
                    currentFile = null;
                }
                inCodeBlock = false;
            }
            else if (inCodeBlock && currentFile != null)
            {
                currentContent.AppendLine(line);
            }
        }
        
        return files;
    }
    
    public async Task WriteAsync(string baseDirectory, List<FileDirective> files, CancellationToken ct = default)
    {
        foreach (var file in files)
        {
            var fullPath = Path.Combine(baseDirectory, file.Path);
            var directory = Path.GetDirectoryName(fullPath);
            
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            await File.WriteAllTextAsync(fullPath, file.Content.TrimEnd(), ct);
        }
    }
}
