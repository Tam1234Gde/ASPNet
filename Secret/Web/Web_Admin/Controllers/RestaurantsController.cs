using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.Restaurants;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Рестораны
    /// </summary>
    [Authorize]
    public class RestaurantsController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;
        private int user_id;

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
        /// <param name="BL"></param>
        public RestaurantsController(ILogger<RestaurantsController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor RestaurantsController");
            _bl = BL;
        }

        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех ресторанов
        /// GET: /Restaurants
        /// </summary>
        [HttpGet]
        public IActionResult GetRestaurants()
        {
            try
            {
                List<CRestaurant> restaurants = _bl.Restaurants.GetListAdmin();

                List<object> result = new List<object>();
                foreach (CRestaurant item in restaurants)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Address,
                        item.Latitude,
                        item.Longitude,
                        item.Hours_Work,
                        item.Hours_WorkApp,
                        item.Hours_HardWork,
                        item.Delivery_Price,
                        item.Flag_Car,
                        item.Flag_Delivery,
                        item.Flag_Incafe,
                        item.Flag_Myself,
                        item.Express_Delivery_Time,
                        item.Delivery_Time,
                        item.TgChatID,
                        item.ClientID,
                        item.BookingHour,
                        item.Hours_BookingWork,
                        item.BookingPhone,
                        item.CityID,
                        CityName = item.City != null ? item.City : string.Empty,
                        item.TgChatBookingID,
                        item.TgChatQrID,
                        item.Flag_Hidden,
                        item.Flag_Hide,
                        item.HiddenTitle,
                        item.HiddenText,
                        item.YouKassaLogin,
                        item.YouKassaPassword
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
        /// Получить ресторан по ID
        /// GET: /Restaurants/GetRestaurant?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetRestaurant(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CRestaurant restaurant = _bl.Restaurants.Get(ID: ID);
                if (restaurant == null)
                    return Json(new { success = false, message = "Ресторан не найден" });

                object result = new
                {
                    restaurant.ID,
                    restaurant.Address,
                    restaurant.Latitude,
                    restaurant.Longitude,
                    restaurant.Hours_Work,
                    restaurant.Hours_WorkApp,
                    restaurant.Hours_HardWork,
                    restaurant.Delivery_Price,
                    restaurant.Flag_Car,
                    restaurant.Flag_Delivery,
                    restaurant.Flag_Incafe,
                    restaurant.Flag_Myself,
                    restaurant.Express_Delivery_Time,
                    restaurant.Delivery_Time,
                    restaurant.TgChatID,
                    restaurant.ClientID,
                    restaurant.BookingHour,
                    restaurant.Hours_BookingWork,
                    restaurant.BookingPhone,
                    restaurant.CityID,
                    CityName = restaurant.City != null ? restaurant.City : string.Empty,
                    restaurant.TgChatBookingID,
                    restaurant.TgChatQrID,
                    restaurant.Flag_Hidden,
                    restaurant.Flag_Hide,
                    restaurant.HiddenTitle,
                    restaurant.HiddenText,
                    restaurant.YouKassaLogin,
                    restaurant.YouKassaPassword
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список ресторанов по городу
        /// GET: /Restaurants/GetRestaurantsByCity?CityID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetRestaurantsByCity(int CityID)
        {
            try
            {
                if (CityID <= 0)
                    return Json(new { success = false, message = "Неверный ID города" });

                List<CRestaurant> allRestaurants = _bl.Restaurants.GetListAdmin();

                List<CRestaurant> filteredRestaurants = new List<CRestaurant>();
                foreach (CRestaurant item in allRestaurants)
                {
                    if (item.CityID == CityID)
                    {
                        filteredRestaurants.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CRestaurant item in filteredRestaurants)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Address,
                        item.Latitude,
                        item.Longitude,
                        item.Hours_Work,
                        item.Delivery_Price,
                        item.Flag_Delivery,
                        item.Flag_Incafe,
                        item.Flag_Myself,
                        item.Express_Delivery_Time,
                        item.Delivery_Time,
                        item.CityID,
                        CityName = item.City != null ? item.City : string.Empty,
                        item.Flag_Hidden,
                        item.Flag_Hide
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
        /// Получить список активных ресторанов (не скрытых)
        /// GET: /Restaurants/GetActiveRestaurants
        /// </summary>
        [HttpGet]
        public IActionResult GetActiveRestaurants()
        {
            try
            {
                List<CRestaurant> allRestaurants = _bl.Restaurants.GetListAdmin();

                List<CRestaurant> activeRestaurants = new List<CRestaurant>();
                foreach (CRestaurant item in allRestaurants)
                {
                    if (!item.Flag_Hide && !item.Flag_Hidden)
                    {
                        activeRestaurants.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CRestaurant item in activeRestaurants)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Address,
                        item.Latitude,
                        item.Longitude,
                        item.Hours_Work,
                        item.Delivery_Price,
                        item.Flag_Delivery,
                        item.Flag_Incafe,
                        item.Flag_Myself,
                        item.Express_Delivery_Time,
                        item.Delivery_Time,
                        item.CityID,
                        CityName = item.City != null ? item.City : string.Empty
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
        /// Список ресторанов
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<CRestaurant>? list_restaurants = [];
            CUser? user = _bl.Users.Get(ID: Helper_User.GetCurrentUserID(HttpContext.User));
            if (user != null && user.Flag_Admin && !user.RestaurantID.HasValue || user != null && user.Flag_Root)
            {
                list_restaurants = _bl.Restaurants.GetListAdmin();
            }
            else if (user != null && user.Flag_Admin && user.RestaurantID.HasValue)
            {
                list_restaurants = _bl.Restaurants.GetListAdmin()?.Where(p => p.ID == user.RestaurantID.Value).ToList();
            }
            return View(model: list_restaurants);
        }

        /// <summary>
        /// Таблица > Список ресторанов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Restaurants_Part_Table()
        {
            List<CRestaurant>? list_restaurants = _bl.Restaurants.GetListAdmin();
            return View(model: list_restaurants);
        }

        #region Добавление

        /// <summary>
        /// Добавление ресторана
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            Model_Restaurants_AddEdit model = new()
            {
                List_Cities = _bl.Cities.GetList()
            };

            return View(model: model);
        }

        /// <summary>
        /// Добавление ресторана
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Restaurants_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                int restaurant_id = _bl.Restaurants.Add(Address: data.Adress, Latitude: data.Latitude, Longitude: data.Longitude, Hours_Work: data.Hours_Work,
                                                        Hours_WorkApp: data.Hours_WorkApp, Hours_HardWork: data.Hours_HardWork, Delivery_Price: data.Delivery_Price,
                                                        Flag_Car: data.Flag_Car, Flag_Delivery: data.Flag_Delivery, Flag_Incafe: data.Flag_Incafe,
                                                        Flag_Myself: data.Flag_Myself, Express_Delivery_Time: data.Express_Delivery_Time, Delivery_Time: data.Delivery_Time,
                                                        TgChatID: data.TgChatID, ClientID: data.ClientID, Hours_BookingWork: data.Hours_BookingWork,
                                                        BookingHour: data.BookingHour, BookingPhone: data.BookingPhone, CityID: data.CityID,
                                                        TgChatBookingID: data.TgChatBookingID, TgChatQrID: data.TgChatQrID, Flag_Hidden: data.Flag_Hidden, Flag_Hide: data.Flag_Hide,
                                                        HiddenTitle: data.HiddenTitle, HiddenText: data.HiddenText, YouKassaLogin: data.YouKassaLogin,
                                                        YouKassaPassword: data.YouKassaPassword);

                if (restaurant_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = restaurant_id });
                }

                return RedirectToAction("Index"); //Возвращаем к списку ресторанов
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение ресторана
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CRestaurant? restaurant = _bl.Restaurants.Get(ID: ID);
            if (restaurant != null)
            {
                Model_Restaurants_AddEdit data = new()
                {
                    ID = restaurant.ID,
                    Adress = restaurant.Address,
                    Latitude = restaurant.Latitude,
                    Longitude = restaurant.Longitude,
                    Hours_Work = restaurant.Hours_Work,
                    Hours_WorkApp = restaurant.Hours_WorkApp,
                    Hours_HardWork = restaurant.Hours_HardWork,
                    Delivery_Price = restaurant.Delivery_Price,
                    Flag_Myself = restaurant.Flag_Myself,
                    Flag_Incafe = restaurant.Flag_Incafe,
                    Flag_Delivery = restaurant.Flag_Delivery,
                    Flag_Car = restaurant.Flag_Car,
                    Express_Delivery_Time = restaurant.Express_Delivery_Time,
                    Delivery_Time = restaurant.Delivery_Time,
                    TgChatID = restaurant.TgChatID,
                    ClientID = restaurant.ClientID,
                    BookingHour = restaurant.BookingHour,
                    Hours_BookingWork = restaurant.Hours_BookingWork,
                    BookingPhone = restaurant.BookingPhone,
                    CityID = restaurant.CityID,
                    TgChatBookingID = restaurant.TgChatBookingID,
                    TgChatQrID = restaurant.TgChatQrID,
                    Flag_Hidden = restaurant.Flag_Hidden,
                    Flag_Hide = restaurant.Flag_Hide,
                    HiddenTitle = restaurant.HiddenTitle,
                    HiddenText = restaurant.HiddenText,
                    YouKassaLogin = restaurant.YouKassaLogin,
                    YouKassaPassword = restaurant.YouKassaPassword,
                    List_Cities = _bl.Cities.GetList()
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }
        }

        /// <summary>
        /// Изменение ресторана
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Restaurants_AddEdit data)
        {
            if (data != null)
            {
                int restaurant_id = _bl.Restaurants.Edit(ID: data.ID, Address_New: data.Adress, Latitude_New: data.Latitude, Longitude_New: data.Longitude,
                                                         Hours_Work_New: data.Hours_Work, Hours_WorkApp_New: data.Hours_WorkApp, Hours_HardWork_New: data.Hours_HardWork,
                                                         Delivery_Price_New: data.Delivery_Price, Flag_Car_New: data.Flag_Car, Flag_Delivery_New: data.Flag_Delivery,
                                                         Flag_Incafe_New: data.Flag_Incafe, Flag_Myself_New: data.Flag_Myself,
                                                         Express_Delivery_Time_New: data.Express_Delivery_Time, Delivery_Time_New: data.Delivery_Time,
                                                         TgChatID_New: data.TgChatID, ClientID_New: data.ClientID, Hours_BookingWork_New: data.Hours_BookingWork,
                                                         BookingHour_New: data.BookingHour, BookingPhone_New: data.BookingPhone, CityID_New: data.CityID,
                                                         TgChatBookingID_New: data.TgChatBookingID,
                                                         TgChatQrID_New: data.TgChatQrID, Flag_Hidden_New: data.Flag_Hidden, Flag_Hide_New: data.Flag_Hide, HiddenTitle_New: data.HiddenTitle,
                                                         HiddenText_New: data.HiddenText, YouKassaLogin_New: data.YouKassaLogin, YouKassaPassword_New: data.YouKassaPassword);

                if (restaurant_id > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model: data);
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
        /// Удаление ресторана
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CRestaurant? restaurant = _bl.Restaurants.Get(ID: ID);

                if (restaurant != null)
                {
                    Model_Restaurants_AddEdit data = new()
                    {
                        Adress = restaurant.Address
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
        /// Удаление ресторана
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Restaurants_AddEdit Data)
        {
            if (Data != null)
            {
                int restaurant_id = _bl.Restaurants.Del(ID: Data.ID, UserID: UserID);

                if (restaurant_id > 0)
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

        /// <summary>
        /// Выполняется для всех методов в контроллере
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Flag_ShowMenu = Get_FlagAdmin();
            ViewBag.RestaurantID = Get_RestaurantID();
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


        /// <summary>
        /// Получение Флага > Админ
        /// </summary>
        /// <returns></returns>
        public int? Get_RestaurantID()
        {
            CUser? user = _bl.Users.Get(ID: Helper_User.GetCurrentUserID(HttpContext.User));
            if (user != null && user.Flag_Admin)
            {
                ViewBag.RestaurantID = user.RestaurantID;
                return user.RestaurantID;
            }

            return null;
        }

        #endregion Получение Флага > Админ

    }
}