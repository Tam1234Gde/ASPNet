using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AIEngineerStudio.Models;
namespace AIEngineerStudio.Services;

public sealed class AttachmentService
{
    public static readonly IReadOnlyList<string> SupportedImageModels =
    ["gpt-image-2", "gpt-image-2.5-flare", "gpt-image-2.5-sunburst", "nano-banana-2", "grok-imagine-image"];
    public const string DefaultImageModel = "gpt-image-2";
    public const long MaxSize = 25 * 1024 * 1024;

    public static string ResolveImageModel(string? requested)
    {
        string candidate = requested?.Trim() ?? string.Empty;
        return SupportedImageModels.Contains(candidate, StringComparer.OrdinalIgnoreCase)
            ? SupportedImageModels.First(model => string.Equals(model, candidate, StringComparison.OrdinalIgnoreCase))
            : DefaultImageModel;
    }

    public static bool IsSupportedImageModel(string? model) =>
        SupportedImageModels.Contains(model?.Trim() ?? string.Empty, StringComparer.OrdinalIgnoreCase);

    public static void ValidateImageRequest(string mode, string prompt, string size, IReadOnlyList<Attachment> files)
    {
        if (string.IsNullOrWhiteSpace(prompt)) throw new InvalidDataException("Добавьте описание результата.");
        if (string.IsNullOrWhiteSpace(size)) throw new InvalidDataException("Укажите размер изображения.");
        if (mode == "edit" && files.Count == 0) throw new InvalidDataException("Для редактирования добавьте хотя бы одно изображение.");
        if (files.Any(file => !file.Mime.StartsWith("image/", StringComparison.OrdinalIgnoreCase))) throw new InvalidDataException("Операции с изображениями принимают только изображения.");
    }
    public static void ValidateVideoRequest(string model, string prompt, string size, string seconds)
    {
        if (string.IsNullOrWhiteSpace(model) || IsSupportedImageModel(model)) throw new InvalidDataException("Укажите точный ID видеомодели вашего API, а не модель изображений.");
        if (string.IsNullOrWhiteSpace(prompt)) throw new InvalidDataException("Добавьте описание видео.");
        string[] dimensions = size.Split('x');
        if (dimensions.Length != 2 || !int.TryParse(dimensions[0], out int width) || !int.TryParse(dimensions[1], out int height) || width <= 0 || height <= 0)
            throw new InvalidDataException("Размер видео должен иметь формат 1280x720.");
        if (!int.TryParse(seconds, out int duration) || duration <= 0) throw new InvalidDataException("Длительность видео должна быть положительным целым числом секунд.");
        if (model.StartsWith("sora-", StringComparison.OrdinalIgnoreCase) &&
            (!(duration is 4 or 8 or 12) || !(size is "1280x720" or "720x1280" or "1792x1024" or "1024x1792")))
            throw new InvalidDataException("Sora: длительность 4, 8 или 12 секунд; размер 1280x720, 720x1280 (Pro также 1792x1024, 1024x1792).");
    }

