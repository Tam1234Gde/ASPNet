using Newtonsoft.Json;

namespace HIDE.RestAndRant.Business_YooKassa.Definitions
{
    internal class CYokassa_Refund
    {
        /// <summary>
        /// Сумма, которую нужно вернуть пользователю.
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public CYokassa_Amount Amount { get; set; } = null!;

        /// <summary>
        /// Идентификатор платежа в ЮKassa.
        /// </summary>
        [JsonProperty(PropertyName = "payment_id")]
        public string Payment_id { get; set; } = null!;
    }
}