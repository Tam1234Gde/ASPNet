using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Oper.Helpers;

namespace HIDE.RestAndRant.Web_Oper.Controllers
{
    public class CommonController : Controller
    {
        private readonly ILogger _log;

        readonly BL_Context _bl;


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public CommonController(ILogger<CommonController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor CommonController");
            _bl = BL;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Получение имени пользователя

        /// <summary>
        /// Получение имени пользователя
        /// </summary>
        /// <returns></returns>
        public string? GetUserName()
        {
            CUser? user = _bl.Users.Get(ID: Helper_Client.GetCurrentClientID(HttpContext.User));
            if (user != null)
            {
                return user.Name;
            }
            return "";
        }

        #endregion Получение имени пользователя

    }
}