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
    /// Контроллер - Города
    /// </summary>
    /// <param name="_bl"></param>
    [Tags("Cities_v4 - Города")]
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/Cities/[action]")]
    [ApiController]
    public class CitiesController_v4(BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает дату последнего изменения города
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetCitiesChangeDate()
        {
            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Cities_DateChange);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: val);
        }


        /// <summary>
        /// Возвращает список городов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CCity>))]
        public JsonResult GetCitiesList()
        {
            List<CCity>? list_result = _bl.Cities.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }

    }
}