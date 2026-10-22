using System.Windows;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Highlighting;
using WpfClipboard = System.Windows.Clipboard;

namespace AISoftCraftDuck.Controls;

public partial class CodeBlockControl : System.Windows.Controls.UserControl
{
    public static readonly DependencyProperty CodeProperty = DependencyProperty.Register(nameof(Code), typeof(string), typeof(CodeBlockControl), new PropertyMetadata("", OnCodeChanged));
    public new static readonly DependencyProperty LanguageProperty = DependencyProperty.Register(nameof(Language), typeof(string), typeof(CodeBlockControl), new PropertyMetadata("", OnLanguageChanged));

    public string Code { get => (string)GetValue(CodeProperty); set => SetValue(CodeProperty, value); }
    public new string Language { get => (string)GetValue(LanguageProperty); set => SetValue(LanguageProperty, value); }
    public ICommand CopyCommand => new CommunityToolkit.Mvvm.Input.RelayCommand(() => WpfClipboard.SetText(Code ?? string.Empty));

    public CodeBlockControl()
    {
        InitializeComponent();
        Editor.Text = Code ?? string.Empty;
        ApplyHighlighting();
    }

    private static void OnCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (CodeBlockControl)d;
        if (control.Editor is not null) control.Editor.Text = e.NewValue as string ?? string.Empty;
    }
    private static void OnLanguageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((CodeBlockControl)d).ApplyHighlighting();
    private void ApplyHighlighting()
    {
        if (Editor is null) return;
        var language = Language.Trim().ToLowerInvariant() switch { "cs" or "csharp" => ".cs", "js" or "javascript" => ".js", "ts" or "typescript" => ".ts", "py" or "python" => ".py", "xml" or "xaml" => ".xml", "json" => ".json", "html" => ".html", "css" => ".css", "sql" => ".sql", "bash" or "sh" => ".sh", _ => "." + Language.Trim().TrimStart('.') };
        Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(language) ?? HighlightingManager.Instance.GetDefinition(Language);
    }
}
