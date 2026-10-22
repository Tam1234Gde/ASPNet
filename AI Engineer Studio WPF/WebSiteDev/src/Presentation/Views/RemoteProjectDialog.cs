using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using AIEngineerStudio.Models;

namespace AIEngineerStudio.Views;

public sealed class RemoteProjectDialog : Window
{
    readonly TextBox name = Box(), host = Box(), port = Box(), user = Box(), path = Box(), key = Box(), fingerprint = Box(), deploy = Box(), excludes = Box();
    readonly PasswordBox password = new() { Padding = new Thickness(8), Margin = new Thickness(0, 2, 0, 7) };
    readonly ComboBox os = new() { ItemsSource = Enum.GetValues<RemoteOperatingSystem>(), Margin = new Thickness(0, 2, 0, 7) };
    readonly ComboBox auth = new() { ItemsSource = Enum.GetValues<RemoteAuthentication>(), Margin = new Thickness(0, 2, 0, 7) };
    readonly ComboBox manager = new() { ItemsSource = Enum.GetValues<WindowsAppManager>(), Margin = new Thickness(0, 2, 0, 7) };
    readonly TextBox application = Box(), statePath = Box(), database = Box();
    readonly CheckBox databaseReviewed = new() { Content = "Проверены все пути и файлы базы данных (исключены из обновления и копий)" };
    readonly CheckBox auto = new() { Content = "Автоматически отправлять изменения", Foreground = Brushes.White };
    public RemoteProject? Result { get; private set; }
    public bool UpdateRequested { get; private set; }
    static TextBox Box() => new() { Padding = new Thickness(8), Margin = new Thickness(0, 2, 0, 7) };
    readonly RemoteProject? edit;
    public RemoteProjectDialog(RemoteProject? edit = null)
    {
        this.edit = edit;
        Title = edit is null ? "Добавить удалённый проект" : "Настройки удалённого проекта"; Width = 650; Height = 780; WindowStartupLocation = WindowStartupLocation.CenterOwner; Background = (Brush)Application.Current.Resources["PanelBg"];
        var root = new StackPanel { Margin = new Thickness(24) }; Content = new ScrollViewer { Content = root, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
        root.Children.Add(new TextBlock { Text = Title, FontSize = 22, Foreground = (Brush)Application.Current.Resources["Text"], Margin = new Thickness(0, 0, 0, 14) });
        Add(root, "Название", name); Add(root, "ОС сервера", os); Add(root, "SSH host", host); Add(root, "Порт", port); Add(root, "Пользователь", user); Add(root, "Удалённый путь (Windows: C:/Sites/my-site)", path); Add(root, "Аутентификация", auth);
        Add(root, "Приватный ключ", key); var browse = new Button { Content = "Выбрать ключ", Padding = new Thickness(8) }; browse.Click += (_, _) => { var d = new OpenFileDialog { Filter = "SSH key|*|Все файлы|*.*" }; if (d.ShowDialog() == true) key.Text = d.FileName; }; root.Children.Add(browse);
        Add(root, "Пароль SSH (если выбран Password)", password); Add(root, "SHA-256 fingerprint хоста (не проверяется)", fingerprint); Add(root, "Механизм управления Windows", manager); Add(root, "Имя службы / IIS Application Pool", application); Add(root, "Папка состояния и резервных копий (пусто — внутри проекта)", statePath); Add(root, "Команда деплоя / PowerShell (старый режим)", deploy); Add(root, "Исключения через ;", excludes);
        root.Children.Add(new TextBlock { Text = "Внимание: пароль хранится открыто; ключ SSH-сервера не проверяется. Секреты проекта загружаются на сервер. Обновление запускается только вручную.", TextWrapping = TextWrapping.Wrap, Foreground = Brushes.Orange });
        Add(root, "Исключения базы данных через ;", database); database.Text = edit?.DatabaseExclusions ?? new RemoteProject().DatabaseExclusions;
         databaseReviewed.IsChecked = edit?.DatabaseExclusionsReviewed ?? false; root.Children.Add(databaseReviewed);
         manager.SelectedItem = edit?.AppManager ?? WindowsAppManager.NotConfigured; application.Text = edit?.ApplicationName ?? ""; statePath.Text = edit?.StatePath ?? "";
        var actions = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 18, 0, 0) };
         var save = new Button { Content = "Сохранить", Padding = new Thickness(12), Margin = new Thickness(0, 0, 8, 0) }; save.Click += (_, _) => { UpdateRequested = false; Save(); }; actions.Children.Add(save);
         var update = new Button { Content = "Обновить сейчас", Padding = new Thickness(12) }; update.Click += (_, _) => { UpdateRequested = true; Save(); }; actions.Children.Add(update); root.Children.Add(actions);
        os.SelectedItem = edit?.OperatingSystem ?? RemoteOperatingSystem.Windows; auth.SelectedItem = edit?.Authentication ?? RemoteAuthentication.PrivateKey;
        if (edit is not null) { name.Text=edit.Name; host.Text=edit.Host; port.Text=edit.Port.ToString(); user.Text=edit.User; path.Text=edit.RemotePath; key.Text=edit.PrivateKeyPath; password.Password=edit.Password; fingerprint.Text=edit.HostFingerprint; deploy.Text=edit.DeployCommand; excludes.Text=edit.ExcludePatterns; auto.IsChecked=edit.AutoSync; }
        else { port.Text = "22"; excludes.Text = new RemoteProject().ExcludePatterns; }
    }
    void Save()
    {
        if (string.IsNullOrWhiteSpace(host.Text) || string.IsNullOrWhiteSpace(user.Text) || string.IsNullOrWhiteSpace(path.Text) || !int.TryParse(port.Text, out var p) || p is < 1 or > 65535) { MessageBox.Show("Заполните host, корректный порт, пользователя и путь."); return; }
        var selectedAuth = (RemoteAuthentication)(auth.SelectedItem ?? RemoteAuthentication.PrivateKey);
        if (selectedAuth == RemoteAuthentication.PrivateKey && string.IsNullOrWhiteSpace(key.Text)) { MessageBox.Show("Укажите приватный SSH-ключ."); return; }
        if (selectedAuth == RemoteAuthentication.Password && string.IsNullOrEmpty(password.Password)) { MessageBox.Show("Укажите пароль SSH."); return; }
        var enteredFingerprint = fingerprint.Text.Trim();
        if (enteredFingerprint.StartsWith("SHA256:", StringComparison.OrdinalIgnoreCase)) enteredFingerprint = enteredFingerprint[7..];
        enteredFingerprint = enteredFingerprint.Replace(":", "").Replace(" ", "");
        if (enteredFingerprint.Length > 0 && !Regex.IsMatch(enteredFingerprint, "^[a-fA-F0-9]{64}$")) { MessageBox.Show("Fingerprint должен содержать 64 hex-символа. Можно вставить SHA256:... или только значение.", "SSH", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
        Result = new RemoteProject { AppManager=(WindowsAppManager)(manager.SelectedItem ?? WindowsAppManager.NotConfigured), ApplicationName=application.Text.Trim(), StatePath=statePath.Text.Trim(), Name=string.IsNullOrWhiteSpace(name.Text) ? host.Text.Trim() : name.Text.Trim(), OperatingSystem=(RemoteOperatingSystem)(os.SelectedItem ?? RemoteOperatingSystem.Windows), Authentication=selectedAuth, Host=host.Text.Trim(), Port=p, User=user.Text.Trim(), RemotePath=path.Text.Trim(), PrivateKeyPath=key.Text.Trim(), Password=password.Password, HostFingerprint=enteredFingerprint.Length == 0 ? "" : "SHA256:" + enteredFingerprint.ToLowerInvariant(), DeployCommand=deploy.Text.Trim(), ExcludePatterns=excludes.Text.Trim(), DatabaseExclusions=database.Text.Trim(), DatabaseExclusionsReviewed=databaseReviewed.IsChecked == true, ConnectionTimeoutSeconds=edit?.ConnectionTimeoutSeconds ?? 30, OperationTimeoutSeconds=edit?.OperationTimeoutSeconds ?? 120, AutoSync=false };
        DialogResult = true;
    }
    static void Add(Panel root, string label, Control control) { root.Children.Add(new TextBlock { Text=label, Foreground=(Brush)Application.Current.Resources["Muted"] }); root.Children.Add(control); }
}
