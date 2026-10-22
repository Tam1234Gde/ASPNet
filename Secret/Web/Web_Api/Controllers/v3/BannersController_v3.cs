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
    /// Контроллер - Банеры
    /// </summary>
    /// <param name="_bl"></param>
    /// <param name="_log"></param>
    [Tags("Banners_v3 - Банеры")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/Banners/[action]")]
    [ApiController]
    public class BannersController_v3(ILogger<BannersController_v3> _log, BL_Context _bl) : Controller   //TODO: logs
    {

        /// <summary>
        /// Возвращает список id активных баннеров (+ дата изменения баннера для каждого по отдельности)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CBanner_IDs>))]
        public JsonResult GetActiveBanners_ByCity(int CityID)
        {
            List<CBanner_IDs>? list_result = _bl.Banners.GetListIDs_ByCity(CityID: CityID);

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Возвращает список баннеров
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CBanner>))]
        public JsonResult GetBannersList() //TODO: MAE - пометил как неиспользуемое, 24.02.26, позже будет удалено.
        {
            if (_log.IsEnabled(logLevel: LogLevel.Trace))
            {
                _log.LogTrace(message: "Call GetBannersList");
            }

            List<CBanner>? list_result = _bl.Banners.GetList();

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Возвращает список баннеров по городу
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CBanner>))]
        public JsonResult GetBannersList_ByCity(int CityID) //TODO: MAE - пометил как неиспользуемое, 24.02.26, позже будет удалено.
        {
            List<CBanner>? list_result = _bl.Banners.GetList_ByCity(CityID: CityID);

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Возвращает баннер по ID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetBanner_ByID(int ID)
        {
            CBanner_Light? banner = _bl.Banners.Get_Light(ID);

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: banner);
        }


        /// <summary>
        /// Получает изображение баннера большого
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        public JsonResult BannerImageBig_Get(int BannerID)
        {
            if (BannerID < 0)
            {
                _log.LogWarning(message: "Не удалось идентифицировать баннер");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            byte[]? image = _bl.Banners.ImageBig_Get(ID: BannerID);

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: image);
        }


        /// <summary>
        /// Получает изображение баннера маленького
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        public JsonResult BannerImageSmall_Get(int BannerID)
        {
            if (BannerID < 0)
            {
                _log.LogWarning(message: "Не удалось идентифицировать баннер");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            byte[]? image = _bl.Banners.ImageSmall_Get(ID: BannerID);

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: image);
        }


        /// <summary>
        /// Возвращает дату последнего изменения ,баннеров
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetBanners_ChangeDate() //TODO: MAE - пометил как неиспользуемое, 24.02.26, позже будет удалено.
        {
            DateTime? date_max = _bl.Banners.GetMaxLastEditDate();

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: date_max);
        }
    }
}
