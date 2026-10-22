using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using AISoftCraftDuck.Models;

namespace AISoftCraftDuck.Services;

public interface IApiDuckClient 
{ 
    IAsyncEnumerable<string> StreamAsync(IReadOnlyList<ChatMessage> messages, string model, CancellationToken ct = default);
    Task<string> RunAgentAsync(IReadOnlyList<ChatMessage> messages, string model, Func<string, Dictionary<string, object?>, Task<string>> executeTool, Action<string> progress, CancellationToken ct = default); 
}

public sealed class ApiDuckClient : IApiDuckClient
{
    private readonly HttpClient http;
    private readonly IEnvService env;

    public ApiDuckClient(HttpClient http, IEnvService env)
    {
        this.http = http;
        this.env = env;
    }

    public async IAsyncEnumerable<string> StreamAsync(IReadOnlyList<ChatMessage> messages, string model, [EnumeratorCancellation] CancellationToken ct = default)
    {
        // Get fresh settings each time to pick up any changes
        var currentSettings = env.Current;
        var apiKey = NormalizeApiKey(currentSettings.OpenAiKey ?? currentSettings.ApiKey);
        
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            yield return "⚠ Error: API Key is not configured. Please set it in Settings.";
            yield break;
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = JsonContent.Create(new
        {
            model,
            messages = messages.Select(message => new
            {
                role = message.Role,
                content = message.Attachments is { Count: > 0 }
                    ? (object)new object[] { (object)new { type = "text", text = message.Content } }.Concat(message.Attachments.Select(file => file.MimeType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
                        ? (object)new { type = "image_url", image_url = new { url = $"data:{file.MimeType};base64,{Convert.ToBase64String(file.Content)}" } }
                        : new { type = "file", file = new { filename = file.FileName, file_data = $"data:{file.MimeType};base64,{Convert.ToBase64String(file.Content)}" } }))
                    .ToArray()
                    : message.Content
            }),
            stream = true
        });
        
        HttpResponseMessage? response = null;
        Stream? stream = null;
        StreamReader? reader = null;

        try
        {
            response = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(ct);
                yield return $"⚠ HTTP {(int)response.StatusCode}: {response.ReasonPhrase}\n{errorContent}";
                yield break;
            }
            
            stream = await response.Content.ReadAsStreamAsync(ct);
            reader = new StreamReader(stream);
            
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync(ct);
                if (line is null || !line.StartsWith("data:")) continue;
                
                var data = line[5..].Trim();
                if (data == "[DONE]") yield break;
                
                if (!TryParseStreamChunk(data, out var content))
                    continue;
                    
                if (!string.IsNullOrEmpty(content))
                    yield return content;
            }
        }
        finally
        {
            reader?.Dispose();
            stream?.Dispose();
            response?.Dispose();
        }
    }

    public async Task<string> RunAgentAsync(IReadOnlyList<ChatMessage> messages, string model, Func<string, Dictionary<string, object?>, Task<string>> executeTool, Action<string> progress, CancellationToken ct = default)
    {
        var history = messages.Select(x => new Dictionary<string, object?> { ["role"] = x.Role, ["content"] = x.Content }).ToList();
        var toolNames = new[] { "list_files", "read_file", "write_file", "edit_file_part", "create_folder", "delete_item", "move_item", "get_project_tree", "search_code", "execute_command", "get_system_info" };
        var definitions = toolNames.Select(name => new
        {
            type = "function",
            function = new
            {
                name,
                description = ToolDescription(name),
                parameters = ToolParameters(name)
            }
        });
        for (var step = 0; step < 30; step++)
        {
            var key = NormalizeApiKey(env.Current.OpenAiKey ?? env.Current.ApiKey);
            if (string.IsNullOrWhiteSpace(key)) return "⚠ API-ключ не настроен.";
            using var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            request.Content = JsonContent.Create(new { model, messages = history, tools = definitions, tool_choice = "auto", stream = false });
            using var response = await http.SendAsync(request, ct); var body = await response.Content.ReadAsStringAsync(ct);
            if (!response.IsSuccessStatusCode) return $"⚠ HTTP {(int)response.StatusCode}: {body}";
            JsonDocument document;
            try { document = JsonDocument.Parse(body); }
            catch (JsonException) { return $"⚠ API вернул не-JSON ответ: {body[..Math.Min(body.Length, 500)]}"; }
            using (document)
            {
            var message = document.RootElement.GetProperty("choices")[0].GetProperty("message");
            if (!message.TryGetProperty("tool_calls", out var calls) || calls.GetArrayLength() == 0)
                return message.TryGetProperty("content", out var content) ? content.GetString() ?? "" : "";
            history.Add(JsonSerializer.Deserialize<Dictionary<string, object?>>(message.GetRawText())!);
            foreach (var call in calls.EnumerateArray())
            {
                var function = call.GetProperty("function"); var name = function.GetProperty("name").GetString() ?? "";
                var args = JsonSerializer.Deserialize<Dictionary<string, object?>>(function.GetProperty("arguments").GetString() ?? "{}") ?? [];
                progress($"🛠 {name}({string.Join(", ", args.Select(x => $"{x.Key}={x.Value}"))})");
                var result = await executeTool(name, args); progress($"↳ {result[..Math.Min(result.Length, 240)]}");
                history.Add(new Dictionary<string, object?> { ["role"] = "tool", ["tool_call_id"] = call.GetProperty("id").GetString(), ["content"] = result });
            }
            }
        }
        return "⚠ Превышен лимит шагов автономного агента.";
    }

    private static string? NormalizeApiKey(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var key = value.Trim().Trim('"', '\'');
        // Settings users sometimes paste the complete HTTP header rather than the token.
        if (key.StartsWith("Authorization:", StringComparison.OrdinalIgnoreCase)) key = key["Authorization:".Length..].Trim();
        if (key.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) key = key[7..].Trim();
        return key;
    }

    private static string ToolDescription(string name) => name switch
    {
        "write_file" => "Создаёт или полностью перезаписывает UTF-8 файл в активном проекте.",
        "read_file" => "Читает файл активного проекта.",
        "edit_file_part" => "Заменяет точный фрагмент текста в файле.",
        "execute_command" => "Выполняет команду в папке активного проекта для сборки, тестов или проверки.",
        _ => $"Выполняет {name} внутри активного проекта."
    };

    private static object ToolParameters(string name)
    {
        Dictionary<string, object> P(params (string Key, string Description)[] values) => values.ToDictionary(x => x.Key, x => (object)new { type = "string", description = x.Description });
        return name switch
        {
            "write_file" => new { type = "object", properties = P(("filepath", "Относительный путь файла, например src/index.html"), ("content", "Полное содержимое файла")), required = new[] { "filepath", "content" } },
            "read_file" => new { type = "object", properties = P(("filepath", "Относительный путь файла"), ("start_line", "Необязательная начальная строка"), ("end_line", "Необязательная конечная строка")), required = new[] { "filepath" } },
            "edit_file_part" => new { type = "object", properties = P(("filepath", "Относительный путь файла"), ("old_str", "Точный исходный фрагмент"), ("new_str", "Новый фрагмент")), required = new[] { "filepath", "old_str", "new_str" } },
            "create_folder" => new { type = "object", properties = P(("folder_path", "Относительный путь создаваемой папки")), required = new[] { "folder_path" } },
            "delete_item" => new { type = "object", properties = P(("path", "Относительный путь файла или папки")), required = new[] { "path" } },
            "move_item" => new { type = "object", properties = P(("source_path", "Исходный путь"), ("destination_path", "Целевой путь")), required = new[] { "source_path", "destination_path" } },
            "search_code" => new { type = "object", properties = P(("query", "Текст или регулярное выражение"), ("file_pattern", "Маска файлов, например *.cs")), required = new[] { "query" } },
            "execute_command" => new { type = "object", properties = P(("command", "Команда терминала"), ("timeout", "Таймаут в секундах")), required = new[] { "command" } },
            "list_files" => new { type = "object", properties = P(("directory", "Относительная папка, по умолчанию .")) },
            _ => new { type = "object", properties = new Dictionary<string, object>() }
        };
    }

    private static bool TryParseStreamChunk(string json, out string? content)
    {
        content = null;
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                var choice = choices[0];
                if (choice.TryGetProperty("delta", out var delta) && delta.TryGetProperty("content", out var contentElement))
                {
                    content = contentElement.GetString();
                    return true;
                }
            }
        }
        catch (JsonException)
        {
            return false;
        }
        return false;
    }
}
