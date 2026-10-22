using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Net;
using HIDE.RestAndRant.Common;

namespace HIDE.RestAndRant.Web_Api_Test;

[TestFixture(Author = "SOV")]
internal class ClientTest
{
    #region Comment - by MAE - 14.05.2026 - После доработки APi - требуетися обновить и проверить
    //private ClientsController_v1 _clientController;

    //public ClientTest()
    //{
    //    TestHelper.Init();

    //    _clientController = new ClientsController_v1(AppLog.CreateLogger<ClientsController_v1>(), new());
    //}

    //[Test]
    //public void GetCode_ValidNumber()
    //{
    //    JsonResult res = _clientController.Phone_GetCode(new Model_Clients_Phone_GetCode_v1()
    //    {
    //        Phone = "77777777777"
    //    });

    //    NUnit.Framework.Assert.That(res.StatusCode == (int)HttpStatusCode.OK, Is.True);
    //}

    //[Test]
    //public void Phone_Login_ValidAuth()
    //{
    //    JsonResult res = _clientController.Phone_Login(new Model_Clients_Phone_Login_v1()
    //    {
    //        Phone = "77777777777",
    //        Code = "1234"

    //    });

    //    NUnit.Framework.Assert.That(res.StatusCode == (int)HttpStatusCode.OK, Is.True);

    //    Result? result = (Result?)res.Value;
    //    Console.WriteLine(result);

    //    string guid = ((string?)result?.Data) ?? "";

    //    NUnit.Framework.Assert.That(result?.Code > 0, Is.True);
    //    NUnit.Framework.Assert.That(TestHelper.IdRegex().IsMatch(guid), Is.True);
    //}
    #endregion Comment - by MAE - 14.05.2026 - После доработки APi - требуетися обновить и проверить
}