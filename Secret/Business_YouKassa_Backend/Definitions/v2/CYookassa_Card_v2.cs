using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// Данные банковской карты
    /// </summary>
    public class CYookassa_Card_v2
    {
        /// <summary>
        /// Первые 6 цифр номера карты (BIN)
        /// </summary>
        [JsonProperty(PropertyName = "first6")]
        public string? First6 { get; set; }


        /// <summary>
        /// Последние 4 цифры номера карты
        /// </summary>
        [JsonProperty(PropertyName = "last4")]
        public required string Last4 { get; set; }


        /// <summary>
        /// Срок действия, год, YYYY
        /// </summary>
        [JsonProperty(PropertyName = "expiry_year")]
        public required string Expiry_Year { get; set; }


        /// <summary>
        /// Срок действия, месяц, MM
        /// </summary>
        [JsonProperty(PropertyName = "expiry_month")]
        public required string Expiry_Month { get; set; }


        /// <summary>
        /// Тип банковской карты
        /// </summary>
        [JsonProperty(PropertyName = "card_type")]
        public required string Card_Type { get; set; }
    }
}
