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
    /// Контроллер - Столики в ресторане
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("RestaurantTables_v3 - Столики в ресторане")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/RestaurantTables/[action]")]
    [ApiController]
    public class RestaurantTablesController_v3(ILogger<RestaurantTablesController_v3> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает дату последнего изменения столиков в ресторане
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        [Obsolete]
        public JsonResult GetRestaurantTablesChangeDate()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Возвращает дату последнего изменения столиков в ресторане");
            }

            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Restaurant_Tables_DateChange);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Получает список столиков по коду ресторана
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CRestaurant_Table>))]
        public JsonResult GetRestaurantTablesList(int RestaurantID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Получает список столиков по коду ресторана");
            }

            List<CRestaurant_Table>? list_result = _bl.Restaurant_Tables.GetList(RestaurantID: RestaurantID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }
    }
}