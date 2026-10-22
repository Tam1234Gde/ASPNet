using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// Способ платежа
    /// </summary>
    public class CYookassa_Payment_Method_v2
    {
        /// <summary>
        /// Код способа оплаты
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = null!;


        /// <summary>
        /// Идентификатор сохраненного способа оплаты
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; } = null!;


        /// <summary>
        /// Статус проверки и сохранения способа оплаты
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; } = null!;


        /// <summary>
        /// Признак сохранения способа оплаты для автоплатежей
        /// </summary>
        [JsonProperty(PropertyName = "saved")]
        public bool Saved { get; set; }


        /// <summary>
        /// Название способа оплаты
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string? Title { get; set; }


        /// <summary>
        /// Данные банковской карты
        /// </summary>
        [JsonProperty(PropertyName = "card")]
        public CYookassa_Card_v2? Card { get; set; }


        /// <summary>
        /// Статус проверки и сохранения способа оплаты
        /// </summary>
        [JsonProperty(PropertyName = "holder")]
        public CYookassa_Recipient_v2 Holder { get; set; } = null!;


        /// <summary>
        /// Выбранный сценарий подтверждения привязки. Присутствует, когда привязка ожидает подтверждения от пользователя
        /// </summary>
        [JsonProperty(PropertyName = "confirmation")]
        public CYookassa_Confirmation_Result_v2? Confirmation { get; set; }


        /// <summary>
        /// Любые дополнительные данные, которые нужны вам для работы
        /// </summary>
        [JsonProperty(PropertyName = "metadata")]
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
