using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Admin_ForMenu.Helpers;

namespace HIDE.RestAndRant.Web_Admin_New.Controllers
{
    /// <summary>
    /// Данные о картах
    /// </summary>
    [Authorize]
    public class ClientPay_CardsController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;
        private int user_id;


        #region API Methods (JSON)

        /// <summary>
        /// Получить список карт клиента по ID
        /// GET: /ClientPay_Cards?ClientID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetClientPayCards(int ClientID)
        {
            try
            {
                if (ClientID <= 0)
                    return Json(new { success = false, message = "Неверный ID клиента" });

                List<CClientPay_Card_Data> cards = _bl.ClientPay_Cards.GetList_CardData(ClientID: ClientID);

                List<object> result = new List<object>();
                foreach (CClientPay_Card_Data item in cards)
                {
                    result.Add(new
                    {
                        item.ID
                    });
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion


        /// <summary>
        /// Определение ID пользователя
        /// </summary>
        private int UserID
        {
            get
            {
                if (user_id <= 0 && HttpContext != null)
                {
                    user_id = Helper_User.GetCurrentUserID(HttpContext.User);
                }
                return user_id;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Logger"></param>
        /// <param name="BL"></param>
        public ClientPay_CardsController(ILogger<ClientPay_CardsController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor ClientPay_CardsController");
            _bl = BL;
        }




        /// <summary>
        /// Список
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int ClientID)
        {
            List<CClientPay_Card_Data>? list_result = _bl.ClientPay_Cards.GetList_CardData(ClientID: ClientID);
            return View(model: list_result);
        }


        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ClientPay_Cards_Part_Table(int ClientID)
        {
            return Index(ClientID);
        }


        /// <summary>
        /// Редирект на страницу с описанием ошибки
        /// </summary>
        /// <param name="ErrCode"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        private RedirectToActionResult RedirectToError(int ErrCode, string Url)
        {
            return RedirectToAction(actionName: Contst_Admin.ErrorActionName,
                                 controllerName: Contst_Admin.ErrorControllerName,
                                 routeValues: new { ErrCode, Url });
        }


        /// <summary>
        /// Выполняется для всех методов в контроллере
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Flag_ShowMenu = Get_FlagAdmin();
            base.OnActionExecuting(filterContext);
        }


        #region Получение Флага > Админ

        /// <summary>
        /// Получение Флага > Админ
        /// </summary>
        /// <returns></returns>
        public bool Get_FlagAdmin()
        {
            CUser? user = _bl.Users.Get(ID: Helper_User.GetCurrentUserID(HttpContext.User));
            if (user != null && user.Flag_Root)
            {
                ViewBag.Flag_ShowMenu = true;
                return true;
            }

            ViewBag.Flag_ShowMenu = false;
            return false;
        }

        #endregion Получение Флага > Админ
    }
}