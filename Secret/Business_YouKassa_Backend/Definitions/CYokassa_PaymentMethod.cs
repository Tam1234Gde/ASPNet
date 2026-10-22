using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions
{
	public class CYokassa_PaymentMethod
	{
		[JsonProperty(PropertyName = "payment_method_data")]
		public string PaymentMethodData { get; set; } = null!;
	}
}