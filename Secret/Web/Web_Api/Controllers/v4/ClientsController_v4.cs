using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Web_Api.Auth;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v4;
using HIDE.RestAndRant.Web_Api.Models.v4._Enum;
using HIDE.RestAndRant.Web_Api.Models.v4.Clients;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Api.Controllers.v4
{
    /// <summary>
    /// Контроллер - Клиенты
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("Clients_v4 - Клиенты")]
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/Clients/[action]")]
    [ApiController]
    [BasicAuth]
    public class ClientsController_v4(ILogger<ClientsController_v4> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает дату обновления данных клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DateTime?))]
        public JsonResult GetClientData_DateLastUpdate()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            CClient? client_data = _bl.Clients.Get(ID: client.ID);

            _bl.Clients.Edit_LastAction(ID: client.ID);

            if (client_data == null)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: Errors.Err201, Message: Errors.Err201_str);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: client_data.Date_LastUpdate);
        }


        /// <summary>
        /// Получение данных клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Model_Clients_Data_Client_v4))]
        public JsonResult GetClientData()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            CClient? cclient = _bl.Clients.Get(client.ID);
            if (cclient == null)
            {
                _log.LogError(message: "Данные клиента не найдены");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)Errors.Err201);
            }

            Model_Clients_Data_Client_v4 client_result = new()
            {
                ID = cclient.ID,
                Guid = cclient.Guid,
                Phone = cclient.Phone,
                Name = cclient.Name,
                Date_Birth = cclient.Date_Birth,
                Date_Registration = cclient.Date_Registration,
                Flag_IsPolicy = cclient.Flag_IsPolicy,
                Car_Brand = cclient.Car_Brand,
                Car_RegNumber = cclient.Car_RegNumber,
                Car_Color = cclient.Car_Color,
                Delivery_Address = cclient.Delivery_Address,
                Delivery_Apartment = cclient.Delivery_Apartment,
                Delivery_EntranceCode = cclient.Delivery_EntranceCode,
                Delivery_EntranceNumber = cclient.Delivery_EntranceNumber,
                Delivery_Floor = cclient.Delivery_Floor,
                Name_Another = cclient.Name_Another,
                Phone_Another = cclient.Phone_Another,
            };

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: client_result);
        }


        /// <summary>
        /// Возвращает дату обновления фото у клиента, или null - если фото отсутствует
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DateTime?))]
        public JsonResult Photo_GetDate()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            CClient? client_data = _bl.Clients.Get(ID: client.ID);

            if (client_data == null)
            {
                _log.LogError(message: "Данные клиента не найдены");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)Errors.Err201);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: client_data.Date_PhotoUpdate);
        }


        /// <summary>
        /// Возвращает фото клиента или null если его нет
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        public JsonResult Photo_Get()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            byte[]? photo = _bl.Clients.Photo_Get(ID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: photo);
        }


        /// <summary>
        /// Получить количество текущих бонусов клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int), Description = "Количество бонусов")]
        public JsonResult GetBonuses()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            CClient? cclient = _bl.Clients.Get(client.ID);
            if (cclient == null)
            {
                _log.LogError(message: "Данные клиента не найдены");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)Errors.Err201);
            }

            decimal? bonuses = _bl.Bonuses.Check_Bonuses(client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: bonuses);
        }


        /// <summary>
        /// Возвращает дату последнего изменения количества бонусов клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetBonuses_ChangeDate()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            DateTime? date_max = _bl.Bonuses.GetMaxLastEditDate(ClientID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: date_max);
        }


        /// <summary>
        /// Вход/Регистрация и получение кода по СМС
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///  POST
        ///  
        ///     {   
        ///         "Phone": "79030000001"
        ///     }
        /// 
        /// </remarks>        
        /// <param name="Data">Телефон, для которого осуществляется вход</param>
        /// <returns>JSON с результатом запроса - ОК или ошибка</returns>
        [HttpPost]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(EResult_v4), Description = "Код успеха (200), или код ошибки")]
        public JsonResult Phone_GetCode(Model_Clients_Phone_GetCode_v4 Data)
        {
            string requestGuid = (DateTime.Now.Ticks - new DateTime(2023, 1, 1).Ticks).ToString("x");


            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: $"#{requestGuid}# Start # Phone='{Data.Phone}' ## ip={GetIp()}");
            }


            if (string.IsNullOrWhiteSpace(Data.Phone))
            {

                _log.LogError(message: "{}", args: $"Empty phone #{requestGuid}# Err # ClientID==null");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: Errors.Err200);
            }

            CClient? client = _bl.Clients.GetOrAdd(Phone: Data.Phone, Flag_SendSms: true);

            if (client == null)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # ClientID==null");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }


            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: $"#{requestGuid}# End # ClientID='{client.ID}'");
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK);
        }


        /// <summary>
        /// Подтверждение SMS-кода и возврат данных клиента
        /// </summary>
        /// <param name="Data">Данные JSON для класса Mod_Client_LoginSmsCode_v1</param>
        /// <returns>JSON с результатами в поле Data в формате Mod_Client_Data_v1</returns>
        [HttpPost]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<Result_v4>), Description = "Code - код клиента или код ошибки (если отрицательное), Data - guid клиента")]
        public JsonResult Phone_Login(Model_Clients_Phone_Login_v4 Data)
        {
            string requestGuid = (DateTime.Now.Ticks - new DateTime(2023, 1, 1).Ticks).ToString("x");


            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: $"#{requestGuid}# Start # Phone='{Data.Phone}' Code='{Data.Code}' ## ip={GetIp()}");
            }



            if (string.IsNullOrWhiteSpace(Data.Phone) || string.IsNullOrWhiteSpace(Data.Code))
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {Errors.Err3011} - '{Errors.Err3011_str}' ## {Data}");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: Errors.Err3011, Message: Errors.Err3011_str);
            }

            CClient? client = _bl.Clients.GetClient_ByPhone_AndSms(Phone: Data.Phone, SmsCode: Data.Code);
            if (client == null)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {Errors.Err3011} - '{Errors.Err3011_str}' ## {Data}");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)ELogin_Status_v4.SmsCodeErr, Message: Errors.Err3011_str);
            }


            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: $"#{requestGuid}# OK # Phone='{Data.Phone}' Code='{Data.Code}'");
            }


            if (client.Date_Registration != null && client.Date_Registration > DateTime.Now.AddHours(-1))
            {
                _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client.ID), ShablonID: EClient_Notification_Shablon.Auth, OrderID: null);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: client.ID, Data: client.Guid);
        }


        /// <summary>
        /// Установить флаг политики
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult SetFlag_IsPolicy()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            CClient? cclient = _bl.Clients.Get(client.ID);
            if (cclient == null)
            {
                _log.LogError(message: "Данные клиента не найдены");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)Errors.Err201);
            }

            if (!cclient.Flag_IsPolicy)
            {
                _bl.Clients.Flag_IsPolicy_Set(ID: cclient.ID, Flag_IsPolicy: true);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: client.ID);
        }


        /// <summary>
        /// Редактирование данных пользователя
        /// </summary>
        /// <param name="Data"></param>
        /// <returns>Код отредактированного клиента в Data</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult Edit_Data(Model_Clients_EditData_v4 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            int clientid = _bl.Clients.Edit(ID: client.ID, Name_New: Data.Name, Date_Birth_New: Data.Date_Birth);

            if (clientid < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: clientid, Message: Errors.GetErrorString(ErrorVal: clientid));
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: clientid);
        }


        /// <summary>
        /// Устанавливает фотографию пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult Photo_Set(Model_Clients_PhotoSet_v4 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            int clientid = _bl.Clients.Photo_Set(ID: client.ID, Photo: Data.FileData);

            if (clientid < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: clientid, Message: Errors.GetErrorString(ErrorVal: clientid));
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: clientid);
        }


        /// <summary>
        /// Удаление аккаунта
        /// </summary>
        /// <returns>Код удаленного аккаунта или код ошибки</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult Del_Account()
        {
            string requestGuid = (DateTime.Now.Ticks - new DateTime(2023, 1, 1).Ticks).ToString("x");


            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: $"#{requestGuid}# Start");
            }


            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "{}", args: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            int res = _bl.Clients.Del_Account(ClientID: client.ID);

            if (res < 0)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {res} # Ошибка удаления аккаунта");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: res, Message: Errors.GetErrorString(res));
            }

            if (res != client.ID)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {res} # Ошибка удаления аккаунта, код не совпадает");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: Errors.Err200);
            }

            res = _bl.Orders.Del_All(ClientID: client.ID);
            if (res < 0)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {res} # Ошибка удаления заказов аккаунта");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: res, Message: Errors.GetErrorString(res));
            }

            if (res != client.ID)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {res} # Ошибка удаления аккаунта, код не совпадает");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: Errors.Err200);
            }

            res = _bl.Order_Favorites.Del_All(ClientID: client.ID);
            if (res < 0)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {res} # Ошибка удаления избранных заказов аккаунта");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: res, Message: Errors.GetErrorString(res));
            }

            if (res != client.ID)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {res} # Ошибка удаления аккаунта, код не совпадает");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: Errors.Err200);
            }

            res = _bl.Bonuses.Del_All(ClientID: client.ID);
            if (res < 0)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {res} # Ошибка обнуления бонусов аккаунта");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: res, Message: Errors.GetErrorString(res));
            }

            if (res != client.ID)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {res} # Ошибка удаления аккаунта, код не совпадает");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: Errors.Err200);
            }

            res = _bl.ClientPay_Cards.Cards_DelAll(ClientID: client.ID);
            if (res < 0)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {res} # Ошибка удаления банковских карт аккаунта");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: res, Message: Errors.GetErrorString(res));
            }
            if (res != client.ID)
            {

                _log.LogError(message: "{}", args: $"#{requestGuid}# Err # {res} # Ошибка удаления банковских карт аккаунта, код не совпадает");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: Errors.Err200);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK);
        }


        /// <summary>
        /// Возвращает IP-адрес клиента
        /// </summary>
        /// <returns></returns>
        private string? GetIp()
        {
            try
            {
                return Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }
            catch (Exception e)
            {
                _log.LogError(exception: e, message: $"Error while get IP");
            }

            return null;
        }


        /// <summary>
        /// Обновить дату последней активности вручную
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult Edit_LastAction()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            int result = _bl.Clients.Edit_LastAction(ID: client.ID);

            if(result > 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK);
            }
            else
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }
        }
    }
}