using Newtonsoft.Json;

namespace HIDE.RestAndRant.Business_YooKassa.Definitions
{
    public class CYokassa_ReceiptData
    {
        [JsonProperty("items")]
        public List<CYokassa_ReceiptItem>? Items { get; set; }
    }
}