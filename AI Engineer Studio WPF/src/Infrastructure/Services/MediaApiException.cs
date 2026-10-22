using System.Text.Json;
namespace AIEngineerStudio.Services;

public sealed class MediaApiException : Exception
{
    public int Status { get; }
    public string Code { get; }
    public string Title { get; }
    public string Advice { get; }
    public MediaApiException(int status, string body) : base("Ошибка медиа API")
    {
        Status = status;
        string code = string.Empty;
        string message = string.Empty;
        try
        {
            using JsonDocument document = JsonDocument.Parse(body);
            if (document.RootElement.TryGetProperty("error", out JsonElement error) && error.ValueKind == JsonValueKind.Object)
            {
                if (error.TryGetProperty("code", out JsonElement value) && value.ValueKind == JsonValueKind.String) code = value.GetString() ?? "";
                if (error.TryGetProperty("message", out JsonElement text) && text.ValueKind == JsonValueKind.String) message = text.GetString() ?? "";
            }
        }
        catch (JsonException) { }
        Code = code;
        bool unknown = code.Equals("modelnotfound", StringComparison.OrdinalIgnoreCase) || code.Equals("model_not_found", StringComparison.OrdinalIgnoreCase) || message.Contains("Unknown image model", StringComparison.OrdinalIgnoreCase);
        Title = unknown ? "Модель недоступна" : status switch
        {
            401 or 403 => "Нет доступа к медиа API",
            404 => "Операция или модель не найдена",
            413 => "Вложения слишком большие",
            429 => "Лимит запросов исчерпан",
            >= 500 => "Медиа-сервис временно недоступен",
            _ => "Запрос не принят"
        };
        Advice = unknown ? "Откройте параметры медиа и выберите поддерживаемую модель. Используйте точный ID модели для выбранной операции."
            : status switch
            {
                401 or 403 => "Проверьте API-ключ, баланс и разрешения выбранной модели.",
                404 => "Проверьте базовый URL API и поддержку маршрута /videos для выбранной модели. Модель чата не обязательно поддерживает Video API.",
            413 => "Уменьшите размер или количество вложений.",
                429 => "Подождите перед повтором; проверьте лимиты и баланс.",
                >= 500 => "Попробуйте позже. Автоматический повтор отключён, чтобы избежать двойного списания.",
                _ => "Проверьте модель, параметры и поддержку выбранной операции вашим API."
            };
    }
}
