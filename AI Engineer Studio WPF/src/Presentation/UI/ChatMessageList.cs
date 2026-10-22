using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AIEngineerStudio.UI;

// Items are factories, not UIElements: recycling releases Markdown, images and handlers.
public sealed class ChatMessageList : ListBox
{
    public sealed record Row(Func<UIElement> Create);
    ScrollViewer? scroller;

    public ChatMessageList()
    {
        Background = Brushes.Transparent;
        BorderThickness = new Thickness(0);
        SetValue(ScrollViewer.CanContentScrollProperty, true);
        SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
        SetValue(VirtualizingPanel.IsVirtualizingProperty, true);
        SetValue(VirtualizingPanel.VirtualizationModeProperty, VirtualizationMode.Recycling);
        SetValue(VirtualizingPanel.ScrollUnitProperty, ScrollUnit.Pixel);
        SetValue(VirtualizingPanel.CacheLengthProperty, new VirtualizationCacheLength(0.5));

        var style = new Style(typeof(ListBoxItem));
        style.Setters.Add(new Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch));
        style.Setters.Add(new Setter(PaddingProperty, new Thickness(0)));
        style.Setters.Add(new Setter(MarginProperty, new Thickness(20, 0, 20, 0)));
        style.Setters.Add(new Setter(FocusableProperty, false));
        var template = new ControlTemplate(typeof(ListBoxItem));
        template.VisualTree = new FrameworkElementFactory(typeof(ContentPresenter));
        style.Setters.Add(new Setter(TemplateProperty, template));
        ItemContainerStyle = style;

        Loaded += (_, _) => scroller = FindOwnScroller(this);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        scroller = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);
        if (element is ListBoxItem container && item is Row row)
            container.Content = row.Create();
    }

    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        if (element is ListBoxItem container)
            container.Content = null;
        base.ClearContainerForItemOverride(element, item);
    }

    public IEnumerable<Border> RealizedBorders => Enumerable.Range(0, Items.Count)
        .Select(i => ItemContainerGenerator.ContainerFromIndex(i)).OfType<ListBoxItem>()
        .Select(c => c.Content).OfType<Border>();

    // Deliberately find only the ListBox template's viewer. Chat cards and Markdown can contain
    // their own ScrollViewers; subscribing to one of those made the old paging logic unstable.
    public ScrollViewer? Scroller => scroller ??= FindOwnScroller(this);

    ScrollViewer? FindOwnScroller(DependencyObject parent)
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            // Standard WPF themes may use an unnamed viewer. Ownership, not its name,
            // distinguishes the list viewer from viewers inside message cards.
            if (child is ScrollViewer viewer)
            {
                if (ReferenceEquals(viewer.TemplatedParent, this)) return viewer;
                continue;
            }
            if (child is ListBoxItem) continue;
            if (FindOwnScroller(child) is { } result) return result;
        }
        return null;
    }

    public void ScrollToEnd()
    {
        ApplyTemplate();
        UpdateLayout();
        var viewer = Scroller;
        if (viewer is null)
        {
            // The default ListBox template creates its ScrollViewer lazily.
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded,
                new Action(ScrollToEnd));
            return;
        }

        // ScrollIntoView is intentionally not used here: with pixel virtualization it
        // can realize the last row and then restore an old offset, making the button
        // appear to do nothing. The list viewer owns the authoritative extent.
        viewer.ScrollToVerticalOffset(Math.Max(0, viewer.ExtentHeight - viewer.ViewportHeight));
        viewer.ScrollToEnd();
    }
}
