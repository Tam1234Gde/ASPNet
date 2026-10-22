using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AIEngineerStudio.UI;

public static class MarkdownRenderer
{
    public static FrameworkElement Render(string markdown)
    {
        var panel = new StackPanel();
        var code = false;
        TextBlock? codeBlock = null;

        foreach (var raw in markdown.Replace("\r", "").Split('\n'))
        {
            if (raw.StartsWith("```"))
            {
                code = !code;
                if (code)
                {
                    codeBlock = new TextBlock
                    {
                        FontFamily = new FontFamily("Consolas"),
                        TextWrapping = TextWrapping.Wrap,
                        Padding = new Thickness(10),
                        Margin = new Thickness(0, 5, 0, 5)
                    };
                    codeBlock.SetResourceReference(TextBlock.ForegroundProperty, "Text");
                    codeBlock.SetResourceReference(TextBlock.BackgroundProperty, "SubBg");
                    panel.Children.Add(codeBlock);
                }
                continue;
            }

            if (code)
            {
                codeBlock!.Text += raw + "\n";
                continue;
            }

            var text = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 2, 0, 2)
            };
            text.SetResourceReference(TextBlock.ForegroundProperty, raw.StartsWith("## ") ? "Text" : "Body");

            if (raw.StartsWith("## "))
            {
                text.Text = raw[3..];
                text.FontSize = 15;
                text.FontWeight = FontWeights.SemiBold;
            }
            else if (raw.StartsWith("- "))
            {
                text.Text = "• " + raw[2..];
            }
            else
            {
                var pieces = raw.Split("**");
                for (var i = 0; i < pieces.Length; i++)
                    text.Inlines.Add(new Run(pieces[i]) { FontWeight = i % 2 == 1 ? FontWeights.Bold : FontWeights.Normal });
            }

            panel.Children.Add(text);
        }

        return panel;
    }
}
