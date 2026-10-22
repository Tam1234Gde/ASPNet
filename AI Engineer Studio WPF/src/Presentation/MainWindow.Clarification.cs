using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using AIEngineerStudio.Models;

namespace AIEngineerStudio;

public partial class MainWindow
{
    const string ClarificationProtocol = """
        Возвращай один JSON-объект без Markdown-ограждений. Формат обычного ответа:
        {"kind":"text","text":"ответ"}.
        Только если есть существенная неопределённость, задай один вопрос:
        {"kind":"question","id":"unique-id","text":"вопрос","selection":"single","allowCustomAnswer":true,"options":[{"id":"a","text":"вариант 1"},{"id":"b","text":"вариант 2"}]}.
        У вариантов допустимы description (краткое пояснение), recommended (рекомендация, не выбор).
        У вопроса допустимы description, allowAdvice, allowSkip. «Посоветуй» не помещай в options:
        для этого есть отдельная кнопка, рекомендация не является ответом пользователя.
        Запрос advice: верни kind=text с рекомендацией, не задавай новый вопрос.
        Запрос finish: подведи текстовый итог без нового вопроса. Запрос skip: пропущенное не согласовано.
        Запрос revise: предыдущий ответ исправлен, перепроверь зависимые решения.
        selection может быть multiple. От 2 до 12 вариантов с уникальными стабильными ID.
        Не превращай обычную беседу в анкету, учитывай предыдущие ответы. Пользователь может сменить тему.
        Для запроса prompt возвращай {"kind":"prompt","text":"готовый промпт"} даже при оставшихся вопросах.
        Промпт содержит разделы: цель, требования, согласованные решения, ограничения, критерии готовности,
        открытые вопросы и предположения. Не выдумывай требования. Предположения не являются согласованными решениями.
        Запрос clarify означает уточнить текущую задачу. Никаких действий или инструментов.
        """;

    static ConversationCard? ReadCard(Dictionary<string,string> message) =>
        message.TryGetValue("card", out string? raw) ? ConversationCard.Parse(raw) : null;
    static void StoreCard(Dictionary<string,string> message, ConversationCard card) => message["card"] = JsonSerializer.Serialize(card);
    bool CanClarify => !loadingProject && activeProject is not null && activeChatType == ChatType.Conversation && !busyChats.Contains(ChatType.Conversation) && !importingAttachments;
    void UpdateClarificationActions()
    {
        if (ClarificationActions is null) return;
        ClarificationActions.Visibility = activeChatType == ChatType.Conversation && settings.InteractiveTaskClarification ? Visibility.Visible : Visibility.Collapsed;
        ClarificationActions.IsEnabled = CanClarify;
    }
    async void ClarifyTask_Click(object sender, RoutedEventArgs e)
    {
        if (!CanClarify || !settings.InteractiveTaskClarification) return;
        if (activeProject is not null) activeProject.SurveyStopped = false;
        await SendConversationAsync("Уточни текущую задачу. " + PromptInput.Text.Trim(), "clarify");
    }
    async void GeneratePrompt_Click(object sender, RoutedEventArgs e)
    {
        if (!CanClarify || !settings.InteractiveTaskClarification) return;
        await SendConversationAsync("Сформируй готовый промпт для разработки по текущему обсуждению. " + PromptInput.Text.Trim(), "prompt");
    }
    Dictionary<string,string>? customAnswerTarget;
    Dictionary<string,string>? PendingQuestion() => conversationHistory.LastOrDefault(m => ReadCard(m) is { Kind: "question", State: "pending" } c && (string.IsNullOrEmpty(c.DiscussionId) || c.DiscussionId == activeProject?.DiscussionId));
    void LinkQuestionAnswer(Dictionary<string,string> user, string text, string intent, Dictionary<string,string>? question, List<string>? ids)
    {
        Dictionary<string,string>? pending = PendingQuestion();
        foreach (Dictionary<string,string> item in conversationHistory)
        {
            ConversationCard? card = ReadCard(item);
            if (card is not { Kind: "question", State: "pending" }) continue;
            if (intent == "advice") continue;
            bool answered = ReferenceEquals(item, pending) && intent == "auto" && ReferenceEquals(question, item);
            card.State = answered ? "answered" : "expired";
            if (answered)
            {
                card.Answer = text; card.SelectedIds = ids ?? new();
                user["reply_to"] = card.Id;
                user["content"] = $"В ответ на вопрос «{card.Text}» (возможна смена темы):\n{text}";
            }
            StoreCard(item, card);
            RefreshSurveyMessage(item);
        }
    }
    async void AnswerQuestionText(Dictionary<string,string> message, string text)
    {
        if (!CanClarify || !settings.InteractiveTaskClarification || !ReferenceEquals(PendingQuestion(), message)) return;
        if (pendingAttachments.Count > 0) { StatusText.Text = "Уберите вложения перед ответом."; return; }
        await SendConversationAsync(text, "auto", message, new List<string>());
    }

