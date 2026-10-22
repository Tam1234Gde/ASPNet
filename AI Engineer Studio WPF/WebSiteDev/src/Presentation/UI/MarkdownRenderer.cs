using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AIEngineerStudio.UI;

/// <summary>Безопасный, выделяемый рендерер CommonMark/GFM для сообщений чата.</summary>
public static class MarkdownRenderer
{
    private static readonly Regex HeadingPattern = new("^(#{1,6})\\s+(.+?)\\s*#*\\s*$", RegexOptions.Compiled);
    private static readonly Regex OrderedPattern = new("^\\s*(\\d+)\\.\\s+(.+)$", RegexOptions.Compiled);
    private static readonly Regex LinkPattern = new("^(!?)\\[([^]]*)\\]\\(([^ )]+)(?:\\s+\\\"[^\"]*\\\")?\\)", RegexOptions.Compiled);

    public static FrameworkElement Render(string markdown)
    {
        var viewer = new RichTextBox
        {
            IsReadOnly = true,
            IsDocumentEnabled = true,
            BorderThickness = new Thickness(0),
            Padding = new Thickness(0),
            VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            Focusable = true
        };
        viewer.SetResourceReference(Control.ForegroundProperty, "Body");
        viewer.Background = Brushes.Transparent;
        var document = new FlowDocument { PagePadding = new Thickness(0), TextAlignment = TextAlignment.Left };
        viewer.Document = document;

        var lines = markdown.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        var index = 0;
        while (index < lines.Length)
        {
            var line = lines[index];
            if (line.StartsWith("```", StringComparison.Ordinal) || line.StartsWith("~~~", StringComparison.Ordinal))
            {
                var fence = line[..3];
                var language = line.Length > 3 ? line[3..].Trim() : string.Empty;
                index++;
                var code = new List<string>();
                while (index < lines.Length && !lines[index].StartsWith(fence, StringComparison.Ordinal)) code.Add(lines[index++]);
                if (index < lines.Length) index++;
                AddCode(document, string.Join("\n", code), language);
                continue;
            }
            if (IsTableStart(lines, index)) { AddTable(document, lines, ref index); continue; }
            if (Regex.IsMatch(line, "^\\s{0,3}([-*_])(?:\\s*\\1){2,}\\s*$")) { document.Blocks.Add(new Paragraph(new Run()) { BorderBrush = Brush("Border"), BorderThickness = new Thickness(0, 0, 0, 1), Margin = new Thickness(0, 9, 0, 9) }); index++; continue; }
            var heading = HeadingPattern.Match(line);
            if (heading.Success)
            {
                var level = heading.Groups[1].Length;
                var paragraph = StyledParagraph(7 - level == 6 ? 25 : 27 - level * 2, FontWeights.SemiBold, "Text");
                AddInlines(paragraph, heading.Groups[2].Value);
                document.Blocks.Add(paragraph); index++; continue;
            }
            if (line.StartsWith(">", StringComparison.Ordinal))
            {
                var quote = new Section { BorderBrush = Brush("Accent"), BorderThickness = new Thickness(3, 0, 0, 0), Padding = new Thickness(10, 2, 0, 2), Margin = new Thickness(0, 5, 0, 5) };
                while (index < lines.Length && lines[index].StartsWith(">", StringComparison.Ordinal))
                {
                    var paragraph = StyledParagraph(); AddInlines(paragraph, lines[index].TrimStart('>').TrimStart()); quote.Blocks.Add(paragraph); index++;
                }
                document.Blocks.Add(quote); continue;
            }
            if (IsListLine(line)) { AddList(document, lines, ref index); continue; }
            if (string.IsNullOrWhiteSpace(line)) { index++; continue; }

            var normal = StyledParagraph();
            AddInlines(normal, line);
            document.Blocks.Add(normal); index++;
        }
        return viewer;
    }

    private static void AddCode(FlowDocument document, string code, string language)
    {
        var paragraph = new Paragraph { Margin = new Thickness(0, 7, 0, 7), Padding = new Thickness(12), FontFamily = new FontFamily("Cascadia Code, Consolas"), FontSize = 12 };
        paragraph.SetResourceReference(Block.BackgroundProperty, "SubBg");
        paragraph.SetResourceReference(TextElement.ForegroundProperty, "Text");
        if (!string.IsNullOrWhiteSpace(language)) paragraph.Inlines.Add(new Run(language.ToUpperInvariant() + "\n") { FontSize = 9, FontWeight = FontWeights.Bold, Foreground = Brush("Accent") });
        paragraph.Inlines.Add(new Run(code)); document.Blocks.Add(paragraph);
    }

    private static void AddList(FlowDocument document, string[] lines, ref int index)
    {
        var ordered = OrderedPattern.IsMatch(lines[index]);
        var list = new List { MarkerStyle = ordered ? TextMarkerStyle.Decimal : TextMarkerStyle.Disc, Margin = new Thickness(18, 3, 0, 3), Padding = new Thickness(14, 0, 0, 0) };
        while (index < lines.Length && IsListLine(lines[index]) && OrderedPattern.IsMatch(lines[index]) == ordered)
        {
            var text = ordered ? OrderedPattern.Match(lines[index]).Groups[2].Value : Regex.Replace(lines[index], "^\\s*[-+*]\\s+", string.Empty);
            var item = new ListItem(); var paragraph = StyledParagraph();
            var task = Regex.Match(text, "^\\[([ xX])\\]\\s+");
            if (task.Success) { paragraph.Inlines.Add(new Run(task.Groups[1].Value.Equals("x", StringComparison.OrdinalIgnoreCase) ? "☑ " : "☐ ") { Foreground = Brush("Accent") }); text = text[task.Length..]; }
            AddInlines(paragraph, text); item.Blocks.Add(paragraph); list.ListItems.Add(item); index++;
        }
        document.Blocks.Add(list);
    }

