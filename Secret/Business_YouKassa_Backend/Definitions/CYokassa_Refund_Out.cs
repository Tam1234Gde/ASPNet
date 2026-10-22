using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions
{
	/// <summary>
	/// Обьект возвращаемый с API при возврате средств 
	/// --
	/// https://yookassa.ru/developers/api#refund_object
	/// </summary>
	internal class CYokassa_Refund_Out
	{
		/// <summary>
		/// Идентификатор возврата платежа в ЮKassa.
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public string ID { get; set; } = null!;


		/// <summary>
		/// Идентификатор платежа в ЮKassa.
		/// </summary>
		[JsonProperty(PropertyName = "payment_id")]
		public string Payment_id { get; set; } = null!;


		/// <summary>
		/// Идентификатор возврата платежа в ЮKassa. Возможные значения:
		///       pending — возврат создан, но пока еще обрабатывается;
		///       succeeded — возврат успешно завершен, указанная в запросе сумма переведена на платежное средство пользователя(финальный и неизменяемый статус);
		///       canceled — возврат отменен, инициатор и причина отмены указаны в объекте cancellation_details(финальный и неизменяемый статус).
		/// </summary>
		[JsonProperty(PropertyName = "status")]
		public string Status { get; set; } = null!;


		/// <summary>
		/// Время создания возврата. Указывается по UTC и передается в формате ISO 8601, например 2017-11-03T11:52:31.827Z
		/// </summary>
		[JsonProperty(PropertyName = "created_at")]
		public string? Created_at { get; set; }



		/// <summary>
		/// Сумма, которую нужно вернуть пользователю.
		/// </summary>
		[JsonProperty(PropertyName = "amount")]
		public CYokassa_Amount Amount { get; set; } = null!;


		/// <summary>
		/// Основание для возврата денег пользователю.
		/// </summary>
		[JsonProperty(PropertyName = "description")]
		public string? Description { get; set; }

	}
}
