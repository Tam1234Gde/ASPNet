using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;
using HIDE.RestAndRant.Web_Admin_ForMenu.Helpers;
using HIDE.RestAndRant.Web_Admin_ForMenu.Models;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    /// <summary>
    /// История отправки кастомных уведомлений
    /// </summary>
    [Authorize]
    public class PushHistoryController(ILogger<OrdersController> _log, BL_Context _bl) : Controller
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
        /// Получить список всех запланированных уведомлений
        /// GET: /PushHistory/PushHistory_Index
        /// </summary>
        [HttpGet]
        public IActionResult GetPushHistory()
        {
            try
            {
                List<CClientNotification_History_ForAdmin> history = _bl.Cllient_Notification_Planned.GetList();

                List<object> result = new List<object>();
                foreach (CClientNotification_History_ForAdmin item in history)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Content,
                        item.Date_Created,
                        item.Date_Send,
                        item.Flag_Selected
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
        /// Получить запланированное уведомление по ID
        /// GET: /PushHistory/GetPushHistoryItem?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetPushHistoryItem(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CClient_Notification_Planned notification = _bl.Cllient_Notification_Planned.Get(ID: ID);
                if (notification == null)
                    return Json(new { success = false, message = "Уведомление не найдено" });

                object result = new
                {
                    notification.ID,
                    notification.Name,
                    notification.Content,
                    notification.Date_Created,
                    notification.Date_Send,
                    notification.Flag_Selected
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список запланированных уведомлений за период
        /// GET: /PushHistory/GetPushHistoryByDate?Date_From={date}&Date_To={date}
        /// </summary>
        [HttpGet]
        public IActionResult GetPushHistoryByDate(string? Date_From, string? Date_To)
        {
            try
            {
                DateTime? dateFrom = HIDE_Convert.ToDateTime_Nullable(Date_From);
                if (!dateFrom.HasValue)
                {
                    dateFrom = DateTime.Now.Date.AddDays(-30);
                }

                DateTime? dateTo = HIDE_Convert.ToDateTime_Nullable(Date_To);
                if (!dateTo.HasValue)
                {
                    dateTo = DateTime.Now.Date;
                }

                List<CClientNotification_History_ForAdmin> allHistory = _bl.Cllient_Notification_Planned.GetList();

                List<CClientNotification_History_ForAdmin> filteredHistory = new List<CClientNotification_History_ForAdmin>();
                foreach (CClientNotification_History_ForAdmin item in allHistory)
                {
                    if (item.Date_Created >= dateFrom && item.Date_Created <= dateTo)
                    {
                        filteredHistory.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CClientNotification_History_ForAdmin item in filteredHistory)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Content,
                        item.Date_Created,
                        item.Date_Send,
                        item.Flag_Selected
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
        /// Получить список неотправленных запланированных уведомлений
        /// GET: /PushHistory/GetUnsentPushHistory
        /// </summary>
        [HttpGet]
        public IActionResult GetUnsentPushHistory()
        {
            try
            {
                List<CClientNotification_History_ForAdmin> allHistory = _bl.Cllient_Notification_Planned.GetList();

                List<CClientNotification_History_ForAdmin> unsentHistory = new List<CClientNotification_History_ForAdmin>();
                foreach (CClientNotification_History_ForAdmin item in allHistory)
                {
                    if (!item.Flag_Selected)
                    {
                        unsentHistory.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CClientNotification_History_ForAdmin item in unsentHistory)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Content,
                        item.Date_Created,
                        item.Date_Send,
                        item.Flag_Selected
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
        /// Получить список отправленных запланированных уведомлений
        /// GET: /PushHistory/GetSentPushHistory
        /// </summary>
        [HttpGet]
        public IActionResult GetSentPushHistory()
        {
            try
            {
                List<CClientNotification_History_ForAdmin> allHistory = _bl.Cllient_Notification_Planned.GetList();

                List<CClientNotification_History_ForAdmin> sentHistory = new List<CClientNotification_History_ForAdmin>();
                foreach (CClientNotification_History_ForAdmin item in allHistory)
                {
                    if (item.Flag_Selected)
                    {
                        sentHistory.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CClientNotification_History_ForAdmin item in sentHistory)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Content,
                        item.Date_Created,
                        item.Date_Send,
                        item.Flag_Selected
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


        //TODO: DVV - Проверить правильность отображения, сделать формирование Excel при скачивании файла из списка клиентов, метод для этого есть в _dl_Client_Notification_Planned_Data => GetList_Planned(), если нужны методы в бизнесе пиши мне => MAE
        #region Debug Comment - MAE - восставновить после внесения изменений
        /// <summary>
        /// Главная
        /// </summary>
        /// <returns></returns>
        public IActionResult PushHistory_Index()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Главная");
            }

            List<CClientNotification_History_ForAdmin>? list = [.. _bl.Cllient_Notification_Planned.GetList().OrderByDescending(x => x.Date_Created)];
            return View(model: list);
        }


        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PushHistory_Part_Table()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Таблица > Список");
            }

            return PushHistory_Index();
        }


        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetFile(int ID, string Name)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Скачать файл");
            }

            Client_Notification_Planned_Data_oper _dl_Notification_Planned_Data = new();
            Clients _bl_Client = new();

            List<int>? list_IDs = _dl_Notification_Planned_Data.GetList_Planned(Client_Notification_Planned_ID: ID)?.Select(p => p.ClientID).ToList();

            if(list_IDs == null || list_IDs.Count == 0)
            {
                return NotFound();
            }

            List<CClient> list_Clients = _bl_Client.GetList_ByIDs(List_IDs: list_IDs);

            if (list_Clients == null || list_Clients.Count == 0)
            {
                return NotFound();
            }

            return File(Helper_Excel.CreateExcelFromClients(List_Clients: list_Clients), "application/force-download", $"{ID}. {Name} ({DateTime.Now}).xlsx");
        }
        #endregion Debug Comment - MAE - восставновить после внесения изменений


        /// <summary>
        /// Редирект на страницу с описанием ошибки
        /// </summary>
        /// <param name="ErrCode"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        private RedirectToActionResult RedirectToError(int ErrCode, string Url)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Редирект на страницу с описанием ошибки");
            }

            return RedirectToAction(actionName: Contst_Admin.ErrorActionName,
                                 controllerName: Contst_Admin.ErrorControllerName,
                                 routeValues: new { ErrCode, Url });

        }

        #region Получение имени пользователя

        /// <summary>
        /// Получение имени пользователя
        /// </summary>
        /// <returns></returns>
        public string? Get_UserName()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Получение имени пользователя");
            }

            CUser? user = _bl.Users.Get(ID: Helper_User.GetCurrentUserID(HttpContext.User));
            if (user != null)
            {
                return user.Name;
            }
            return "";
        }

        #endregion Получение имени пользователя

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


        #region Удаление

        /// <summary>
        /// Удаление запланированного пуш-уведомления
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление запланированного пуш-уведомления1");
            }

            if (ID > 0)
            {
                CClient_Notification_Planned? notification = _bl.Cllient_Notification_Planned.Get(ID: ID);

                if (notification != null)
                {
                    return View(model: notification);
                }
                else
                {
                    return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
                }
            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }

        /// <summary>
        /// Удаление запланированного пуш-уведомления
        /// </summary>
        /// <param name="Data">Данные пуш-уведомления</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(CClient_Notification_Planned Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление запланированного пуш-уведомления2");
            }

            if (Data != null)
            {
                int notification_id = _bl.Cllient_Notification_Planned.Delete(ID: Data.ID, Flag_Del: true);

                if (notification_id > 0)
                {
                    return RedirectToAction("PushHistory_Index");
                }
                else
                {
                    return View(model: Data);
                }
            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }

        #endregion Удаление
    }
}