    async void AnswerQuestion(Dictionary<string,string> message, List<string> ids)
    {
        if (!CanClarify || !settings.InteractiveTaskClarification || !ReferenceEquals(PendingQuestion(), message)) return;
        ConversationCard? card = ReadCard(message);
        if (card is null || ids.Count == 0 || ids.Any(id => !card.Options.Any(o => o.Id == id)) || (card.Selection == "single" && ids.Count != 1)) return;
        if (pendingAttachments.Count > 0) { StatusText.Text = "Уберите вложения перед ответом."; return; }
        if (!string.IsNullOrWhiteSpace(PromptInput.Text)) { StatusText.Text = "Отправьте или сохраните текст черновика перед выбором варианта."; return; }
        await SendConversationAsync(string.Join("; ", card.Options.Where(o => ids.Contains(o.Id)).Select(o => o.Text)), "auto", message, ids);
    }
    void RefreshCardAvailability()
    {
        foreach (Border border in ChatPanel.RealizedBorders)
            if (border.Tag is Dictionary<string,string> message && border.Child is StackPanel panel)
                foreach (FrameworkElement child in panel.Children.OfType<FrameworkElement>())
                    if (Equals(child.Tag, "questionControls")) child.IsEnabled = CanClarify && settings.InteractiveTaskClarification && ReferenceEquals(PendingQuestion(), message);
    }
    void RenderConversationCard(Dictionary<string,string> message)
    {
        ConversationCard? card = ReadCard(message);
        if (renderingMessageBorder is null) { AddMessageDecoration(() => RenderConversationCard(message)); return; }
        if (card is null || renderingMessageBorder.Child is not StackPanel panel) return;
        Border border = renderingMessageBorder;
        // Rebuild this card only, using current model state rather than stale message text.
        panel.Children.Clear();
        panel.Children.Add(AIEngineerStudio.UI.MarkdownRenderer.Render(card.Text));
        if (!string.IsNullOrWhiteSpace(card.Description)) panel.Children.Add(new TextBlock { Text = card.Description, TextWrapping = TextWrapping.Wrap });
        border.Tag = message;
        if (card.Kind == "prompt")
        {
            WrapPanel actions = new();
            actions.Children.Add(CardButton("Копировать промпт", () => { try { Clipboard.SetText(ReadCard(message)!.Text); } catch (Exception ex) { StatusText.Text = ex.Message; } }));
            actions.Children.Add(CardButton("Редактировать", () => EditPrompt(message)));
            actions.Children.Add(CardButton("Продолжить уточнение", () => ClarifyTask_Click(this, new RoutedEventArgs())));
            actions.Children.Add(CardButton("Передать в разработку", () => TransferPrompt(message)));
            if (card.NeedsUpdate) panel.Children.Add(new TextBlock { Text = "Итог требует обновления: решения изменились.", TextWrapping = TextWrapping.Wrap });
            panel.Children.Add(actions);
            return;
        }
        StackPanel controls = new() { Tag = "questionControls" };
        List<CheckBox> choices = new();
        TextBox? customInput = null;
        card.SelectedIds ??= new();
        foreach (ChoiceOption option in card.Options)
        {
            if (card.Selection == "single") controls.Children.Add(CardButton(OptionLabel(option), () => AnswerQuestion(message, new List<string> { option.Id })));
            else
            {
                CheckBox choice = new() { Content = new TextBlock { Text = OptionLabel(option), TextWrapping = TextWrapping.Wrap }, Tag = option.Id, IsChecked = card.SelectedIds?.Contains(option.Id) == true, Margin = new Thickness(4) };
                choice.SetResourceReference(Control.ForegroundProperty, "Text");
                choice.Checked += (_,_) => { card.SelectedIds ??= new(); if (!card.SelectedIds.Contains(option.Id)) card.SelectedIds.Add(option.Id); StoreCard(message, card); };
                choice.Unchecked += (_,_) => { card.SelectedIds?.Remove(option.Id); StoreCard(message, card); };
                choices.Add(choice); controls.Children.Add(choice);
            }
        }
        if (card.Selection == "multiple") controls.Children.Add(CardButton("Ответить", () => AnswerQuestion(message, choices.Where(c => c.IsChecked == true).Select(c => (string)c.Tag).ToList())));
        if (card.AllowCustomAnswer)
        {
            customInput = new TextBox { Text = card.CustomDraft, MinHeight = 34, Margin = new Thickness(0, 6, 0, 4), TextWrapping = TextWrapping.Wrap, AcceptsReturn = true, MaxHeight = 120 };
            customInput.SetResourceReference(Control.ForegroundProperty, "Text");
            customInput.TextChanged += (_, _) => { card.CustomDraft = customInput.Text; StoreCard(message, card); };
            controls.Children.Add(customInput);
            controls.Children.Add(CardButton("Отправить свой ответ", () =>
            {
                var value = customInput.Text.Trim();
                if (value.Length == 0) { StatusText.Text = "Введите собственный ответ."; return; }
                AnswerQuestionText(message, value);
            }));
        }
        if (card.AllowAdvice) controls.Children.Add(CardButton("Посоветуй", () => SurveyAction(message, "advice")));
        if (card.AllowSkip) controls.Children.Add(CardButton("Пропустить", () => SurveyAction(message, "skip")));
        if (card.AllowFinish) controls.Children.Add(CardButton("Завершить опрос", () => SurveyAction(message, "finish")));
        controls.IsEnabled = CanClarify && settings.InteractiveTaskClarification && ReferenceEquals(PendingQuestion(), message);
        panel.Children.Add(controls);
        TextBlock state = new() { Text = card.State == "answered" ? "Ответ: " + card.Answer : card.State == "pending" ? "Выберите вариант или продолжите беседу." : "Вопрос больше не актуален", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(4) };
        state.SetResourceReference(TextBlock.ForegroundProperty, "Muted"); panel.Children.Add(state);
        if (card.State == "answered") panel.Children.Add(CardButton("Изменить ответ", () => ReviseAnswer(message)));
        if (card.State == "skipped") state.Text = "Пропущено — решение не принято.";
        RefreshCardAvailability();
    }
    Button CardButton(string text, Action action)
    {
        Button button = new() { Content = new TextBlock { Text = text, TextWrapping = TextWrapping.Wrap }, Margin = new Thickness(0,4,6,4), Padding = new Thickness(8,5,8,5), HorizontalContentAlignment = HorizontalAlignment.Left };
        button.Click += (_, _) => { try { action(); } catch (Exception ex) { StatusText.Text = "Ошибка: " + ex.Message; } };
        return button;
    }
    void EditPrompt(Dictionary<string,string> message)
    {
        ConversationCard? card = ReadCard(message); if (card is null) return;
        Window dialog = new() { Owner = this, Title = "Редактировать промпт", Width = 720, Height = 540, MinWidth = 360, MinHeight = 300, WindowStartupLocation = WindowStartupLocation.CenterOwner };
        dialog.SetResourceReference(BackgroundProperty, "PanelBg");
        DockPanel root = new() { Margin = new Thickness(16) };
        TextBox editor = new() { Text = card.Text, AcceptsReturn = true, TextWrapping = TextWrapping.Wrap, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
        WrapPanel footer = new();
        footer.Children.Add(CardButton("Сохранить", () => { if (string.IsNullOrWhiteSpace(editor.Text)) return; card.Text = editor.Text; StoreCard(message, card); message["content"] = card.Text; SaveProject(); dialog.DialogResult = true; }));
        footer.Children.Add(CardButton("Отмена", () => dialog.Close()));
        DockPanel.SetDock(footer, Dock.Bottom); root.Children.Add(footer); root.Children.Add(editor); dialog.Content = root;
        if (dialog.ShowDialog() == true) RefreshSurveyMessage(message);
    }
    void TransferPrompt(Dictionary<string,string> message)
    {
        ConversationCard? card = ReadCard(message); if (card is null || activeProject is null) return;
        SwitchChat(ChatType.Development);
        if (activeChatType != ChatType.Development) return;
        string draft = PromptInput.Text;
        if (!string.IsNullOrWhiteSpace(draft))
        {
            MessageBoxResult choice = MessageBox.Show(this, "В разработке уже есть черновик.\nДа — заменить\nНет — добавить\nОтмена — сохранить без изменений", "Передать промпт", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (choice == MessageBoxResult.Cancel) return;
            if (choice == MessageBoxResult.No) { PromptInput.Text = draft + "\n\n" + card.Text; SaveAttachmentDraft(); return; }
        }
        PromptInput.Text = card.Text; SaveAttachmentDraft();
        StatusText.Text = "Промпт в черновике. Для запуска отправьте его самостоятельно.";
    }
}
