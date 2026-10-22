from pathlib import Path
p=Path(r'src\Presentation\MainWindow.xaml.cs')
s=p.read_text(encoding='utf-8')
a=s.index(' void DiffTab_Click')
b=s.index(' void ShowAgent()',a)
r='''     void DiffTab_Click(object s,RoutedEventArgs e) {
         inspectorTab="diff"; InspectorPanel.Children.Clear(); AddInspector("ПРОСМОТР DIFF",true);
         ComboBox fileBox=new ComboBox { MinWidth=260, Margin=new Thickness(0,0,0,10) };
         List<string> files=Directory.EnumerateFiles(projectPath,"*",SearchOption.AllDirectories).Where(path=>!path.Contains("\\bin\\")&&!path.Contains("\\obj\\")&&!path.Contains("\\node_modules\\")).Take(200).Select(path=>Path.GetRelativePath(projectPath,path)).ToList();
         foreach(string path in files) fileBox.Items.Add(path);
         TextBox diffText=new TextBox { IsReadOnly=true, AcceptsReturn=true, TextWrapping=TextWrapping.NoWrap, HorizontalScrollBarVisibility=ScrollBarVisibility.Auto, VerticalScrollBarVisibility=ScrollBarVisibility.Auto, FontFamily=new FontFamily("Consolas"), FontSize=11, MinHeight=420, Padding=new Thickness(10) };
         diffText.SetResourceReference(BackgroundProperty,"AppBg"); diffText.SetResourceReference(ForegroundProperty,"Body");
         fileBox.SelectionChanged += (_,_) => LoadDiff(fileBox.SelectedItem?.ToString(),diffText);
         InspectorPanel.Children.Add(fileBox); InspectorPanel.Children.Add(diffText);
         Button refresh=new Button { Content="↻ Обновить diff", Margin=new Thickness(0,10,0,0) }; refresh.Click += (_,_) => LoadDiff(fileBox.SelectedItem?.ToString(),diffText); InspectorPanel.Children.Add(refresh);
         if(fileBox.Items.Count>0) fileBox.SelectedIndex=0; else diffText.Text="Файлы проекта не найдены.";
     }
     void LoadDiff(string? relativePath,TextBox target) {
         if(string.IsNullOrWhiteSpace(relativePath)){target.Text="Выберите файл.";return;}
         string fullPath=Path.Combine(projectPath,relativePath);
         try { string[] current=File.Exists(fullPath)?File.ReadAllLines(fullPath):Array.Empty<string>(); string[] baseline=GitVersionService.ReadFileFromHead(projectPath,relativePath); target.Text=BuildUnifiedDiff(relativePath,baseline,current); } catch(Exception ex){target.Text=$"Не удалось построить diff: {ex.Message}";}
     }
     static string BuildUnifiedDiff(string path,string[] before,string[] after) {
         List<string> lines=new List<string>{$"--- a/{path}",$"+++ b/{path}",$"@@ -1,{before.Length} +1,{after.Length} @@"}; int max=Math.Max(before.Length,after.Length);
         for(int index=0;index<max;index++) { if(index<before.Length&&index<after.Length&&before[index]==after[index]) lines.Add("  "+before[index]); else { if(index<before.Length) lines.Add("- "+before[index]); if(index<after.Length) lines.Add("+ "+after[index]); } }
         return string.Join(Environment.NewLine,lines);
     }
     void PreviewTab_Click(object s,RoutedEventArgs e) {
         inspectorTab="preview"; InspectorPanel.Children.Clear(); AddInspector("ЖИВОЙ ПРЕДПРОСМОТР",true);
         StackPanel toolbar=new StackPanel{Orientation=Orientation.Horizontal,Margin=new Thickness(0,0,0,8)}; TextBox url=new TextBox{Text="http://localhost:5173",Width=220}; toolbar.Children.Add(url);
         Button open=new Button{Content="↗ Открыть",Margin=new Thickness(6,0,0,0)}; open.Click+=(_,_)=>{try{Process.Start(new ProcessStartInfo(url.Text){UseShellExecute=true});}catch{}}; toolbar.Children.Add(open); Button reload=new Button{Content="↻ Обновить",Margin=new Thickness(6,0,0,0)}; reload.Click+=(_,_)=>livePreviewBrowser?.Refresh(); toolbar.Children.Add(reload); InspectorPanel.Children.Add(toolbar);
         previewStatus=new TextBlock{Text="Подключение к предпросмотру…",FontSize=10,Margin=new Thickness(0,0,0,8)}; previewStatus.SetResourceReference(TextBlock.ForegroundProperty,"Muted"); InspectorPanel.Children.Add(previewStatus);
         livePreviewBrowser=new WebBrowser{Height=500,MinWidth=320}; livePreviewBrowser.Navigated+=(_,_)=>previewStatus.Text="● Предпросмотр обновлён"; InspectorPanel.Children.Add(livePreviewBrowser); StartPreviewWatcher(); livePreviewBrowser.Navigate(url.Text);
     }
     void StartPreviewWatcher() { previewWatcher?.Dispose(); if(!Directory.Exists(projectPath))return; previewWatcher=new FileSystemWatcher(projectPath){IncludeSubdirectories=true,EnableRaisingEvents=true,NotifyFilter=NotifyFilters.LastWrite|NotifyFilters.FileName}; FileSystemEventHandler refresh=(_,_)=>Dispatcher.BeginInvoke(new Action(()=>{if(inspectorTab=="preview") livePreviewBrowser?.Refresh();})); previewWatcher.Changed+=refresh; previewWatcher.Created+=refresh; previewWatcher.Renamed+=(_,_)=>Dispatcher.BeginInvoke(new Action(()=>{if(inspectorTab=="preview") livePreviewBrowser?.Refresh();})); }
'''
p.write_text(s[:a]+r+s[b:],encoding='utf-8')
