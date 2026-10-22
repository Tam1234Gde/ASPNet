using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Api.Auth;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v4._Enum;
using HIDE.RestAndRant.Web_Api.Models.v4.Client_Notifications;

namespace HIDE.RestAndRant.Web_Api.Controllers.v4
{
    /// <summary>
    /// Контроллер - Уведомления пользователей
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("ClientNotifications_v4 - Уведомления пользователей")]
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/ClientNotifications/[action]")]
    [ApiController]
    [BasicAuth]
    public class ClientNotificationsController_v4(ILogger<ClientNotificationsController_v4> _log, BL_Context _bl) : Controller
    {
        /// <summary>
        /// Получение списка нотификаций
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CClient_Notification>))]
        public JsonResult GetList()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            List<CClient_Notification> client_data = _bl.Client_Notifications.GetList(ClientID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: client_data);
        }


        /// <summary>
        /// Получение информации о последней даты нотификации, если null - то нотификаций нет
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DateTime?))]
        public JsonResult GetLastEventDate()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            DateTime? date = _bl.Client_Notifications.GetLastEventDate(ClientID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: date);
        }


        /// <summary>
        /// Пометить все нотификации как прочитанные
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult MarkAs_Read_All()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            int result = _bl.Client_Notifications.Read_All(ClientID: client.ID);

            if (result < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: result, Data: Errors.Err200, Message: Errors.Err200_str);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: result);
        }


        /// <summary>
        /// Отправка токена регистрации
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult AddToken(Model_Client_Notification_Token_v4 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            int clientnotificationid = _bl.Client_Notification_Tokens.Add(ClientID: client.ID, TokenTypeID: Data.TokenTypeID, DeviceID: Data.DeviceID, Token: Data.Token);

            if (clientnotificationid < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: clientnotificationid, Data: Errors.Err200, Message: Errors.Err200_str);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: clientnotificationid);
        }


        /// <summary>
        /// Удаление токена регистрации
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult DeleteToken(Model_Client_Notification_Token_Del_v4 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            int clientnotificationid = _bl.Client_Notification_Tokens.Del(DeviceID: Data.DeviceID);

            if (clientnotificationid < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: clientnotificationid, Data: Errors.Err200, Message: Errors.Err200_str);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: clientnotificationid);
        }

    }
}