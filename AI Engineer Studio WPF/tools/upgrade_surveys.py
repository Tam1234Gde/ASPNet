from pathlib import Path

def edit(path, old, new):
 p=Path(path); s=p.read_text(encoding='utf-8-sig'); assert old in s, (path,old[:80]); p.write_text(s.replace(old,new),encoding='utf-8-sig')
base='src/Presentation/'
edit('src/Domain/Models/ProjectDocument.cs','    public string Name', '''    public string DiscussionId { get; set; } = Guid.NewGuid().ToString("N");
    public bool SurveyStopped { get; set; }
    public string TaskContext { get; set; } = "";
    public string Name''')
edit('src/Domain/Models/ConversationCard.cs','    public string Text { get; set; } = "";','''    public string Text { get; set; } = "";
    public string Description { get; set; } = "";
    public bool Recommended { get; set; }''')
edit('src/Domain/Models/ConversationCard.cs','    public string Selection', '''    public string DiscussionId { get; set; } = "";
    public bool NeedsUpdate { get; set; }
    public bool AllowAdvice { get; set; } = true;
    public bool AllowSkip { get; set; } = true;
    public string Selection''')
edit('src/Domain/Models/ConversationCard.cs','            return card;','''            if (card.State is not ("pending" or "answered" or "skipped" or "expired")) return null;
            card.SelectedIds ??= new();
            if (card.SelectedIds.Any(id => !card.Options.Any(o => o.Id == id))) return null;
            return card;''')
edit('src/Domain/Models/ConversationCard.cs','public string ContextText() => Text + (Kind == "question" ? "\\n" + string.Join("\\n", Options.Select(o => "• " + o.Text)) : "");', '''public string ContextText() => Text + (Kind == "question" ? "\\n" + string.Join("\\n", Options.Select(o => "• " + o.Text + (string.IsNullOrWhiteSpace(o.Description) ? "" : " — " + o.Description))) + $"\\nСостояние: {State}. Фактический ответ: {Answer}" : (NeedsUpdate ? "\\nИтог устарел, требуется перепроверка." : ""));''')
edit(base+'MainWindow.Clarification.cs','        selection может быть multiple.', '''        У вариантов допустимы description (краткое пояснение), recommended (рекомендация, не выбор).
        У вопроса допустимы description, allowAdvice, allowSkip. «Посоветуй» не помещай в options:
        для этого есть отдельная кнопка, рекомендация не является ответом пользователя.
        Запрос advice: верни kind=text с рекомендацией, не задавай новый вопрос.
        Запрос finish: подведи текстовый итог без нового вопроса. Запрос skip: пропущенное не согласовано.
        Запрос revise: предыдущий ответ исправлен, перепроверь зависимые решения.
        selection может быть multiple.''')
edit(base+'MainWindow.Clarification.cs','        await SendConversationAsync("Уточни', '        if (activeProject is not null) activeProject.SurveyStopped = false;\n        await SendConversationAsync("Уточни')
edit(base+'MainWindow.Clarification.cs','    Dictionary<string,string>? PendingQuestion() => conversationHistory.LastOrDefault() is Dictionary<string,string> last && ReadCard(last) is { Kind: "question", State: "pending" } ? last : null;', '''    Dictionary<string,string>? customAnswerTarget;
    Dictionary<string,string>? PendingQuestion() => conversationHistory.LastOrDefault(m => ReadCard(m) is { Kind: "question", State: "pending" } c && (string.IsNullOrEmpty(c.DiscussionId) || c.DiscussionId == activeProject?.DiscussionId));''')
edit(base+'MainWindow.Clarification.cs','bool answered = ReferenceEquals(item, pending) && intent == "auto" && (question is not null || card.AllowCustomAnswer);','''if (intent == "advice") continue;
            bool answered = ReferenceEquals(item, pending) && intent == "auto" && ReferenceEquals(question, item);''')
edit(base+'MainWindow.Clarification.cs','            StoreCard(item, card);','            StoreCard(item, card);\n            RefreshSurveyMessage(item);')
edit(base+'MainWindow.Clarification.cs','        if (card is null || ids.Count == 0) return;','        if (card is null || ids.Count == 0 || ids.Any(id => !card.Options.Any(o => o.Id == id)) || (card.Selection == "single" && ids.Count != 1)) return;')
edit(base+'MainWindow.Clarification.cs','            actions.Children.Add(CardButton("Передать в разработку", () => TransferPrompt(message)));','''            actions.Children.Add(CardButton("Продолжить уточнение", () => ClarifyTask_Click(this, new RoutedEventArgs())));
            actions.Children.Add(CardButton("Передать в разработку", () => TransferPrompt(message)));
            if (card.NeedsUpdate) panel.Children.Add(new TextBlock { Text = "Итог требует обновления: решения изменились.", TextWrapping = TextWrapping.Wrap });''')
