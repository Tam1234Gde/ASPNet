using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
	/// <summary>
	/// Объект платежа
	/// </summary>
	public class CYookassa_Payment_v2
	{
		/// <summary>
		/// Сумма, которую нужно оплатить
		/// </summary>
		[JsonProperty(PropertyName = "amount")]
		public CYookassa_Amount_v2 Amount { get; set; } = null!;


        /// <summary>
        /// Данные для формирования чека
        /// </summary>
        [JsonProperty(PropertyName = "receipt")]
		public CYookassa_Receipt_v2? Receipt { get; set; }


        /// <summary>
        /// Идентификатор сохраненного способа оплаты
        /// </summary>
        [JsonProperty(PropertyName = "payment_method_id")]
		public string? Payment_Method_ID { get; set; }


        /// <summary>
        /// Данные для оплаты конкретным способом
        /// </summary>
        [JsonProperty(PropertyName = "payment_method_data")]
		public CYookassa_Payment_Method_Data_v2 Payment_Method_Data { get; set; } = null!;


        /// <summary>
        /// Данные, необходимые для инициирования выбранного сценария подтверждения платежа пользователем
        /// </summary>
        [JsonProperty(PropertyName = "confirmation")]
		public CYookassa_Confirmation_v2 Confirmation { get; set; } = null!;


		/// <summary>
		/// Описание платежа
		/// </summary>
		[JsonProperty(PropertyName = "description")]
		public string? Description { get; set; }


        /// <summary>
        /// Автоматический прием поступившего платежа
        /// </summary>
        [JsonProperty(PropertyName = "capture")]
        public bool Capture { get; set; }


        /// <summary>
        /// Сохранение платежных данных для проведения автоплатежей
        /// </summary>
        [JsonProperty(PropertyName = "save_payment_method")]
		public bool SavePaymentMethod { get; set; }


		/// <summary>
		/// Любые дополнительные данные, которые нужны вам для работы
		/// </summary>
		[JsonProperty(PropertyName = "metadata")]
		public Dictionary<string, object>? Metadata { get; set; }
	}
}