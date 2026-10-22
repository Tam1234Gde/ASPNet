using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Admin.Helpers;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Бонусы клиента 
    /// </summary>
    [Authorize]
    public class BonusesController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public BonusesController(ILogger<BonusesController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor BonusesController");
            _bl = BL;
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех бонусов
        /// GET: /Bonuses/Bonuses_Index
        /// </summary>
        [HttpGet]
        public IActionResult GetBonuses()
        {
            try
            {
                List<CBonus> bonuses = _bl.Bonuses.GetList();

                List<object> result = new List<object>();
                foreach (CBonus item in bonuses)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.ClientID,
                        ClientName = item.Client ?? string.Empty,
                        item.OrderID,
                        item.Bonuses,
                        item.Date_Action
                    });
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить бонус по ID
        /// GET: /Bonuses/GetBonus?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetBonus(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CBonus bonus = _bl.Bonuses.Get(ID: ID);
                if (bonus == null)
                    return Json(new { success = false, message = "Бонус не найден" });

                object result = new
                {
                    bonus.ID,
                    bonus.ClientID,
                    ClientName = bonus.Client ?? string.Empty,
                    bonus.OrderID,
                    bonus.Bonuses,
                    bonus.Date_Action
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список бонусов по клиенту
        /// GET: /Bonuses/GetBonusesByClient?ClientID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetBonusesByClient(int ClientID)
        {
            try
            {
                if (ClientID <= 0)
                    return Json(new { success = false, message = "Неверный ID клиента" });

                List<CBonus> allBonuses = _bl.Bonuses.GetList();

                List<CBonus> filteredBonuses = new List<CBonus>();
                foreach (CBonus item in allBonuses)
                {
                    if (item.ClientID == ClientID)
                    {
                        filteredBonuses.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CBonus item in filteredBonuses)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.ClientID,
                        ClientName = item.Client ?? string.Empty,
                        item.OrderID,
                        item.Bonuses,
                        item.Date_Action
                    });
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить сумму бонусов по клиенту
        /// GET: /Bonuses/GetBonusesSumByClient?ClientID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetBonusesSumByClient(int ClientID)
        {
            try
            {
                if (ClientID <= 0)
                    return Json(new { success = false, message = "Неверный ID клиента" });

                List<CBonus> allBonuses = _bl.Bonuses.GetList();

                decimal totalBonuses = 0;
                foreach (CBonus item in allBonuses)
                {
                    if (item.ClientID == ClientID)
                    {
                        totalBonuses += item.Bonuses;
                    }
                }

                return Json(new { success = true, data = new { ClientID, TotalBonuses = totalBonuses } });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion


        /// <summary>
        /// Список изменений начисления бонусов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Bonuses_Index()
        {
            List<CBonus>? list = _bl.Bonuses.GetList();
            return View(model: list);
        }

        /// <summary>
        /// Таблица > Список изменений
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Bonuses_Part_Table()
        {
            return Bonuses_Index();
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