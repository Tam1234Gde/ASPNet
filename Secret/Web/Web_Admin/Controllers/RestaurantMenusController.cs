using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.RestaurantMenus;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Меню ресторана
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Authorize]
    public class RestaurantMenusController(ILogger<OrdersController> _log, BL_Context _bl) : Controller
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
        /// Получить список меню ресторана по ID ресторана
        /// GET: /RestaurantMenus/RestaurantMenu_Index?RestaurantID={id}
        /// </summary>
        [HttpGet]
        [Route("RestaurantMenu_Index")]
        public IActionResult GetRestaurantMenus(int RestaurantID)
        {
            try
            {
                if (RestaurantID <= 0)
                    return Json(new { success = false, message = "Неверный ID ресторана" });

                List<CRestaurant_Menu> menus = _bl.Restaurant_Menus.GetList(RestaurantID: RestaurantID);

                List<object> result = new List<object>();
                foreach (CRestaurant_Menu item in menus)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.RestaurantID,
                        item.Flag_Active,
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
        /// Получить меню ресторана по ID
        /// GET: /RestaurantMenus/GetRestaurantMenu?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetRestaurantMenu(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CRestaurant_Menu menu = _bl.Restaurant_Menus.Get(ID: ID);
                if (menu == null)
                    return Json(new { success = false, message = "Меню не найдено" });

                object result = new
                {
                    menu.ID,
                    menu.Name,
                    menu.RestaurantID,
                    menu.Flag_Active,
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список активных меню ресторана
        /// GET: /RestaurantMenus/GetActiveRestaurantMenus?RestaurantID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetActiveRestaurantMenus(int RestaurantID)
        {
            try
            {
                if (RestaurantID <= 0)
                    return Json(new { success = false, message = "Неверный ID ресторана" });

                List<CRestaurant_Menu> allMenus = _bl.Restaurant_Menus.GetList(RestaurantID: RestaurantID);

                List<CRestaurant_Menu> activeMenus = new List<CRestaurant_Menu>();
                foreach (CRestaurant_Menu item in allMenus)
                {
                    if (item.Flag_Active)
                    {
                        activeMenus.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CRestaurant_Menu item in activeMenus)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.RestaurantID,
                        item.Flag_Active,
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
        /// Список
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Index(int RestaurantID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Список");
            }

            Model_RestaurantMenus_Index model = new()
            {
                List_Restaurant_Menus = _bl.Restaurant_Menus.GetList(RestaurantID),
                RestaurantID = RestaurantID
            };
            return View(model: model);
        }


        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Part_Table(int RestaurantID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Таблица > Список");
            }

            return RestaurantMenu_Index(RestaurantID);
        }


        #region Добавление


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Add(int RestaurantID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление1");
            }

            Model_RestaurantMenus_CRUD model = new()
            {
                RestaurantID = RestaurantID
            };
            return View(model: model);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RestaurantMenu_Add(Model_RestaurantMenus_CRUD data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление2");
            }

            if (ModelState.IsValid && data != null)
            {
                int result = _bl.Restaurant_Menus.Add(Name: data.Name, RestaurantID: data.RestaurantID, Flag_Active: data.Flag_Active);

                if (result < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = result });
                }
                return RedirectToAction(actionName: "RestaurantMenu_Index", routeValues: new { data.RestaurantID });
            }
            else
            {
                return View();
            }
        }


        #endregion


        #region Изменение

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Edit(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение1");
            }

            CRestaurant_Menu? elem = _bl.Restaurant_Menus.Get(ID: ID);
            if (elem != null)
            {
                Model_RestaurantMenus_CRUD data = new()
                {
                    ID = elem.ID,
                    Name = elem.Name,
                    Flag_Active = elem.Flag_Active,
                    RestaurantID = elem.RestaurantID
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RestaurantMenu_Edit(Model_RestaurantMenus_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение2");
            }

            if (Data != null)
            {
                int result = _bl.Restaurant_Menus.Edit(ID: Data.ID, Name: Data.Name, RestaurantID: Data.RestaurantID, Flag_Active: Data.Flag_Active);

                if (result > 0)
                {
                    return RedirectToAction(actionName: "RestaurantMenu_Index", routeValues: new { Data.RestaurantID });
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
        /// Удаление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Del(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление1");
            }

            if (ID > 0)
            {
                CRestaurant_Menu? elem = _bl.Restaurant_Menus.Get(ID: ID);

                if (elem != null)
                {
                    Model_RestaurantMenus_CRUD data = new()
                    {
                        Name = elem.Name,
                        RestaurantID = elem.RestaurantID
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
        /// Удаление
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RestaurantMenu_Del(Model_RestaurantMenus_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление2");
            }

            if (Data != null)
            {
                int result = _bl.Restaurant_Menus.Del(ID: Data.ID, UserID: UserID);

                if (result > 0)
                {
                    return RedirectToAction(actionName: "RestaurantMenu_Index", routeValues: new { Data.RestaurantID });
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


        #region Мигратор

        /// <summary>
        /// Мигратор
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Migrator(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Мигратор");
            }

            if (ID > 0)
            {
                CRestaurant_Menu? elem = _bl.Restaurant_Menus.Get(ID: ID);

                if (elem != null)
                {
                    Model_RestaurantMenus_CRUD data = new()
                    {
                        Name = elem.Name,
                        RestaurantID = elem.RestaurantID
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
        /// Миграция блюд
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RestaurantMenu_Migrator(Model_RestaurantMenus_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Миграция блюд");
            }

            if (Data != null)
            {
                int result = _bl.Restaurant_Menu_Dishes.Migrator(RestaurantMenuID: Data.ID, UserID: UserID);

                if (result > 0)
                {
                    return RedirectToAction(actionName: "RestaurantMenu_Index", routeValues: new { Data.RestaurantID });
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

        #endregion Мигратор


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

    }
}