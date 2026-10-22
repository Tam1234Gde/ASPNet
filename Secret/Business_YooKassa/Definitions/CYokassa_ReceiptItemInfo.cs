using Newtonsoft.Json;

namespace HIDE.RestAndRant.Business_YooKassa.Definitions;

internal class CYokassa_ReceiptItemInfo
{
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("quantity")] public string Quantity { get; set; }
    [JsonProperty("vat_code")] public string VatCode { get; set; }
    [JsonProperty("payment_subject")] public string PaymentSubject { get; set; }
    [JsonProperty("payment_mode")] public string PaymentMode { get; set; }
    [JsonProperty("amount")] public CYokassa_Amount Amount { get; set; }

    public CYokassa_ReceiptItemInfo(string description, string quantity, string vatCode, string paymentSubject, string paymentMode, CYokassa_Amount amount)
    {
        Description = description;
        Quantity = quantity;
        VatCode = vatCode;
        PaymentSubject = paymentSubject;
        PaymentMode = paymentMode;
        Amount = amount;
    }

    public CYokassa_ReceiptItemInfo(string description, CYokassa_Amount amount)
    {
        Description = description;
        Amount = amount;

        Quantity = "1.00";
        VatCode = "1";
        PaymentSubject = "service";
        PaymentMode = "full_payment";
    }
}