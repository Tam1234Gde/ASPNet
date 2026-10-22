using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AISoftCraftDuck.Models;
using AISoftCraftDuck.Services;

namespace AISoftCraftDuck.ViewModels;

public sealed partial class ChatItem : ObservableObject
{
    public string Role { get; }
    public string SenderLabel { get; }
    public ChatItem(string role, string content, string? model, IReadOnlyList<Attachment>? attachments = null) { Role = role; Content = content; SenderLabel = role == "You" ? "Вы" : model ?? "Ассистент"; Attachments = attachments ?? []; }
    // Compatibility overload for existing callers.
    public ChatItem(string role, string content, IReadOnlyList<Attachment>? attachments = null) : this(role, content, null, attachments) { }
    public IReadOnlyList<Attachment> Attachments { get; }
    [ObservableProperty] private string content = "";
}

public sealed partial class MainViewModel : ObservableObject
{
    private readonly IApiDuckClient api; private readonly IEnvService envService; private readonly FileDirectiveService directives; private readonly IGitAutoCommitService gitService; private readonly WorkspaceService workspace; private readonly ProjectToolService tools;
    private FileSystemWatcher? fileWatcher;
    public ObservableCollection<ChatItem> Messages { get; } = new();
    public ObservableCollection<string> Files { get; } = new();
    public ObservableCollection<string> Activity { get; } = new();
    public ObservableCollection<WorkspaceProject> Projects { get; } = new();
    [ObservableProperty] private WorkspaceProject? currentProject;
    [ObservableProperty] private string prompt = "";
    [ObservableProperty] private string attachmentSummary = "";
    private readonly List<Attachment> pendingAttachments = [];
    [ObservableProperty] private string projectDirectory = Environment.CurrentDirectory;
    [ObservableProperty] private string selectedModel = "gpt-5.6-luna";
    [ObservableProperty] private string selectedTheme = "Visual Studio Dark";
    [ObservableProperty] private bool isBusy, settingsOpen, activityOpen;
    [ObservableProperty] private string apiKey = "", baseUrl = "", openAiKey = "", systemPrompt = "";
    [ObservableProperty] private string versionControl = "None";
    [ObservableProperty] private bool versionControlLocal = true;
    public string[] Models { get; } = ["claude-sonnet-4-6", "gpt-5.6-terra", "gpt-5.6-luna", "deepseek-v4-flash", "deepseek-v4-pro", "grok-4.5", "claude-opus-4-6", "qwen3.7-max", "kimi-k3"];
    public string[] VersionControls { get; } = ["None", "Git", "SVN"];
    public string[] Themes { get; } = ["Visual Studio Dark", "Visual Studio Light", "Midnight Purple", "High Contrast"];
    partial void OnSelectedThemeChanged(string value)
    {
        var colors = value switch
        {
            "Visual Studio Light" => ("#F3F3F3", "#FFFFFF", "#FFFFFF", "#1E1E1E", "#616161", "#007ACC", "#DCDCDC", "#E5E5E5"),
            "Midnight Purple" => ("#15111F", "#201A2B", "#282033", "#F2ECFF", "#C9BDD9", "#9D5CFF", "#3B304A", "#100D17"),
            "High Contrast" => ("#000000", "#000000", "#111111", "#FFFFFF", "#FFFFFF", "#FFFF00", "#FFFFFF", "#000000"),
            _ => ("#1E1E1E", "#252526", "#2D2D30", "#F1F1F1", "#C8C8C8", "#007ACC", "#3F3F46", "#181818")
        };
        var resources = System.Windows.Application.Current?.Resources;
        if (resources is null) return;
        SetBrush("Bg", colors.Item1); SetBrush("TitleBarBg", colors.Item8); SetBrush("Panel", colors.Item2); SetBrush("CardBg", colors.Item3); SetBrush("TextPrimary", colors.Item4); SetBrush("TextSecondary", colors.Item5); SetBrush("PrimaryPurple", colors.Item6); SetBrush("AccentPurple", colors.Item6); SetBrush("Border", colors.Item7);
        // Brushes declared in XAML may be frozen by WPF; replace them instead of mutating.
        void SetBrush(string key, string color) => resources[key] = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color));
    }

    public MainViewModel(IApiDuckClient api, IEnvService envService, FileDirectiveService directives, IGitAutoCommitService gitService, WorkspaceService workspace, ProjectToolService tools)
    {
        this.api = api; this.envService = envService; this.directives = directives; this.gitService = gitService; this.workspace = workspace; this.tools = tools;
        var current = envService.Current; ApiKey = current.ApiKey ?? ""; BaseUrl = current.BaseUrl; OpenAiKey = current.OpenAiKey ?? "";
        VersionControl = current.VersionControl; VersionControlLocal = current.VersionControlLocal; UpdateSystemPrompt();
        gitService.OnGitAction += message => AddActivity(message);
        RefreshFiles(); SetupFileWatcher(); gitService.StartMonitoring(ProjectDirectory, VersionControl, VersionControlLocal);
        _ = LoadProjectsAsync();
    }
    private void UpdateSystemPrompt() => SystemPrompt = $"Ты — автономный ИИ-разработчик. Ты работаешь внутри каталога проекта '{Path.GetFullPath(ProjectDirectory)}'. У тебя есть доступ к структуре и файлам проекта через инструменты. Используй их для просмотра, чтения, создания и изменения файлов и папок проекта.";
    private void AddActivity(string message) => System.Windows.Application.Current?.Dispatcher.Invoke(() => Activity.Add($"{DateTime.Now:HH:mm:ss}  {message}"));
    [RelayCommand] private void ToggleSettings() => SettingsOpen = !SettingsOpen;
    [RelayCommand] private void ToggleActivity() => ActivityOpen = !ActivityOpen;
    [RelayCommand] private async Task SaveSettingsAsync()
    {
        await envService.SaveAsync(new DuckSettings(BaseUrl, ApiKey, null, OpenAiKey, envService.Current.Proxy, VersionControl, VersionControlLocal));
        gitService.StopMonitoring(); gitService.StartMonitoring(ProjectDirectory, VersionControl, VersionControlLocal); SettingsOpen = false; AddActivity("Настройки сохранены");
    }
    [RelayCommand] private async Task ChooseFolderAsync()
    {
        var dialog = new FolderBrowserDialog(); if (dialog.ShowDialog() != DialogResult.OK) return;
        var name = Path.GetFileName(dialog.SelectedPath.TrimEnd(Path.DirectorySeparatorChar));
        await OpenProjectAsync(new WorkspaceProject(Guid.NewGuid().ToString("N"), string.IsNullOrWhiteSpace(name) ? dialog.SelectedPath : name, dialog.SelectedPath, DateTime.UtcNow), true);
    }
    [RelayCommand] private async Task CreateProjectAsync()
    {
        var dialog = new FolderBrowserDialog { Description = "Выберите папку для нового проекта" }; if (dialog.ShowDialog() != DialogResult.OK) return;
        var name = $"Project-{DateTime.Now:yyyyMMdd-HHmmss}"; var directory = Path.Combine(dialog.SelectedPath, name); Directory.CreateDirectory(directory);
        await OpenProjectAsync(new WorkspaceProject(Guid.NewGuid().ToString("N"), name, directory, DateTime.UtcNow), true);
    }
    [RelayCommand] private async Task OpenProjectAsync(WorkspaceProject? project = null)
    {
        project ??= CurrentProject; if (project is null) return;
        await SaveCurrentChatAsync(); CurrentProject = project; ProjectDirectory = project.Directory; Messages.Clear();
        foreach (var message in await workspace.LoadChatAsync(project.Id)) Messages.Add(new ChatItem(message.Role, message.Content, message.Model));
        RefreshFiles(); SetupFileWatcher(); gitService.StopMonitoring(); gitService.StartMonitoring(ProjectDirectory, VersionControl, VersionControlLocal); UpdateSystemPrompt(); AddActivity($"Открыт проект: {project.Name}");
    }
    [RelayCommand] private async Task DeleteProjectAsync()
    {
        if (CurrentProject is null) return; var project = CurrentProject; workspace.DeleteChat(project.Id); Projects.Remove(project); CurrentProject = null; Messages.Clear(); await workspace.SaveAsync(Projects); AddActivity($"Удалена запись проекта: {project.Name}");
    }
    private async Task LoadProjectsAsync() { foreach (var project in await workspace.LoadAsync()) Projects.Add(project); }
    private async Task SaveCurrentChatAsync() { if (CurrentProject is null) return; await workspace.SaveChatAsync(CurrentProject.Id, Messages); }
    private async Task OpenProjectAsync(WorkspaceProject project, bool addToRegistry)
    {
        if (addToRegistry && !Projects.Any(x => x.Directory.Equals(project.Directory, StringComparison.OrdinalIgnoreCase))) { Projects.Add(project); await workspace.SaveAsync(Projects); }
        await OpenProjectAsync(project);
    }
    [RelayCommand] private void AddFiles()
    {
        var dialog = new OpenFileDialog { Multiselect = true, Title = "Добавить файлы в проект" }; if (dialog.ShowDialog() != DialogResult.OK) return;
        foreach (var file in dialog.FileNames) try { var destination = Path.Combine(ProjectDirectory, Path.GetFileName(file)); File.Copy(file, destination, true); AddActivity($"Добавлен файл: {Path.GetFileName(file)}"); } catch (Exception ex) { AddActivity($"Ошибка копирования: {ex.Message}"); }
        RefreshFiles();
    }
    [RelayCommand] private void AttachFiles()
    {
        var dialog = new OpenFileDialog { Multiselect = true, Title = "Прикрепить файлы или изображения", Filter = "Все файлы|*.*" }; if (dialog.ShowDialog() != DialogResult.OK) return;
        const int maxBytes = 10 * 1024 * 1024;
        foreach (var path in dialog.FileNames)
        {
            try
            {
                var bytes = File.ReadAllBytes(path);
                if (bytes.Length > maxBytes) { AddActivity($"Файл слишком большой (максимум 10 МБ): {Path.GetFileName(path)}"); continue; }
                var mime = GetMimeType(path); pendingAttachments.Add(new Attachment(Path.GetFileName(path), mime, bytes));
            }
            catch (Exception ex) { AddActivity($"Не удалось прикрепить файл: {ex.Message}"); }
        }
        AttachmentSummary = pendingAttachments.Count == 0 ? "" : $"📎 Прикреплено: {string.Join(", ", pendingAttachments.Select(x => x.FileName))}";
    }
    private static string GetMimeType(string path) => Path.GetExtension(path).ToLowerInvariant() switch { ".png" => "image/png", ".jpg" or ".jpeg" => "image/jpeg", ".gif" => "image/gif", ".webp" => "image/webp", ".pdf" => "application/pdf", ".txt" => "text/plain", ".json" => "application/json", _ => "application/octet-stream" };
    private void SetupFileWatcher()
    {
        fileWatcher?.Dispose(); if (!Directory.Exists(ProjectDirectory)) return;
        try { fileWatcher = new FileSystemWatcher(ProjectDirectory) { IncludeSubdirectories = true, NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite };
            fileWatcher.Changed += (_, e) => System.Windows.Application.Current?.Dispatcher.Invoke(RefreshFiles);
            fileWatcher.Created += (_, e) => { System.Windows.Application.Current?.Dispatcher.Invoke(RefreshFiles); AddActivity($"Создан файл: {Path.GetRelativePath(ProjectDirectory, e.FullPath)}"); };
            fileWatcher.Deleted += (_, _) => System.Windows.Application.Current?.Dispatcher.Invoke(RefreshFiles); fileWatcher.Renamed += (_, _) => System.Windows.Application.Current?.Dispatcher.Invoke(RefreshFiles); fileWatcher.EnableRaisingEvents = true; } catch { }
    }
    private void RefreshFiles() { Files.Clear(); if (!Directory.Exists(ProjectDirectory)) return; try { foreach (var f in Directory.EnumerateFiles(ProjectDirectory, "*", SearchOption.AllDirectories).Take(500)) Files.Add(Path.GetRelativePath(ProjectDirectory, f)); } catch { } }
    [RelayCommand] private async Task SendAsync()
    {
        if ((string.IsNullOrWhiteSpace(Prompt) && pendingAttachments.Count == 0) || IsBusy) return; var text = Prompt; var attachments = pendingAttachments.ToArray(); Prompt = ""; pendingAttachments.Clear(); AttachmentSummary = ""; Messages.Add(new ChatItem("You", text, attachments)); var answer = new ChatItem("Assistant", "", SelectedModel); Messages.Add(answer); IsBusy = true;
        try { var history = new List<ChatMessage> { new("system", SystemPrompt + "\nСначала определи тип запроса. Для вопроса дай содержательный ответ без вызова инструментов. Для задачи разработки используй инструменты для анализа, создания и редактирования файлов; после изменений обязательно выполни подходящую сборку или тесты. В чате кратко объясняй выполненную работу.") }; history.AddRange(Messages.Where(x => x != answer).Select(x => new ChatMessage(x.Role == "You" ? "user" : "assistant", x.Content, x.Attachments)));
            var progressLog = new List<string> { "⏳ Обрабатываю запрос…" };
            answer.Content = progressLog[0];
            var progress = new Progress<string>(status =>
            {
                AddActivity(status);
                progressLog.Add($"`{status}`");
                answer.Content = string.Join("\n\n", progressLog);
            });
            var finalAnswer = await api.RunAgentAsync(history, SelectedModel, (name, args) => Task.FromResult(tools.Execute(ProjectDirectory, name, args)), status => ((IProgress<string>)progress).Report(status));
            answer.Content = $"{string.Join("\n\n", progressLog)}\n\n---\n\n{finalAnswer}";
            var parsed = directives.Parse(answer.Content); if (parsed.Count > 0) { AddActivity($"ИИ изменяет/создаёт {parsed.Count} файл(а): {string.Join(", ", parsed.Select(x => x.Path))}"); await directives.WriteAsync(ProjectDirectory, parsed); AddActivity($"ИИ применил {parsed.Count} файл(а)"); RefreshFiles(); }
        } catch (Exception ex) { answer.Content = $"⚠️ {ex.Message}"; } finally { IsBusy = false; await SaveCurrentChatAsync(); }
    }
}
