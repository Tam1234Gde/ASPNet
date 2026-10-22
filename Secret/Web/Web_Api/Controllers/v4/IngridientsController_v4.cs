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
    /// Контроллер - Ингредиенты
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("Ingridients_v4 - Ингредиенты")]
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/Ingridients/[action]")]
    [ApiController]
    public class IngridientsController_v4(ILogger<IngridientsController_v4> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Получает изображение ингредиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        public JsonResult IngridientImage_Get(int IngridientID)
        {
            if (IngridientID < 0)
            {
                _log.LogWarning(message: "Не удалось идентифицировать ингредиент");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err);
            }

            //bool check = _bl.Ingridients.Check_Ingridient(ID: IngridientID);

            //if (!check)
            //{
            //    byte[]? image = _bl.Ingridients.Image_Get(ID: IngridientID);
            //    return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: image);
            //}


            byte[]? image = _bl.Ingridients.Image_Get(ID: IngridientID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: image);
        }


        /// <summary>
        /// Получает категорию ингредиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        public JsonResult IngridientCategory_Get()
        {
            List<CDish_Ingridient_Category?> result = _bl.Dish_IngridientCategory.GetAllIngridients();

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: result);
        }

    }
}