    private static bool IsListLine(string line) => Regex.IsMatch(line, "^\\s*[-+*]\\s+") || OrderedPattern.IsMatch(line);
    private static bool IsTableStart(string[] lines, int i) => i + 1 < lines.Length && lines[i].Contains('|') && Regex.IsMatch(lines[i + 1], "^\\s*\\|?\\s*:?-{3,}:?\\s*(\\|\\s*:?-{3,}:?\\s*)+\\|?\\s*$");

    private static void AddTable(FlowDocument document, string[] lines, ref int index)
    {
        var rows = new List<string[]> { SplitCells(lines[index++]) }; index++; // separator
        while (index < lines.Length && lines[index].Contains('|') && !string.IsNullOrWhiteSpace(lines[index])) rows.Add(SplitCells(lines[index++]));
        var table = new Table { CellSpacing = 0, BorderBrush = Brush("Border"), BorderThickness = new Thickness(1), Margin = new Thickness(0, 6, 0, 6) };
        var group = new TableRowGroup(); table.RowGroups.Add(group);
        var columns = rows.Max(row => row.Length); for (var i = 0; i < columns; i++) table.Columns.Add(new TableColumn());
        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            var row = new TableRow();
            for (var column = 0; column < columns; column++)
            {
                var cell = new TableCell { BorderBrush = Brush("Border"), BorderThickness = new Thickness(.5), Padding = new Thickness(7, 4, 7, 4) };
                if (rowIndex == 0) { cell.SetResourceReference(TableCell.BackgroundProperty, "SubBg"); cell.FontWeight = FontWeights.SemiBold; }
                var p = StyledParagraph(); AddInlines(p, column < rows[rowIndex].Length ? rows[rowIndex][column] : string.Empty); cell.Blocks.Add(p); row.Cells.Add(cell);
            }
            group.Rows.Add(row);
        }
        document.Blocks.Add(table);
    }

    private static string[] SplitCells(string line) => line.Trim().Trim('|').Split('|').Select(cell => cell.Trim()).ToArray();
    private static Paragraph StyledParagraph(double size = 13, FontWeight? weight = null, string color = "Body")
    {
        var paragraph = new Paragraph { Margin = new Thickness(0, 3, 0, 3), FontSize = size };
        paragraph.SetResourceReference(TextElement.ForegroundProperty, color); if (weight.HasValue) paragraph.FontWeight = weight.Value; return paragraph;
    }

    private static void AddInlines(Paragraph paragraph, string text)
    {
        for (var i = 0; i < text.Length;)
        {
            if (text[i] == '\\' && i + 1 < text.Length) { paragraph.Inlines.Add(new Run(text[i + 1].ToString())); i += 2; continue; }
            if (text[i] == '`') { var end = text.IndexOf('`', i + 1); if (end > i) { var run = new Run(text[(i + 1)..end]) { FontFamily = new FontFamily("Cascadia Code, Consolas") }; run.SetResourceReference(TextElement.BackgroundProperty, "SubBg"); paragraph.Inlines.Add(run); i = end + 1; continue; } }
            var link = LinkPattern.Match(text[i..]);
            if (link.Success) { AddLink(paragraph, link.Groups[2].Value, link.Groups[3].Value, link.Groups[1].Value == "!"); i += link.Length; continue; }
            if (Starts(text, i, "**") || Starts(text, i, "__") || Starts(text, i, "~~") || text[i] == '*' || text[i] == '_')
            {
                var marker = Starts(text, i, "**") || Starts(text, i, "__") || Starts(text, i, "~~") ? text.Substring(i, 2) : text[i].ToString(); var end = text.IndexOf(marker, i + marker.Length, StringComparison.Ordinal);
                if (end > i + marker.Length) { var span = new Span(); if (marker == "~~") span.TextDecorations = TextDecorations.Strikethrough; else if (marker.Length == 2) span.FontWeight = FontWeights.Bold; else span.FontStyle = FontStyles.Italic; AddInlinesTo(span, text[(i + marker.Length)..end]); paragraph.Inlines.Add(span); i = end + marker.Length; continue; }
            }
            paragraph.Inlines.Add(new Run(text[i].ToString())); i++;
        }
    }
    private static void AddInlinesTo(Span span, string value) => span.Inlines.Add(new Run(value));
    private static bool Starts(string text, int start, string value) => start + value.Length <= text.Length && text.AsSpan(start, value.Length).SequenceEqual(value);
    private static void AddLink(Paragraph paragraph, string title, string address, bool image)
    {
        var link = new Hyperlink(new Run(image ? "▧ " + title : title)) { NavigateUri = Uri.TryCreate(address, UriKind.Absolute, out var uri) ? uri : null, ToolTip = address };
        link.SetResourceReference(TextElement.ForegroundProperty, "Accent"); link.TextDecorations = TextDecorations.Underline;
        link.RequestNavigate += (_, args) => { if (args.Uri is not null) Process.Start(new ProcessStartInfo(args.Uri.AbsoluteUri) { UseShellExecute = true }); args.Handled = true; }; paragraph.Inlines.Add(link);
    }
    private static Brush Brush(string key) => (Brush)Application.Current.Resources[key];
}
