using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// Данные для оплаты конкретным способом
    /// </summary>
    public class CYookassa_Payment_Method_Data_v2
    {
        /// <summary>
        /// Код способа оплаты
        /// </summary>
        [JsonProperty(PropertyName = "type")] 
        public string Type { get; set; } = null!;


        /// <summary>
        /// Данные, необходимые для инициирования сценария подтверждения привязки
        /// </summary>
        [JsonProperty(PropertyName = "confirmation")]
        public CYookassa_Confirmation_v2? Confirmation { get; set; }


        /// <summary>
        /// Любые дополнительные данные, которые нужны вам для работы
        /// </summary>
        [JsonProperty(PropertyName = "metadata")]
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