    public static readonly string Root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AIEngineerStudio", "Attachments");
    public static string Mime(string path) => Path.GetExtension(path).ToLowerInvariant() switch {
        ".png" => "image/png", ".jpg" or ".jpeg" => "image/jpeg", ".webp" => "image/webp", ".gif" => "image/gif",
        ".mp4" => "video/mp4", ".webm" => "video/webm", ".mov" => "video/quicktime", ".mp3" => "audio/mpeg", ".wav" => "audio/wav", ".pdf" => "application/pdf", _ => "application/octet-stream" };
    public static async Task<Attachment> ImportAsync(string path, CancellationToken token)
    {
        FileInfo info = new(path);
        if (!info.Exists || info.Length == 0 || info.Length > MaxSize) throw new InvalidDataException("Файл должен быть непустым и не больше 25 МБ.");
        Directory.CreateDirectory(Root);
        string target = Path.Combine(Root, Guid.NewGuid().ToString("N") + info.Extension);
        await using FileStream input = File.OpenRead(path);
        await using FileStream output = File.Create(target);
        await input.CopyToAsync(output, token);
        return new Attachment(info.Name, target, Mime(path), info.Length);
    }
    static async Task<string> DataAsync(Attachment file, CancellationToken token)
    {
        if (!File.Exists(file.Path) || new FileInfo(file.Path).Length > MaxSize) throw new IOException($"Вложение недоступно: {file.Name}");
        return $"data:{file.Mime};base64,{Convert.ToBase64String(await File.ReadAllBytesAsync(file.Path, token))}";
    }
    static HttpClient Client(string key, string url)
    {
        string normalizedUrl = ApiConfiguration.NormalizeBaseUrl(url);
        if (!Uri.TryCreate(normalizedUrl + "/", UriKind.Absolute, out Uri? uri) || uri.Scheme != "https") throw new InvalidOperationException("Для передачи файлов требуется HTTPS API URL.");
        HttpClient client = new() { Timeout = TimeSpan.FromMinutes(15) };
        ApiConfiguration.Configure(client, normalizedUrl, key);
        return client;
    }
    static Task<JsonDocument> ReadAsync(HttpResponseMessage response, CancellationToken token) => MediaResponseReader.ReadAsync(response, token);
    public async Task<ChatMessage> SendAsync(string key, string url, string model, IReadOnlyList<ChatMessage> history, string mode, string size, string seconds, string videoId, CancellationToken token)
    {
        if ((mode == "image" || mode == "edit") && !IsSupportedImageModel(model))
            throw new InvalidOperationException($"Модель изображений «{model}» не поддерживается. Выберите одну из: {string.Join(", ", SupportedImageModels)}.");
        if (mode == "video") ValidateVideoRequest(model, history.Last().Content, size, seconds);
        using HttpClient client = Client(key, url);
        ChatMessage last = history.Last();
        string endpoint;
        HttpContent payload;
        if (mode == "chat")
        {
            List<object> messages = new();
            foreach (ChatMessage message in history)
            {
                List<object> parts = new();
                if (!string.IsNullOrEmpty(message.Content)) parts.Add(new { type = "text", text = message.Content });
                foreach (Attachment file in message.Attachments ?? [])
                {
                    string data = await DataAsync(file, token);
                    if (file.Mime.StartsWith("image/")) parts.Add(new { type = "image_url", image_url = new { url = data } });
                    else if (file.Mime.StartsWith("video/")) parts.Add(new { type = "video_url", video_url = new { url = data } });
                    else if (file.Mime == "audio/wav" || file.Mime == "audio/mpeg") parts.Add(new { type = "input_audio", input_audio = new { data = data[(data.IndexOf(',') + 1)..], format = file.Mime == "audio/wav" ? "wav" : "mp3" } });
                    else parts.Add(new { type = "file", file = new { filename = file.Name, file_data = data } });
                }
                messages.Add(new { role = message.Role, content = parts });
            }
            endpoint = "chat/completions";
            payload = new StringContent(JsonSerializer.Serialize(new { model, messages }), Encoding.UTF8, "application/json");
        }
        else if (mode == "remix")
        {
            endpoint = "videos/" + Uri.EscapeDataString(videoId) + "/remix";
            payload = new StringContent(JsonSerializer.Serialize(new { prompt = last.Content }), Encoding.UTF8, "application/json");
        }
        else if (mode == "image")
        {
            endpoint = "images/generations";
            payload = new StringContent(JsonSerializer.Serialize(new { model, prompt = last.Content, size }), Encoding.UTF8, "application/json");
        }
        else
        {
            MultipartFormDataContent form = new();
            form.Add(new StringContent(model), "model"); form.Add(new StringContent(last.Content), "prompt");
            form.Add(new StringContent(size), "size");
            if (mode == "video") form.Add(new StringContent(seconds), "seconds");
            foreach (Attachment file in last.Attachments ?? [])
            {
                ByteArrayContent bytes = new(await File.ReadAllBytesAsync(file.Path, token));
                bytes.Headers.ContentType = new MediaTypeHeaderValue(file.Mime);
                form.Add(bytes, mode == "edit" ? "image" : "input_reference", file.Name);
            }
            endpoint = mode == "edit" ? "images/edits" : "videos";
            payload = form;
        }
        using (payload)
        using (HttpResponseMessage response = await client.PostAsync(endpoint, payload, token))
        using (JsonDocument document = await ReadOperationAsync(response, mode, endpoint, model, token))
        {
            JsonElement root = document.RootElement;
            if (mode == "video" || mode == "remix")
            {
                if (!root.TryGetProperty("id", out JsonElement jobId) || jobId.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(jobId.GetString()))
                    throw new InvalidDataException("Video API не вернул ID задачи в формате OpenAI /videos. Проверьте документацию провайдера. Запрос мог быть оплачен; автоматический повтор отключён.");
                string id = jobId.GetString()!;
                try
                {
                for (int attempt = 0; attempt < 180; attempt++)
                {
                    await Task.Delay(5000, token);
                    using HttpResponseMessage poll = await client.GetAsync("videos/" + Uri.EscapeDataString(id), token);
                    using JsonDocument status = await ReadAsync(poll, token);
                    string? state = status.RootElement.GetProperty("status").GetString();
                    if (state == "failed") throw new InvalidOperationException("Генерация видео завершилась ошибкой: " + status.RootElement.ToString());
                    if (state != "completed") continue;
                    using HttpResponseMessage download = await client.GetAsync("videos/" + Uri.EscapeDataString(id) + "/content", HttpCompletionOption.ResponseHeadersRead, token);
                    download.EnsureSuccessStatusCode();
                    Attachment result = await StoreAsync(download.Content, "video.mp4", token);
                    return new ChatMessage("assistant", "Видео готово. ID для ремикса: " + id, [result]);
                }
                throw new TimeoutException("Ожидание видео истекло.");
                }
                catch (Exception ex)
                {
                    string detail = ex is MediaApiException api ? $"{api.Title}. {api.Advice} Код: {api.Status} · {api.Code}" : ex.Message;
                    throw new InvalidOperationException($"Не удалось получить видео. ID задачи: {id}.\n{detail}\nСоздание повторно не отправлялось. Проверьте задачу у провайдера перед новой генерацией.", ex);
                }
            }
            List<Attachment> files = new();
            string text = "";
            if (root.TryGetProperty("choices", out JsonElement choices))
            {
                JsonElement message = choices[0].GetProperty("message");
                if (message.TryGetProperty("content", out JsonElement content))
                {
                    if (content.ValueKind == JsonValueKind.String) text = content.GetString() ?? "";
                    else if (content.ValueKind == JsonValueKind.Array)
                        foreach (JsonElement part in content.EnumerateArray())
                        {
                            if (part.TryGetProperty("text", out JsonElement t)) text += t.GetString();
                            else if (part.TryGetProperty("image_url", out JsonElement image)) files.Add(await ReceiveAsync(image.ValueKind == JsonValueKind.String ? image.GetString()! : image.GetProperty("url").GetString()!, "image.png", token));
                        }
                }
                if (message.TryGetProperty("images", out JsonElement images)) foreach (JsonElement image in images.EnumerateArray()) files.Add(await ReceiveAsync(image.GetProperty("image_url").GetProperty("url").GetString()!, "image.png", token));
            }
            if (root.TryGetProperty("data", out JsonElement items)) foreach (JsonElement item in items.EnumerateArray())
            {
                string source = item.TryGetProperty("b64_json", out JsonElement b64) ? "data:image/png;base64," + b64.GetString() : item.GetProperty("url").GetString()!;
                files.Add(await ReceiveAsync(source, "image.png", token));
            }
            if (text.Length == 0 && files.Count == 0) throw new InvalidDataException("API не вернул текст или поддерживаемое вложение.");
            return new ChatMessage("assistant", text, files);
        }
    }
    static async Task<JsonDocument> ReadOperationAsync(HttpResponseMessage response, string mode, string endpoint, string model, CancellationToken token)
    {
        try { return await ReadAsync(response, token); }
        catch (MediaApiException ex) when (mode is "video" or "remix")
        {
            throw new InvalidOperationException($"{ex.Title}\n{ex.Advice}\nОперация: POST {endpoint}; модель: {model}\nКод: {ex.Status} · {ex.Code}\nАвтоматический повтор отключён.", ex);
        }
    }

