using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions;

/// <summary>
/// <para>Результат покупки</para>
/// <para>Если платеж проводится в первый раз, то в ответе будет Confirmation</para>
/// <para>Если проводится повторный платеж, то Confirmation уже не будет</para>
/// <para></para>
/// <para></para>
/// </summary>
public class CYokassa_BuyResult
{
	/// <summary>
	/// Идентификатор оплаты
	/// </summary>
	[JsonProperty(PropertyName = "id")]
	public string? Id { get; set; }

	/// <summary>
	/// Статус подписки, нормальный статус "succeeded". Если статус перешёл в состояние "canceled", то нужно смотреть <see cref="CancellationDetails"/>
	/// </summary>
	[JsonProperty(PropertyName = "status")]
	public string? Status { get; set; }

	/// <summary>
	/// Данные отмены
	/// </summary>
	[JsonProperty(PropertyName = "cancellation_details")]
	public CYokassa_Pay_CancellationDetails? CancellationDetails { get; set; }

	/// <summary>
	/// Данные подтверждения платежа
	/// </summary>
	[JsonProperty(PropertyName = "confirmation")]
	public CYokassa_BuyResultConfirmation? Confirmation { get; set; }

	/// <summary>
	/// Метаданные, конечный пользователь 
	/// </summary>
	[JsonProperty(PropertyName = "metadata")]
	public Dictionary<string, object>? Metadata { get; set; }

	[JsonProperty(PropertyName = "payment_method")]
	public CYokassa_Buy_PaymentMethod? PaymentMethod { get; set; }

	public bool IsPayed()
	{
		return Status is "succeeded";
	}
}