using Microsoft.Win32;
using AIEngineerStudio.Models;
using AIEngineerStudio.Services;
using AIEngineerStudio.UI;
using AIEngineerStudio.Views;
using System.IO;
using System.Diagnostics;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace AIEngineerStudio;
public partial class MainWindow : Window
{
    readonly HashSet<string> selectedFiles = new();
    readonly ProjectFileService fileService = new();
    readonly NeuralBridgeService neuralService = new();
    readonly ComputerControlService computerService = new();
    readonly ProjectStore projectStore = new();
    ProjectDocument? activeProject;
    readonly List<Dictionary<string,string>> history = new();
    public AppSettings settings = SettingsStore.Load();
    public string ApiKey { get => settings.ApiKey; set => settings.ApiKey = value; }
    public string BaseUrl { get => settings.BaseUrl; set => settings.BaseUrl = value; }
    public string Model { get => ModelBox.SelectedItem?.ToString() ?? settings.Model; set { settings.Model=value; if (ModelBox != null) ModelBox.SelectedItem = value; } }
    public double Temperature { get => settings.Temperature; set => settings.Temperature = value; }
    public int MaxTokens { get => settings.MaxTokens; set => settings.MaxTokens = value; }
    public string PromptTemplate { get => settings.PromptTemplate; set => settings.PromptTemplate=value; }
    public string PythonPath { get => settings.PythonPath; set => settings.PythonPath=value; }
    public bool ComputerControlEnabled { get => settings.ComputerControlEnabled; set => settings.ComputerControlEnabled=value; }
    public void SaveSettings() => SettingsStore.Save(settings);
    string projectPath = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName ?? Environment.CurrentDirectory;
    FileSystemWatcher? projectWatcher;
    public MainWindow() { InitializeComponent(); Loaded += (_,_) => SelectProject(); ModelBox.ItemsSource = new[] { "gpt-5.6-terra", "gpt-5.6-luna", "claude-sonnet-4-6", "claude-opus-4-6", "deepseek-v4-flash", "deepseek-v4-pro" }; ModelBox.SelectedItem=settings.Model; if(ModelBox.SelectedIndex<0)ModelBox.SelectedIndex=0; PromptBox.ItemsSource=settings.Prompts; PromptBox.DisplayMemberPath="Name"; PromptBox.SelectedIndex=0; SetTheme(settings.Theme); RefreshTree(); ShowAgent(); AddMarkdown("Ассистент", "## Готов к работе\n\nВыберите файлы контекста слева и опишите задачу. Поддерживаются **Markdown**, `inline code` и блоки кода.", true); }
    void TitleBar_MouseLeftButtonDown(object s, MouseButtonEventArgs e) { if(e.ClickCount==2) Maximize_Click(s,e); else DragMove(); }
    void Minimize_Click(object s,RoutedEventArgs e)=>WindowState=WindowState.Minimized;
    void Maximize_Click(object s,RoutedEventArgs e)=>WindowState=WindowState==WindowState.Maximized?WindowState.Normal:WindowState.Maximized;
    void Close_Click(object s,RoutedEventArgs e)=>Close();
    void SelectProject() { var dialog=new ProjectDialog(projectStore.Names); if(dialog.ShowDialog()!=true){Close();return;} try { if(dialog.SelectedProject!=null) activeProject=projectStore.Load(dialog.SelectedProject); else activeProject=projectStore.Create(dialog.NewName!,dialog.DirectoryPath!,dialog.Description??""); } catch(Exception ex){MessageBox.Show($"Не удалось открыть проект: {ex.Message}");return;} projectPath=activeProject.WorkingDirectory; history.Clear(); history.AddRange(activeProject.History.Select(x=>new Dictionary<string,string>{{"role",x.Role},{"content",x.Content}})); ContextPanel.Visibility=Visibility.Visible; ContextColumn.Width=new GridLength(250); ContextInfo.Text=$"{activeProject.Name}\n{activeProject.Description}"; StartProjectWatcher(); RefreshTree(); StatusText.Text=$"Проект: {activeProject.Name} · {projectPath}"; }
    void StartProjectWatcher() { projectWatcher?.Dispose(); if(!Directory.Exists(projectPath))return; projectWatcher=new FileSystemWatcher(projectPath){IncludeSubdirectories=true,EnableRaisingEvents=true,NotifyFilter=NotifyFilters.FileName|NotifyFilters.DirectoryName|NotifyFilters.LastWrite}; FileSystemEventHandler refresh=(_,_)=>Dispatcher.BeginInvoke(new Action(RefreshTree)); RenamedEventHandler renamed=(_,_)=>Dispatcher.BeginInvoke(new Action(RefreshTree)); projectWatcher.Created+=refresh;projectWatcher.Deleted+=refresh;projectWatcher.Changed+=refresh;projectWatcher.Renamed+=renamed; }

