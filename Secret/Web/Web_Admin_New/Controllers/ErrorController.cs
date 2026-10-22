using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    /// <summary>
    /// Ошибки
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Страница ошибки
        /// </summary>
        /// <param name="text">Текст ошибки</param>
        /// <returns></returns>
        [Authorize]
        public IActionResult Index(string? text = null, string? statuscode = null)
        {
            ViewBag.StatusCode = statuscode;
            ViewBag.Text = text;
            return View();
        }
    }
}