using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using AIEngineerStudio.Services;

namespace AIEngineerStudio;

public partial class MainWindow
{
    readonly HashSet<string> expandedExplorerPaths = new(StringComparer.OrdinalIgnoreCase);
    readonly DispatcherTimer explorerTimer = new() { Interval = TimeSpan.FromMilliseconds(350) };
    CancellationTokenSource? explorerCancellation;
    string explorerRoot = "";
    string? explorerSelection;
    bool rebuildingExplorer;
    bool explorerClosed;
    bool ShowGenerated => ExplorerGenerated.IsChecked == true;

    void InitializeExplorer()
    {
        explorerTimer.Tick += (_, _) => { explorerTimer.Stop(); RefreshTree(); };
        Closed += (_, _) =>
        {
            explorerClosed = true;
            explorerTimer.Stop();
            explorerCancellation?.Cancel();
            projectWatcher?.Dispose();
        };
    }

    void ScheduleExplorerRefresh()
    {
        if (explorerClosed) return;
        explorerTimer.Stop();
        explorerTimer.Start();
    }

    void StartProjectWatcher()
    {
        projectWatcher?.Dispose();
        projectWatcher = null;
        if (!Directory.Exists(projectPath)) return;
        try
        {
            projectWatcher = new FileSystemWatcher(projectPath)
            { IncludeSubdirectories = true, NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName };
            FileSystemEventHandler changed = (_, _) => QueueExplorerRefresh();
            projectWatcher.Created += changed;
            projectWatcher.Deleted += changed;
            projectWatcher.Renamed += (_, _) => QueueExplorerRefresh();
            projectWatcher.Error += (_, _) => QueueExplorerRefresh();
            projectWatcher.EnableRaisingEvents = true;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or ArgumentException)
        { StatusText.Text = "Автообновление недоступно: " + exception.Message; }
    }

    void QueueExplorerRefresh()
    {
        if (!explorerClosed && !Dispatcher.HasShutdownStarted)
            Dispatcher.BeginInvoke(new Action(ScheduleExplorerRefresh));
    }

    async void RefreshTree()
    {
        if (ProjectTree is null || ExplorerStatus is null || explorerClosed) return;
        explorerCancellation?.Cancel();
        explorerCancellation = new CancellationTokenSource();
        CancellationToken token = explorerCancellation.Token;
        string root = projectPath;
        string query = FilterBox.Text.Trim();
        bool show = ShowGenerated;
        if (!string.Equals(explorerRoot, root, StringComparison.OrdinalIgnoreCase))
        {
            expandedExplorerPaths.Clear();
            expandedExplorerPaths.Add(root);
            explorerSelection = null;
            selectedFiles.Clear();
            explorerRoot = root;
        }
        ExplorerStatus.Text = query.Length > 0 ? "Поиск…" : "Загрузка…";
        try
        {
            List<ExplorerEntry> entries = await Task.Run(() => query.Length == 0
                ? ExplorerFileService.Read(root, show, token)
                : ExplorerFileService.Search(root, root, query, show, token), token);
            if (token.IsCancellationRequested) return;
            rebuildingExplorer = true;
            ProjectTree.Items.Clear();
            TreeViewItem rootItem = CreateExplorerItem(new ExplorerEntry
            { Path = root, IsDirectory = true, Children = entries }, token, query.Length > 0);
            ProjectTree.Items.Add(rootItem);
            rootItem.IsExpanded = true;
            rebuildingExplorer = false;
            ExplorerStatus.Text = entries.Count == 0
                ? (query.Length > 0 ? "Ничего не найдено" : "Папка пуста")
                : (query.Length > 0 ? "Результаты поиска" : "Двойной щелчок — открыть · F5 — обновить");
            ContextInfo.Text = activeProject?.Name ?? System.IO.Path.GetFileName(root);
            ContextInfo.ToolTip = root;
        }
        catch (OperationCanceledException) { }
        catch (Exception exception)
        { if (!token.IsCancellationRequested) ExplorerStatus.Text = "Ошибка: " + exception.Message; }
        finally { rebuildingExplorer = false; }
    }

