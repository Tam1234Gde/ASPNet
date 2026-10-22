using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AIEngineerStudio.Services;

public sealed class MediaResponseException : Exception
{
    public MediaResponseException(string message, Exception? inner = null) : base(message, inner) { }
}

public static class MediaResponseReader
{
    private const int MaxResponseBytes = 128 * 1024 * 1024;

    public static async Task<JsonDocument> ReadAsync(HttpResponseMessage response, CancellationToken token)
    {
        try
        {
            await using Stream input = await response.Content.ReadAsStreamAsync(token);
            byte[] bytes = await ReadBoundedAsync(input, token);
            foreach (string encoding in response.Content.Headers.ContentEncoding.Reverse())
            {
                using MemoryStream compressed = new(bytes);
                using Stream decoder = encoding.ToLowerInvariant() switch
                {
                    "gzip" => new GZipStream(compressed, CompressionMode.Decompress),
                    "deflate" => new ZLibStream(compressed, CompressionMode.Decompress),
                    "br" => new BrotliStream(compressed, CompressionMode.Decompress),
                    "identity" => compressed,
                    _ => throw new MediaResponseException("API использует неподдерживаемое сжатие ответа.")
                };
                bytes = await ReadBoundedAsync(decoder, token);
            }
            // Some gateways omit Content-Encoding. Never strip a gzip signature as text.
            if (bytes.Length >= 2 && bytes[0] == 0x1f && bytes[1] == 0x8b)
            {
                using MemoryStream compressed = new(bytes);
                using GZipStream gzip = new(compressed, CompressionMode.Decompress);
                bytes = await ReadBoundedAsync(gzip, token);
            }
            string body = Normalize(new UTF8Encoding(false, true).GetString(bytes));
            if (!response.IsSuccessStatusCode) throw new MediaApiException((int)response.StatusCode, body);
            JsonDocument document = JsonDocument.Parse(body);
            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                document.Dispose();
                throw new MediaResponseException("API вернул неожиданный формат ответа вместо JSON-объекта.");
            }
            return document;
        }
        catch (Exception exception) when (exception is JsonException or InvalidDataException or DecoderFallbackException)
        {
            throw new MediaResponseException($"Не удалось прочитать ответ медиа API (HTTP {(int)response.StatusCode}). Ответ повреждён или имеет неподдерживаемый формат. Проверьте адрес API и состояние сервиса.", exception);
        }
    }

    private static string Normalize(string text)
    {
        int start = 0;
        while (start < text.Length && (text[start] == '\uFEFF' || text[start] < 0x20 || char.IsWhiteSpace(text[start]))) start++;
        // Do not remove arbitrary text or alter control characters inside JSON strings.
        return text[start..];
    }

    private static async Task<byte[]> ReadBoundedAsync(Stream input, CancellationToken token)
    {
        using MemoryStream output = new();
        byte[] buffer = new byte[81920];
        int count;
        while ((count = await input.ReadAsync(buffer, token)) != 0)
        {
            if (output.Length + count > MaxResponseBytes) throw new MediaResponseException("Ответ медиа API превышает безопасный размер 128 МБ.");
            await output.WriteAsync(buffer.AsMemory(0, count), token);
        }
        return output.ToArray();
    }
}
