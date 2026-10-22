using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business_YooKassa;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
namespace HIDE.RestAndRant.Web_Api_Test;

[TestClass]
public static class ClientPayCardsControllerUnitTest
{
    #region Comment - by MAE - 14.05.2026 - После доработки APi - требуетися обновить и проверить
    //private readonly ClientPayCardsController_v1 _controller_V1;
    //private readonly Yookassa _yookassa;
    //public const string DefaultReturnUrl = "https://www.example.com/return_url";

    ///// <summary>
    ///// Конструктор
    ///// </summary>
    //public ClientPayCardsControllerUnitTest()
    //{
    //    TestHelper.Init();
    //    ILogger<ClientPayCardsController_v1> logger = AppLog.CreateLogger<ClientPayCardsController_v1>();
    //    _controller_V1 = new ClientPayCardsController_v1(_log: logger, _bl: new BL_Context());
    //    _yookassa = new Yookassa(AppConfig.Yookassa_Login, AppConfig.Yookassa_Password);
    //}

    ///// <summary>
    ///// Получение даты обновления карт для неавторизованного пользователя
    ///// </summary>
    //[TestMethod]
    //public void AddCardTest()
    //{
    //    string? cardId = _yookassa.RegisterCard(1, TestHelper.DefaultCardNumber, "123", "07", "2032", DefaultReturnUrl);
    //    Assert.IsNotNull(cardId);
    //    _controller_V1.MockController(Login: TestHelper.ClientID, Password: TestHelper.ClientGuid);

    //    Model_ClientPayCards_AddNewCard_v1 Data = new()
    //    {
    //        CardLastNum = "8453",
    //        CardIdent = cardId,
    //        //ClientPay_Card_Type = "Maestro",
    //        PaymentIdent = cardId,
    //        Summ = 1
    //    };

    //    JsonResult res = _controller_V1.AddNewCard(Data: Data);

    //    Result? result = (Result?)res.Value;
    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(Consts.Status_OK, result.Status);
    //}

    ///// <summary>
    ///// Получение даты обновления карт для неавторизованного пользователя
    ///// </summary>
    //[TestMethod]
    //public void GetLastEventDate_NoAuth()
    //{
    //    _controller_V1.MockController();

    //    JsonResult res = _controller_V1.GetLastUpdate();

    //    Result? result = (Result?)res.Value;
    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(Consts.Status_Err, result.Status);
    //}

    ///// <summary>
    ///// Получение даты обновления карт для авторизованного пользователя
    ///// </summary>
    //[TestMethod]
    //public void GetLastEventDate()
    //{
    //    _controller_V1.MockController(Login: TestHelper.ClientID, Password: TestHelper.ClientGuid);

    //    JsonResult res = _controller_V1.GetLastUpdate();

    //    Result? result = (Result?)res.Value;
    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(Consts.Status_OK, result.Status);
    //}

    ///// <summary>
    ///// Добавление новой карты для неавторизованного пользователя
    ///// </summary>
    //[TestMethod]
    //public void AddNewCard_NoAuth()
    //{
    //    _controller_V1.MockController();

    //    JsonResult res = _controller_V1.AddNewCard(Data: new Model_ClientPayCards_AddNewCard_v1());

    //    Result? result = (Result?)res.Value;
    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(Consts.Status_Err, result.Status);
    //}

    ///// <summary>
    ///// Добавление новой карты для авторизованного пользователя, но данных карты нет
    ///// </summary>
    //[TestMethod]
    //public void AddNewCard_Empty()
    //{
    //    _controller_V1.MockController(Login: TestHelper.ClientID, Password: TestHelper.ClientGuid);

    //    JsonResult res = _controller_V1.AddNewCard(Data: new Model_ClientPayCards_AddNewCard_v1());

    //    Result? result = (Result?)res.Value;
    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(Consts.Status_Err, result.Status);
    //}

    ///// <summary>
    ///// Добавление новой карты для авторизованного пользователя, но данные карты неактуальны
    ///// </summary>
    //[TestMethod]
    //public void AddInvalidNewCard()
    //{
    //    _controller_V1.MockController(Login: TestHelper.ClientID, Password: TestHelper.ClientGuid);

    //    JsonResult res = _controller_V1.AddNewCard(Data: new Model_ClientPayCards_AddNewCard_v1
    //    {
    //        Summ = (decimal)1.0,
    //        PaymentIdent = "2cc838ca-000f-5000-a000-1f71321c4abc",
    //        CardLastNum = "4444",
    //        //ClientPay_Card_Type = "Visa",
    //        CardIdent = "2cc838ca-000f-5000-a000-1f71321c4c65"
    //    });

    //    Result? result = (Result?)res.Value;
    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(Consts.Status_Err, result.Status);
    //}
    #endregion Comment - by MAE - 14.05.2026 - После доработки APi - требуетися обновить и проверить
}