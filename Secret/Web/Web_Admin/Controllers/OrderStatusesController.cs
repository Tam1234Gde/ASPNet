using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.OrderStatuses;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Статусы заказа
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Authorize]
    public class OrderStatusesController(ILogger<OrderStatusesController> _log, BL_Context _bl) : Controller
    {
        private int user_id;

        /// <summary>
        /// Определение ID пользователя
        /// </summary>
        private int UserID
        {
            get
            {
                if (_log.IsEnabled(LogLevel.Information))
                {
                    _log.LogInformation(message: "{}", args: "Выполнение метода Определение ID пользователя");
                }

                if (user_id <= 0 && HttpContext != null)
                {
                    user_id = Helper_User.GetCurrentUserID(HttpContext.User);
                }
                return user_id;
            }
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех статусов заказов
        /// GET: /OrderStatuses/OrderStatuses_Index
        /// </summary>
        [HttpGet]
        [Route("OrderStatuses_Index")]
        public IActionResult GetOrderStatuses()
        {
            try
            {
                List<CCommon_Ordered> statuses = _bl.OrderStatuses.GetList();

                List<object> result = new List<object>();
                foreach (CCommon_Ordered item in statuses)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.OrderNum,
                        item.ColorHex
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
        /// Получить статус заказа по ID
        /// GET: /OrderStatuses/GetOrderStatus?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetOrderStatus(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CCommon_Ordered status = _bl.OrderStatuses.Get(ID: ID);
                if (status == null)
                    return Json(new { success = false, message = "Статус не найден" });

                object result = new
                {
                    status.ID,
                    status.Name,
                    status.OrderNum,
                    status.ColorHex
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion


        /// <summary>
        /// Список
        /// </summary>
        /// <returns></returns>
        public IActionResult OrderStatuses_Index()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Список");
            }

            List<CCommon_Ordered>? list = _bl.OrderStatuses.GetList();
            return View(model: list);
        }

        /// <summary>
        /// Частичное представление > Таблица
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult OrderStatuses_Part_Table()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Частичное представление > Таблица");
            }

            return OrderStatuses_Index();
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult OrderStatuses_Edit(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение1");
            }

            CCommon_Ordered? order_status = _bl.OrderStatuses.Get(ID: ID);
            if (order_status != null)
            {
                Model_OrderStatuses_CRUD data = new()
                {
                    ID = order_status.ID,
                    Name = order_status.Name,
                    OrderNum = order_status.OrderNum,
                    ColorHex = order_status.ColorHex,
                };

                return View(model: data);
            }

            return RedirectToAction(controllerName: "Error", actionName: "Index");
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult OrderStatuses_Edit(Model_OrderStatuses_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение2");
            }

            if (Data != null)
            {
                int id = _bl.OrderStatuses.Edit(ID: Data.ID, Name: Data.Name, ColorHex: Data.ColorHex, OrderNum: Data.OrderNum);

                if (id > 0)
                {
                    return RedirectToAction("OrderStatuses_Index");
                }
                else
                {
                    return View(model: Data);
                }
            }

            return RedirectToAction(controllerName: "Error", actionName: "Index");
        }


        /// <summary>
        /// Выполняется для всех методов в контроллере
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Выполняется для всех методов в контроллере");
            }

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
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Получение Флага > Админ");
            }

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