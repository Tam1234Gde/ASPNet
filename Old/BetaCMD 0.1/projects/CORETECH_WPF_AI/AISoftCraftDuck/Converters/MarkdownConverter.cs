using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace AISoftCraftDuck.Converters;

/// <summary>Lightweight WPF Markdown renderer; avoids creating WebBrowser controls for every chat message.</summary>
public sealed class MarkdownConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var document = new FlowDocument { PagePadding = new Thickness(0), Background = System.Windows.Media.Brushes.Transparent, Foreground = System.Windows.Media.Brushes.White, FontSize = 14 };
        var text = value as string ?? string.Empty;
        foreach (var line in text.Replace("\r\n", "\n").Split('\n'))
        {
            var paragraph = new Paragraph { Margin = new Thickness(0, 2, 0, 2) };
            var heading = Regex.Match(line, "^(#{1,6})\\s+(.*)$");
            var content = heading.Success ? heading.Groups[2].Value : Regex.Replace(line, "^[-*+]\\s+", "• ");
            if (heading.Success) { paragraph.FontWeight = FontWeights.Bold; paragraph.FontSize = Math.Max(15, 21 - heading.Groups[1].Value.Length); }
            var position = 0;
            foreach (Match match in Regex.Matches(content, "(\\*\\*.+?\\*\\*|`[^`]+`|\\*[^*]+\\*)"))
            {
                if (match.Index > position) paragraph.Inlines.Add(content[position..match.Index]);
                var token = match.Value;
                if (token.StartsWith("**")) paragraph.Inlines.Add(new Bold(new Run(token[2..^2])));
                else if (token.StartsWith('`')) paragraph.Inlines.Add(new Run(token[1..^1]) { FontFamily = new System.Windows.Media.FontFamily("Consolas"), Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(45, 46, 56)) });
                else paragraph.Inlines.Add(new Italic(new Run(token[1..^1])));
                position = match.Index + match.Length;
            }
            if (position < content.Length) paragraph.Inlines.Add(content[position..]);
            document.Blocks.Add(paragraph);
        }
        return document;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => System.Windows.Data.Binding.DoNothing;
}
