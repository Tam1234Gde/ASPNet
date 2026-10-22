using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions
{
	public class CYokassa_ReceiptItem
	{
		[JsonProperty("fiscal_attribute")]
		public string? Fiscal_Attribute { get; set; }

		[JsonProperty("fiscal_document_number")]
		public string? Fiscal_Document_Number { get; set; }

		[JsonProperty("fiscal_storage_number")]
		public string? Fiscal_Storage_Number { get; set; }
	}
}