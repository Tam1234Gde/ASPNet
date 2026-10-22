using Newtonsoft.Json;

namespace HIDE.RestAndRant.Business_YooKassa.Definitions;

/// <summary>
/// Данные подтверждения
/// </summary>
public class CYokassa_BuyResultConfirmation
{
    /// <summary>
    /// Тип подтверждения
    /// </summary>
    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }

    /// <summary>
    /// Ссылка для перехода когда платеж подтверждён
    /// </summary>
    [JsonProperty(PropertyName = "return_url")]
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Ссылка для перехода к оплате
    /// </summary>
    [JsonProperty(PropertyName = "confirmation_url")]
    public string? ConfirmationUrl { get; set; }
}