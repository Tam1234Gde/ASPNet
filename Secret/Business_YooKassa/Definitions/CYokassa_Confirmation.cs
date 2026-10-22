using Newtonsoft.Json;

namespace HIDE.RestAndRant.Business_YooKassa.Definitions
{
    public class CYokassa_Confirmation
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = null!;

        [JsonProperty(PropertyName = "return_url")]
        public string ReturnUrl { get; set; } = null!;
    }
}