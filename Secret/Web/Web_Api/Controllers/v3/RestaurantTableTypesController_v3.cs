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
    /// Контроллер - Типы столиков в ресторане
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("RestaurantTableTypes_v3 - Типы столиков в ресторане")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/RestaurantTableTypes/[action]")]
    [ApiController]
    public class RestaurantTableTypesController_v3(ILogger<RestaurantTableTypesController_v3> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает дату последнего изменения типов столика
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetRestaurantTableTypesChangeDate()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Возвращает дату последнего изменения типов столика");
            }

            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Restaurant_Table_Types_DateChange);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Получает список типов столика
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CCommon>))]
        public JsonResult GetRestaurantTableTypesList()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Получает список типов столика");
            }

            List<CCommon>? list_result = _bl.Restaurant_Table_Types.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Получает список доступных типов столика по RestaurantID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CCommon>))]
        public JsonResult GetRestaurantTableTypesListByRestaurantID(int RestaurantID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Получает список доступных типов столика по RestaurantID");
            }

            List<CCommon>? list_result = _bl.Restaurant_Table_Types.GetListByRestaurantID(RestaurantID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }

    }
}