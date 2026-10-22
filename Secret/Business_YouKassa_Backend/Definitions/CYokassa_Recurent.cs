using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions
{
	internal class CYokassa_Recurent
	{
		/// <summary>
		/// Сумма, которую нужно вернуть пользователю.
		/// </summary>
		[JsonProperty(PropertyName = "amount")]
		public CYokassa_Amount Amount { get; set; } = null!;


		/// <summary>
		/// Идентификатор платежа в ЮKassa.
		/// </summary>
		[JsonProperty(PropertyName = "capture")]
		public bool Capture { get; set; }

		/// <summary>
		/// Идентификатор сохраненного метода оплаты
		/// </summary>
		[JsonProperty(PropertyName = "payment_method_id")]
		public string CardIdent { get; set; } = null!;


		/// <summary>
		/// Описание платежа
		/// </summary>
		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; } = null!;

		[JsonProperty("receipt")]
		public CYokassa_Receipt? Receipt { get; set; }
	}
}
