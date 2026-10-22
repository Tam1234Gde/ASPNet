using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v3._Enum;

namespace HIDE.RestAndRant.Web_Api.Controllers.v3
{
    /// <summary>
    /// Контроллер - Типы статусов в ресторане
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("RestaurantTableStatuses_v3 - Типы статусов в ресторане")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/RestaurantTableStatuses/[action]")]
    [ApiController]
    public class RestaurantTableStatusesController_v3(ILogger<RestaurantTableStatusesController_v3> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает дату последнего изменения статусов столика
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetRestaurantTableStatusesChangeDate()
        {
            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Restaurant_Table_Statuses_DateChange);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Получает список статусов столика
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CCommon>))]
        public JsonResult GetRestaurantTableStatusesList()
        {
            List<CRestaurant_Table_Status>? list_result = _bl.Restaurant_Table_Statuses.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Получает изображение стола
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        public JsonResult Get_RestaurantTableStatusImage(int RestaurantTableStatusID)
        {
            if (RestaurantTableStatusID < 0)
            {
                _log.LogWarning(message: "Не удалось идентифицировать статус столика");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            byte[]? image = _bl.Restaurant_Table_Statuses.Get_Image(ID: RestaurantTableStatusID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: image);
        }

    }
}