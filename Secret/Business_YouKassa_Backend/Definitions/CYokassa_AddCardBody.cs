#if DEBUG
using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions;

internal class CYokassa_AddCardBody
{
	/// <summary>
	/// Сумма, которую нужно оплатить
	/// </summary>
	[JsonProperty(PropertyName = "amount")]
	public CYokassa_Amount Amount { get; set; } = null!;

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

	/// <summary>
	/// Нужно ли сразу захватить платеж, или нужно подтверждение от менеджера
	/// </summary>
	[JsonProperty(PropertyName = "capture")]
	public bool Capture { get; set; }

	[JsonProperty("payment_method_data")] public CYokassa_PaymentMethodData PaymentMethodData { get; set; } = null!;
}

internal class CYokassa_PaymentMethodData
{
	[JsonProperty(PropertyName = "card")] public CYokassa_CardData CardData { get; set; } = null!;

	[JsonProperty(PropertyName = "type")] public string Type { get; set; } = null!;
}

internal class CYokassa_CardData
{
	[JsonProperty(PropertyName = "cardholder")]
	public string CardHolder { get; set; } = null!;

	[JsonProperty(PropertyName = "csc")]
	public string Csc { get; set; } = null!;

	[JsonProperty(PropertyName = "expiry_month")]
	public string ExpiryMonth { get; set; } = null!;

	[JsonProperty(PropertyName = "expiry_year")]
	public string ExpiryYear { get; set; } = null!;

	[JsonProperty(PropertyName = "number")]
	public string Number { get; set; } = null!;
}

#endif