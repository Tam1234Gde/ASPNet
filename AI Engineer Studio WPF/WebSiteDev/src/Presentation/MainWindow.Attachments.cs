using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using AIEngineerStudio.Models;
using AIEngineerStudio.Services;
namespace AIEngineerStudio;

public partial class MainWindow
{
    readonly List<Attachment> pendingAttachments = new();
    readonly WrapPanel attachmentTray = new();
    readonly AttachmentService attachmentService = new();
    CancellationTokenSource? mediaCancellation;
    bool importingAttachments;
    string mediaMode = "agent", mediaModel = "", mediaSize = "1024x1024", mediaSeconds = "4", mediaVideoId = "";
    string DraftPath => Path.Combine(AttachmentService.Root, "draft-" + Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(projectPath))) + ".json");
    bool restoringChatDraft;
    string ConversationDraftPath => Path.ChangeExtension(DraftPath, ".conversation.json");
    string ActiveDraftPath => activeChatType == ChatType.Conversation ? ConversationDraftPath : DraftPath;
    sealed record Draft(string Text, List<Attachment> Attachments);
    void InitializeAttachments()
    {
        Grid grid = (Grid)((FrameworkElement)PromptInput.Parent).Parent;
        grid.RowDefinitions[2].Height = GridLength.Auto;
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        UIElement footer = grid.Children.Cast<UIElement>().First(element => Grid.GetRow(element) == 2);
        Grid.SetRow(footer, 3);
        grid.RowDefinitions[3].Height = new GridLength(34);
        grid.RowDefinitions[2].Height = GridLength.Auto;
        Grid.SetRow(attachmentTray, 2);
        grid.Children.Add(new ScrollViewer { Content = attachmentTray, MaxHeight = 260, VerticalScrollBarVisibility = ScrollBarVisibility.Auto });
        Grid.SetRow((UIElement)attachmentTray.Parent, 2);
        Composer.AllowDrop = true;
        Composer.PreviewDragOver += (_, e) => { e.Effects = !isWaiting && e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None; e.Handled = true; };
        Composer.PreviewDrop += async (_, e) => { e.Handled = true; if (e.Data.GetData(DataFormats.FileDrop) is string[] paths) await ImportAttachments(paths); };
        DataObject.AddPastingHandler(PromptInput, async (_, e) => {
            if (Clipboard.ContainsFileDropList()) { e.CancelCommand(); await ImportAttachments(Clipboard.GetFileDropList().Cast<string>()); }
            else if (Clipboard.ContainsImage() && !isWaiting)
            {
                e.CancelCommand();
                try {
                    Directory.CreateDirectory(AttachmentService.Root);
                    string path = Path.Combine(AttachmentService.Root, Guid.NewGuid().ToString("N") + ".png");
                    PngBitmapEncoder encoder = new(); encoder.Frames.Add(BitmapFrame.Create(Clipboard.GetImage()));
                    using (FileStream stream = File.Create(path)) encoder.Save(stream);
                    await ImportAttachments([path]);
                } catch (Exception ex) { StatusText.Text = ex.Message; }
            }
        });
        PromptInput.TextChanged += (_, _) => SaveAttachmentDraft();
        Closing += (_, _) => { SaveAttachmentDraft(); mediaCancellation?.Cancel(); };
    }
    void SaveAttachmentDraft()
    {
        if (activeProject == null || restoringChatDraft) return;
        try { Directory.CreateDirectory(AttachmentService.Root); File.WriteAllText(ActiveDraftPath, JsonSerializer.Serialize(new Draft(PromptInput.Text, pendingAttachments))); }
        catch (Exception ex) { StatusText.Text = "Не удалось сохранить черновик: " + ex.Message; }
    }
    void LoadAttachmentDraft()
    {
        bool previousRestoring = restoringChatDraft;
        restoringChatDraft = true;
        pendingAttachments.Clear();
        Draft? draft = null;
        try { if (File.Exists(ActiveDraftPath)) draft = JsonSerializer.Deserialize<Draft>(File.ReadAllText(ActiveDraftPath)); } catch (Exception ex) { StatusText.Text = ex.Message; }
        pendingAttachments.AddRange(draft?.Attachments ?? []);
        PromptInput.Text = draft?.Text ?? "";
        RefreshAttachmentTray();
        restoringChatDraft = previousRestoring;
    }
    async void AttachFiles_Click(object sender, RoutedEventArgs e)
    {
        if (isWaiting) return;
        OpenFileDialog dialog = new() { Multiselect = true, Title = "Вложения · до 8 файлов, 25 МБ каждый", Filter = "Все файлы|*.*|Изображения|*.png;*.jpg;*.jpeg;*.webp;*.gif|Видео|*.mp4;*.webm;*.mov|Документы|*.pdf;*.txt;*.docx" };
        if (dialog.ShowDialog(this) == true) await ImportAttachments(dialog.FileNames);
    }
    async Task ImportAttachments(IEnumerable<string> paths)
    {
        if (activeChatType == ChatType.Conversation) { StatusText.Text = "Общение — текстовый режим. Для вложений используйте медиа в Разработке."; return; }
        if (isWaiting || importingAttachments) return;
        importingAttachments = true; SendButton.IsEnabled = false;
        try {
            foreach (string path in paths)
            {
                if (pendingAttachments.Count >= 8) throw new InvalidDataException("Не больше 8 вложений в сообщении.");
                StatusText.Text = "Подготовка: " + Path.GetFileName(path);
                pendingAttachments.Add(await AttachmentService.ImportAsync(path, CancellationToken.None));
                RefreshAttachmentTray(); SaveAttachmentDraft();
            }
            StatusText.Text = "Вложения готовы. Отправка начнётся вместе с сообщением.";
        } catch (Exception ex) { MessageBox.Show(this, ex.Message, "Вложения"); }
        finally { importingAttachments = false; SendButton.IsEnabled = true; }
    }
    void RefreshAttachmentTray()
    {
        attachmentTray.Children.Clear();
        foreach (Attachment attachment in pendingAttachments.ToList()) attachmentTray.Children.Add(AttachmentCard(attachment, true));
    }
    Border AttachmentCard(Attachment attachment, bool removable)
    {
        StackPanel stack = new();
        Border card = new() { Child = stack, Width = 164, Padding = new Thickness(9), Margin = new Thickness(0, 5, 8, 5), CornerRadius = new CornerRadius(9), BorderThickness = new Thickness(1) };
        card.SetResourceReference(BackgroundProperty, "PanelBg"); card.SetResourceReference(Border.BorderBrushProperty, "Border");
        if (attachment.Mime.StartsWith("image/") && File.Exists(attachment.Path))
        {
            try { BitmapImage bitmap = new(); bitmap.BeginInit(); bitmap.CacheOption = BitmapCacheOption.OnLoad; bitmap.DecodePixelWidth = 280; bitmap.UriSource = new Uri(attachment.Path); bitmap.EndInit(); bitmap.Freeze(); stack.Children.Add(new Image { Source = bitmap, Height = 80, Stretch = Stretch.Uniform }); }
            catch { stack.Children.Add(new TextBlock { Text = "Превью недоступно" }); }
        }
        TextBlock title = new() { Text = attachment.Name, TextTrimming = TextTrimming.CharacterEllipsis, ToolTip = attachment.Name, Margin = new Thickness(0, 5, 0, 3) }; title.SetResourceReference(TextBlock.ForegroundProperty, "Body"); stack.Children.Add(title);
        TextBlock summary = new() { Text = attachment.Summary, FontSize = 10 }; summary.SetResourceReference(TextBlock.ForegroundProperty, "Muted"); stack.Children.Add(summary);
        Button action = new() { Content = removable ? "✕ Убрать" : "Просмотр / сохранить", Margin = new Thickness(0, 6, 0, 0), Padding = new Thickness(4) };
        action.Click += (_, _) => { if (removable) { if (isWaiting) return; pendingAttachments.RemoveAll(file => file.Path == attachment.Path); RefreshAttachmentTray(); SaveAttachmentDraft(); } else PreviewAttachment(attachment); };
        stack.Children.Add(action); return card;
    }
    void ShowMessageAttachments(IReadOnlyList<Attachment>? attachments)
    {
        if (attachments == null || attachments.Count == 0) return;
        if (renderingMessageBorder is null) { AddMessageDecoration(() => ShowMessageAttachments(attachments)); return; }
        WrapPanel panel = new(); foreach (Attachment file in attachments) panel.Children.Add(AttachmentCard(file, false));
        if (renderingMessageBorder?.Child is StackPanel message) message.Children.Add(panel);
        FollowChatEnd();
    }
    void PreviewAttachment(Attachment attachment)
    {
        if (!File.Exists(attachment.Path)) { MessageBox.Show(this, "Файл вложения недоступен."); return; }
        Window viewer = new() { Owner = this, Title = attachment.Name, Width = 800, Height = 600, WindowStartupLocation = WindowStartupLocation.CenterOwner };
        DockPanel panel = new() { Margin = new Thickness(16) }; viewer.Content = panel;
        Button save = new() { Content = "Сохранить как…", Margin = new Thickness(0, 8, 0, 0) }; DockPanel.SetDock(save, Dock.Bottom); panel.Children.Add(save);
        save.Click += (_, _) => { SaveFileDialog dialog = new() { FileName = attachment.Name }; if (dialog.ShowDialog(viewer) == true) try { File.Copy(attachment.Path, dialog.FileName, true); } catch (Exception ex) { MessageBox.Show(viewer, ex.Message); } };
        if (attachment.Mime.StartsWith("image/"))
        {
            try { BitmapImage bitmap = new(); bitmap.BeginInit(); bitmap.CacheOption = BitmapCacheOption.OnLoad; bitmap.DecodePixelWidth = 1600; bitmap.UriSource = new Uri(attachment.Path); bitmap.EndInit(); panel.Children.Add(new Image { Source = bitmap, Stretch = Stretch.Uniform }); } catch { panel.Children.Add(new TextBlock { Text = "Формат не поддерживается просмотрщиком. Сохраните файл." }); }
        }
        else if (attachment.Mime.StartsWith("video/") || attachment.Mime.StartsWith("audio/"))
        {
            MediaElement player = new() { Source = new Uri(attachment.Path), LoadedBehavior = MediaState.Manual, UnloadedBehavior = MediaState.Close };
            StackPanel controls = new() { Orientation = Orientation.Horizontal }; DockPanel.SetDock(controls, Dock.Bottom); panel.Children.Add(controls);
            Button play = new() { Content = "▶" }; play.Click += (_, _) => player.Play(); controls.Children.Add(play);
            Button pause = new() { Content = "Ⅱ" }; pause.Click += (_, _) => player.Pause(); controls.Children.Add(pause);
            player.MediaFailed += (_, _) => MessageBox.Show(viewer, "Кодек не поддерживается системой. Сохраните файл и откройте своим плеером.");
            viewer.Closed += (_, _) => player.Close(); panel.Children.Add(player);
        }
        else panel.Children.Add(new TextBlock { Text = attachment.Name + "\n" + attachment.Summary + "\nСохраните файл для просмотра. Автоматический запуск отключён.", TextWrapping = TextWrapping.Wrap });
        viewer.ShowDialog();
    }
    static IReadOnlyList<Attachment> MessageAttachments(Dictionary<string, string> message) => message.TryGetValue("attachments", out string? json) ? JsonSerializer.Deserialize<List<Attachment>>(json) ?? [] : [];
    static Dictionary<string, string> StoredMessage(ChatMessage message) => new() { ["role"] = message.Role, ["content"] = message.Content, ["attachments"] = JsonSerializer.Serialize(message.Attachments ?? []), ["card"] = JsonSerializer.Serialize(message.Card), ["reply_to"] = message.ReplyToQuestionId ?? "", ["sent_at"] = message.SentAt?.ToString("O") ?? "", ["duration_ms"] = message.DurationMs?.ToString() ?? "" };
    void MediaTools_Click(object sender, RoutedEventArgs e)
    {
        if (isWaiting) return;
        Window dialog = new() { Owner = this, Title = "Медиа · назначение и параметры", Width = 540, Height = 650, WindowStartupLocation = WindowStartupLocation.CenterOwner, Background = (Brush)FindResource("AppBg") };
        StackPanel panel = new() { Margin = new Thickness(24) }; dialog.Content = new ScrollViewer { Content = panel, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
        panel.SetResourceReference(Panel.BackgroundProperty, "AppBg");
        TextBlock heading = new() { Text = "✦ Мультимодальная студия", FontSize = 22, Margin = new Thickness(0, 0, 0, 16) }; heading.SetResourceReference(TextBlock.ForegroundProperty, "Text"); panel.Children.Add(heading);
        panel.Children.Add(new TextBlock { Text = "Эти режимы не запускают инструменты агента. API должен поддерживать выбранный endpoint и модель. Видео: создание по тексту/фото и ремикс серверного ролика по ID. Импорт локального видео для монтажа не поддерживается.", TextWrapping = TextWrapping.Wrap });
        ComboBox modes = new() { ItemsSource = new[] { "Диалог с вложениями", "Генерация изображения", "Редактирование изображения", "Генерация видео / анимация фото", "Редактирование видео · ремикс по ID", "Агент · инструменты проекта" }, SelectedIndex = mediaMode switch { "image" => 1, "edit" => 2, "video" => 3, "remix" => 4, "agent" => 5, _ => 0 }, Margin = new Thickness(0, 14, 0, 8) }; panel.Children.Add(modes);
        panel.Children.Add(new TextBlock { Text = "Модель генерации" });
        ComboBox model = new() { IsEditable = true, IsTextSearchEnabled = false, MinWidth = 280, HorizontalAlignment = HorizontalAlignment.Stretch }; panel.Children.Add(model);
        TextBlock modelHint = new() { TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 6, 0, 0) }; panel.Children.Add(modelHint);
        string imageSelection = settings.MediaImageModel;
        string videoSelection = settings.MediaVideoModel;
        int previousMode = -1;
        panel.Children.Add(new TextBlock { Text = "Размер (изображения: 1024x1024, видео: 1280x720)", Margin = new Thickness(0, 10, 0, 0) }); TextBox size = new() { Text = mediaSize }; panel.Children.Add(size);
        panel.Children.Add(new TextBlock { Text = "Длительность видео, секунды", Margin = new Thickness(0, 10, 0, 0) }); TextBox seconds = new() { Text = mediaSeconds }; panel.Children.Add(seconds);
        panel.Children.Add(new TextBlock { Text = "ID исходного видео (для ремикса)", Margin = new Thickness(0, 10, 0, 0) }); TextBox videoId = new() { Text = mediaVideoId }; panel.Children.Add(videoId);
        void UpdateMediaModel()
        {
            bool imageMode = modes.SelectedIndex is 1 or 2;
            bool videoMode = modes.SelectedIndex == 3;
            if (previousMode is 1 or 2) imageSelection = model.Text.Trim();
            if (previousMode == 3) videoSelection = model.Text.Trim();
            model.ItemsSource = imageMode ? AttachmentService.SupportedImageModels : videoMode ? new[] { "sora-2", "sora-2-pro" } : Array.Empty<string>();
            model.Text = imageMode ? imageSelection : videoMode ? videoSelection : "";
            modelHint.Text = imageMode ? "Выберите модель изображения. Доступность зависит от вашего API." : videoMode ? "Выберите пример модели или введите ID от провайдера. Наличие в списке не гарантирует доступность API." : modes.SelectedIndex == 4 ? "Для ремикса модель определяется исходным видео." : "Используется модель диалога из верхней панели.";
            previousMode = modes.SelectedIndex;
            model.IsEnabled = imageMode || videoMode;
            size.IsEnabled = imageMode || videoMode;
            seconds.IsEnabled = videoMode;
            videoId.IsEnabled = modes.SelectedIndex == 4;
            if (imageMode && string.IsNullOrWhiteSpace(model.Text)) model.Text = AttachmentService.DefaultImageModel;
            if (videoMode && AttachmentService.IsSupportedImageModel(model.Text)) model.Text = "";
            if (videoMode && size.Text == "1024x1024") size.Text = "1280x720";
            if (imageMode && size.Text == "1280x720") size.Text = "1024x1024";
        }
        modes.SelectionChanged += (_, _) => UpdateMediaModel();
        UpdateMediaModel();
        Button apply = new() { Content = "Применить", Margin = new Thickness(0, 18, 0, 0) }; panel.Children.Add(apply);
        apply.Click += (_, _) =>
        {
            string selectedModel = model.Text.Trim();
            if (modes.SelectedIndex is 1 or 2 or 3 && string.IsNullOrWhiteSpace(selectedModel)) { MessageBox.Show(dialog, "Выберите модель или укажите ID модели вашего API."); return; }
            if (modes.SelectedIndex is 1 or 2 && !AttachmentService.IsSupportedImageModel(selectedModel)) { MessageBox.Show(dialog, "Выберите поддерживаемую модель изображения из списка. Модель не была заменена автоматически."); return; }
            if (modes.SelectedIndex == 3 && AttachmentService.IsSupportedImageModel(selectedModel)) { MessageBox.Show(dialog, "Для видео нужна видеомодель, а не модель изображения."); return; }
            if (modes.SelectedIndex is 1 or 2) imageSelection = selectedModel;
            if (modes.SelectedIndex == 3) videoSelection = selectedModel;
            string oldImage = settings.MediaImageModel;
            string oldVideo = settings.MediaVideoModel;
            settings.MediaImageModel = imageSelection;
            settings.MediaVideoModel = videoSelection;
            try { SaveSettings(); }
            catch (Exception ex) { settings.MediaImageModel = oldImage; settings.MediaVideoModel = oldVideo; MessageBox.Show(dialog, "Не удалось сохранить выбор модели: " + ex.Message); return; }
            mediaMode = new[] { "chat", "image", "edit", "video", "remix", "agent" }[modes.SelectedIndex];
            mediaModel = selectedModel; mediaSize = size.Text.Trim(); mediaSeconds = seconds.Text.Trim(); mediaVideoId = videoId.Text.Trim();
            ComposerStatus.Text = "  " + modes.SelectedItem + (model.IsEnabled ? " · " + mediaModel : "");
            dialog.Close();
        };
        dialog.ShowDialog();
    }
    void ReturnToAgent_Click(object sender, RoutedEventArgs e)
    {
        if (isWaiting && mediaCancellation == null) return;
        mediaMode = "agent";
        mediaCancellation?.Cancel();
        ComposerStatus.Text = "  Режим агента · инструменты проекта";
        StatusText.Text = pendingAttachments.Count == 0 ? "Режим агента" : "Режим агента. Снимите вложения или выберите медиа для их отправки.";
        ShowAgent();
    }

    void ShowMediaError(Exception exception)
    {
        string title = exception is MediaResponseException ? "Не удалось прочитать ответ API" : "Медиа-запрос не выполнен";
        string message = exception is MediaApiException api
            ? $"{api.Title}\n\n{api.Advice}\n\nКод: {api.Status}{(string.IsNullOrWhiteSpace(api.Code) ? "" : " · " + api.Code)}"
            : exception.Message;
        if (exception is MediaResponseException) message += "\n\nЧерновик и вложения сохранены. Запрос мог быть обработан сервером — проверьте результат перед повторной отправкой. Автоматический повтор отключён.";
        AddMarkdown("Медиа", "### " + title + "\n\n" + message.Replace("\n", "  \n"), true);
        StatusText.Text = title + ". Черновик сохранён.";
    }

    async Task SendMediaAsync()
    {
        string text = PromptInput.Text.Trim();
        if (text.Length == 0 && pendingAttachments.Count == 0) return;
        if (mediaMode == "edit" && (pendingAttachments.Count == 0 || pendingAttachments.Any(file => !file.Mime.StartsWith("image/")))) { MessageBox.Show(this, "Для редактирования добавьте изображения."); return; }
        if (mediaMode == "image" && pendingAttachments.Count != 0) { MessageBox.Show(this, "Для работы с исходными изображениями выберите редактирование."); return; }
        if (mediaMode == "video" && (pendingAttachments.Count > 1 || pendingAttachments.Any(file => !file.Mime.StartsWith("image/")))) { MessageBox.Show(this, "Видео принимает текст и максимум одно изображение-референс, не исходное видео."); return; }
        if (mediaMode == "remix" && (pendingAttachments.Count > 0 || string.IsNullOrWhiteSpace(mediaVideoId))) { MessageBox.Show(this, "Для ремикса нужен ID видео на сервере, без локальных вложений."); return; }
        if (mediaMode is "image" or "edit")
        {
            if (!AttachmentService.IsSupportedImageModel(mediaModel))
            {
                if (MessageBox.Show(this, $"Модель «{mediaModel}» не поддерживается. Использовать gpt-image-2? Запрос ещё не отправлен.", "Модель изображения", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
                mediaModel = AttachmentService.DefaultImageModel;
            }
            try { AttachmentService.ValidateImageRequest(mediaMode, text, mediaSize, pendingAttachments); }
            catch (Exception ex) { ShowMediaError(ex); return; }
        }
        if (mediaMode == "video")
        {
            try { AttachmentService.ValidateVideoRequest(mediaModel, text, mediaSize, mediaSeconds); }
            catch (Exception ex) { ShowMediaError(ex); return; }
        }
        string requestMode = mediaMode;
        string requestModel = mediaMode == "chat" ? Model : mediaModel;
        ChatMessage user = new("user", text, pendingAttachments.ToList());
        AddMarkdown("Вы", text, false); ShowMessageAttachments(user.Attachments);
        var currentHistory = activeChatType == ChatType.Development ? developmentHistory : conversationHistory;
        currentHistory.Add(StoredMessage(user)); SaveProject();
        SetWaiting(true);
        mediaCancellation = new CancellationTokenSource(TimeSpan.FromMinutes(16));
        Button cancel = new() { Content = "Отменить передачу / ожидание", Margin = new Thickness(0, 5, 0, 8) }; cancel.Click += (_, _) => mediaCancellation?.Cancel(); ChatPanel.Items.Add(cancel);
        StatusText.Text = "Передача вложений / ожидание API… Отмена не гарантирует отмену оплаченной задачи на сервере.";
        try
        {
            List<ChatMessage> messages = currentHistory.Select(item => new ChatMessage(item["role"], item["content"])).ToList();
            messages[^1] = user;
            ChatMessage answer = await attachmentService.SendAsync(ApiKey, BaseUrl, requestModel, messages, requestMode, mediaSize, mediaSeconds, mediaVideoId, mediaCancellation.Token);
            currentHistory.Add(StoredMessage(answer)); SaveProject(); AddMarkdown(requestModel, answer.Content, true); ShowMessageAttachments(answer.Attachments);
            pendingAttachments.Clear(); PromptInput.Clear(); RefreshAttachmentTray(); SaveAttachmentDraft(); StatusText.Text = "Готово";
        }
        catch (Exception ex)
        {
            currentHistory.RemoveAt(currentHistory.Count - 1); SaveProject();
            if (ex is OperationCanceledException) AddMarkdown("Медиа", "Ожидание отменено. Черновик сохранён. Сервер мог продолжить платную генерацию: проверьте её результат перед повтором.", true);
            else ShowMediaError(ex);
        }
        finally { ChatPanel.Items.Remove(cancel); mediaCancellation.Dispose(); mediaCancellation = null; SetWaiting(false); ComposerStatus.Text = mediaMode == "agent" ? "  Режим агента · инструменты проекта" : "  Медиа: " + mediaMode; }
    }
}
