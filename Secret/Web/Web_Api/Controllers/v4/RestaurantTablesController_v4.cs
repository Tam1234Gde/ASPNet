using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v4._Enum;

namespace HIDE.RestAndRant.Web_Api.Controllers.v4
{
    /// <summary>
    /// Контроллер - Столики в ресторане
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("RestaurantTables_v4 - Столики в ресторане")]
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/RestaurantTables/[action]")]
    [ApiController]
    public class RestaurantTablesController_v4(ILogger<RestaurantTablesController_v4> _log, BL_Context _bl) : ControllerBase
    {

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
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }
    }
}