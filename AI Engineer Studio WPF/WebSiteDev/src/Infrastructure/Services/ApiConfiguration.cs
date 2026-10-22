using System.Net.Http;
using System.Net.Http.Headers;

namespace AIEngineerStudio.Services;

/// <summary>Единая нормализация и настройка OpenAI-compatible API для всех функций приложения.</summary>
public static class ApiConfiguration
{
    public const string DefaultBaseUrl = "https://apiduck.sytes.net/v1";

    public static string NormalizeKey(string? value)
    {
        string key = (value ?? string.Empty).Trim();
        if (key.Length >= 2 && key[0] == key[^1] && (key[0] == '"' || key[0] == '\''))
            key = key[1..^1].Trim();
        if (key.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            key = key[7..].Trim();
        return key;
    }

    public static string NormalizeBaseUrl(string? value)
    {
        string url = string.IsNullOrWhiteSpace(value) ? DefaultBaseUrl : value.Trim().Trim('"', '\'').TrimEnd('/');
        const string completions = "/chat/completions";
        if (url.EndsWith(completions, StringComparison.OrdinalIgnoreCase))
            url = url[..^completions.Length].TrimEnd('/');
        return url;
    }

    public static HttpRequestMessage CreateRequest(HttpMethod method, string baseUrl, string? key, string suffix)
    {
        string normalizedKey = NormalizeKey(key);
        HttpRequestMessage request = new(method, NormalizeBaseUrl(baseUrl) + suffix);
        if (!string.IsNullOrEmpty(normalizedKey))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", normalizedKey);
            request.Headers.TryAddWithoutValidation("x-api-key", normalizedKey);
        }
        return request;
    }

    public static void Configure(HttpClient client, string? baseUrl, string? key)
    {
        client.BaseAddress = new Uri(NormalizeBaseUrl(baseUrl) + "/", UriKind.Absolute);
        string normalizedKey = NormalizeKey(key);
        client.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(normalizedKey)
            ? null
            : new AuthenticationHeaderValue("Bearer", normalizedKey);
        client.DefaultRequestHeaders.Remove("x-api-key");
        if (!string.IsNullOrEmpty(normalizedKey))
            client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", normalizedKey);
    }
}
