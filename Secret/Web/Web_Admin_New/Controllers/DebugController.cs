using Microsoft.AspNetCore.Mvc;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
#if DEBUG

    /// <summary>
    /// Debug
    /// </summary>
    public class DebugController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

#endif
}