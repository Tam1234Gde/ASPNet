using Newtonsoft.Json;

namespace HIDE.RestAndRant.Business_YooKassa.Definitions;

public class CYokassa_Buy_PaymentMethod
{
    [JsonProperty(PropertyName = "id")]
    public string? Id { get; set; }
}