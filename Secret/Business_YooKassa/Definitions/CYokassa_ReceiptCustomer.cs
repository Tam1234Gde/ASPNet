using Newtonsoft.Json;

namespace HIDE.RestAndRant.Business_YooKassa.Definitions;

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
            Email = email;
            return;
        }

        Phone = phone;
    }
}