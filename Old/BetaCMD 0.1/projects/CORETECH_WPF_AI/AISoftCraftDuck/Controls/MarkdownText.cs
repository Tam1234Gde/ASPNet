using System.Diagnostics;
using System.Text.Json;
using System.Windows.Media.Imaging;
using Markdig.Extensions.Tables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using WpfBrushes = System.Windows.Media.Brushes;
using WpfColor = System.Windows.Media.Color;
using WpfFontFamily = System.Windows.Media.FontFamily;

namespace AISoftCraftDuck.Controls;

/// <summary>Рендерит Markdown (CommonMark с таблицами и задачами) в нативный WPF FlowDocument.</summary>
public sealed class MarkdownText : System.Windows.Controls.RichTextBox
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    public static readonly DependencyProperty MarkdownProperty = DependencyProperty.Register(
        nameof(Markdown), typeof(string), typeof(MarkdownText), new PropertyMetadata(string.Empty, OnMarkdownChanged));

    public string Markdown
    {
        get => (string)GetValue(MarkdownProperty);
        set => SetValue(MarkdownProperty, value);
    }

    public MarkdownText()
    {
        IsReadOnly = true;
        IsDocumentEnabled = true;
        IsHitTestVisible = true;
        BorderThickness = new Thickness(0);
        Background = WpfBrushes.Transparent;
        Padding = new Thickness(0);
        VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
        HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
    }

    private static void OnMarkdownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((MarkdownText)d).Render(e.NewValue as string ?? string.Empty);

    private void Render(string markdown)
    {
        var document = new FlowDocument
        {
            PagePadding = new Thickness(0),
            Background = WpfBrushes.Transparent,
            Foreground = Foreground,
            FontSize = 14,
            FontFamily = new WpfFontFamily("Segoe UI"),
            TextAlignment = TextAlignment.Left
        };

        try
        {
            foreach (var block in Markdig.Markdown.Parse(markdown, Pipeline))
                AddBlock(document.Blocks, block);
        }
        catch
        {
            // A partially typed streaming response must always remain visible.
            document.Blocks.Add(new Paragraph(new Run(markdown)) { Margin = new Thickness(0) });
        }

        Document = document;
    }

    private static void AddBlock(BlockCollection target, Markdig.Syntax.Block block)
    {
        switch (block)
        {
            case HeadingBlock heading:
                var headingParagraph = new Paragraph { Margin = new Thickness(0, 12, 0, 5), FontWeight = FontWeights.Bold, FontSize = Math.Max(16, 27 - heading.Level * 2) };
                AddInlines(headingParagraph.Inlines, heading.Inline);
                target.Add(headingParagraph);
                break;

            case ParagraphBlock paragraph:
                var p = new Paragraph { Margin = new Thickness(0, 3, 0, 5), LineHeight = 21 };
                AddInlines(p.Inlines, paragraph.Inline);
                target.Add(p);
                break;

            case Markdig.Extensions.Tables.Table table:
                AddTable(target, table);
                break;

            case HtmlBlock html:
                // Raw HTML is deliberately rendered as text: no scripts, event handlers or unsafe markup.
                target.Add(new Paragraph(new Run(System.Net.WebUtility.HtmlDecode(System.Text.RegularExpressions.Regex.Replace(html.Lines.ToString(), "<[^>]+>", string.Empty))) { Foreground = new SolidColorBrush(WpfColor.FromRgb(98, 91, 112)) }));
                break;

            case QuoteBlock quote:
                var quoteSection = new Section { BorderBrush = new SolidColorBrush(WpfColor.FromRgb(139, 92, 246)), BorderThickness = new Thickness(3, 0, 0, 0), Padding = new Thickness(12, 2, 0, 2), Margin = new Thickness(0, 6, 0, 6), Foreground = new SolidColorBrush(WpfColor.FromRgb(205, 213, 228)) };
                foreach (var child in quote) AddBlock(quoteSection.Blocks, child);
                target.Add(quoteSection);
                break;

            case ListBlock list:
                var wpfList = new System.Windows.Documents.List { MarkerStyle = list.IsOrdered ? TextMarkerStyle.Decimal : TextMarkerStyle.Disc, Margin = new Thickness(20, 4, 0, 5), Padding = new Thickness(12, 0, 0, 0) };
                foreach (var item in list.OfType<ListItemBlock>())
                {
                    var listItem = new ListItem();
                    foreach (var child in item) AddBlock(listItem.Blocks, child);
                    wpfList.ListItems.Add(listItem);
                }
                target.Add(wpfList);
                break;

            case FencedCodeBlock fenced when string.Equals(fenced.Info, "poll", StringComparison.OrdinalIgnoreCase):
                target.Add(new BlockUIContainer(new PollControl(fenced.Lines.ToString())) { Margin = new Thickness(0, 7, 0, 7) });
                break;

            case FencedCodeBlock fenced when string.Equals(fenced.Info, "mermaid", StringComparison.OrdinalIgnoreCase):
                // Mermaid is retained as a safe diagram source; it is never executed as HTML/JavaScript.
                AddCodeBlock(target, fenced.Lines.ToString(), "mermaid");
                break;

            case FencedCodeBlock fenced:
                AddCodeBlock(target, fenced.Lines.ToString(), fenced.Info);
                break;

            case CodeBlock code:
                AddCodeBlock(target, code.Lines.ToString(), null);
                break;

            case ThematicBreakBlock:
                target.Add(new BlockUIContainer(new Border { Height = 1, Background = new SolidColorBrush(WpfColor.FromRgb(72, 74, 88)), Margin = new Thickness(0, 10, 0, 10) }));
                break;

            default:
                // Preserve content from extensions not requiring a dedicated WPF representation.
                if (block is LeafBlock leaf)
                {
                    var fallback = new Paragraph { Margin = new Thickness(0, 3, 0, 5) };
                    AddInlines(fallback.Inlines, leaf.Inline);
                    target.Add(fallback);
                }
                break;
        }
    }

    private static void AddTable(BlockCollection target, Markdig.Extensions.Tables.Table table)
    {
        var grid = new Grid { Margin = new Thickness(0, 7, 0, 7) };
        var rows = table.OfType<Markdig.Extensions.Tables.TableRow>().ToArray();
        var columnCount = rows.Select(row => row.OfType<Markdig.Extensions.Tables.TableCell>().Count()).DefaultIfEmpty().Max();
        for (var i = 0; i < columnCount; i++) grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        for (var rowIndex = 0; rowIndex < rows.Length; rowIndex++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var cells = rows[rowIndex].OfType<Markdig.Extensions.Tables.TableCell>().ToArray();
            for (var columnIndex = 0; columnIndex < cells.Length; columnIndex++)
            {
                var text = new TextBlock { Padding = new Thickness(9, 6, 9, 6), TextWrapping = TextWrapping.Wrap, Foreground = WpfBrushes.Black, Background = rowIndex == 0 ? new SolidColorBrush(WpfColor.FromRgb(240, 234, 255)) : WpfBrushes.White };
                var span = new Span();
                foreach (var cellBlock in cells[columnIndex].OfType<LeafBlock>()) AddInlines(span.Inlines, cellBlock.Inline);
                text.Inlines.Add(span);
                Grid.SetRow(text, rowIndex); Grid.SetColumn(text, columnIndex); grid.Children.Add(new Border { BorderBrush = new SolidColorBrush(WpfColor.FromRgb(230, 225, 236)), BorderThickness = new Thickness(.5), Child = text });
            }
        }
        target.Add(new BlockUIContainer(new Border { CornerRadius = new CornerRadius(8), BorderBrush = new SolidColorBrush(WpfColor.FromRgb(230, 225, 236)), BorderThickness = new Thickness(1), Child = grid }));
    }

    private static void AddCodeBlock(BlockCollection target, string code, string? language)
    {
        var codeControl = new CodeBlockControl
        {
            Code = code.TrimEnd('\r', '\n'),
            Language = language?.Trim() ?? string.Empty,
            MaxHeight = 360
        };
        target.Add(new BlockUIContainer(new Border { Background = new SolidColorBrush(WpfColor.FromRgb(30, 31, 39)), CornerRadius = new CornerRadius(8), Margin = new Thickness(0, 7, 0, 7), Child = codeControl }));
    }

    private static void AddInlines(InlineCollection target, ContainerInline? container)
    {
        if (container is null) return;
        for (var inline = container.FirstChild; inline is not null; inline = inline.NextSibling)
        {
            switch (inline)
            {
                case LiteralInline literal: target.Add(new Run(literal.Content.ToString())); break;
                case CodeInline code: target.Add(new Run(code.Content) { FontFamily = new WpfFontFamily("Cascadia Code"), Background = new SolidColorBrush(WpfColor.FromRgb(45, 46, 56)) }); break;
                case EmphasisInline emphasis:
                    var span = new Span(); AddInlines(span.Inlines, emphasis);
                    if (emphasis.DelimiterCount >= 2) span.FontWeight = FontWeights.Bold; else span.FontStyle = FontStyles.Italic;
                    target.Add(span); break;
                case LinkInline image when image.IsImage && Uri.TryCreate(image.Url, UriKind.Absolute, out var imageUri) && imageUri.Scheme is "https" or "http":
                    try
                    {
                        var bitmap = new BitmapImage(); bitmap.BeginInit(); bitmap.UriSource = imageUri; bitmap.DecodePixelWidth = 800; bitmap.CacheOption = BitmapCacheOption.OnLoad; bitmap.EndInit();
                        target.Add(new InlineUIContainer(new System.Windows.Controls.Image { Source = bitmap, MaxWidth = 800, MaxHeight = 480, Stretch = Stretch.Uniform, Margin = new Thickness(0, 5, 0, 5) }));
                    }
                    catch { target.Add(new Run($"[Изображение: {image.Url}]")); }
                    break;
                case LinkInline link when !string.IsNullOrWhiteSpace(link.Url):
                    var hyperlink = new Hyperlink { NavigateUri = Uri.TryCreate(link.Url, UriKind.Absolute, out var uri) ? uri : null, Foreground = new SolidColorBrush(WpfColor.FromRgb(124, 58, 237)), TextDecorations = TextDecorations.Underline };
                    hyperlink.RequestNavigate += (_, e) => Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                    AddInlines(hyperlink.Inlines, link); target.Add(hyperlink); break;
                case LineBreakInline: target.Add(new LineBreak()); break;
                default: if (inline is ContainerInline nested) AddInlines(target, nested); break;
            }
        }
    }
}
