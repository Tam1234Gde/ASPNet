using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v3._Enum;
namespace HIDE.RestAndRant.Web_Api.Controllers.v3
{
    /// <summary>
    /// Контроллер - Рестораны
    /// </summary>
    /// <param name="_bl"></param>
    [Tags("Restaurants_v3 - Рестораны")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/Restaurants/[action]")]
    [ApiController]
    public class RestaurantsController_v3(BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает дату последнего изменения ресторанов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetRestaurantsChangeDate()
        {
            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Restaurants_DateChange);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Возвращает список ресторанов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CRestaurant>))]
        public JsonResult GetRestaurantsList()
        {
            List<CRestaurant>? list_result = _bl.Restaurants.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Возвращает ресторан
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <remarks>
        /// Пример запроса:
        ///  GET
        ///  
        ///     v3/Restaurants/RestaurantID/1
        ///         
        /// </remarks> 
        /// <returns></returns>
        [HttpGet("{RestaurantID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CRestaurant))]
        [Obsolete]
        public JsonResult GetRestaurant(int RestaurantID)
        {
            CRestaurant? result = _bl.Restaurants.Get(RestaurantID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: result);
        }


        /// <summary>
        /// Возвращает список планировок
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CRestaurant_Hall>))]
        [Obsolete]
        public JsonResult GetRestaurantHallsList([Required] int RestaurantID, [Required] ERestaurant_Table_Type Restaurant_Table_TypeID)
        {
            List<CRestaurant_Hall>? list_result = _bl.Restaurant_Halls.GetListByRestaurantID(RestaurantID, Restaurant_Table_TypeID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        ///// <summary>
        ///// Возвращает планировку ресторана
        ///// </summary>
        ///// <param name="Restaurant_HallID">Код планировки</param>
        ///// <returns></returns>
        //[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        //[HttpGet]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public JsonResult GetRestaurantImage([FromQuery][Required] int Restaurant_HallID)
        //{
        //    byte[]? result = _bl.Restaurant_Halls.Get(ID: Restaurant_HallID)?.Restaurant_Image;
        //    return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: result);
        //}


        /// <summary>
        /// Возвращает планировку помещения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        public JsonResult GetRestaurantImage(int RestaurantID, ERestaurant_Table_Type Restaurant_Table_TypeID)
        {
            byte[]? result = _bl.Restaurant_Halls.Get(RestaurantID: RestaurantID, Restaurant_Table_TypeID: Restaurant_Table_TypeID)?.Restaurant_Image;
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: result);
        }


        /// <summary>
        /// Возвращает список ресторанов по коду города
        /// </summary>
        /// <param name="CityID">Код города</param>
        /// <remarks>
        /// Пример запроса:
        ///  GET
        ///  
        ///     v3/Restaurants/GetRestaurantsListByCityID/1
        ///         
        /// </remarks> 
        /// <returns></returns>
        [HttpGet("{CityID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CRestaurant>))]
        public JsonResult GetRestaurantsListByCityID(int CityID)
        {
            List<CRestaurant>? list_result = _bl.Restaurants.GetListByCityID(CityID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }

    }
}