    TreeViewItem CreateExplorerItem(ExplorerEntry entry, CancellationToken token, bool searching)
    {
        TreeViewItem item = new() { Tag = entry, ToolTip = entry.Error ?? entry.Path };
        item.Header = ExplorerHeader(entry);
        item.ContextMenu = CreateExplorerMenu(entry, item);
        item.PreviewMouseRightButtonDown += (_, e) =>
        {
            if (FindExplorerItem(e.OriginalSource as DependencyObject) != item) return;
            item.IsSelected = true;
            item.Focus();
            e.Handled = true;
        };
        item.Selected += (_, e) =>
        {
            if (e.OriginalSource != item) return;
            explorerSelection = entry.Path;
            ExplorerStatus.Text = Path.GetRelativePath(projectPath, entry.Path);
        };
        if (entry.Error is not null) return item;
        if (entry.IsDirectory && !entry.IsLink)
        {
            if (entry.Children is not null)
                foreach (ExplorerEntry child in entry.Children) item.Items.Add(CreateExplorerItem(child, token, searching));
            else item.Items.Add(new TreeViewItem { Header = "Загрузка…", IsEnabled = false });
            bool loaded = entry.Children is not null;
            item.Expanded += async (_, e) =>
            {
                if (e.OriginalSource != item) return;
                if (!searching) expandedExplorerPaths.Add(entry.Path);
                if (loaded || token.IsCancellationRequested) return;
                loaded = true;
                bool show = ShowGenerated;
                try
                {
                    List<ExplorerEntry> children = await Task.Run(() => ExplorerFileService.Read(entry.Path, show, token), token);
                    if (token.IsCancellationRequested) return;
                    item.Items.Clear();
                    foreach (ExplorerEntry child in children) item.Items.Add(CreateExplorerItem(child, token, searching));
                }
                catch (OperationCanceledException) { }
            };
            item.Collapsed += (_, e) =>
            { if (e.OriginalSource == item && !rebuildingExplorer && !searching) expandedExplorerPaths.Remove(entry.Path); };
            item.IsExpanded = searching ? entry.Children?.Count > 0 : expandedExplorerPaths.Contains(entry.Path);
        }
        if (string.Equals(explorerSelection, entry.Path, StringComparison.OrdinalIgnoreCase)) item.IsSelected = true;
        return item;
    }

