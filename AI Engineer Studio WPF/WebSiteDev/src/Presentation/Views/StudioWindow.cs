using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;

namespace AIEngineerStudio;

public class StudioWindow : Window
{
    protected readonly Grid Root = new() { Margin = new Thickness(24) };
    protected readonly StackPanel Footer = new() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
    public StudioWindow(MainWindow owner, string title, string subtitle)
    {
        Owner = owner; Title = title; Width = 1040; Height = 760; MinWidth = 760; MinHeight = 540; WindowStartupLocation = WindowStartupLocation.CenterOwner;
        foreach (var key in new[] { "AppBg", "PanelBg", "SubBg", "CardBg", "Border", "Text", "Body", "Muted", "Accent", "Success" }) Resources[key] = owner.FindResource(key);
        SetResourceReference(BackgroundProperty, "AppBg"); SetResourceReference(ForegroundProperty, "Text");
        Resources["ChoiceCard"] = (Style)XamlReader.Parse("""
<Style xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" TargetType="ListBoxItem"><Setter Property="HorizontalContentAlignment" Value="Stretch"/><Setter Property="Margin" Value="0,0,0,8"/><Setter Property="Template"><Setter.Value><ControlTemplate TargetType="ListBoxItem"><Border x:Name="B" CornerRadius="10" Padding="14" Background="{DynamicResource CardBg}" BorderBrush="{DynamicResource Border}" BorderThickness="1"><ContentPresenter/></Border><ControlTemplate.Triggers><Trigger Property="IsSelected" Value="True"><Setter TargetName="B" Property="BorderBrush" Value="{DynamicResource Accent}"/><Setter TargetName="B" Property="BorderThickness" Value="2"/></Trigger><Trigger Property="IsMouseOver" Value="True"><Setter TargetName="B" Property="BorderBrush" Value="{DynamicResource Accent}"/></Trigger></ControlTemplate.Triggers></ControlTemplate></Setter.Value></Setter></Style>
""");
        Root.RowDefinitions.Add(new() { Height = GridLength.Auto }); Root.RowDefinitions.Add(new()); Root.RowDefinitions.Add(new() { Height = GridLength.Auto });
        var header = new StackPanel(); header.Children.Add(Text(title, 26)); header.Children.Add(Text(subtitle, 12, "Muted")); header.Margin = new Thickness(0, 0, 0, 20); Root.Children.Add(header);
        Grid.SetRow(Footer, 2); Footer.Margin = new Thickness(0, 16, 0, 0); Root.Children.Add(Footer); Content = Root;
    }
    public static TextBlock Text(string text, double size = 14, string color = "Text") { var t = new TextBlock { Text = text, FontSize = size, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 6) }; t.SetResourceReference(TextBlock.ForegroundProperty, color); return t; }
    protected Button Action(string label, Action action, bool accent = false) { var b = new Button { Content = label, Margin = new Thickness(6, 0, 0, 0), Padding = new Thickness(16, 9, 16, 9) }; if (accent) b.SetResourceReference(StyleProperty, "AccentButton"); b.Click += (_, _) => action(); return b; }
    protected Border Card(string title, string description, UIElement editor)
    {
        var p = new StackPanel(); p.Children.Add(Text(title, 16)); p.Children.Add(Text(description, 12, "Muted")); p.Children.Add(editor);
        var card = new Border { Child = p, CornerRadius = new CornerRadius(12), BorderThickness = new Thickness(1), Padding = new Thickness(18), Margin = new Thickness(0, 0, 0, 12) }; card.SetResourceReference(BackgroundProperty, "PanelBg"); card.SetResourceReference(Border.BorderBrushProperty, "Border"); return card;
    }
    protected ListBox Choices() { var list = new ListBox { Background = Brushes.Transparent, BorderThickness = new Thickness(0), ItemContainerStyle = (Style)Resources["ChoiceCard"] }; list.SetResourceReference(ForegroundProperty, "Text"); ScrollViewer.SetHorizontalScrollBarVisibility(list, ScrollBarVisibility.Disabled); return list; }
}
