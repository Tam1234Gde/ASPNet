using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Oper.Helpers;
using HIDE.RestAndRant.Web_Oper.Models.Bookings;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Oper.Controllers
{
    /// <summary>
    /// Бронирование залов
    /// </summary>
    [Authorize]
    public class BookingHallsController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public BookingHallsController(ILogger<BookingHallsController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor BookingHallsController");
            _bl = BL;
        }

        /// <summary>
        /// Список броней
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());
            List<CBooking>? list_bookings = [];
            if (RestaurantID > 0)
            {
                list_bookings = _bl.Bookings.GetListByRestaurantID(RestaurantID: RestaurantID);
            }

            ViewBag.RestaurantID = RestaurantID;

            return View(model: list_bookings);
        }

        /// <summary>
        /// Таблица > Список броней
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult BookingHalls_Part_Table()
        {
            return Index();
        }

        /// <summary>
        /// Время
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Time_Part_Table(int? Restaurant_TableID, bool Flag_Edit)
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());

            CRestaurant? restaurant = _bl.Restaurants.Get(RestaurantID);
            List<string> list_times_restaurant = Helper_ToArray.StringArr_ToListString(restaurant?.Hours_Work ?? "");

            List<CBooking>? list_bookings = [];

            if (Restaurant_TableID > 0)
            {
                list_bookings = _bl.Bookings.GetListByRestaurant_TableID(HIDE_Convert.ToInt(Restaurant_TableID));
            }

            List<DateTime> list_dates = [];
            DateTime start = HIDE_Convert.ToDateTime(list_times_restaurant.FirstOrDefault());
            while (start < HIDE_Convert.ToDateTime(list_times_restaurant.LastOrDefault()))
            {

                start = start.AddMinutes(AppConfig.RangeOrder);
                if (start >= DateTime.Now)
                {
                    list_dates.Add(start);
                }
            }

            if (list_bookings != null && list_bookings.Count != 0 && !Flag_Edit)
            {
                foreach (var booking in list_bookings)
                {
                    list_dates.Remove(booking.Date_Start);
                }
            }

            Model_Bookings_AddEdit model = new()
            {
                List_Times = list_dates
            };

            ViewBag.RestaurantID = RestaurantID;

            return View(model);
        }

        #region Добавление

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());

            Model_Bookings_AddEdit model = new()
            {
                List_Clients = _bl.Clients.GetList(),
                List_Restaurant_Tables = _bl.Restaurant_Tables.GetList(RestaurantID: RestaurantID)
            };

            ViewBag.RestaurantID = RestaurantID;

            return View(model);
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Bookings_AddEdit data)
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());

            if (ModelState.IsValid && data != null)
            {
                //CRestaurant? restaurant = _bl.Restaurants.Get(RestaurantID);
                //List<string> list_times_restaurant = Helper_ToArray.StringArr_ToListString(restaurant?.Hours_Work ?? "");

                data.List_Clients = _bl.Clients.GetList();
                data.List_Restaurant_Tables = _bl.Restaurant_Tables.GetList(RestaurantID: RestaurantID);

                if (data.ClientID <= 0 && string.IsNullOrWhiteSpace(data.Name) && string.IsNullOrWhiteSpace(data.Phone))
                {
                    ModelState.AddModelError("ClientID", "Поле 'Выберите клиента или заполните поля Имя и Телефон' обязательно к заполнению");
                    return View(data);
                }

                if (data.Restaurant_TableID <= 0)
                {
                    ModelState.AddModelError("Restaurant_TableID", "Поле 'Выберите стол' обязательно к заполнению");
                    return View(data);
                }

                if (HIDE_Convert.ToDateTime(data.Date_Start) <= DateTime.MinValue || HIDE_Convert.ToDateTime(data.Date_End) <= DateTime.MinValue)
                {
                    ModelState.AddModelError("Date_Start", "Поле 'Укажите время' обязательно к заполнению");
                    return View(data);
                }

                DateTime date_start = HIDE_Convert.ToDateTime(data.Date_Start);
                DateTime date_end = HIDE_Convert.ToDateTime(data.Date_End);

                int booking_id = _bl.Bookings.Add(Restaurant_TableID: data.Restaurant_TableID, ClientID: data.ClientID > 0 ? data.ClientID : null, Date_Start: date_start,
                                                  Date_End: date_end, Phone: data.Phone, Name: data.Name);

                if (booking_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = booking_id });
                }

                int table_reserve = _bl.Restaurant_Tables.Edit_Flag_IsAvailable(ID: data.Restaurant_TableID, Flag_IsAvailable: false);

                if (table_reserve < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = table_reserve });
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(data);
            }
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());
            CBooking? booking = _bl.Bookings.Get(ID: ID);
            if (booking != null)
            {
                Model_Bookings_AddEdit data = new()
                {
                    ID = booking.ID,
                    ClientID = booking.ClientID,
                    Date_End = string.Format("{0:dd.MM.yyyy HH:mm:ss}", booking.Date_End),
                    Date_Start = string.Format("{0:dd.MM.yyyy HH:mm:ss}", booking.Date_Start),
                    Phone = booking.Phone,
                    Name = booking.Name,
                    Restaurant_TableID = booking.Restaurant_TableID,
                    List_Clients = _bl.Clients.GetList(),
                    List_Restaurant_Tables = _bl.Restaurant_Tables.GetList(RestaurantID)
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
        public IActionResult Edit(Model_Bookings_AddEdit data)
        {
            if (data != null)
            {
                int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());
                data.List_Clients = _bl.Clients.GetList();
                data.List_Restaurant_Tables = _bl.Restaurant_Tables.GetList(RestaurantID: RestaurantID);

                if (data.ClientID <= 0 && string.IsNullOrWhiteSpace(data.Name) && string.IsNullOrWhiteSpace(data.Phone))
                {
                    ModelState.AddModelError("ClientID", "Поле 'Выберите клиента или заполните поля Имя и Телефон' обязательно к заполнению");
                    return View(data);
                }

                if (data.Restaurant_TableID <= 0)
                {
                    ModelState.AddModelError("Restaurant_TableID", "Поле 'Выберите стол' обязательно к заполнению");
                    return View(data);
                }

                if (HIDE_Convert.ToDateTime(data.Date_Start) <= DateTime.MinValue || HIDE_Convert.ToDateTime(data.Date_End) <= DateTime.MinValue)
                {
                    ModelState.AddModelError("Date_Start", "Поле 'Укажите время' обязательно к заполнению");
                    return View(data);
                }

                DateTime date_start = HIDE_Convert.ToDateTime(data.Date_Start);
                DateTime date_end = HIDE_Convert.ToDateTime(data.Date_End);

                int booking_id = _bl.Bookings.Edit(ID: data.ID, Restaurant_TableID: data.Restaurant_TableID, ClientID: data.ClientID > 0 ? data.ClientID : null, Date_Start: date_start,
                                                   Date_End: date_end, Phone: data.Phone, Name: data.Name);

                if (booking_id > 0)
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
        /// Удаление брони
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CBooking? booking = _bl.Bookings.Get(ID: ID);

                if (booking != null)
                {
                    Model_Bookings_AddEdit data = new()
                    {
                        Restaurant_Table = booking.Restaurant_Table,
                        Restaurant_TableID = booking.Restaurant_TableID
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
        /// Удаление брони
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Bookings_AddEdit Data)
        {
            if (Data != null)
            {
                int booking_id = _bl.Bookings.Del(ID: Data.ID, UserID: Helper_Client.GetCurrentClientID(HttpContext.User));

                if (booking_id < 0)
                {
                    return View(model: Data);
                }

                return RedirectToAction("Index");
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
        /// Считываем куки > Получаем RestaurantID
        /// </summary>
        /// <returns></returns>
        private string? GetRestaurantIDByCookies()
        {
            CUser? user = _bl.Users.Get(ID: Helper_Client.GetCurrentClientID(HttpContext.User));
            int? restaurantID = user?.RestaurantID ?? null;

            if (user != null && user.Flag_Root)
            {
                ViewBag.Restaurants = _bl.Restaurants.GetList();
            }
            else
            {
                ViewBag.Restaurants = _bl.Restaurants.GetList()?.Where(p => p.ID == restaurantID).ToList();
            }

            string? RestaurantID;
            if (Request.Cookies["RestaurantCookies"] != null)
            {
                RestaurantID = Request.Cookies["RestaurantCookies"];
            }
            else
            {
                RestaurantID = ((List<CRestaurant>)ViewBag.Restaurants).FirstOrDefault()?.ID.ToString();
            }

            return RestaurantID;
        }


        /// <summary>
        /// Добавить в куки для сохранения ресторана
        /// </summary>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        public string AddRestaurantCookies(string page, string value)
        {
            var CookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(60)
            };
            Response.Cookies.Append(page, value, options: CookieOptions);

            return value;
        }
    }
}