    FrameworkElement ExplorerHeader(ExplorerEntry entry)
    {
        (string label, string color) = ExplorerFileStyle(entry);
        StackPanel panel = new() { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 3, 8, 3) };
        panel.Children.Add(new Border
        {
            Width = 40, CornerRadius = new CornerRadius(4), Margin = new Thickness(0, 0, 8, 0),
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)) { Opacity = 0.19 },
            Child = new TextBlock { Text = label, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)),
                FontSize = 10, FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Center, Padding = new Thickness(2) }
        });
        // Keep the badge foreground vivid (opacity belongs to its brush, not the element).
        TextBlock name = new() { Text = entry.Error is null ? Path.GetFileName(entry.Path.TrimEnd(Path.DirectorySeparatorChar)) : "⚠ " + Path.GetFileName(entry.Path),
            VerticalAlignment = VerticalAlignment.Center };
        name.SetResourceReference(TextBlock.ForegroundProperty, "Body");
        panel.Children.Add(name);
        if (entry.IsLink) panel.Children.Add(new TextBlock { Text = " ↗", Foreground = Brushes.SkyBlue });
        return panel;
    }

    internal static (string Label, string Color) ExplorerFileStyle(ExplorerEntry entry) => entry.IsDirectory ? ("DIR", "#D9A441") :
        Path.GetExtension(entry.Path).ToLowerInvariant() switch
        {
            ".cs" => ("C#", "#BB86FC"), ".xaml" => ("XAML", "#48BFA6"),
            ".py" => ("PY", "#D9AD38"), ".js" or ".jsx" => ("JS", "#D9AD38"),
            ".ts" or ".tsx" => ("TS", "#459FE8"), ".json" => ("{ }", "#D69847"),
            ".xml" or ".html" or ".svg" => ("</>", "#E38763"),
            ".css" or ".scss" => ("CSS", "#AA85E8"), ".md" or ".txt" => ("TXT", "#5EAAD9"),
            ".png" or ".jpg" or ".jpeg" or ".gif" or ".webp" or ".ico" => ("IMG", "#DB7BB1"),
            ".mp4" or ".webm" or ".mov" => ("VID", "#E77993"),
            ".mp3" or ".wav" or ".ogg" => ("AUD", "#57BDA8"),
            ".csproj" or ".sln" or ".slnx" => ("PRJ", "#B990E5"),
            ".zip" or ".7z" or ".gz" => ("ZIP", "#CF9A60"),
            _ => ("FILE", "#8C9EAF")
        };

    ContextMenu CreateExplorerMenu(ExplorerEntry entry, TreeViewItem item)
    {
        ContextMenu menu = new();
        void Add(string title, Action action)
        {
            MenuItem command = new() { Header = title };
            command.Click += (_, _) => ExplorerAction(action);
            menu.Items.Add(command);
        }
        Add(entry.IsDirectory ? "Раскрыть / свернуть" : "Открыть файл", () =>
        { if (entry.IsDirectory) item.IsExpanded = !item.IsExpanded; else OpenExplorerPath(entry.Path); });
        Add("Копировать полный путь", () => Clipboard.SetText(entry.Path));
        Add("Копировать относительный путь", () => Clipboard.SetText(Path.GetRelativePath(projectPath, entry.Path)));
        Add("Показать в проводнике Windows", () => Process.Start(new ProcessStartInfo("explorer.exe", $"/select,\"{entry.Path}\"") { UseShellExecute = true }));
        menu.Items.Add(new Separator());
        Add("Обновить дерево", RefreshTree);
        return menu;
    }

    void ExplorerAction(Action action)
    {
        try { action(); }
        catch (Exception exception) { ExplorerStatus.Text = "Не удалось выполнить действие: " + exception.Message; }
    }
    void OpenExplorerPath(string path) => ExplorerAction(() => Process.Start(new ProcessStartInfo(path) { UseShellExecute = true }));
    static TreeViewItem? FindExplorerItem(DependencyObject? source)
    {
        while (source is not null)
        {
            if (source is TreeViewItem item) return item;
            source = source is Visual ? VisualTreeHelper.GetParent(source) : LogicalTreeHelper.GetParent(source);
        }
        return null;
    }
    void ProjectTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        TreeViewItem? item = FindExplorerItem(e.OriginalSource as DependencyObject);
        if (item?.Tag is ExplorerEntry { IsDirectory: false, Error: null } entry)
        { OpenExplorerPath(entry.Path); e.Handled = true; }
    }
    void Explorer_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F5) { RefreshTree(); e.Handled = true; }
        if (e.Key == Key.Enter && ProjectTree.SelectedItem is TreeViewItem item && item.Tag is ExplorerEntry entry)
        { if (entry.IsDirectory) item.IsExpanded = !item.IsExpanded; else OpenExplorerPath(entry.Path); e.Handled = true; }
    }
    void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (FilterPlaceholder is null) return;
        FilterPlaceholder.Visibility = string.IsNullOrEmpty(FilterBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        explorerCancellation?.Cancel();
        ScheduleExplorerRefresh();
    }
    void ExplorerRefresh_Click(object sender, RoutedEventArgs e) => RefreshTree();
    void ExplorerClear_Click(object sender, RoutedEventArgs e) { FilterBox.Clear(); FilterBox.Focus(); }
    void ExplorerGenerated_Click(object sender, RoutedEventArgs e) => RefreshTree();
    void ExplorerCollapse_Click(object sender, RoutedEventArgs e)
    {
        expandedExplorerPaths.Clear();
        expandedExplorerPaths.Add(projectPath);
        FilterBox.Clear();
        RefreshTree();
    }
    void SwitchProject_Click(object sender, RoutedEventArgs e) => SelectProject();
}
