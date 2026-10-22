using System.Text;
using System.Windows;
using System.Windows.Controls;
using AIEngineerStudio.Models;

namespace AIEngineerStudio;

public partial class MainWindow
{
    string? failedConversationIntent;
    static string OptionLabel(ChoiceOption option) => option.Text
        + (option.Recommended ? " · Рекомендуется" : "")
        + (string.IsNullOrWhiteSpace(option.Description) ? "" : "\n" + option.Description);

    // Update only the realized card. Unrealized rows read fresh state when recycled.
    void RefreshSurveyMessage(Dictionary<string,string> message)
    {
        if (activeChatType != ChatType.Conversation) return;
        foreach (var border in ChatPanel.RealizedBorders.ToList())
        {
            if (!ReferenceEquals(border.Tag, message)) continue;
            var previous = renderingMessageBorder;
            var replacement = CreateMarkdown(message.GetValueOrDefault("role") == "user" ? "Вы" : Model,
                message.GetValueOrDefault("content", ""), message.GetValueOrDefault("role") != "user");
            var content = replacement.Child;
            replacement.Child = null;
            border.Child = content;
            renderingMessageBorder = border;
            try { RenderConversationCard(message); }
            finally { renderingMessageBorder = previous; }
        }
    }

    void ExpireSurveyQuestions()
    {
        foreach (var item in conversationHistory)
        {
            if (ReadCard(item) is not { Kind: "question", State: "pending" } card) continue;
            card.State = "expired";
            StoreCard(item, card);
            RefreshSurveyMessage(item);
        }
        customAnswerTarget = null;
    }

    async void SurveyAction(Dictionary<string,string> message, string intent)
    {
        if (!CanClarify || !settings.InteractiveTaskClarification || !ReferenceEquals(PendingQuestion(), message)) return;
        var card = ReadCard(message);
        if (card is null) return;
        // Actions must never silently consume an unrelated draft.
        if (!string.IsNullOrWhiteSpace(PromptInput.Text) || pendingAttachments.Count > 0)
        { StatusText.Text = "Сначала отправьте или сохраните черновик и вложения."; return; }
        if (intent != "advice")
        {
            card.State = intent == "skip" ? "skipped" : "expired";
            StoreCard(message, card);
            customAnswerTarget = null;
            if (intent == "finish" && activeProject is not null) activeProject.SurveyStopped = true;
            SaveProject(); RefreshSurveyMessage(message);
        }
        string text = intent switch
        {
            "advice" => $"Посоветуй по вопросу «{card.Text}». Это запрос рекомендации, а не согласование решения.",
            "skip" => $"Пропускаю вопрос «{card.Text}». Он остаётся открытым, решения нет. Продолжи уточнение при необходимости.",
            _ => "Заверши опрос. Сохрани принятые ответы и перечисли оставшиеся открытые вопросы."
        };
        await SendConversationAsync(text, intent);
    }

    void ReviseAnswer(Dictionary<string,string> message)
    {
        if (!CanClarify || !settings.InteractiveTaskClarification || ReadCard(message) is not { State: "answered" } card) return;
        var edited = EditSurveyText("Изменить ответ", card.Answer);
        if (edited is null || edited == card.Answer) return;
        // Preserve audit history; the correction explicitly supersedes the original answer.
        card.Answer = edited; card.SelectedIds.Clear();
        StoreCard(message, card);
        ExpireSurveyQuestions();
        MarkPromptsStale();
        var correction = new Dictionary<string,string>
        {
            ["role"] = "user", ["reply_to"] = card.Id,
            ["content"] = $"Исправление ответа на вопрос «{card.Text}». Предыдущий ответ заменён: {edited}. Зависимые решения требуют перепроверки.",
            ["sent_at"] = DateTime.Now.ToString("O")
        };
        conversationHistory.Add(correction);
        ChatPanel.Items.Add(HistoryRow(correction));
        SaveProject(); RefreshSurveyMessage(message);
        StatusText.Text = "Ответ исправлен. Продолжите уточнение для перепроверки зависимых решений.";
    }

