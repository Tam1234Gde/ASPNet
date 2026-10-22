using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.RestaurantMenu_Dishes;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Блюда меню
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Authorize]
    public class RestaurantMenu_DishesController(ILogger<OrdersController> _log, BL_Context _bl) : Controller
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
        /// Получить список категорий меню ресторана по ID меню
        /// GET: /RestaurantMenu_Dishes/RestaurantMenu_Dish_Index?RestaurantMenuID={id}
        /// </summary>
        [HttpGet]
        [Route("RestaurantMenu_Dish_Index")]
        public IActionResult GetRestaurantMenuCategories(int RestaurantMenuID)
        {
            try
            {
                if (RestaurantMenuID <= 0)
                    return Json(new { success = false, message = "Неверный ID меню" });

                List<CRestaurant_Menu_Dish> menuDishes = _bl.Restaurant_Menu_Dishes.GetListForAdmin(RestaurantMenuID, null);

                // Получаем уникальные категории из блюд
                Dictionary<int, string> categories = new Dictionary<int, string>();
                foreach (CRestaurant_Menu_Dish item in menuDishes)
                {
                    if (item.Dish != null)
                    {
                        // Если Dish - это объект с Dish_Category
                        // Или если есть отдельное свойство для категории
                    }
                }

                // Возвращаем список блюд с информацией о категориях
                List<object> result = new List<object>();
                foreach (CRestaurant_Menu_Dish item in menuDishes)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Restaurant_MenuID,
                        item.DishID,
                        DishName = item.Dish != null ? item.Dish.ToString() : string.Empty,
                        item.Flag_Available,
                        item.Price,
                        item.OrderNum,
                        item.FlagNew
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
        /// Получить список блюд в категории по ID меню и ID категории
        /// GET: /RestaurantMenu_Dishes/RestaurantMenu_Dish_Index?RestaurantMenuID={id}&Dish_CategoryID={id}
        /// </summary>
        [HttpGet]
        [Route("RestaurantMenu_Dish_Index")]
        public IActionResult GetRestaurantMenuDishesByCategory(int RestaurantMenuID, int Dish_CategoryID)
        {
            try
            {
                if (RestaurantMenuID <= 0)
                    return Json(new { success = false, message = "Неверный ID меню" });

                if (Dish_CategoryID <= 0)
                    return Json(new { success = false, message = "Неверный ID категории" });

                List<CRestaurant_Menu_Dish> menuDishes = _bl.Restaurant_Menu_Dishes.GetListForAdmin(RestaurantMenuID, Dish_CategoryID);

                List<object> result = new List<object>();
                foreach (CRestaurant_Menu_Dish item in menuDishes)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Restaurant_MenuID,
                        item.DishID,
                        DishName = item.Dish != null ? item.Dish.ToString() : string.Empty,
                        item.Flag_Available,
                        item.Price,
                        item.OrderNum,
                        item.FlagNew
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
        /// Получить блюдо меню по ID
        /// GET: /RestaurantMenu_Dishes/GetRestaurantMenuDish?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetRestaurantMenuDish(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CRestaurant_Menu_Dish menuDish = _bl.Restaurant_Menu_Dishes.Get(ID: ID);
                if (menuDish == null)
                    return Json(new { success = false, message = "Блюдо меню не найдено" });

                object result = new
                {
                    menuDish.ID,
                    menuDish.Restaurant_MenuID,
                    menuDish.DishID,
                    DishName = menuDish.Dish != null ? menuDish.Dish.ToString() : string.Empty,
                    menuDish.Flag_Available,
                    menuDish.Price,
                    menuDish.OrderNum,
                    menuDish.FlagNew
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список доступных блюд меню по ID меню
        /// GET: /RestaurantMenu_Dishes/GetAvailableRestaurantMenuDishes?RestaurantMenuID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetAvailableRestaurantMenuDishes(int RestaurantMenuID)
        {
            try
            {
                if (RestaurantMenuID <= 0)
                    return Json(new { success = false, message = "Неверный ID меню" });

                List<CRestaurant_Menu_Dish> allMenuDishes = _bl.Restaurant_Menu_Dishes.GetListForAdmin(RestaurantMenuID, null);

                List<CRestaurant_Menu_Dish> availableDishes = new List<CRestaurant_Menu_Dish>();
                foreach (CRestaurant_Menu_Dish item in allMenuDishes)
                {
                    if (item.Flag_Available)
                    {
                        availableDishes.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CRestaurant_Menu_Dish item in availableDishes)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Restaurant_MenuID,
                        item.DishID,
                        DishName = item.Dish != null ? item.Dish.ToString() : string.Empty,
                        item.Price,
                        item.OrderNum,
                        item.FlagNew
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
        /// <param name="RestaurantMenuID">Код меню</param>
        /// <param name="Dish_CategoryID">Код категории</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Index(int RestaurantMenuID, int? Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Список");
            }

            Model_RestaurantMenu_Dishes_Index model = new()
            {
                RestaurantMenuID = RestaurantMenuID,
                List_Categories = _bl.Dish_Categories.GetList_Short_SubCategories(),
                List_Categories_ForSelect = Helper_SelectListItems.GetList(_bl.Dish_Categories.GetList_Short())
            };

            if (Dish_CategoryID.HasValue && Dish_CategoryID.Value > 0)
            {
                model.List_CRestaurant_Menu_Dishes = _bl.Restaurant_Menu_Dishes.GetListForAdmin(RestaurantMenuID, Dish_CategoryID);
                model.Dish_CategoryID = Dish_CategoryID.Value;
            }

            return View(model: model);
        }


        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <param name="RestaurantMenuID">Код меню</param>
        /// <param name="Dish_CategoryID">Код категории</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Part_Table(int RestaurantMenuID, int? Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Таблица > Список");
            }

            return RestaurantMenu_Dish_Index(RestaurantMenuID, Dish_CategoryID);
        }


        #region Добавление


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Add(int RestaurantMenuID, int Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление1");
            }

            Model_RestaurantMenu_Dishes_CRUD model = new()
            {
                Restaurant_MenuID = RestaurantMenuID,
                Dish_CategoryID = Dish_CategoryID,
                List_Dishes = _bl.Dishes.GetList_ForAdmin_v2(RestaurantMenuID)
            };
            return View(model: model);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RestaurantMenu_Dish_Add(Model_RestaurantMenu_Dishes_CRUD data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление2");
            }

            if (ModelState.IsValid && data != null)
            {
                int result = _bl.Restaurant_Menu_Dishes.Add(Restaurant_MenuID: data.Restaurant_MenuID, DishID: data.DishID, Flag_Available: data.Flag_Available,
                                                            Price: data.Price, OrderNum: data.OrderNum, FlagNew: data.FlagNew);

                if (result < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = result });
                }

                return RedirectToAction(actionName: "RestaurantMenu_Dish_Index", routeValues: new { RestaurantMenuID = data.Restaurant_MenuID, data.Dish_CategoryID });
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
        public IActionResult RestaurantMenu_Dish_Edit(int ID, int Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение1");
            }

            CRestaurant_Menu_Dish? elem = _bl.Restaurant_Menu_Dishes.Get(ID: ID);
            if (elem != null)
            {
                Model_RestaurantMenu_Dishes_CRUD data = new()
                {
                    ID = elem.ID,
                    DishID = elem.DishID,
                    Dish = elem.Dish,
                    Restaurant_MenuID = elem.Restaurant_MenuID,
                    Flag_Available = elem.Flag_Available,
                    Price = elem.Price,
                    OrderNum = elem.OrderNum,
                    FlagNew = elem.FlagNew,
                    Dish_CategoryID = Dish_CategoryID
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
        public IActionResult RestaurantMenu_Dish_Edit(Model_RestaurantMenu_Dishes_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение2");
            }

            if (Data != null)
            {
                int result = _bl.Restaurant_Menu_Dishes.Edit(ID: Data.ID, Restaurant_MenuID: Data.Restaurant_MenuID, DishID: Data.DishID,
                                                             Flag_Available: Data.Flag_Available, Price: Data.Price, OrderNum: Data.OrderNum,
                                                             FlagNew: Data.FlagNew);

                if (result > 0)
                {
                    return RedirectToAction(actionName: "RestaurantMenu_Dish_Index", routeValues: new { RestaurantMenuID = Data.Restaurant_MenuID, Data.Dish_CategoryID });
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


        #region Скрыть\Восстановить

        /// <summary>
        /// Скрыть\Восстановить
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Del(int ID, bool Flag_Available, int Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Скрыть\\Восстановить");
            }

            if (ID > 0)
            {
                CRestaurant_Menu_Dish? elem = _bl.Restaurant_Menu_Dishes.Get(ID: ID);

                if (elem != null)
                {
                    Model_RestaurantMenu_Dishes_CRUD data = new()
                    {
                        Restaurant_Menu = elem.Restaurant_Menu,
                        Restaurant_MenuID = elem.Restaurant_MenuID,
                        Dish = elem.Dish,
                        Flag_Available = Flag_Available,
                        Dish_CategoryID = Dish_CategoryID
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
        public IActionResult RestaurantMenu_Dish_Del(Model_RestaurantMenu_Dishes_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление0");
            }

            if (Data != null)
            {
                int result = _bl.Restaurant_Menu_Dishes.Edit_Flag_Available(ID: Data.ID, Flag_Available: !Data.Flag_Available);

                if (result > 0)
                {
                    return RedirectToAction(actionName: "RestaurantMenu_Dish_Index", routeValues: new { RestaurantMenuID = Data.Restaurant_MenuID, Data.Dish_CategoryID });
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

        #endregion Скрыть\Восстановить


        #region Удалить все товары категории

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="Dish_CategoryID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RestaurantMenu_DishCategory_Del(int RestaurantMenuID, int Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление1");
            }

            if (Dish_CategoryID > 0)
            {
                int result = _bl.Restaurant_Menu_Dishes.DelAllCategoryDishes(Restaurant_MenuID: RestaurantMenuID, Dish_CategoryID: Dish_CategoryID);

                if (result > 0)
                {
                    return RedirectToAction(actionName: "RestaurantMenu_Dish_Index", routeValues: new { RestaurantMenuID, Dish_CategoryID });
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }

        #endregion Удалить все товары категории

        #region Открыть все товары категории

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="Dish_CategoryID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RestaurantMenu_DishCategory_Open(int RestaurantMenuID, int Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление2");
            }

            if (Dish_CategoryID > 0)
            {
                int result = _bl.Restaurant_Menu_Dishes.OpenAllCategoryDishes(Restaurant_MenuID: RestaurantMenuID, Dish_CategoryID: Dish_CategoryID);

                if (result > 0)
                {
                    return RedirectToAction(actionName: "RestaurantMenu_Dish_Index", routeValues: new { RestaurantMenuID, Dish_CategoryID });
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }

        #endregion Удалить все товары категории

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