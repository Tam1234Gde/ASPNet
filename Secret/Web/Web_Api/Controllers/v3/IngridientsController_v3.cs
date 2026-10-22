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
    /// Контроллер - Ингредиенты
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("Ingridients_v3 - Ингредиенты")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/Ingridients/[action]")]
    [ApiController]
    public class IngridientsController_v3(ILogger<IngridientsController_v3> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает дату последнего изменения ингредиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        [Obsolete]
        public JsonResult GetIngridientsChangeDate()
        {
            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Ingridients_DateChange);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Получает список ингредиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CIngridient>))]
        [Obsolete]
        public JsonResult GetIngridientsList()
        {
            List<CIngridient>? list_result = _bl.Ingridients.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


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
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            bool check = _bl.Ingridients.Check_Ingridient(ID: IngridientID);

            if (!check)
            {
                byte[]? image = _bl.Ingridients.Image_Get(ID: IngridientID);
                return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: image);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: null);
        }

    }
}