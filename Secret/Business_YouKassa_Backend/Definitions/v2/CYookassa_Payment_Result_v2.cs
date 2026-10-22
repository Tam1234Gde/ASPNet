using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// <para>Объект платежа - результат</para>
    /// <para>Если платеж проводится в первый раз, то в ответе будет Confirmation</para>
    /// <para>Если проводится повторный платеж, то Confirmation уже не будет</para>
    /// </summary>
    public class CYookassa_Payment_Result_v2
    {
        /// <summary>
        /// Идентификатор платежа в ЮKassa
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; } = null!;


        /// <summary>
        /// Статус платежа, нормальный статус "succeeded". Если статус перешёл в состояние "canceled", то нужно смотреть <see cref="CancellationDetails"/>
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; } = null!;


        /// <summary>
        /// Сумма платежа. Иногда партнеры ЮKassa берут с пользователя дополнительную комиссию, которая не входит в эту сумму
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public CYookassa_Amount_v2 Amount { get; set; } = null!;


        /// <summary>
        /// Получатель платежа
        /// </summary>
        [JsonProperty(PropertyName = "recipient")]
        public CYookassa_Recipient_v2 Recipient { get; set; } = null!;


        /// <summary>
        /// Способ оплаты, который был использован для этого платежа
        /// </summary>
        [JsonProperty(PropertyName = "payment_method")]
        public CYookassa_Payment_Method_v2 Payment_Method { get; set; } = null!;


        /// <summary>
        /// Выбранный способ подтверждения платежа. Присутствует, когда платеж ожидает подтверждения от пользователя
        /// </summary>
        [JsonProperty(PropertyName = "confirmation")]
        public CYookassa_Confirmation_Result_v2? Confirmation { get; set; }


        /// <summary>
        /// Время создания заказа. Указывается по UTC и передается в формате ISO 8601. Пример: 2017-11-03T11:52:31.827Z
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime Created_At { get; set; }


        /// <summary>
        /// Время создания заказа. Указывается по UTC и передается в формате ISO 8601. Пример: 2017-11-03T11:52:31.827Z
        /// </summary>
        [JsonProperty(PropertyName = "test")]
        public bool Test { get; set; }


        /// <summary>
        /// Признак оплаты заказа
        /// </summary>
        [JsonProperty(PropertyName = "paid")]
        public bool Paid { get; set; }


        /// <summary>
        /// Возможность провести возврат по API
        /// </summary>
        [JsonProperty(PropertyName = "refundable")]
        public bool Refundable { get; set; }


        /// <summary>
        /// Статус регистрации чека
        /// </summary>
        [JsonProperty(PropertyName = "receipt_registration")]
        public string? Receipt_registration { get; set; }


        /// <summary>
        /// Любые дополнительные данные, которые нужны вам для работы (например, ваш внутренний идентификатор заказа) 
        /// </summary>
        [JsonProperty(PropertyName = "metadata")]
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
