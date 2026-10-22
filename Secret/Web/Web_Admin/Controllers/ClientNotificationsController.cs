using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Admin.Models.ClientNotifications;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Нотификации клиента
    /// </summary>
    [Authorize]
    public class ClientNotificationsController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;


        #region API Methods (JSON)

        /// <summary>
        /// Получить список уведомлений клиента по ID
        /// GET: /ClientNotifications?ClientID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetClientNotifications(int ClientID)
        {
            try
            {
                if (ClientID <= 0)
                    return Json(new { success = false, message = "Неверный ID клиента" });

                List<CClient_Notification> notifications = _bl.Client_Notifications.GetList(ClientID: ClientID);

                List<object> result = new List<object>();
                foreach (CClient_Notification item in notifications)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.ClientID,
                        item.OrderID,
                        item.Date_Act
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
        /// Получить уведомление по ID
        /// GET: /ClientNotifications/GetClientNotification?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetClientNotification(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CClient_Notification notification = _bl.Client_Notifications.Get(ID: ID);
                if (notification == null)
                    return Json(new { success = false, message = "Уведомление не найдено" });

                object result = new
                {
                    notification.ID,
                    notification.ClientID,
                    notification.OrderID,
                    notification.Date_Act
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить количество уведомлений клиента
        /// GET: /ClientNotifications/GetCount?ClientID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetCount(int ClientID)
        {
            try
            {
                if (ClientID <= 0)
                    return Json(new { success = false, message = "Неверный ID клиента" });

                List<CClient_Notification> notifications = _bl.Client_Notifications.GetList(ClientID: ClientID);
                int count = notifications != null ? notifications.Count : 0;

                return Json(new { success = true, data = new { ClientID, Count = count } });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public ClientNotificationsController(ILogger<ClientNotificationsController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor ClientNotificationsController");
            _bl = BL;
        }

        /// <summary>
        /// Список
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int ClientID)
        {
            Model_ClientNotifications_Index model = new();
            if (ClientID > 0)
            {
                model.Client = _bl.Clients.Get(ClientID);
                model.List_ClientNotifications = [.. _bl.Client_Notifications.GetList(ClientID: ClientID).OrderByDescending(p => p.Date_Act)];
            }
            return View(model: model);
        }

        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ClientNotifications_Part_Table(int ClientID)
        {
            return Index(ClientID);
        }

    }
}