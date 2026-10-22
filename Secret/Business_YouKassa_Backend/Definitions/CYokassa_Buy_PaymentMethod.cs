using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions;

public class CYokassa_Buy_PaymentMethod
{
	[JsonProperty(PropertyName = "id")]
	public string? Id { get; set; }
}