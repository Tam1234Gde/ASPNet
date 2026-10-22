using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.RestaurantMenu_Dish_Volumes;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Объемы блюд в ресторане
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Authorize]
    public class RestaurantMenu_Dish_VolumesController(ILogger<RestaurantMenu_Dish_VolumesController> _log, BL_Context _bl) : Controller           //TODO: Logs
    {
        readonly Images Images_BL = new();
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


        /// <summary>
        /// Список
        /// </summary>
        /// <param name="Restaurant_Menu_DishID">Код блюда ресторана</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Volume_Index(int Restaurant_Menu_DishID, int? Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Список");
            }

            var dish = _bl.Restaurant_Menu_Dishes.Get(Restaurant_Menu_DishID);

            Model_RestaurantMenu_Dish_Volumes_Index model = new()
            {
                List_Restaurant_Menu_Dish_Volumes = _bl.Restaurant_Menu_Dish_Volumes.GetList(Restaurant_Menu_DishID),
                Restaurant_Menu_DishID = Restaurant_Menu_DishID,
                Restaurant_Menu_Dish = dish?.Dish ?? String.Empty,
                RestaurantMenuID = dish!.Restaurant_MenuID
            };

            if (Dish_CategoryID.HasValue && Dish_CategoryID.Value > 0)
            {
                model.Dish_CategoryID = Dish_CategoryID.Value;
            }

            return View(model: model);
        }


        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Volume_Index_Part_Table(int Restaurant_Menu_DishID, int? Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Таблица > Список");
            }

            return RestaurantMenu_Dish_Volume_Index(Restaurant_Menu_DishID, Dish_CategoryID);
        }


        #region Добавление


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Volume_Add(int Restaurant_Menu_DishID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление1");
            }

            Model_RestaurantMenu_Dish_Volumes_CRUD model = new()
            {
                List_Volumes = _bl.Volumes.GetList(),
                Restaurant_Menu_DishID = Restaurant_Menu_DishID
            };

            return View(model: model);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RestaurantMenu_Dish_Volume_Add(Model_RestaurantMenu_Dish_Volumes_CRUD data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление2");
            }

            if (ModelState.IsValid && data != null)
            {
                int? volumes_count = _bl.Restaurant_Menu_Dish_Volumes.GetList(Restaurant_Menu_DishID: data.Restaurant_Menu_DishID)?.Count;
                if (volumes_count != null && volumes_count >= 3)
                {
                    ModelState.AddModelError(key: "VolumeID", errorMessage: "Для товара доступны только 3 объёма");
                    data.List_Volumes = _bl.Volumes.GetList();
                    return View(model: data);
                }

                int dish_volume_id = _bl.Restaurant_Menu_Dish_Volumes.Add(Restaurant_Menu_DishID: data.Restaurant_Menu_DishID, VolumeID: data.VolumeID, Price: data.Price, OrderNum: data.OrderNum);
                if (dish_volume_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index");
                }

                return RedirectToAction(controllerName: "RestaurantMenu_Dish_Volumes", actionName: "RestaurantMenu_Dish_Volume_Index", routeValues: new { data.Restaurant_Menu_DishID });
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
        public IActionResult RestaurantMenu_Dish_Volume_Edit(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение1");
            }

            CRestaurant_Menu_Dish_Volume? dish_volume = _bl.Restaurant_Menu_Dish_Volumes.Get(ID: ID);
            if (dish_volume != null)
            {
                Model_RestaurantMenu_Dish_Volumes_CRUD data = new()
                {
                    ID = dish_volume.ID,
                    Price = dish_volume.VolumePrice,
                    List_Volumes = _bl.Volumes.GetList(),
                    OrderNum = dish_volume.OrderNum,
                    Restaurant_Menu_DishID = dish_volume.Restaurant_Menu_DishID
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RestaurantMenu_Dish_Volume_Edit(Model_RestaurantMenu_Dish_Volumes_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение2");
            }

            if (Data != null)
            {
                int dish_volume_id = _bl.Restaurant_Menu_Dish_Volumes.Edit(ID: Data.ID, Restaurant_Menu_DishID: Data.Restaurant_Menu_DishID, VolumeID: Data.VolumeID,
                                                                           Price: Data.Price, OrderNum: Data.OrderNum);

                if (dish_volume_id > 0)
                {
                    return RedirectToAction(controllerName: "RestaurantMenu_Dish_Volumes", actionName: "RestaurantMenu_Dish_Volume_Index", routeValues: new { Data.Restaurant_Menu_DishID });
                }
                else
                {
                    return View(model: Data);
                }
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }
        }

        #endregion


        #region Удаление

        /// <summary>
        /// Удаление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Volume_Del(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление1");
            }

            if (ID > 0)
            {
                CRestaurant_Menu_Dish_Volume? elem = _bl.Restaurant_Menu_Dish_Volumes.Get(ID: ID);

                if (elem != null)
                {
                    Model_RestaurantMenu_Dish_Volumes_CRUD data = new()
                    {
                        Restaurant_Menu_DishID = elem.Restaurant_Menu_DishID,
                        Volume = elem.VolumeValue ?? 0m
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
        public IActionResult RestaurantMenu_Dish_Volume_Del(Model_RestaurantMenu_Dish_Volumes_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление2");
            }

            if (Data != null)
            {
                int result = _bl.Restaurant_Menu_Dish_Volumes.Del(ID: Data.ID, UserID: UserID);

                if (result > 0)
                {
                    return RedirectToAction(actionName: "RestaurantMenu_Dish_Volume_Index", routeValues: new { Data.Restaurant_Menu_DishID });
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