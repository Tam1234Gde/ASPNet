using Microsoft.AspNetCore.Mvc;

namespace HIDE.RestAndRant.Web_Site_Menu.Controllers
{
    /// <summary>
    /// Страницы с ошибками 404 и 500
    /// </summary>
    [Route("error")]
    public class ErrorController : Controller
    {
        /// <summary>
        /// Страница 404
        /// </summary>
        /// <returns></returns>
        [Route("404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        /// <summary>
        /// Страница 500
        /// </summary>
        /// <returns></returns>
        [Route("500")]
        public IActionResult AppError()
        {
            return View();
        }

    }
}