using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions
{
	internal class CYokassa_Buy
	{
		/// <summary>
		/// Сумма, которую нужно оплатить
		/// </summary>
		[JsonProperty(PropertyName = "amount")]
		public CYokassa_Amount Amount { get; set; } = null!;

		/// <summary>
		/// Нужно ли сразу захватить платеж, или нужно подтверждение от менеджера
		/// </summary>
		[JsonProperty(PropertyName = "capture")]
		public bool Capture { get; set; }

		[JsonProperty(PropertyName = "confirmation")]
		public CYokassa_Confirmation Confirmation { get; set; } = null!;

		/// <summary>
		/// Описание платежа
		/// </summary>
		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; } = null!;

		/// <summary>
		/// Нужно ли сразу захватить платеж, или нужно подтверждение от менеджера
		/// </summary>
		[JsonProperty(PropertyName = "save_payment_method")]
		public bool SavePaymentMethod { get; set; }

		[JsonProperty(PropertyName = "metadata")]
		public Dictionary<string, object>? Metadata { get; set; }
	}
}