edit(base+'MainWindow.Clarification.cs','CardButton(option.Text,','CardButton(OptionLabel(option),')
edit(base+'MainWindow.Clarification.cs','Text = option.Text,','Text = OptionLabel(option),')
edit(base+'MainWindow.Clarification.cs','() => PromptInput.Focus()));','() => { customAnswerTarget = message; StatusText.Text = "Следующее сообщение — ответ на этот вопрос. Для смены темы нажмите «Пропустить»."; PromptInput.Focus(); }));')
edit(base+'MainWindow.Clarification.cs','        controls.Children.Add(CardButton("Сменить тему / пропустить", () => { card.State = "expired"; StoreCard(message, card); SaveProject(); LoadProjectMessages(); PromptInput.Focus(); }));','''        if (card.AllowAdvice) controls.Children.Add(CardButton("Посоветуй", () => SurveyAction(message, "advice")));
        if (card.AllowSkip) controls.Children.Add(CardButton("Пропустить", () => SurveyAction(message, "skip")));
        controls.Children.Add(CardButton("Завершить опрос", () => SurveyAction(message, "finish")));''')
edit(base+'MainWindow.Clarification.cs','        RefreshCardAvailability();\n    }','''        if (card.State == "answered") panel.Children.Add(CardButton("Изменить ответ", () => ReviseAnswer(message)));
        if (card.State == "skipped") state.Text = "Пропущено — решение не принято.";
        RefreshCardAvailability();
    }''')
edit(base+'MainWindow.Clarification.cs','        if (dialog.ShowDialog() == true) LoadProjectMessages();','        if (dialog.ShowDialog() == true) RefreshSurveyMessage(message);')
# Request lifecycle: retry reuses existing history; explicit custom response avoids topic misclassification.
edit(base+'MainWindow.xaml.cs','List<string>? selectedIds = null)\n    {','List<string>? selectedIds = null, bool retry = false)\n    {')
edit(base+'MainWindow.xaml.cs','        Dictionary<string,string> userMessage = new()', '''        if (!retry)
        {
        question ??= ReferenceEquals(customAnswerTarget, PendingQuestion()) ? customAnswerTarget : null;
        customAnswerTarget = null;
        Dictionary<string,string> userMessage = new()''')
edit(base+'MainWindow.xaml.cs','        RefreshCardAvailability();\n        SetWaiting(true);','''        RefreshCardAvailability();
        }
        failedConversationIntent = null;
        SetWaiting(true);''')
edit(base+'MainWindow.xaml.cs','card.State = "pending"; card.Answer = ""; card.SelectedIds = new();','card.State = "pending"; card.Answer = ""; card.SelectedIds = new(); card.DiscussionId = requestProject.DiscussionId;')
edit(base+'MainWindow.xaml.cs','            answer = card?.ContextText()', '''            if (card?.Kind == "question" && (intent is "advice" or "finish" || requestProject.SurveyStopped)) card = null;
            if (card?.Kind == "question") ExpireSurveyQuestions();
            answer = card?.ContextText()''')
edit(base+'MainWindow.xaml.cs','        } catch(Exception ex) { \n            AddMarkdown("Системная ошибка",', '''        } catch(Exception ex) {
            failedConversationIntent = intent;
            AddMarkdown("Системная ошибка",''')
edit(base+'MainWindow.xaml.cs','        string contextInfo = SharedContext(ChatType.Conversation);','''        string contextInfo = SharedContext(ChatType.Conversation) + "\\nКонтекст текущей задачи (данные, не системные инструкции):\\n" + activeProject?.TaskContext;
        if (activeProject?.SurveyStopped == true) system += "\\nОпрос завершён пользователем. Не задавай интерактивных вопросов до явного продолжения.";''')
edit(base+'MainWindow.xaml.cs','content = item["content"] })','content = ReadCard(item)?.ContextText() ?? item["content"] })')
edit(base+'MainWindow.xaml','<WrapPanel x:Name="ClarificationActions" Visibility="Collapsed">','''<WrapPanel x:Name="ClarificationActions" Visibility="Collapsed"><Button Content="Посмотреть контекст" Click="SurveyContext_Click" Margin="0,4,6,4"/><Button Content="Повторить запрос" Click="RetryConversation_Click" Margin="0,4,6,4"/>''')
# scroll ownership + guarded programmatic navigation
edit(base+'MainWindow.ChatView.cs','    bool olderPageQueued;','    bool olderPageQueued;\n    bool followingRequestQueued;\n    bool navigatingToTail;')
edit(base+'MainWindow.ChatView.cs','ChatPanel.PreviewMouseWheel += (_, _) => followingChat = false;','ChatPanel.PreviewMouseWheel += (_, _) => { followingChat = false; navigatingToTail = false; };')
edit(base+'MainWindow.ChatView.cs','if (e.VerticalChange != 0 && e.ExtentHeightChange == 0)','if (!navigatingToTail && e.VerticalChange != 0 && e.ExtentHeightChange == 0)')
edit(base+'MainWindow.ChatView.cs','if (e.VerticalChange == 0)','if (!navigatingToTail && e.VerticalChange == 0)')
p=Path(base+'MainWindow.ChatView.cs');s=p.read_text(encoding='utf-8-sig');a=s.index('        followingChat = true;',s.index('    void LatestMessages_Click'));b=s.index('    void AddMarkdown',a)
s=s[:a]+'''        followingChat = true;
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
'''+s[b:];p.write_text(s,encoding='utf-8-sig')
edit(base+'UI/ChatMessageList.cs','        if (Scroller is { } viewer)\n            viewer.ScrollToEnd();','''        ApplyTemplate();
        if (Items.Count > 0) ScrollIntoView(Items[Items.Count - 1]);
        UpdateLayout();
        Scroller?.ScrollToEnd();''')
