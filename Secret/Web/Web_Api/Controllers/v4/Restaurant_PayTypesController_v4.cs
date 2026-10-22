using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v4._Enum;
using HIDE.RestAndRant.Web_Api.Models.v4.Restaurant_PayTypes;

namespace HIDE.RestAndRant.Web_Api.Controllers.v4
{
    /// <summary>
    /// Контроллер - Доступные типы оплаты в ресторане
    /// </summary>
    /// <param name="_bl"></param>
    [Tags("Restaurant_PayTypes_v4 - Доступные типы оплаты в ресторане")]
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/Restaurant_PayTypes/[action]")]
    [ApiController]
    public class Restaurant_PayTypesController_v4(BL_Context _bl) : ControllerBase
    {



        /// <summary>
        /// Возвращает список доступных типов оплаты в ресторане
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CRestaurant_PayType>))]
        public JsonResult GetRestaurant_PayTypesList([FromQuery] Model_Restaurant_PayTypes_v4 Data)
        {
            List<CRestaurant_PayType>? list_result = _bl.Restaurant_PayTypes.GetList(RestaurantID: Data.RestaurantID, Order_TypeID: Data.Order_TypeID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }

    }
}