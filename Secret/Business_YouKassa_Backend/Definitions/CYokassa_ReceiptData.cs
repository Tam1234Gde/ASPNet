using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions
{
	public class CYokassa_ReceiptData
	{
		[JsonProperty("items")]
		public List<CYokassa_ReceiptItem>? Items { get; set; }
	}
}