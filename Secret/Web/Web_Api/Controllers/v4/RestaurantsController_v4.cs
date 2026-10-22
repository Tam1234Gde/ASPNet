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
using HIDE.RestAndRant.Web_Api.Models.v4._Enum;

namespace HIDE.RestAndRant.Web_Api.Controllers.v4
{
    /// <summary>
    /// Контроллер - Рестораны
    /// </summary>
    /// <param name="_bl"></param>
    [Tags("Restaurants_v4 - Рестораны")]
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/Restaurants/[action]")]
    [ApiController]
    public class RestaurantsController_v4(BL_Context _bl) : ControllerBase
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
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: val);
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
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
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
        //    return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: result);
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
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: result);
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
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }


        /// <summary>
        /// Возвращает номер телефона из настроек админки
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///  GET
        ///  
        ///     v3/Restaurants/GetSetupsPhoneNumber
        ///         
        /// </remarks> 
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string))]
        public JsonResult GetSetupsPhoneNumber()
        {
            CSetup? defaultPhoneNumber = _bl.Setups.Get(Consts.SetupKey_DefaultPhoneNumber);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: defaultPhoneNumber?.Val);
        }



        /// <summary>
        /// Возвращает часы работы ресторана
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///  GET
        ///  
        ///     v3/Restaurants/GetSetupsRestaurantsDefaultWorkHours
        ///         
        /// </remarks> 
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<int>))]
        public JsonResult GetSetupsRestaurantsDefaultWorkHours()
        {
            CSetup? setup = _bl.Setups.Get(Consts.SetupKey_Restaurants_DefaultWorkHours);

            List<int> result = new List<int>();
            if (setup?.Val != null)
            {
                // Вариант А: Если строка содержит числа через разделитель, например "1,2,3,4,5"
                result = setup.Val
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();

                // Вариант Б: Если строка в формате JSON массива, например "[1,2,3,4,5]"
                // result = JsonConvert.DeserializeObject<List<int>>(setup.Val);
            }

            return Helper_Result.GetJsonResult(
                Status: Consts.Status_OK,
                Code: (int)EResult_v4.OK,
                Data: result
            );
        }

    }
}