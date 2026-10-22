using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Oper.Helpers;
using HIDE.RestAndRant.Web_Oper.Models.Clients;

namespace HIDE.RestAndRant.Web_Oper.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ILogger _log;

        readonly BL_Context _bl;


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public ClientsController(ILogger<ClientsController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor ClientsController");
            _bl = BL;
        }

        /// <summary>
        /// Список клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            List<CClient> list_clients = _bl.Clients.GetList();
            return View(model: list_clients);
        }

        /// <summary>
        /// Таблица > Список клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Clients_Part_Table()
        {
            List<CClient> list_clients = _bl.Clients.GetList();
            return View(model: list_clients);
        }

        #region Добавление

        /// <summary>
        /// Добавление клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Добавление клиента
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Clients_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                //int user_id = _bl.Clients.Add(Phone: data.Phone, Name: data.Name, CardData: data.CardData ?? "", Bonuses: data.Bonuses);

                int user_id = 1;

                if (user_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = user_id });
                }

                return RedirectToAction("Index"); //Возвращаем к списку клиентов
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CClient? client = _bl.Clients.Get(ID: ID);
            if (client != null)
            {
                Model_Clients_AddEdit data = new()
                {
                    ID = client.ID,
                    Phone = client.Phone,
                    Name = client.Name,
                    CardData = client.CardData
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }
        }

        /// <summary>
        /// Изменение клиента
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Clients_AddEdit Data)
        {
            if (Data != null)
            {
                //int client_id = _bl.Clients.Edit(ID: Data.ID, Phone: Data.Phone, Name: Data.Name, CardData: Data.CardData ?? "", Bonuses: Data.Bonuses);
                int client_id = 1;

                if (client_id > 0)
                {
                    return RedirectToAction("Index");
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

        #endregion

        #region Удаление

        /// <summary>
        /// Удаление клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CClient? client = _bl.Clients.Get(ID: ID);

                if (client != null)
                {
                    Model_Clients_AddEdit data = new()
                    {
                        Name = client.Name
                    };

                    return View(model: data);
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
        /// Удаление клиента
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Clients_AddEdit Data)
        {
            if (Data != null)
            {
                //int client_id = _bl.Clients.Del(ID: Data.ID);
                int client_id = 1;

                if (client_id > 0)
                {
                    return RedirectToAction("Index");
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

        #region Получение имени пользователя

        /// <summary>
        /// Получение имени клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string? ClientName()
        {
            CClient? client = _bl.Clients.Get(ID: Helper_Client.GetCurrentClientID(HttpContext.User));
            if (client != null)
            {
                return client.Name;
            }
            return "";
        }

        #endregion Получение имени клиента

    }
}