    void SaveProject() { if(activeProject==null)return; activeProject.History=history.Select(x=>new ChatMessage(x["role"],x["content"])).ToList(); projectStore.Save(activeProject); }
    void RefreshTree() {
        // TextChanged срабатывает прямо при загрузке XAML, когда нижние элементы
        // разметки ещё не созданы. В таком случае полное обновление выполнит конструктор.
        if (ProjectTree is null || FilterBox is null || ContextInfo is null) return;
        ProjectTree.Items.Clear();
        var filter=FilterBox.Text.Trim();
        try {
            var root=new TreeViewItem { Header=ExplorerHeader("📂", Path.GetFileName(projectPath), "#E3B341", false), IsExpanded=true, Tag=null };
            var folders=new Dictionary<string,TreeViewItem>{{projectPath,root}};
            foreach(var file in Directory.EnumerateFiles(projectPath,"*",SearchOption.AllDirectories).Where(p=>!p.Contains("\\.git\\")&&!p.Contains("\\bin\\")&&!p.Contains("\\obj\\")&&!p.Contains("\\node_modules\\")).Take(500)) {
                var rel=Path.GetRelativePath(projectPath,file); if(filter.Length>0&&!rel.Contains(filter,StringComparison.OrdinalIgnoreCase)) continue;
                var folder=Path.GetDirectoryName(file)!;
                var chain=new Stack<string>(); for(var d=folder; d!=null && d.StartsWith(projectPath,StringComparison.OrdinalIgnoreCase) && !folders.ContainsKey(d); d=Path.GetDirectoryName(d)) chain.Push(d);
                while(chain.Count>0) { var d=chain.Pop(); var parent=folders[Path.GetDirectoryName(d)!]; var item=new TreeViewItem{Header=ExplorerHeader("📁",Path.GetFileName(d),"#E3B341",false),Tag=null}; parent.Items.Add(item);folders[d]=item; }
                folders[folder].Items.Add(new TreeViewItem { Header=ExplorerHeader(FileIcon(file),Path.GetFileName(file),FileColor(file),selectedFiles.Contains(file)), Tag=file });
            }
            ProjectTree.Items.Add(root);
        } catch { }
        ContextInfo.Text=activeProject==null?"":$"Рабочая папка:\n{activeProject.WorkingDirectory}";
    }
    StackPanel ExplorerHeader(string icon, string name, string color, bool selected) { var p=new StackPanel{Orientation=Orientation.Horizontal}; p.Children.Add(new TextBlock{Text=icon,Foreground=(Brush)new BrushConverter().ConvertFromString(color),Width=19,FontFamily=new FontFamily("Consolas")}); p.Children.Add(new TextBlock{Text=name,Foreground=(Brush)FindResource("Body")}); if(selected) p.Children.Add(new TextBlock{Text="  ✓",Foreground=(Brush)FindResource("Accent"),FontWeight=FontWeights.Bold}); return p; }
    static string FileIcon(string path) => Path.GetExtension(path).ToLowerInvariant() switch { ".cs"=>"C#", ".xaml"=>"XAML", ".py"=>"PY", ".js" or ".ts"=>"JS", ".json"=>"{}", ".md"=>"M↓", ".xml"=>"<>" , ".css"=>"#", ".html"=>"<>" , _=>"·" };
    static string FileColor(string path) => Path.GetExtension(path).ToLowerInvariant() switch { ".cs"=>"#9B59B6", ".xaml"=>"#5CB85C", ".py"=>"#FFD343", ".js" or ".ts"=>"#58A6FF", ".json"=>"#E3B341", ".md"=>"#79C0FF", _=>"#8B949E" };
    void FilterBox_TextChanged(object s,TextChangedEventArgs e) { if(FilterPlaceholder!=null) FilterPlaceholder.Visibility=string.IsNullOrEmpty(FilterBox.Text)?Visibility.Visible:Visibility.Collapsed; RefreshTree(); }
    void ProjectTree_MouseDoubleClick(object s,MouseButtonEventArgs e) { if(ProjectTree.SelectedItem is TreeViewItem i && i.Tag is string path) { try { Process.Start(new ProcessStartInfo(path){UseShellExecute=true}); } catch { } } }
    void SwitchProject_Click(object s,RoutedEventArgs e) => SelectProject();
    void ProjectTree_ContextMenuOpening(object s,ContextMenuEventArgs e) { if(ProjectTree.SelectedItem is not TreeViewItem item || item.Tag is not string path) { e.Handled=true; return; } var menu=new ContextMenu(); var open=new MenuItem{Header="Открыть"};open.Click+=(a,b)=>{try{Process.Start(new ProcessStartInfo(path){UseShellExecute=true});}catch{}};var copy=new MenuItem{Header="Копировать путь"};copy.Click+=(a,b)=>Clipboard.SetText(path);var locate=new MenuItem{Header="Показать в проводнике"};locate.Click+=(a,b)=>Process.Start(new ProcessStartInfo("explorer.exe",$"/select,\"{path}\""){UseShellExecute=true});menu.Items.Add(open);menu.Items.Add(copy);menu.Items.Add(locate);item.ContextMenu=menu; }

