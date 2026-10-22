using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AIEngineerStudio.Models;
using AIEngineerStudio.UI;

namespace AIEngineerStudio;
public partial class MainWindow
{
    const int ChatPageSize = 50;
    Border? renderingMessageBorder;
    sealed class MessageVisual
    {
        public required string Sender;
        public required string Text;
        public bool Assistant;
        public List<Action> Decorations = new();
        public WeakReference<Border>? Realized;
    }
    MessageVisual? lastMessageVisual;
    List<Dictionary<string,string>>? viewHistory;
    int firstVisibleHistoryIndex;
    int viewGeneration;
    bool pagingChat, followingChat = true, buildingChat;
    bool olderPageQueued;
    bool followingRequestQueued;
    bool navigatingToTail;
    bool loadingProject;
    void InitializeChatView()
    {
        ChatPanel.PreviewMouseWheel += (_, _) => { followingChat = false; navigatingToTail = false; };
        ChatPanel.PreviewKeyDown += (_, e) =>
        {
            if (e.Key is System.Windows.Input.Key.Up or System.Windows.Input.Key.PageUp or System.Windows.Input.Key.Home)
                followingChat = false;
        };
        ChatPanel.PreviewMouseDown += (_, e) =>
        {
            if (e.OriginalSource is System.Windows.DependencyObject source)
                for (var node = source; node is not null; node = System.Windows.Media.VisualTreeHelper.GetParent(node))
                {
                    if (node is System.Windows.Controls.Primitives.ScrollBar) { followingChat = false; break; }
                    if (node is not System.Windows.Media.Visual && node is not System.Windows.Media.Media3D.Visual3D) break;
                }
        };
        ChatPanel.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler((_, e) =>
        {
            // Only the ListBox viewer is relevant. Markdown/code/card viewers also raise this
            // routed event and previously made paging fire while scrolling inside a card.
            if (buildingChat || pagingChat || !ReferenceEquals(e.OriginalSource, ChatPanel.Scroller)) return;

            // A user scroll changes the offset without changing the extent. Handle it
            // before layout notifications; otherwise a row remeasure can hide the
            // fact that the user deliberately left the tail.
            if (!navigatingToTail && e.VerticalChange != 0 && e.ExtentHeightChange == 0)
                followingChat = e.VerticalOffset >= e.ExtentHeight - e.ViewportHeight - 32;
            // Extent/viewport changes are layout, not user navigation. In particular,
            // the first layout must not cancel the initial request to show the tail.
            if (e.ExtentHeightChange != 0 || e.ViewportHeightChange != 0)
            {
                if (followingChat) FollowChatEnd();
                return;
            }
            if (!navigatingToTail && e.VerticalChange == 0)
                followingChat = e.VerticalOffset >= e.ExtentHeight - e.ViewportHeight - 32;
            if (!followingChat && e.VerticalChange < 0 && e.VerticalOffset <= 64)
                QueueOlderChatPage();
        }));
    }
    void QueueOlderChatPage()
    {
        if (olderPageQueued || pagingChat || firstVisibleHistoryIndex == 0) return;
        olderPageQueued = true;
        var generation = viewGeneration;
        Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
        {
            olderPageQueued = false;
            if (generation == viewGeneration && !followingChat && ChatPanel.Scroller is { VerticalOffset: <= 64 }) LoadOlderChatPage();
        }));
    }
    void LatestMessages_Click(object sender, RoutedEventArgs e)
    {
        followingChat = true;
        FollowChatEnd();
    }
    void FollowChatEnd()
    {
        if (!followingChat || buildingChat || followingRequestQueued) return;
        followingRequestQueued = true;
        navigatingToTail = true;
        int generation = viewGeneration;
        Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
        {
            followingRequestQueued = false;
            if (generation != viewGeneration) { navigatingToTail = false; FollowChatEnd(); return; }
            if (!followingChat) { navigatingToTail = false; return; }
            ChatPanel.ScrollToEnd();
            Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                if (generation != viewGeneration) return;
                if (followingChat) ChatPanel.ScrollToEnd();
                navigatingToTail = false;
            }));
        }));
    }
    void AddMarkdown(string sender, string markdown, bool assistant)
    {
        if(responseChat.Value.HasValue && responseChat.Value.Value != activeChatType) return;
        var visual = new MessageVisual { Sender=sender, Text=markdown, Assistant=assistant };
        lastMessageVisual = visual;
        ChatPanel.Items.Add(new ChatMessageList.Row(() => BuildMessageVisual(visual)));
        FollowChatEnd();
    }
    UIElement BuildMessageVisual(MessageVisual visual)
    {
        Border border=CreateMarkdown(visual.Sender,visual.Text,visual.Assistant);
        visual.Realized = new WeakReference<Border>(border);
        var previous=renderingMessageBorder; renderingMessageBorder=border;
        try { foreach(var action in visual.Decorations) action(); }
        finally { renderingMessageBorder=previous; }
        return border;
    }
    void AddMessageDecoration(Action action)
    {
        if (lastMessageVisual is null || (responseChat.Value.HasValue && responseChat.Value.Value != activeChatType)) return;
        lastMessageVisual.Decorations.Add(action);
        // The row is already materialized during normal append; decorate it immediately.
        if (lastMessageVisual.Realized?.TryGetTarget(out var border) == true)
        {
            var previous = renderingMessageBorder; renderingMessageBorder = border;
            try { action(); } finally { renderingMessageBorder = previous; }
        }
    }
    ChatMessageList.Row HistoryRow(Dictionary<string,string> message)
    {
        return new ChatMessageList.Row(() =>
        {
            string role=message.GetValueOrDefault("role","assistant");
            var visual=new MessageVisual { Sender=role=="user" ? "Вы" : Model, Text=message.GetValueOrDefault("content", ""), Assistant=role!="user" };
            if(message.TryGetValue("sent_at",out var date) && DateTime.TryParse(date,out var sent)) visual.Sender += $" · {sent:dd.MM.yyyy HH:mm:ss}";
            if(message.TryGetValue("duration_ms",out var duration)) visual.Sender += $" · {duration} мс";
            visual.Decorations.Add(() => ShowMessageAttachments(MessageAttachments(message)));
            if(activeChatType==ChatType.Conversation) visual.Decorations.Add(() => RenderConversationCard(message));
            return BuildMessageVisual(visual);
        });
    }
    void ResetChatView()
    {
        viewGeneration++; buildingChat=true;
        try
        {
            ChatPanel.Items.Clear(); lastMessageVisual=null;
            viewHistory=activeChatType==ChatType.Development ? developmentHistory : conversationHistory;
            firstVisibleHistoryIndex=Math.Max(0,viewHistory.Count-ChatPageSize);
            for(int i=firstVisibleHistoryIndex;i<viewHistory.Count;i++) ChatPanel.Items.Add(HistoryRow(viewHistory[i]));
            if(viewHistory.Count==0) AddMarkdown("Ассистент",activeChatType==ChatType.Development ? "## Готов к работе\n\nОпишите задачу разработки." : "## Чат для общения\n\nЗадавайте вопросы без выполнения действий.",true);
            followingChat=true;
        }
        finally { buildingChat=false; }
        FollowChatEnd();
    }
    void LoadOlderChatPage()
    {
        if (pagingChat || firstVisibleHistoryIndex == 0 || viewHistory is null || ChatPanel.Scroller is not { } scroll) return;
        pagingChat = true;
        var generation = viewGeneration;
        var oldExtent = scroll.ExtentHeight;
        var oldOffset = scroll.VerticalOffset;
        try
        {
            var start = Math.Max(0, firstVisibleHistoryIndex - ChatPageSize);
            // Insert in natural order. Reversed inserts caused generator churn and intermittent
            // wrong ordering while a variable-height row was still being measured.
            for (var i = firstVisibleHistoryIndex - 1; i >= start; i--)
                ChatPanel.Items.Insert(0, HistoryRow(viewHistory[i]));
            firstVisibleHistoryIndex = start;

            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                try
                {
                    if (generation != viewGeneration || followingChat || ChatPanel.Scroller is not { } current) return;
                    ChatPanel.UpdateLayout();
                    // Extent delta is the stable anchor for pixel scrolling and variable-height rows.
                    // Do not ScrollIntoView: it itself changes the offset and causes visible jumps.
                    current.ScrollToVerticalOffset(oldOffset + Math.Max(0, current.ExtentHeight - oldExtent));
                }
                finally { pagingChat = false; }
            }));
        }
        catch
        {
            pagingChat = false;
            throw;
        }
    }
}
