using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// Получатель платежа
    /// </summary>
    public class CYookassa_Recipient_v2
    {
        /// <summary>
        /// Идентификатор магазина в ЮKassa
        /// </summary>
        [JsonProperty(PropertyName = "account_id")]
        public string AccountID { get; set; } = null!;


        /// <summary>
        /// Идентификатор субаккаунта. Используется для разделения потоков платежей в рамках одного аккаунта
        /// </summary>
        [JsonProperty(PropertyName = "gateway_id")]
        public string GatewayID { get; set; } = null!;
    }
}
