using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Web_Api.Auth;

namespace HIDE.RestAndRant.Web_Api.Controllers.v4
{
    /// <summary>
    /// Тестовый контроллер
    /// </summary>
    [Tags("Test_v4")]
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/Test/[action]")]
    [ApiController]
    [BasicAuth]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestController_v4 : ControllerBase
    {
        /// <summary>
        /// Возвращает текущее время сервера
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDateTime")]
        public string GetDateTime()
        {
            return DateTime.Now.ToString() + " Secure";
        }


        /// <summary>
        /// Возвращает текущее время сервера
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDateTime_NoAuth")]
        [AllowAnonymous]
        public string GetDateTime_NoAuth()
        {
            return DateTime.Now.ToString();
        }

    }
}