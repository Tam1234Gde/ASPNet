using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions;

public class CYokassa_ReceiptCustomer
{
	[JsonProperty("email")]
	public string? Email { get; set; }

	[JsonProperty("phone")]
	public string? Phone { get; set; }

	public CYokassa_ReceiptCustomer(string? email, string? phone)
	{
		if (!string.IsNullOrWhiteSpace(email))
		{
			this.Email = email;
			return;
		}

		this.Phone = phone;
	}
}