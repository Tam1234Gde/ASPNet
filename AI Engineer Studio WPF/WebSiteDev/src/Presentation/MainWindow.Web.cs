using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AIEngineerStudio.Services;

namespace AIEngineerStudio;
public partial class MainWindow
{
    readonly WebProcess webServer = new();
    readonly WebProcess webCommand = new();
    CancellationTokenSource? webOperation;
    CancellationTokenSource? developmentCancellation;
    Window? webWindow;
    void InitializeWeb()
    {
        Closed += (_, _) => { webOperation?.Cancel(); developmentCancellation?.Cancel(); webServer.Dispose(); webCommand.Dispose(); webWindow?.Close(); };
    }
    void WebWorkspace_Click(object sender, RoutedEventArgs args)
    {
        if (activeProject is null) return;
        if (webWindow is not null) { webWindow.Activate(); return; }
        var project = activeProject;
        var config = project.Web;
        var panel = new StackPanel { Margin = new Thickness(18) };
        var window = new Window { Owner = this, Title = "WebSiteDev — рабочий цикл", Width = 850, Height = 850, MinWidth = 650, MinHeight = 500, Content = new ScrollViewer { Content = panel, VerticalScrollBarVisibility = ScrollBarVisibility.Auto } };
        window.SetResourceReference(BackgroundProperty, "PanelBg"); window.SetResourceReference(ForegroundProperty, "Text");
        webWindow = window;
        TextBox Field(string label, string value, bool multiline = false)
        {
            panel.Children.Add(new TextBlock { Text = label, Margin = new Thickness(0,8,0,3), TextWrapping = TextWrapping.Wrap });
            var box = new TextBox { Text = value, TextWrapping = TextWrapping.Wrap, AcceptsReturn = multiline, MinHeight = multiline ? 65 : 28 };
            panel.Children.Add(box); return box;
        }
        var requirements = Field("Подтверждённые требования текущей задачи", config.Requirements, true);
        var criteria = Field("Критерии готовности (сборка и HTTP не заменяют проверку сценариев)", config.AcceptanceCriteria, true);
        panel.Children.Add(new TextBlock { Text = "Пресеты команд: статический HTML (Python), npm (Vite/React/Vue), ASP.NET Core. Наличие среды и работоспособность конкретного проекта проверяются отдельно.", TextWrapping = TextWrapping.Wrap });
        var stack = new ComboBox { ItemsSource = new[] { "HTML/CSS/JS + Python", "npm (Vite/React/Vue)", "ASP.NET Core", "Пользовательский стек" }, SelectedItem = config.ConfirmedStack };
        panel.Children.Add(stack);
        var install = Field("Установка зависимостей — явный запуск", config.InstallCommand);
        var build = Field("Сборка/проверка — конечная команда", config.BuildCommand);
        var start = Field("Команда сервера — управляемый фоновый процесс", config.StartCommand);
        var url = Field("Локальный HTTP URL (порт должен быть свободен)", config.Url);
        var seconds = Field("Таймаут конечной команды, секунд (1–3600)", config.CommandTimeoutSeconds.ToString());
        var startup = Field("Таймаут готовности сервера, секунд (1–3600)", config.StartupTimeoutSeconds.ToString());
        var external = new CheckBox { Content = "Открывать во внешнем браузере (рекомендуется для современных сайтов)", IsChecked = config.ExternalPreview, Margin = new Thickness(0,10,0,10) }; panel.Children.Add(external);
        var actions = new WrapPanel(); panel.Children.Add(actions);
        var status = new TextBlock { TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0,10,0,10) }; panel.Children.Add(status);
        var log = new TextBox { IsReadOnly = true, AcceptsReturn = true, TextWrapping = TextWrapping.Wrap, Height = 180, VerticalScrollBarVisibility = ScrollBarVisibility.Auto }; panel.Children.Add(log);
        void Add(string label, Action action) { var button = new Button { Content = label, Margin = new Thickness(3) }; button.Click += (_, _) => { try { action(); } catch (Exception ex) { status.Text = ex.Message; } }; actions.Children.Add(button); }
        void Save()
        {
            if (!int.TryParse(seconds.Text, out int limit) || limit < 1 || limit > 3600 || !int.TryParse(startup.Text, out int wait) || wait < 1 || wait > 3600) throw new InvalidOperationException("Некорректный таймаут.");
            if (stack.SelectedItem is not string selected) throw new InvalidOperationException("Выберите стек.");
            config.ConfirmedStack = selected; config.Requirements = requirements.Text; config.AcceptanceCriteria = criteria.Text;
            config.InstallCommand = install.Text; config.BuildCommand = build.Text; config.StartCommand = start.Text;
            config.Url = url.Text; config.ExternalPreview = external.IsChecked == true; config.CommandTimeoutSeconds = limit; config.StartupTimeoutSeconds = wait;
            SaveProject(); status.Text = "Стек и требования подтверждены пользователем.";
        }
        async void Run(bool server, string command)
        {
            if (webOperation != null || isWaiting) { status.Text = "Дождитесь текущей операции."; return; }
            if (string.IsNullOrEmpty(config.ConfirmedStack)) { status.Text = "Сначала подтвердите стек и команды."; return; }
            webOperation = new CancellationTokenSource();
            try { await (server ? webServer : webCommand).RunAsync(project.WorkingDirectory, command, server ? config.StartupTimeoutSeconds : config.CommandTimeoutSeconds, server ? new Uri(config.Url) : null, webOperation.Token); }
            catch (Exception ex) { status.Text = ex.Message; }
            finally { webOperation.Dispose(); webOperation = null; }
        }
        Add("Подставить пресет", () => {
            switch (stack.SelectedIndex) {
                case 0: install.Text = ""; build.Text = ""; start.Text = "python -m http.server 5173 --bind 127.0.0.1"; break;
                case 1: install.Text = "npm ci"; build.Text = "npm run build"; start.Text = "npm run dev -- --host 127.0.0.1 --port 5173 --strictPort"; break;
                case 2: install.Text = "dotnet restore"; build.Text = "dotnet build --no-restore"; start.Text = "dotnet run --no-launch-profile --urls http://127.0.0.1:5173"; break;
            }
            status.Text = "Проверьте команды и подтвердите. Пресет не устанавливает стек.";
        });
        Add("Подтвердить и сохранить", Save);
        Add("Зависимости", () => Run(false, config.InstallCommand));
        Add("Сборка", () => Run(false, config.BuildCommand));
        Add("Запустить", () => Run(true, config.StartCommand));
        Add("Остановить сервер", () => { webOperation?.Cancel(); webServer.Stop(); });
        Add("Перезапустить", () => { if (webOperation != null) return; webServer.Stop(); Run(true, config.StartCommand); });
        Add("Отменить команду", () => { webOperation?.Cancel(); webCommand.Stop(); });
        Add("Отменить генерацию", () => developmentCancellation?.Cancel());
        Add("Предпросмотр", () => {
            if (!webServer.Running) throw new InvalidOperationException("Сначала запустите сервер.");
            if (config.ExternalPreview) Process.Start(new ProcessStartInfo(config.Url) { UseShellExecute = true });
            else {
                var browser = new WebBrowser();
                var preview = new Window { Owner = window, Title = "Предпросмотр — legacy WebBrowser, современные JS API могут не поддерживаться", Width = 950, Height = 700, Content = browser };
                preview.Show(); browser.Navigate(config.Url);
            }
        });
        Add("Публикация SSH…", () => RemoteProject_Click(sender, args));
        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        timer.Tick += (_, _) => { var text = webServer.Log + "\n" + webCommand.Log; if (log.Text != text) log.Text = text; window.Title = "WebSiteDev | " + webServer.Status + " | " + webCommand.Status; };
        timer.Start();
        window.Closed += (_, _) => { timer.Stop(); webWindow = null; };
        window.Show();
    }
    string WebTaskContext() => activeProject is null ? "" : $"\nWebSiteDev. Подтверждённый стек: {activeProject.Web.ConfirmedStack}\nТребования пользователя: {activeProject.Web.Requirements}\nКритерии: {activeProject.Web.AcceptanceCriteria}\nНе меняй стек без подтверждения. Не запускай dev-сервер через execute_command: пользователь запускает его через панель Web. Не публикуй автоматически. Не выполняй миграции без согласования. В финале сопоставь каждый критерий: выполнено/частично/не выполнено/не проверено. HTTP и сборка не доказывают работоспособность пользовательских сценариев.";
}
