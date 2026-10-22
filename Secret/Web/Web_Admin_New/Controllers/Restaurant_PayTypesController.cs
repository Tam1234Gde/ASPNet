using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin_ForMenu.Helpers;
using HIDE.RestAndRant.Web_Admin_ForMenu.Models.Restaurant_PayTypes;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    /// <summary>
    /// Доступные типы оплаты в ресторане
    /// </summary>
    /// <param name="_log">Логи</param>
    /// <param name="_bl">Бизнес</param>
    [Authorize]
    public class Restaurant_PayTypesController(ILogger<Restaurant_PayTypesController> _log, BL_Context _bl) : Controller
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


        /// <summary>
        /// Список
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Restaurant_PayType_Index(int RestaurantID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Список");
            }

            Model_Restaurant_PayTypes_Index model = new()
            {
                RestaurantID = RestaurantID,
                List_Restaurant_PayTypes = _bl.Restaurant_PayTypes.GetList(RestaurantID: RestaurantID, Order_TypeID: null)
            };

            return View(model: model);
        }


        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Restaurant_PayType_Part_Table(int RestaurantID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Таблица > Список");
            }

            return Restaurant_PayType_Index(RestaurantID);
        }


        #region Добавление


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Restaurant_PayType_Add(int RestaurantID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление1");
            }

            Model_Restaurant_PayTypes_CRUD model = new()
            {
                List_Pay_Types = _bl.Pay_Types.GetList(),
                List_Order_Types = _bl.Order_Types.GetList(),
                List_Restaurants = _bl.Restaurants.GetList(),
                RestaurantID = RestaurantID
            };

            return View(model: model);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Restaurant_PayType_Add(Model_Restaurant_PayTypes_CRUD data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление2");
            }

            if (ModelState.IsValid && data != null)
            {
                int result = _bl.Restaurant_PayTypes.Add(RestaurantID: data.RestaurantID, Order_TypeID: data.Order_TypeID, Pay_TypeID: data.Pay_TypeID);

                if (result < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = result });
                }

                return RedirectToAction(controllerName: "Restaurant_PayTypes", actionName: "Restaurant_PayType_Index", routeValues: new { data.RestaurantID });
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
        public IActionResult Restaurant_PayType_Edit(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение1");
            }

            CRestaurant_PayType? elem = _bl.Restaurant_PayTypes.Get(ID: ID);
            if (elem != null)
            {
                Model_Restaurant_PayTypes_CRUD data = new()
                {
                    ID = elem.ID,
                    RestaurantID = elem.RestaurantID,
                    Order_TypeID = elem.Order_TypeID,
                    Pay_TypeID = elem.Pay_TypeID,
                    List_Pay_Types = _bl.Pay_Types.GetList(),
                    List_Order_Types = _bl.Order_Types.GetList(),
                    List_Restaurants = _bl.Restaurants.GetList()
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
        public IActionResult Restaurant_PayTypes_Edit(Model_Restaurant_PayTypes_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение2");
            }

            if (Data != null)
            {
                int result = _bl.Restaurant_PayTypes.Edit(ID: Data.ID, RestaurantID: Data.RestaurantID, Order_TypeID: Data.Order_TypeID, Pay_TypeID: Data.Pay_TypeID);

                if (result > 0)
                {
                    return RedirectToAction(controllerName: "Restaurant_PayTypes", actionName: "Restaurant_PayType_Index", routeValues: new { Data.RestaurantID });
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
        public IActionResult Restaurant_PayTypes_Del(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление1");
            }

            if (ID > 0)
            {
                CRestaurant_PayType? elem = _bl.Restaurant_PayTypes.Get(ID: ID);

                if (elem != null)
                {
                    Model_Restaurant_PayTypes_CRUD data = new()
                    {
                        Restaurant = elem.Restaurant,
                        Pay_Type = elem.Pay_Type,
                        Order_Type = elem.Order_Type,
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
        public IActionResult Restaurant_PayTypes_Del(Model_Restaurant_PayTypes_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление2");
            }

            if (Data != null)
            {
                int result = _bl.Restaurant_PayTypes.Del(ID: Data.ID, UserID: UserID);

                if (result > 0)
                {
                    return RedirectToAction(controllerName: "Restaurant_PayTypes", actionName: "Restaurant_PayType_Index", routeValues: new { Data.RestaurantID });
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


        #region Добавить все варианты оплаты

        /// <summary>
        /// Добавить все варианты оплаты
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Restaurant_PayType_AddAll(int RestaurantID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавить все варианты оплаты");
            }

            if (RestaurantID > 0)
            {
                CRestaurant? elem = _bl.Restaurants.Get(ID: RestaurantID);

                if (elem != null)
                {
                    Model_Restaurant_PayTypes_CRUD data = new()
                    {
                        RestaurantID = RestaurantID,
                        Restaurant = elem.Address
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
        /// Добавить все варианты оплаты
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Restaurant_PayType_AddAll(Model_Restaurant_PayTypes_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавить все варианты оплаты");
            }

            if (Data != null)
            {
                int result = _bl.Restaurant_PayTypes.AddAll(RestaurantID: Data.RestaurantID);

                if (result > 0)
                {
                    return RedirectToAction(controllerName: "Restaurant_PayTypes", actionName: "Restaurant_PayType_Index", routeValues: new { Data.RestaurantID });
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