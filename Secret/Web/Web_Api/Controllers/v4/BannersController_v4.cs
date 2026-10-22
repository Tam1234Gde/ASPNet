using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    /// Контроллер - Банеры
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("Banners_v4 - Банеры")] //TODO: MAE
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/Banners/[action]")]
    [ApiController]
    public class BannersController_v4 : Controller   //TODO: logs
    {


        private readonly ILogger<BannersController_v4> _log;
        private readonly BL_Context _bl;
        private readonly IMemoryCache _cache;

        public BannersController_v4(ILogger<BannersController_v4> log, BL_Context bl, IMemoryCache cache)
        {
            _log = log;
            _bl = bl;
            _cache = cache;
        }

        /// <summary>
        /// Возвращает список id активных баннеров (+ дата изменения баннера для каждого по отдельности)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CBanner>))]
        public JsonResult GetActiveBanners_ByCity(int CityID)
        {
            List<CBanner_IDs>? list_result = _bl.Banners.GetListIDs_ByCity(CityID: CityID);

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
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

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: banner);
        }

        /// <summary>
        /// Получает изображение баннера большого
        /// </summary>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "BannerID" })] // кеш на 5 минут, зависит от BannerID
        public JsonResult BannerImageBig_Get(int BannerID)
        {
            if (BannerID < 0)
            {
                _log.LogWarning("Не удалось идентифицировать баннер");
                return Helper_Result.GetJsonResult(Consts.Status_Err, (int)EResult_v4.Err);
            }

            string cacheKey = $"BannerImageBig_{BannerID}";

            if (!_cache.TryGetValue(cacheKey, out byte[]? image))
            {
                image = _bl.Banners.ImageBig_Get(BannerID);

                if (image != null)
                {
                    _cache.Set(cacheKey, image, TimeSpan.FromMinutes(5));
                }
            }

            return Helper_Result.GetJsonResult(Consts.Status_OK, (int)EResult_v4.OK, image);
        }

        /// <summary>
        /// Получает изображение баннера маленького
        /// </summary>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "BannerID" })]
        public JsonResult BannerImageSmall_Get(int BannerID)
        {
            if (BannerID < 0)
            {
                _log.LogWarning("Не удалось идентифицировать баннер");
                return Helper_Result.GetJsonResult(Consts.Status_Err, (int)EResult_v4.Err);
            }

            string cacheKey = $"BannerImageSmall_{BannerID}";

            if (!_cache.TryGetValue(cacheKey, out byte[]? image))
            {
                image = _bl.Banners.ImageSmall_Get(BannerID);
                if (image != null)
                {
                    _cache.Set(cacheKey, image, TimeSpan.FromMinutes(5));
                }
            }

            return Helper_Result.GetJsonResult(Consts.Status_OK, (int)EResult_v4.OK, image);
        }

    }
}
