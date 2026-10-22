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
using HIDE.RestAndRant.Web_Api.Models.v3._Enum;
using HIDE.RestAndRant.Web_Api.Models.v3.Ratings;

namespace HIDE.RestAndRant.Web_Api.Controllers.v3
{
    /// <summary>
    /// Контроллер - Оценка приложения
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("Ratings_v3 - Оценки приложения")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/Ratings/[action]")]
    [ApiController]
    [BasicAuth]
    public class RatingsController_v3(ILogger<RatingsController_v3> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Проверка требуется ли предложить оценку пользователю
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(bool))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public JsonResult CheckRating()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            bool result = _bl.Ratings.CheckRating(ClientID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: result);
        }


        /// <summary>
        /// Добавление оценки
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult Add(Model_Ratings_Add_v3 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            int result = _bl.Ratings.Add(ClientID: client.ID, Grade: Data.Grade, Text: Data.Text);

            if (result < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: result, Data: Errors.Err202, Message: Errors.Err202_str);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: result);
        }

    }
}