    void OpenFolder_Click(object s,RoutedEventArgs e) { var dlg=new OpenFolderDialog { Title="Выберите корень проекта", InitialDirectory=projectPath }; if(dlg.ShowDialog()==true) { projectPath=dlg.FolderName; selectedFiles.Clear(); RefreshTree(); StatusText.Text=$"Проект: {projectPath}"; } }
    void ModelBox_SelectionChanged(object s,SelectionChangedEventArgs e) { if(ModelBox.SelectedItem!=null) StatusText.Text=$"Модель: {ModelBox.SelectedItem}"; }
    void NewChat_Click(object s,RoutedEventArgs e) { ChatPanel.Children.Clear(); AddMarkdown("Ассистент","## Новая сессия\n\nКонтекст очищен. Чем помочь?",true); }
    void PromptInput_KeyDown(object s,KeyEventArgs e) { if(e.Key==Key.Enter && Keyboard.Modifiers==ModifierKeys.Control) { Send(); e.Handled=true; } }
    void Send_Click(object s,RoutedEventArgs e)=>Send();
    async void Send() {
        var text=PromptInput.Text.Trim(); if(text.Length==0)return;
        AddMarkdown("Вы",text,false); PromptInput.Clear(); history.Add(new Dictionary<string,string>{{"role","user"},{"content",text}});
        StatusText.Text="● Генерация ответа…"; ShowAgent();
        try {
            var started=Stopwatch.StartNew(); var answer=await AskNeuralNetwork(); started.Stop();
            history.Add(new Dictionary<string,string>{{"role","assistant"},{"content",answer}}); SaveProject();
             var version = GitVersionService.CommitRequest(projectPath, $"Запрос: {text[..Math.Min(text.Length, 72)]}");
             AddMarkdown(ModelBox.SelectedItem?.ToString()??"Ассистент",answer,true);
            StatusText.Text=$"● Готово · {started.Elapsed.TotalSeconds:F2} сек";
        } catch(Exception ex) { AddMarkdown("Системная ошибка",$"## Не удалось выполнить запрос\n\n`{ex.Message}`\n\nПроверьте API-ключ в Настройках и наличие Python с пакетом `openai`.",true); StatusText.Text="Ошибка запроса"; }
    }
    string BuildContext() => fileService.ReadContext(selectedFiles, projectPath);
    async Task<string> AskNeuralNetwork() {
        var bridge=Path.Combine(AppContext.BaseDirectory,"neural_bridge.py"); if(!File.Exists(bridge)) bridge=Path.Combine(projectPath,"AI Engineer Studio WPF","neural_bridge.py");
        if(!File.Exists(bridge)) throw new FileNotFoundException("Не найден neural_bridge.py",bridge);
        var system=PromptBox.SelectedItem is SystemPrompt selected ? selected.Content : PromptTemplate;
        var payload=JsonSerializer.Serialize(new { api_key=ApiKey, base_url=BaseUrl, model=ModelBox.SelectedItem?.ToString(), temperature=Temperature, max_tokens=MaxTokens, system_prompt=system, context=BuildContext(), project_path=projectPath, computer_control_enabled=settings.ComputerControlEnabled, history });
        var psi=new ProcessStartInfo(PythonPath, $"\"{bridge}\"") { RedirectStandardInput=true, RedirectStandardOutput=true, RedirectStandardError=true, UseShellExecute=false, CreateNoWindow=true, StandardInputEncoding=new System.Text.UTF8Encoding(false), StandardOutputEncoding=new System.Text.UTF8Encoding(false), StandardErrorEncoding=new System.Text.UTF8Encoding(false) };
        using var process=Process.Start(psi) ?? throw new InvalidOperationException("Не удалось запустить Python bridge.");
        await process.StandardInput.WriteAsync(payload); process.StandardInput.Close(); var outputLines=new List<string>(); string? line; while((line=await process.StandardOutput.ReadLineAsync())!=null) { outputLines.Add(line); try { using var ev=JsonDocument.Parse(line); if(ev.RootElement.TryGetProperty("event",out var kind)) { var name=kind.GetString(); if(name=="tool_start") { var tool=ev.RootElement.GetProperty("name").GetString(); Dispatcher.Invoke(()=>AddToolEvent(tool!,ev.RootElement.GetProperty("args").ToString())); } else if(name=="tool_end") Dispatcher.Invoke(()=>AddToolEvent("✓ "+ev.RootElement.GetProperty("name").GetString(),ev.RootElement.GetProperty("result").GetString()??"")); } } catch { } } var output=outputLines.LastOrDefault()??""; var error=await process.StandardError.ReadToEndAsync(); await process.WaitForExitAsync();
        if(string.IsNullOrWhiteSpace(output)) throw new InvalidOperationException(error.Length>0?error:"Bridge не вернул ответ.");
        using var doc=JsonDocument.Parse(output); if(doc.RootElement.TryGetProperty("event",out var eventKind) && eventKind.GetString()=="final") return doc.RootElement.GetProperty("content").GetString() ?? "Пустой ответ модели."; if(doc.RootElement.TryGetProperty("ok",out var ok) && !ok.GetBoolean()) throw new InvalidOperationException(doc.RootElement.GetProperty("error").GetString()); throw new InvalidOperationException("Bridge вернул некорректный ответ.");
    }
    void AddMarkdown(string sender,string markdown,bool assistant) { var box=new Border { BorderThickness=new Thickness(1), CornerRadius=new CornerRadius(8), Padding=new Thickness(14), Margin=new Thickness(0,0,0,14) }; box.SetResourceReference(BackgroundProperty, assistant ? "PanelBg" : "CardBg"); box.SetResourceReference(Border.BorderBrushProperty, "Border"); var p=new StackPanel(); var header=new DockPanel { LastChildFill=false, Margin=new Thickness(0,0,0,8) }; var title=new TextBlock { Text=sender, FontWeight=FontWeights.SemiBold }; title.SetResourceReference(TextBlock.ForegroundProperty, assistant ? "Accent" : "Success"); DockPanel.SetDock(title,Dock.Left); header.Children.Add(title); var copy=new Button { Content="Копировать", Padding=new Thickness(7,2,7,2), FontSize=10, ToolTip="Скопировать текст сообщения" }; copy.Click += (_,_) => Clipboard.SetText(markdown); DockPanel.SetDock(copy,Dock.Right); header.Children.Add(copy); p.Children.Add(header); p.Children.Add(MarkdownRenderer.Render(markdown)); box.Child=p; ChatPanel.Children.Add(box); ChatScroll.ScrollToEnd(); }
    void AgentTab_Click(object s,RoutedEventArgs e)=>ShowAgent();
    void DiffTab_Click(object s,RoutedEventArgs e) { InspectorPanel.Children.Clear(); AddInspector("PRODUCTCARD.TSX",true); AddInspector("@@ -1,4 +1,5 @@ Импорты\n- import { ShoppingCart } from 'lucide-react';\n+ import { ShoppingCart, Flame } from 'lucide-react';\n+ import { motion } from 'framer-motion';\n\n@@ -14,6 +16,12 @@ Кнопка\n- <button className=\"mt-3 bg-violet-600\">\n+ <motion.button whileHover={{ scale: 1.04 }}\n+   whileTap={{ scale: 0.96 }}>\n+   В корзину\n+ </motion.button>",false); var b=new Button{Content="✓ Принять все изменения",Style=(Style)FindResource("AccentButton"),Margin=new Thickness(0,12,0,0)};InspectorPanel.Children.Add(b); }
    void PreviewTab_Click(object s,RoutedEventArgs e) { InspectorPanel.Children.Clear(); AddInspector("ЖИВОЙ ПРЕДПРОСМОТР · SANDBOX",true); var toolbar=new StackPanel{Orientation=Orientation.Horizontal,Margin=new Thickness(0,0,0,10)}; var url=new TextBox{Text="http://localhost:5173",Width=220}; toolbar.Children.Add(url); var open=new Button{Content="↗ Открыть",Margin=new Thickness(6,0,0,0)}; open.Click+=(a,b)=>{try{Process.Start(new ProcessStartInfo(url.Text){UseShellExecute=true});}catch{}};toolbar.Children.Add(open);InspectorPanel.Children.Add(toolbar); var canvas=new Border{Height=430,Background=new SolidColorBrush(Color.FromRgb(7,10,15)),CornerRadius=new CornerRadius(8),Padding=new Thickness(35),Child=new Border{Width=270,Height=330,HorizontalAlignment=HorizontalAlignment.Center,VerticalAlignment=VerticalAlignment.Center,Background=new SolidColorBrush(Color.FromRgb(15,23,42)),CornerRadius=new CornerRadius(15),Padding=new Thickness(18),Child=new StackPanel{Children={new TextBlock{Text="🔥 ХИТ −15%",Foreground=Brushes.White,Background=Brushes.Crimson,Padding=new Thickness(7,3,7,3),HorizontalAlignment=HorizontalAlignment.Left},new TextBlock{Text="🎧",FontSize=64,HorizontalAlignment=HorizontalAlignment.Center,Margin=new Thickness(0,18,0,0)},new TextBlock{Text="Наушники Studio Pro",Foreground=Brushes.White,FontWeight=FontWeights.SemiBold},new TextBlock{Text="● В наличии (14 шт.)",Foreground=(Brush)FindResource("Success"),Margin=new Thickness(0,8,0,0)},new TextBlock{Text="12 990 ₽",Foreground=Brushes.DeepSkyBlue,FontSize=19,Margin=new Thickness(0,14,0,8)},new Button{Content="🛒 В корзину",Style=(Style)FindResource("AccentButton")}}}}}; InspectorPanel.Children.Add(canvas); AddInspector("КОНСОЛЬ ПРЕДПРОСМОТРА\n● 0 ошибок\n[Vite:HMR] Компонент смонтирован",false); }
    void ShowAgent() { InspectorPanel.Children.Clear(); AddInspector("ДЕЙСТВИЯ АГЕНТА",true); AddInspector("[READY] Агент готов к выполнению",false); }
    void AddToolEvent(string name,string detail) { AddInspector($"🛠 {name}\n{detail}",false); InspectorPanel.UpdateLayout(); }
    void AddInspector(string text,bool title) => InspectorPanel.Children.Add(new TextBlock { Text=text, TextWrapping=TextWrapping.Wrap, Foreground=(Brush)FindResource(title?"Text":"Body"), FontFamily=title?new FontFamily("Segoe UI"):new FontFamily("Consolas"), FontWeight=title?FontWeights.SemiBold:FontWeights.Normal, Margin=new Thickness(0,0,0,12), LineHeight=19 });
    void QuickTheme_Click(object s,RoutedEventArgs e)=>SetTheme(settings.Theme=="Светлая"?"Graphite (по умолчанию)":"Светлая");
    void Settings_Click(object s,RoutedEventArgs e)=>new SettingsWindow(this).ShowDialog();
    void Prompts_Click(object s,RoutedEventArgs e) { var w=new PromptsWindow(this); w.ShowDialog(); PromptBox.ItemsSource=null; PromptBox.ItemsSource=settings.Prompts; PromptBox.DisplayMemberPath="Name"; PromptBox.SelectedIndex=0; }
    public void SetTheme(string theme) { settings.Theme=theme; SettingsStore.Save(settings); var colors=theme=="Светлая" ? new[]{("AppBg","#F6F8FA"),("PanelBg","#FFFFFF"),("SubBg","#F0F2F5"),("CardBg","#FFFFFF"),("Border","#D0D7DE"),("Text","#1F2328"),("Body","#3B424A"),("Muted","#6E7781"),("Accent","#6E40C9"),("Success","#1A7F37")} : theme=="Midnight Blue" ? new[]{("AppBg","#0A1020"),("PanelBg","#111B31"),("SubBg","#070C18"),("CardBg","#1A2945"),("Border","#2C4267"),("Text","#E8F1FF"),("Body","#C2D3EE"),("Muted","#8297B8"),("Accent","#2563EB"),("Success","#34D399")} : theme=="Emerald Night" ? new[]{("AppBg","#0A1412"),("PanelBg","#11211E"),("SubBg","#07100E"),("CardBg","#1A302A"),("Border","#315046"),("Text","#ECFDF5"),("Body","#C6E4D9"),("Muted","#8FAC9F"),("Accent","#059669"),("Success","#6EE7B7")} : new[]{("AppBg","#0D1117"),("PanelBg","#161B22"),("SubBg","#090D13"),("CardBg","#21262D"),("Border","#30363D"),("Text","#F0F6FC"),("Body","#C9D1D9"),("Muted","#8B949E"),("Accent","#7C3AED"),("Success","#3FB950")}; foreach(var (key,hex) in colors) Resources[key]=new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex)); }
}
public static class Markdown { public static FrameworkElement Render(string md) { var panel=new StackPanel(); bool code=false; TextBlock? codeBlock=null; foreach(var raw in md.Replace("\r","").Split('\n')) { if(raw.StartsWith("```")){code=!code;if(code){codeBlock=new TextBlock{Foreground=new SolidColorBrush(Color.FromRgb(165,214,255)),FontFamily=new FontFamily("Consolas"),TextWrapping=TextWrapping.Wrap,Background=new SolidColorBrush(Color.FromRgb(10,14,20)),Padding=new Thickness(10),Margin=new Thickness(0,5,0,5)};panel.Children.Add(codeBlock);}continue;} if(code){codeBlock!.Text+=raw+"\n";continue;} var t=new TextBlock{TextWrapping=TextWrapping.Wrap,Margin=new Thickness(0,2,0,2),Foreground=(Brush)Application.Current.Resources["Body"]}; if(raw.StartsWith("## ")){t.Text=raw[3..];t.FontSize=15;t.FontWeight=FontWeights.SemiBold;t.Foreground=(Brush)Application.Current.Resources["Text"];} else if(raw.StartsWith("- ")) t.Text="• "+raw[2..]; else { var pieces=raw.Split("**"); for(int i=0;i<pieces.Length;i++) t.Inlines.Add(new Run(pieces[i]){FontWeight=i%2==1?FontWeights.Bold:FontWeights.Normal}); } panel.Children.Add(t); } return panel; } }