    static async Task<Attachment> ReceiveAsync(string source, string name, CancellationToken token)
    {
        if (source.StartsWith("data:image/", StringComparison.Ordinal))
        {
            byte[] bytes = Convert.FromBase64String(source[(source.IndexOf(',') + 1)..]);
            using ByteArrayContent content = new(bytes);
            return await StoreAsync(content, name, token);
        }
        if (!Uri.TryCreate(source, UriKind.Absolute, out Uri? uri) || uri.Scheme != "https" || uri.IsLoopback) throw new InvalidDataException("Недопустимый URL результата.");
        using HttpClient publicClient = new(new HttpClientHandler { AllowAutoRedirect = false });
        using HttpResponseMessage response = await publicClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, token);
        response.EnsureSuccessStatusCode();
        return await StoreAsync(response.Content, name, token);
    }
    static async Task<Attachment> StoreAsync(HttpContent content, string name, CancellationToken token)
    {
        Directory.CreateDirectory(Root);
        string path = Path.Combine(Root, Guid.NewGuid().ToString("N") + Path.GetExtension(name));
        await using Stream input = await content.ReadAsStreamAsync(token);
        await using FileStream output = File.Create(path);
        byte[] buffer = new byte[81920]; long total = 0; int count;
        while ((count = await input.ReadAsync(buffer, token)) > 0)
        {
            total += count;
            if (total > 200 * 1024 * 1024) throw new InvalidDataException("Результат превышает 200 МБ.");
            await output.WriteAsync(buffer.AsMemory(0, count), token);
        }
        return new Attachment(name, path, Mime(name), total);
    }
}
