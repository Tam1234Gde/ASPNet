using Newtonsoft.Json;

namespace HIDE.RestAndRant.Business_YooKassa.Definitions;

internal class CYokassa_Receipt
{
    [JsonProperty("customer")]
    public CYokassa_ReceiptCustomer? Customer { get; set; }

    [JsonProperty("items")]
    public List<CYokassa_ReceiptItemInfo>? Items { get; set; }
}