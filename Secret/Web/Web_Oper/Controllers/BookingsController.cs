using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Web_Oper.Helpers;
using HIDE.RestAndRant.Web_Oper.Models.Bookings;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Oper.Controllers
{
    /// <summary>
    /// Бронирование
    /// </summary>
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public BookingsController(ILogger<BookingsController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor BookingController");
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
                list_bookings = _bl.Bookings.GetListByRestaurantID_ForAdmin(RestaurantID: RestaurantID)?.OrderByDescending(p => p.Date_Start).ToList();
            }

            List<MBooking>? list_result = [];
            if (list_bookings != null && list_bookings.Count != 0)
            {
                foreach (var booking in list_bookings)
                {
                    MBooking elem = new()
                    {
                        ClientID = booking.ClientID,
                        ID = booking.ID,
                        Name = booking.Name,
                        Client = booking.Client,
                        Restaurant_Table = booking.Restaurant_Table,
                        Restaurant_TableID = booking.Restaurant_TableID,
                        Restaurant_Table_Type = booking.Restaurant_Table_Type,
                        Date_Start = booking.Date_Start,
                        Date_End = booking.Date_End,
                        Order = _bl.Orders.GetByBookingID(booking.ID)
                    };
                    list_result.Add(elem);
                }
            }

            ViewBag.RestaurantID = RestaurantID;

            return View(model: list_result);
        }

        /// <summary>
        /// Таблица > Список броней
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Bookings_Part_Table()
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
            ERestaurant_Table_Type? restaurant_Table_TypeID = ERestaurant_Table_Type.Ordinary;

            if (Restaurant_TableID > 0)
            {
                list_bookings = _bl.Bookings.GetListByRestaurant_TableID(HIDE_Convert.ToInt(Restaurant_TableID));
                restaurant_Table_TypeID = _bl.Restaurant_Tables.Get(HIDE_Convert.ToInt(Restaurant_TableID))?.Restaurant_Table_TypeID;
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
                List_Times = list_dates,
                List_RestaurantTable_Types = _bl.Restaurant_Table_Types.GetList(),
                Restaurant_Table_TypeID = restaurant_Table_TypeID,
            };

            ViewBag.RestaurantID = RestaurantID;

            return View(model);
        }


        /// <summary>
        /// Время
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Time_Part_TableByDate(int? Restaurant_TableID, bool Flag_Edit)
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());

            CRestaurant? restaurant = _bl.Restaurants.Get(RestaurantID);
            List<string> list_times_restaurant = Helper_ToArray.StringArr_ToListString(restaurant?.Hours_Work ?? "");

            List<CBooking>? list_bookings = [];
            ERestaurant_Table_Type? restaurant_Table_TypeID = ERestaurant_Table_Type.Ordinary;

            if (Restaurant_TableID > 0)
            {
                list_bookings = _bl.Bookings.GetListByRestaurant_TableID(HIDE_Convert.ToInt(Restaurant_TableID));
                restaurant_Table_TypeID = _bl.Restaurant_Tables.Get(HIDE_Convert.ToInt(Restaurant_TableID))?.Restaurant_Table_TypeID;
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

            Model_BookingsTime_AddEdit model = new()
            {
                List_Times = list_dates,
                List_RestaurantTable_Types = _bl.Restaurant_Table_Types.GetList(),
                Restaurant_Table_TypeID = restaurant_Table_TypeID,
            };

            ViewBag.RestaurantID = RestaurantID;

            return View(model);
        }

        #region Добавление по номеру стола(в рамках текущего дня)


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Restaurant_Table_TypeID">Код типа стола</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add(ERestaurant_Table_Type? Restaurant_Table_TypeID)
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());

            Model_Bookings_AddEdit model = new()
            {
                List_Clients = _bl.Clients.GetList(),
                List_Restaurant_Tables = Restaurant_Table_TypeID.HasValue ? _bl.Restaurant_Tables.GetListByRestaurant_Table_TypeID(RestaurantID: RestaurantID, Restaurant_HallID: null,
                Restaurant_Table_TypeID: (int)Restaurant_Table_TypeID) : _bl.Restaurant_Tables.GetList(RestaurantID: RestaurantID),
                List_RestaurantTable_Types = _bl.Restaurant_Table_Types.GetListByRestaurantID(RestaurantID: RestaurantID)
            };

            if (model.Restaurant_Table_TypeID.HasValue)
            {
                model.List_Restaurant_Tables = _bl.Restaurant_Tables.GetListByRestaurant_Table_TypeID(RestaurantID: RestaurantID, Restaurant_Table_TypeID: (int)model.Restaurant_Table_TypeID.Value, Restaurant_HallID: null);
                    
            }

            if (!Restaurant_Table_TypeID.HasValue)
            {
                model.Restaurant_Table_TypeID = _bl.Restaurant_Tables.GetList(RestaurantID: RestaurantID)?.FirstOrDefault()?.Restaurant_Table_TypeID;
            }

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
                ERestaurant_Table_Type? restaurant_Table_TypeID = ERestaurant_Table_Type.Ordinary;
                if (data.Restaurant_TableID > 0)
                {
                    restaurant_Table_TypeID = _bl.Restaurant_Tables.Get(HIDE_Convert.ToInt(data.Restaurant_TableID))?.Restaurant_Table_TypeID;
                }
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

                if (HIDE_Convert.ToDateTime(data.Date_Start) <= DateTime.MinValue)
                {
                    ModelState.AddModelError("Date_Start", "Поле 'Укажите время' обязательно к заполнению");
                    return View(data);
                }

                DateTime date_start = HIDE_Convert.ToDateTime(data.Date_Start);
                DateTime date_end = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(data.Date_End))
                {
                    date_end = HIDE_Convert.ToDateTime(data.Date_End);
                }

                if (data.Vip_Time.HasValue && data.Vip_Time.Value > 0 && restaurant_Table_TypeID == ERestaurant_Table_Type.VIP)
                {
                    date_end = date_start.AddHours(data.Vip_Time.Value);
                }

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


        /// <summary>
        /// Таблица > Список броней
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Part_Tables(ERestaurant_Table_Type? Restaurant_Table_TypeID)
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());
            Model_Bookings_AddEdit model = new()
            {
                Restaurant_Table_TypeID = Restaurant_Table_TypeID,
                List_Restaurant_Tables = Restaurant_Table_TypeID.HasValue ? _bl.Restaurant_Tables.GetListByRestaurant_Table_TypeID(RestaurantID: RestaurantID,
                                            Restaurant_Table_TypeID: (int)Restaurant_Table_TypeID, Restaurant_HallID: null) : _bl.Restaurant_Tables.GetList(RestaurantID: RestaurantID)
            };

            if (Restaurant_Table_TypeID.HasValue)
            {
                model.Image = _bl.Restaurant_Halls.Get(RestaurantID: RestaurantID, Restaurant_Table_TypeID: (ERestaurant_Table_Type)Restaurant_Table_TypeID)?.Restaurant_Image;
            }

            ViewBag.RestaurantID = RestaurantID;
            return View(model: model);
        }

        #endregion Добавление по номеру стола(в рамках текущего дня)

        #region Добавление По Дате и времени

        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Restaurant_Table_TypeID">Код типа стола</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddByDate(ERestaurant_Table_Type? Restaurant_Table_TypeID)
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());

            Model_BookingsTime_AddEdit model = new()
            {
                List_Clients = _bl.Clients.GetList(),
                //List_Restaurant_Tables = Restaurant_Table_TypeID.HasValue ? _bl.Restaurant_Tables.GetListByRestaurant_Table_TypeID(RestaurantID: RestaurantID, Restaurant_Table_TypeID: (int)Restaurant_Table_TypeID) : _bl.Restaurant_Tables.GetList(RestaurantID: RestaurantID),
                List_RestaurantTable_Types = _bl.Restaurant_Table_Types.GetListByRestaurantID(RestaurantID: RestaurantID)
            };

            if (!Restaurant_Table_TypeID.HasValue)
            {
                model.Restaurant_Table_TypeID = _bl.Restaurant_Tables.GetList(RestaurantID: RestaurantID)?.FirstOrDefault()?.Restaurant_Table_TypeID;
                //model.List_Restaurant_Tables = _bl.Restaurant_Tables.GetListByRestaurant_Table_TypeID(RestaurantID: RestaurantID, Restaurant_Table_TypeID: (int)model.Restaurant_Table_TypeID);
            }

            ViewBag.RestaurantID = RestaurantID;

            return View(model);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddByDate(Model_BookingsTime_AddEdit data)
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());

            if (ModelState.IsValid && data != null)
            {
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

                if (HIDE_Convert.ToDateTime(data.Date_Start) <= DateTime.MinValue)
                {
                    ModelState.AddModelError("Date_Start", "Поле 'Укажите время' обязательно к заполнению");
                    return View(data);
                }

                DateTime date_start = HIDE_Convert.ToDateTime(data.Date_Start);


                int time = 30;
                if (data.Restaurant_Table_TypeID == ERestaurant_Table_Type.VIP && data.Vip_Time.HasValue)
                {
                    time = data.Vip_Time.Value * 60;
                }


                // Добавление брони
                int booking_id = _bl.Bookings.Add(Restaurant_TableID: data.Restaurant_TableID, ClientID: data.ClientID > 0 ? data.ClientID : null, Date_Start: date_start,
                                                  Date_End: date_start.AddMinutes(time), Phone: data.Phone, Name: data.Name);

                if (booking_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = booking_id });
                }

                //int table_reserve = _bl.Restaurant_Tables.Edit_Flag_IsAvailable(ID: data.Restaurant_TableID, Flag_IsAvailable: false);

                //if (table_reserve < 0)
                //{
                //    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = table_reserve });
                //}

                return RedirectToAction("Index");
            }
            else
            {
                return View(data);
            }
        }


        /// <summary>
        /// Таблица > Список броней
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Part_TablesByDate(ERestaurant_Table_Type? Restaurant_Table_TypeID, string Date_Booking)
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());
            Model_BookingsTime_AddEdit model = new()
            {
                Restaurant_Table_TypeID = Restaurant_Table_TypeID,
                List_Restaurant_Tables = Restaurant_Table_TypeID.HasValue ? _bl.Restaurant_Tables.GetListByRestaurant_Table_TypeID(RestaurantID: RestaurantID,
                Restaurant_Table_TypeID: (int)Restaurant_Table_TypeID, Restaurant_HallID: null) : _bl.Restaurant_Tables.GetList(RestaurantID: RestaurantID)
            };

            

            DateTime date_booking = HIDE_Convert.ToDateTime(Date_Booking);

            if (Restaurant_Table_TypeID != null)
            {
                List<int>? list_tables = _bl.Bookings.GetListByRestaurant_TableTypeID(RestaurantID: RestaurantID,
                                                                                 Restaurant_TableTypeID: (int)Restaurant_Table_TypeID,           
                                                                                 Date_Booking: date_booking, Restaurant_HallID: null)?.Select(p => p.Restaurant_TableID).ToList();

                List<CRestaurant_Table>? list_rest_tables = _bl.Restaurant_Tables.GetListByRestaurant_Table_TypeID(RestaurantID: RestaurantID,
                                                                                                              Restaurant_Table_TypeID: (int)Restaurant_Table_TypeID,
                                                                                                              Restaurant_HallID: null);

                if (list_rest_tables == null || list_rest_tables.Count == 0)
                {
                    return View(model: model);
                }

                List<CRestaurant_Table>? list_result = [];
                foreach (CRestaurant_Table elem in list_rest_tables)
                {
                    if (list_tables != null && list_tables.Count != 0)
                    {
                        foreach (var item in list_tables)
                        {
                            if (elem.ID == item)
                            {
                                elem.Restaurant_Table_StatusID = ERestaurant_Table_Status.Unavailable;
                            }
                        }
                    }
                    list_result.Add(elem);
                }


                model.List_Restaurant_Tables = list_result;

                ViewBag.RestaurantID = RestaurantID;
                return View(model: model);
            }
            else
            {
                return BadRequest("Restaurant_Table_TypeID is required");
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


        /// <summary>
        /// Время следующей брони
        /// </summary>
        /// <param name="Date_Booking">Желаемое время бронирования</param>
        /// <param name="Restaurant_TableID">Код столика</param>
        /// <returns></returns>
        [HttpGet]
        public Model_GetNextBooking? GetNextBooking(DateTime Date_Booking, int Restaurant_TableID)
        {
            CBooking? CBooking = _bl.Bookings.CheckBooking(Restaurant_TableID: Restaurant_TableID, Date_Booking: Date_Booking);
            bool flag = false;
            if (CBooking != null)
            {
                flag = true;
            }

            Model_GetNextBooking model = new()
            {
                Date_Next = _bl.Bookings.Get(Restaurant_TableID: Restaurant_TableID, Date_Booking: Date_Booking)?.Date_Start.ToShortTimeString(),
                Flag_Unavailable = flag
            };

            return model;
        }

    }
}
