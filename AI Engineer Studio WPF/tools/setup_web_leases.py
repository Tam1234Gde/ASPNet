from pathlib import Path
for prefix in ['', 'WebSiteDev/']:
    p = Path(prefix + 'src/Presentation/MainWindow.xaml.cs')
    s = p.read_text(encoding='utf-8-sig')
    s = s.replace('ProjectDocument? activeProject;', '''ProjectDocument? activeProject;
    WorkspaceLease? workspaceLease;
    string leasedPath = "";
    ProjectDocument LeaseProject(ProjectDocument project)
    {
        string path = Path.GetFullPath(project.WorkingDirectory).TrimEnd(Path.DirectorySeparatorChar);
        if (!string.Equals(path, leasedPath, StringComparison.OrdinalIgnoreCase))
        {
            var next = WorkspaceLease.Acquire(path);
            workspaceLease?.Dispose(); workspaceLease = next; leasedPath = path;
        }
        return project;
    }''')
    s = s.replace('Closed += (_,_) => { tokenBalanceTimer.Stop();', 'Closed += (_,_) => { workspaceLease?.Dispose(); tokenBalanceTimer.Stop();')
    s = s.replace('activeProject=current;', 'activeProject=LeaseProject(current);')
    s = s.replace('activeProject=await Task.Run(() => projectStore.Load(dialog.SelectedProject))', 'activeProject=LeaseProject(await Task.Run(() => projectStore.Load(dialog.SelectedProject)))')
    s = s.replace('activeProject=projectStore.Create(dialog.NewName!,dialog.DirectoryPath!,dialog.Description??"",dialog.Stack??"Другой")', 'activeProject=LeaseProject(projectStore.Create(dialog.NewName!,dialog.DirectoryPath!,dialog.Description??"",dialog.Stack??"Другой"))')
    if prefix:
        s = s.replace('async void SelectProject() {', 'async void SelectProject() { if (webOperation != null || webServer.Running || webWindow != null) { MessageBox.Show(this, "Остановите сервер и закройте панель Web перед сменой проекта."); return; }')
    p.write_text(s, encoding='utf-8')
    print(prefix, s.count('LeaseProject'))
