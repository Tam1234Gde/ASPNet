using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AIEngineerStudio.Models;

namespace AIEngineerStudio.Services;

public sealed class TokenBalanceService
{
    static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(12) };

    public async Task<TokenBalanceResult> CountTokensAsync(string baseUrl, string apiKey, string model, IReadOnlyList<ChatMessage> messages, CancellationToken cancellationToken = default)
    {
        string normalizedBaseUrl = ApiConfiguration.NormalizeBaseUrl(baseUrl);
        using HttpRequestMessage balanceRequest = ApiConfiguration.CreateRequest(HttpMethod.Get, normalizedBaseUrl, apiKey, "/balance");
        using HttpResponseMessage balanceResponse = await Client.SendAsync(balanceRequest, cancellationToken);
        string balanceBody = await balanceResponse.Content.ReadAsStringAsync(cancellationToken);

        if (balanceResponse.IsSuccessStatusCode)
        {
            using JsonDocument balanceDocument = JsonDocument.Parse(balanceBody);
            long? balance = FindNumber(balanceDocument.RootElement, "balance", "token_balance", "remaining_tokens", "tokens_balance", "credits");
            if (balance.HasValue)
            {
                return new TokenBalanceResult(balance, null);
            }
        }

        // Совместимость со шлюзами, где отдельный /balance отсутствует.
        List<object> payloadMessages = messages.Select(message => (object)new { role = message.Role, content = message.Content }).ToList();
        using HttpRequestMessage countRequest = ApiConfiguration.CreateRequest(HttpMethod.Post, normalizedBaseUrl, apiKey, "/messages/count_tokens");
        countRequest.Content = new StringContent(JsonSerializer.Serialize(new { model, messages = payloadMessages }), Encoding.UTF8, "application/json");
        using HttpResponseMessage countResponse = await Client.SendAsync(countRequest, cancellationToken);
        string countBody = await countResponse.Content.ReadAsStringAsync(cancellationToken);
        if (!countResponse.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Баланс токенов: HTTP {(int)countResponse.StatusCode}");
        }

        using JsonDocument countDocument = JsonDocument.Parse(countBody);
        JsonElement root = countDocument.RootElement;
        long? fallbackBalance = FindNumber(root, "balance", "token_balance", "remaining_tokens", "tokens_balance", "credits");
        long? count = FindNumber(root, "tokens", "total_tokens", "count", "prompt_tokens", "input_tokens");
        return new TokenBalanceResult(fallbackBalance, count);
    }

    static long? FindNumber(JsonElement element, params string[] names)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (JsonProperty property in element.EnumerateObject())
            {
                if (names.Any(name => string.Equals(name, property.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    long? value = ReadNumber(property.Value);
                    if (value.HasValue) return value;
                }
            }

            foreach (JsonProperty property in element.EnumerateObject())
            {
                long? nested = FindNumber(property.Value, names);
                if (nested.HasValue) return nested;
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement item in element.EnumerateArray())
            {
                long? nested = FindNumber(item, names);
                if (nested.HasValue) return nested;
            }
        }
        return null;
    }

    static long? ReadNumber(JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.Number && value.TryGetInt64(out long number)) return number;
        if (value.ValueKind == JsonValueKind.String && long.TryParse(value.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out long parsed)) return parsed;
        if (value.ValueKind == JsonValueKind.Object) return FindNumber(value, "value", "total", "remaining", "amount");
        return null;
    }
}

public sealed record TokenBalanceResult(long? Balance, long? Count);
