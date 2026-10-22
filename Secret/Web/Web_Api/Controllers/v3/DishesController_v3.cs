using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v3._Enum;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Api.Controllers.v3
{
    /// <summary>
    /// Контроллер - Блюда
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("Dishes_v3 - Блюда")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/Dishes/[action]")]
    [ApiController]
    public class DishesController_v2(ILogger<DishesController_v2> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает дату последнего изменения меню
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetDishesChangeDate()
        {
            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Dishes_DateChange);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Получает список категорий блюд
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CDish_Category>))]
        [Obsolete]
        public JsonResult GetDishCategoriesList()
        {
            List<CDish_Category> list_result = _bl.Dish_Categories.GetList(Flag_IsParent: false);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Получает список категорий блюд по коду ресторана
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <returns></returns>
        [HttpGet("{RestaurantID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CDish_Category>))]
        public JsonResult GetDishCategoriesListByRestaurantID([Required] int RestaurantID)
        {
            List<CDish_Category> list_result = _bl.Dish_Categories.GetListByRestaurantID(RestaurantID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Получает список блюд
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CDish>))]
        [Obsolete]
        public JsonResult GetDishesList()
        {
            List<CDish> list_result = _bl.Dishes.GetList(Include_VolumesList: true, Include_Ingridients: true);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Возвращает список блюд v3
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <remarks>
        /// Пример запроса:
        ///  GET
        ///  
        ///     v3/Dishes/RestaurantID/1
        ///         
        /// </remarks> 
        /// <returns></returns>
        [HttpGet("{RestaurantID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CDish>))]
        public JsonResult GetDishesList_v3(int RestaurantID)
        {
            List<CDish> list_result = _bl.Dishes.GetList_v2(RestaurantID: RestaurantID, Include_VolumesList: true, Include_Ingridients: true);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Получает изображение блюда
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        public JsonResult DishImage_Get(int DishID)
        {
            if (DishID < 0)
            {
                _log.LogWarning(message: "Не удалось идентифицировать блюдо");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            bool check = _bl.Dishes.Check_Dish(ID: DishID);

            if (!check)
            {
                byte[]? image = _bl.Dishes.Image_Get(ID: DishID);
                return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: image);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: null);
        }


        /// <summary>
        /// Возвращает количество блюд, при котором добавляется дополнительное время
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public JsonResult GetDop_Cooking_DishCount()
        {
            int? val = HIDE_Convert.ToInt(_bl.Setups.Get(Key: Consts.SetupKey_Dop_Cooking_DishCount)?.Val ?? null);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Возвращает количество дополнительных минут, требуемых для заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetDop_CookingTime()
        {
            int? val = HIDE_Convert.ToInt(_bl.Setups.Get(Key: Consts.SetupKey_Dop_CookingTime)?.Val ?? null);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Возвращает максимальное количество минут для одного заказа с большим количеством позиций
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        [Obsolete]
        public JsonResult GetMaxTime_ForOrder()
        {
            int? val = HIDE_Convert.ToInt(_bl.Setups.Get(Key: Consts.SetupKey_Dop_MaxTime_ForOrder)?.Val ?? null);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }
    }
}