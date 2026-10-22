using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions;

public class CYokassa_Refund_Result
{
	[JsonProperty("type")]
	public string? Type { get; set; }

	[JsonProperty("parameter")]
	public string? Parameter { get; set; }

	[JsonProperty("description")]
	public string? Description { get; set; }

	[JsonProperty("code")]
	public string? Code { get; set; }

	[JsonProperty("status")]
	public string? Status { get; set; }

	[JsonProperty("id")]
	public string? ID { get; set; }

	[JsonProperty("payment_id")]
	public string? PaymentID { get; set; }

	/// <summary>
	/// Если неправильный уникальный ключ
	/// </summary>
	/// <returns></returns>
	public bool Is_InvalidGuid()
	{
		return Parameter is "Idempotence-Key";
	}

	/// <summary>
	/// Если сумма возврата больше чем сумма оплаты
	/// </summary>
	/// <returns></returns>
	public bool Is_RefundSum_MaxWhenCurrent()
	{
		return Parameter is "amount.value";
	}

	/// <summary>
	/// Возврат не был найден
	/// </summary>
	/// <returns></returns>
	public bool Is_RefundNotFound()
	{
		return Description is "Refund not found or forbidden";
	}

	public bool IsSuccess()
	{
		return Status is "succeeded";
	}
}