    void MarkPromptsStale()
    {
        foreach (var item in conversationHistory)
        {
            if (ReadCard(item) is not { Kind: "prompt" } card) continue;
            card.NeedsUpdate = true; StoreCard(item, card); RefreshSurveyMessage(item);
        }
    }

    string BuildSurveyContext()
    {
        var result = new StringBuilder();
        result.AppendLine("# Текущая задача (не глобальные правила проекта)");
        result.AppendLine("## Цель\nНе зафиксирована отдельно: уточните по обсуждению.");
        result.AppendLine("## Требования\nТолько явно подтверждённые ответы ниже.");
        result.AppendLine("## Ограничения\nНе зафиксированы отдельно.");
        result.AppendLine("## Технологии\nСтек проекта (справочно): " + activeProject?.Stack);
        result.AppendLine("## Предпочтения\nСм. подтверждённые ответы.");
        result.AppendLine("## Согласованные решения");
        var cards = conversationHistory.Select(ReadCard).Where(c => c?.Kind == "question" &&
            (string.IsNullOrEmpty(c.DiscussionId) || c.DiscussionId == activeProject?.DiscussionId)).Cast<ConversationCard>().ToList();
        foreach (var c in cards.Where(c => c.State == "answered")) result.AppendLine($"- {c.Text}\n  Ответ пользователя: {c.Answer}");
        result.AppendLine("## Открытые вопросы");
        foreach (var c in cards.Where(c => c.State != "answered")) result.AppendLine($"- {c.Text} ({c.State}; не согласовано)");
        result.AppendLine("## Предположения и рекомендации модели\nНе являются согласованными требованиями. См. сообщения модели в обсуждении.");
        result.AppendLine($"\nПолучено ответов: {cards.Count(c => c.State == "answered")}. Открытых вопросов: {cards.Count(c => c.State != "answered")}.");
        return result.ToString();
    }

    void SurveyContext_Click(object sender, RoutedEventArgs e)
    {
        if (!CanClarify || activeProject is null) return;
        string text = BuildSurveyContext();
        if (!string.IsNullOrWhiteSpace(activeProject.TaskContext)) text += "\n## Ранее сохранённые заметки пользователя\n" + activeProject.TaskContext;
        var edited = EditSurveyText("Контекст текущей задачи — проверьте противоречия перед сохранением", text);
        if (edited is null) return;
        activeProject.TaskContext = edited;
        MarkPromptsStale(); SaveProject();
    }

    string? EditSurveyText(string title, string text)
    {
        Window dialog = new() { Owner = this, Title = title, Width = 760, Height = 580, MinWidth = 360, MinHeight = 300, WindowStartupLocation = WindowStartupLocation.CenterOwner };
        dialog.SetResourceReference(BackgroundProperty, "PanelBg");
        DockPanel root = new() { Margin = new Thickness(16) };
        TextBox editor = new() { Text = text, AcceptsReturn = true, TextWrapping = TextWrapping.Wrap, VerticalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled };
        WrapPanel footer = new();
        footer.Children.Add(CardButton("Сохранить", () => { if (!string.IsNullOrWhiteSpace(editor.Text)) dialog.DialogResult = true; }));
        footer.Children.Add(CardButton("Отмена", () => dialog.Close()));
        DockPanel.SetDock(footer, Dock.Bottom); root.Children.Add(footer); root.Children.Add(editor); dialog.Content = root;
        return dialog.ShowDialog() == true ? editor.Text : null;
    }

    async void RetryConversation_Click(object sender, RoutedEventArgs e)
    {
        if (!CanClarify || failedConversationIntent is null) { StatusText.Text = "Нет запроса для повтора."; return; }
        await SendConversationAsync("", failedConversationIntent, retry: true);
    }
}
