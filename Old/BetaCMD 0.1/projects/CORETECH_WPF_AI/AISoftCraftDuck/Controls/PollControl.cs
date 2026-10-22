using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using WpfColor = System.Windows.Media.Color;
using WpfBrushes = System.Windows.Media.Brushes;
using WpfSolidColorBrush = System.Windows.Media.SolidColorBrush;

namespace AISoftCraftDuck.Controls;

/// <summary>Безопасный интерактивный опрос. Формат: ```poll {"question":"...","multiple":false,"options":["A","B"]} ```.</summary>
public sealed class PollControl : Border
{
    private readonly StackPanel optionsPanel = new() { Margin = new Thickness(0, 8, 0, 4) };
    private readonly TextBlock result = new() { Foreground = new WpfSolidColorBrush(WpfColor.FromRgb(98, 91, 112)), Margin = new Thickness(0, 8, 0, 0) };
    private readonly List<System.Windows.Controls.CheckBox> options = [];
    private bool multiple;

    public PollControl(string json)
    {
        Background = new WpfSolidColorBrush(WpfColor.FromRgb(250, 250, 253));
        BorderBrush = new WpfSolidColorBrush(WpfColor.FromRgb(220, 210, 245));
        BorderThickness = new Thickness(1);
        CornerRadius = new CornerRadius(12);
        Padding = new Thickness(14);
        var panel = new StackPanel();
        Child = panel;
        try
        {
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            multiple = root.TryGetProperty("multiple", out var multipleNode) && multipleNode.GetBoolean();
            panel.Children.Add(new TextBlock { Text = root.TryGetProperty("question", out var q) ? q.GetString() ?? "Опрос" : "Опрос", FontWeight = FontWeights.SemiBold, FontSize = 15, Foreground = new WpfSolidColorBrush(WpfColor.FromRgb(23, 19, 31)) });
            if (root.TryGetProperty("options", out var choices) && choices.ValueKind == JsonValueKind.Array)
                foreach (var choice in choices.EnumerateArray()) AddOption(choice.GetString() ?? string.Empty);
            panel.Children.Add(optionsPanel);
            var submit = new System.Windows.Controls.Button { Content = "Ответить", HorizontalAlignment = System.Windows.HorizontalAlignment.Left, Padding = new Thickness(14, 7, 14, 7) };
            submit.Click += (_, _) => Submit(submit);
            panel.Children.Add(submit); panel.Children.Add(result);
        }
        catch { panel.Children.Add(new TextBlock { Text = "Некорректный формат опроса.", Foreground = WpfBrushes.IndianRed }); }
    }

    private void AddOption(string label)
    {
        var item = new System.Windows.Controls.CheckBox { Content = label, Foreground = new WpfSolidColorBrush(WpfColor.FromRgb(23, 19, 31)), Margin = new Thickness(2, 3, 2, 3) };
        if (!multiple) item.Checked += (_, _) => { foreach (var other in options.Where(x => x != item)) other.IsChecked = false; };
        options.Add(item); optionsPanel.Children.Add(item);
    }
    private void Submit(System.Windows.Controls.Button submit)
    {
        var choices = options.Where(x => x.IsChecked == true).Select(x => x.Content?.ToString()).ToArray();
        if (choices.Length == 0) { result.Text = "Выберите вариант ответа."; return; }
        result.Text = $"Ваш ответ: {string.Join(", ", choices)}"; submit.IsEnabled = false;
        foreach (var option in options) option.IsEnabled = false;
    }
}
