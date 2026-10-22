using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfKeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace AISoftCraftDuck;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        if (DataContext is ViewModels.MainViewModel vm) Attach(vm);
        DataContextChanged += (_, e) => { if (e.NewValue is ViewModels.MainViewModel model) Attach(model); };
    }

    private void Attach(ViewModels.MainViewModel vm)
    {
        vm.Messages.CollectionChanged += MessagesChanged;
        foreach (var message in vm.Messages) message.PropertyChanged += MessageChanged;
    }

    private void MessagesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (ViewModels.ChatItem item in e.NewItems) item.PropertyChanged += MessageChanged;
        ScrollToBottom();
    }

    private void MessageChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModels.ChatItem.Content)) ScrollToBottom();
    }

    private void ScrollToBottom() => Dispatcher.InvokeAsync(
        () => MessagesScroll.ScrollToEnd(), System.Windows.Threading.DispatcherPriority.Background);

    private void PromptBox_PreviewKeyDown(object sender, WpfKeyEventArgs e)
    {
        if (e.Key != Key.Enter || Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) return;
        if (DataContext is ViewModels.MainViewModel vm && vm.SendCommand.CanExecute(null))
            vm.SendCommand.Execute(null);
        e.Handled = true;
    }

    private void SettingsOverlay_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource == sender && DataContext is ViewModels.MainViewModel vm)
            vm.ToggleSettingsCommand.Execute(null);
    }

    private void MessagesScroll_ScrollChanged(object sender, ScrollChangedEventArgs e) { }
    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { if (e.ClickCount == 2) Maximize_Click(sender, e); else DragMove(); }
    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    private void Maximize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}
