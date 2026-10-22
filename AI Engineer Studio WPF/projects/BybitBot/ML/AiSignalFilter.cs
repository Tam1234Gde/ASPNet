using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BybitBot.Core;
using BybitBot.Domain;
using Microsoft.Extensions.Options;

namespace BybitBot.ML;

public interface IAiSignalFilter
{
    Task<bool> ApproveAsync(Prediction prediction, float[] features, Bar bar, CancellationToken cancellationToken);
}

/// <summary>Консервативный LLM-фильтр. При любой ошибке, невалидном ответе или таймауте сигнал отклоняется.</summary>
public sealed class AiSignalFilter(HttpClient http, IOptions<BotOptions> options, ILogger<AiSignalFilter> logger) : IAiSignalFilter
{
    private readonly AiOptions _options = options.Value.Ai;

    public async Task<bool> ApproveAsync(Prediction prediction, float[] features, Bar bar, CancellationToken cancellationToken)
    {
        if (!_options.Enabled) return true;
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            logger.LogWarning("AI-фильтр включён, но токен не задан: сигнал отклонён.");
            return false;
        }

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_options.BaseUrl.TrimEnd('/') + "/"), "v1/messages"));
            request.Headers.Add("x-api-key", _options.ApiKey);
            request.Headers.Add("anthropic-version", "2023-06-01");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey); // совместимость с OpenAI-совместимыми шлюзами
            request.Content = new StringContent(JsonSerializer.Serialize(new
            {
                model = _options.Model,
                max_tokens = 40,
                temperature = 0,
                system = "Ты риск-фильтр торгового бота. Не прогнозируй цену и не давай советов. Верни строго один JSON: {\"approve\":true} или {\"approve\":false}. Одобряй только если входные числовые признаки не противоречат сигналу. При неопределённости false.",
                messages = new[] { new { role = "user", content = $"signal={(int)prediction.Signal}; confidence={prediction.Confidence:F4}; close={bar.Close}; features=[{string.Join(',', features.Select(x => x.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)))}]" } }
            }), Encoding.UTF8, "application/json");

            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(TimeSpan.FromSeconds(_options.TimeoutSeconds));
            using var response = await http.SendAsync(request, timeout.Token);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("AI-фильтр вернул HTTP {StatusCode}; сигнал отклонён.", response.StatusCode);
                return false;
            }

            var json = await response.Content.ReadAsStringAsync(timeout.Token);
            // Anthropic content[0].text; допускаем text для совместимых прокси.
            using var document = JsonDocument.Parse(json);
            var text = document.RootElement.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.Array
                ? content[0].GetProperty("text").GetString()
                : document.RootElement.TryGetProperty("text", out var direct) ? direct.GetString() : null;
            var approved = text?.Contains("\"approve\":true", StringComparison.OrdinalIgnoreCase) == true;
            logger.LogInformation("AI-фильтр: {Decision}", approved ? "сигнал подтверждён" : "сигнал отклонён");
            return approved;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            logger.LogWarning(ex, "AI-фильтр недоступен; сигнал отклонён.");
            return false;
        }
    }
}
