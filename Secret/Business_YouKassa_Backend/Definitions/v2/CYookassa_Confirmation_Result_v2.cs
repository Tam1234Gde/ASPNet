using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// Выбранный способ подтверждения платежа. Присутствует, когда платеж ожидает подтверждения от пользователя
    /// </summary>
    public class CYookassa_Confirmation_Result_v2
    {
        /// <summary>
        /// Код сценария подтверждения
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = null!;


        /// <summary>
        /// URL, на который необходимо перенаправить пользователя для подтверждения оплаты
        /// </summary>
        [JsonProperty(PropertyName = "confirmation_url")]
        public string Confirmation_Url { get; set; } = null!;
    }
}