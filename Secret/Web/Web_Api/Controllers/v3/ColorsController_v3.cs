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
    /// Контроллер - Цвета посуды блюд
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("Colors_v3 - Цвета посуды блюд")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/Colors/[action]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ColorsController_v3(ILogger<ColorsController_v3> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает дату последнего изменения цветов посуды блюд
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetColorsChangeDate()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Возвращает дату последнего изменения цветов посуды блюд");
            }

            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Colors_DateChange);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Возвращает список цветов посуды блюд
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CColor>))]
        public JsonResult GetColorsList()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Возвращает список цветов посуды блюд");
            }

            List<CColor>? list_result = _bl.Colors.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }

    }
}