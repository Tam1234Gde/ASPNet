using HIDE.RestAndRant.Business_YooKassa;
using HIDE.RestAndRant.Common;

namespace HIDE.RestAndRant.Web_Api_Test;

[TestClass]
public class YookassaUnitTest
{
    private readonly Yookassa _yookassa;
    public const string DefaultReturnUrl = "https://www.example.com/return_url";
    

    /// <summary>
    /// Конструктор
    /// </summary>
    public YookassaUnitTest()
    {
        TestHelper.Init();
        _yookassa = new Yookassa(AppConfig.Yookassa_Login, AppConfig.Yookassa_Password);
        AppConfig.Yookassa_Url = "https://api.yookassa.ru/v3/";
    }

    /// <summary>
    /// Создаёт ссылку для платежа
    /// </summary>
    /// <param name="sum">Сумма платежа</param>
    [TestMethod]
    [DataRow(500)]
    [DataRow(1500)]
    [DataRow(2999)]
    public void CreateUrl(int sum)
    {
        Dictionary<string, object> meta = new()
        {
            { "test", sum }
        };

        string? url = _yookassa.CreatePayUrl(sum, DefaultReturnUrl, false, "Test description", meta);
        Assert.IsNotNull(url);
        Assert.StartsWith("https://yoomoney.ru/checkout/payments", url);
    }

    [TestMethod]
    [DataRow(150)]
    [DataRow(1500)]
    [DataRow(5000)]
    [DataRow(12683)]
    public void RegisterNewCard(int sum)
    {
        string? cardId = _yookassa.RegisterCard(sum, TestHelper.DefaultCardNumber, "123", "07", "2032", DefaultReturnUrl);
#if DEBUG
        Assert.IsNotNull(cardId);
        Assert.IsTrue(TestHelper.IdRegex().IsMatch(cardId));
        Console.WriteLine($"Создана новая карта [{cardId}]");
#else
        Assert.IsNull(cardId);
#endif
    }

    [TestMethod]
    public void GenerateReceipt()
    {
        string? receiptUrl = _yookassa.GenerateReceiptUrl("2cd42748-000f-5000-9000-149c2153d8a2", false);
#if DEBUG
        Assert.IsNull(receiptUrl);
#else
        Assert.IsNotNull(receiptUrl);
        Assert.IsTrue(receiptUrl.StartsWith(Yookassa.OFD_URL));
        Console.WriteLine(receiptUrl);
#endif
    }

}