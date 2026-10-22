using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions;

internal class CYokassa_Receipt
{
	[JsonProperty("customer")]
	public CYokassa_ReceiptCustomer? Customer { get; set; }

	[JsonProperty("items")]
	public List<CYokassa_ReceiptItemInfo> Items { get; set; } = null!; //TODO: ED - проверить это nullable список или нет
}
