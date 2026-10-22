using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Web_Admin.Models.Common;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Общий
    /// </summary>
    [Authorize]
    public class CommonController : Controller
    {
        private readonly ILogger _log;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public CommonController(ILogger<CommonController> Logger)
        {
            _log = Logger;
            _log.LogDebug("Constructor CommonController");
        }

        /// <summary>
        /// Выполнить скрипт
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Sql_Script()
        {
            return View();
        }

        /// <summary>
        /// Выполнить скрипт
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Sql_Script(Model_Script Data)
        {
            if (ModelState.IsValid && Data != null)
            {
                int restaurant_id = 1;

                if (restaurant_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = restaurant_id });
                }

                return View();
            }
            else
            {
                return View();
            }
        }

    }
}