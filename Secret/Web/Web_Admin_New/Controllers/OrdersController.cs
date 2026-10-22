using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Web_Admin_ForMenu.Helpers;
using HIDE.RestAndRant.Web_Admin_ForMenu.Models.Orders;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    /// <summary>
    /// Заказы
    /// </summary>
    [Authorize]
    public class OrdersController : Controller
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
        public OrdersController(ILogger<OrdersController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor OrdersController");
            _bl = BL;
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список заказов за период
        /// GET: /Orders
        /// </summary>
        [HttpGet]
        public IActionResult GetOrders(string? Date_From, string? Date_To)
        {
            try
            {
                DateTime? date_from = HIDE_Convert.ToDateTime_Nullable(Date_From);
                if (!date_from.HasValue)
                {
                    date_from = DateTime.Now.Date.AddDays(-30);
                }

                DateTime? date_to = HIDE_Convert.ToDateTime_Nullable(Date_To);
                if (!date_to.HasValue)
                {
                    date_to = DateTime.Now.Date;
                }

                List<COrder_ForAdmin> orders = _bl.Orders.GetList_ForAdminDates(Date_From: date_from, Date_To: date_to);

                List<object> result = new List<object>();
                foreach (COrder_ForAdmin item in orders)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Order_FavoriteID,
                        item.Date_Act,
                        item.Order_StatusID,
                        Order_Status = item.Order_Status ?? string.Empty,
                        item.Order_TypeID,
                        Order_Type = item.Order_Type ?? string.Empty,
                        item.ClientID,
                        Client = item.Client ?? string.Empty,
                        Client_Phone = item.Client_Phone ?? string.Empty,
                        item.Name,
                        item.Phone,
                        item.Pay_TypeID,
                        Pay_Type = item.Pay_Type ?? string.Empty,
                        item.RestaurantID,
                        Restaurant = item.Restaurant ?? string.Empty,
                        item.Date_To,
                        item.Sum,
                        Car_Brand = item.Car_Brand ?? string.Empty,
                        Car_RegNumber = item.Car_RegNumber ?? string.Empty,
                        Car_Color = item.Car_Color ?? string.Empty,
                        Comment = item.Comment ?? string.Empty,
                        Comment_AddToOrder = item.Comment_AddToOrder ?? string.Empty,
                        Comment_RemoveFromOrder = item.Comment_RemoveFromOrder ?? string.Empty,
                        item.Delivery_Price,
                        item.Booking_Price,
                        item.Flag_Inhouse,
                        item.Flag_Payed,
                        item.BookingID,
                        item.Handing
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
        /// Получить заказ по ID
        /// GET: /Orders/GetOrder?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetOrder(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                COrder_ForAdmin order = _bl.Orders.Get_ForAdmin(ID: ID);
                if (order == null)
                    return Json(new { success = false, message = "Заказ не найден" });

                object result = new
                {
                    order.ID,
                    order.Order_FavoriteID,
                    order.Date_Act,
                    order.Order_StatusID,
                    Order_Status = order.Order_Status ?? string.Empty,
                    order.Order_TypeID,
                    Order_Type = order.Order_Type ?? string.Empty,
                    order.ClientID,
                    Client = order.Client ?? string.Empty,
                    Client_Phone = order.Client_Phone ?? string.Empty,
                    order.Name,
                    order.Phone,
                    order.Pay_TypeID,
                    Pay_Type = order.Pay_Type ?? string.Empty,
                    order.RestaurantID,
                    Restaurant = order.Restaurant ?? string.Empty,
                    order.Date_To,
                    order.Sum,
                    Car_Brand = order.Car_Brand ?? string.Empty,
                    Car_RegNumber = order.Car_RegNumber ?? string.Empty,
                    Car_Color = order.Car_Color ?? string.Empty,
                    Comment = order.Comment ?? string.Empty,
                    Comment_AddToOrder = order.Comment_AddToOrder ?? string.Empty,
                    Comment_RemoveFromOrder = order.Comment_RemoveFromOrder ?? string.Empty,
                    order.Delivery_Price,
                    order.Booking_Price,
                    order.Flag_Inhouse,
                    order.Flag_Payed,
                    order.BookingID,
                    order.Handing
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список заказов по статусу
        /// GET: /Orders/GetOrdersByStatus?Order_StatusID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetOrdersByStatus(int Order_StatusID)
        {
            try
            {
                if (Order_StatusID <= 0)
                    return Json(new { success = false, message = "Неверный ID статуса" });

                DateTime date_from = DateTime.Now.Date.AddDays(-30);
                DateTime date_to = DateTime.Now.Date;

                List<COrder_ForAdmin> allOrders = _bl.Orders.GetList_ForAdminDates(Date_From: date_from, Date_To: date_to);

                List<COrder_ForAdmin> filteredOrders = new List<COrder_ForAdmin>();
                foreach (COrder_ForAdmin item in allOrders)
                {
                    if ((int)item.Order_StatusID == Order_StatusID)
                    {
                        filteredOrders.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (COrder_ForAdmin item in filteredOrders)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Order_FavoriteID,
                        item.Date_Act,
                        item.Order_StatusID,
                        Order_Status = item.Order_Status ?? string.Empty,
                        item.Order_TypeID,
                        Order_Type = item.Order_Type ?? string.Empty,
                        item.ClientID,
                        Client = item.Client ?? string.Empty,
                        Client_Phone = item.Client_Phone ?? string.Empty,
                        item.Name,
                        item.Phone,
                        item.Pay_TypeID,
                        Pay_Type = item.Pay_Type ?? string.Empty,
                        item.RestaurantID,
                        Restaurant = item.Restaurant ?? string.Empty,
                        item.Date_To,
                        item.Sum,
                        item.Flag_Inhouse,
                        item.Flag_Payed,
                        item.BookingID
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
        /// Получить детали заказа по ID (список блюд в заказе)
        /// GET: /Orders/Order_Details?OrderID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetOrderDetails(int OrderID)
        {
            try
            {
                if (OrderID <= 0)
                    return Json(new { success = false, message = "Неверный ID заказа" });

                COrder_ForAdmin order = _bl.Orders.Get_ForAdmin(ID: OrderID);
                if (order == null)
                    return Json(new { success = false, message = "Заказ не найден" });

                List<object> orderDatas = new List<object>();
                if (order.List_OrderDatas != null)
                {
                    foreach (COrder_Data_ForAdmin item in order.List_OrderDatas)
                    {
                        List<object> ingredients = new List<object>();
                        if (item.List_OrderDataIngridients != null)
                        {
                            foreach (COrder_Data_Ingridient_ForAdmin ingr in item.List_OrderDataIngridients)
                            {
                                ingredients.Add(new
                                {
                                    ingr.ID,
                                    ingr.Order_DataID,
                                    ingr.IngridientID,
                                    IngridientName = ingr.Ingridient ?? string.Empty,
                                    ingr.Ingridient_Price,
                                    ingr.Ingridient_Count,
                                    ingr.Ingridient_Summ
                                });
                            }
                        }

                        orderDatas.Add(new
                        {
                            item.ID,
                            item.OrderID,
                            item.DishID,
                            DishName = item.Dish_Name ?? string.Empty,
                            item.Dish_Price,
                            item.Dish_Count,
                            item.Dish_Summ,
                            item.Dish_ColorID,
                            Dish_Color = item.Dish_Color ?? string.Empty,
                            item.Dish_VolumeID,
                            item.Dish_VolumeValue,
                            item.Dish_VolumePrice,
                            item.Bonuses_Minus,
                            item.Bonuses_Plus,
                            Ingredients = ingredients
                        });
                    }
                }

                object result = new
                {
                    order.ID,
                    order.Order_FavoriteID,
                    order.Date_Act,
                    order.Order_StatusID,
                    Order_Status = order.Order_Status ?? string.Empty,
                    order.Order_TypeID,
                    Order_Type = order.Order_Type ?? string.Empty,
                    order.ClientID,
                    Client = order.Client ?? string.Empty,
                    Client_Phone = order.Client_Phone ?? string.Empty,
                    order.Name,
                    order.Phone,
                    order.Pay_TypeID,
                    Pay_Type = order.Pay_Type ?? string.Empty,
                    order.RestaurantID,
                    Restaurant = order.Restaurant ?? string.Empty,
                    order.Date_To,
                    order.Sum,
                    Car_Brand = order.Car_Brand ?? string.Empty,
                    Car_RegNumber = order.Car_RegNumber ?? string.Empty,
                    Car_Color = order.Car_Color ?? string.Empty,
                    Comment = order.Comment ?? string.Empty,
                    Comment_AddToOrder = order.Comment_AddToOrder ?? string.Empty,
                    Comment_RemoveFromOrder = order.Comment_RemoveFromOrder ?? string.Empty,
                    order.Delivery_Price,
                    order.Booking_Price,
                    order.Flag_Inhouse,
                    order.Flag_Payed,
                    order.BookingID,
                    order.Handing,
                    List_OrderDatas = orderDatas
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion


        /// <summary>
        /// Список заказов
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(string? Date_From, string? Date_To)
        {
            Model_Orders_List model = new();

            DateTime? date_from = HIDE_Convert.ToDateTime_Nullable(Date_From);
            if (!date_from.HasValue)
            {
                date_from = DateTime.Now.Date.AddDays(-30);
            }

            DateTime? date_to = HIDE_Convert.ToDateTime_Nullable(Date_To);
            if (!date_to.HasValue)
            {
                date_to = DateTime.Now.Date;
            }

            model.Date_From = date_from.Value;
            model.Date_To = date_to.Value;

            model.List_Orders = _bl.Orders.GetList_ForAdminDates(Date_From: date_from, Date_To: date_to);
            return View(model: model);
        }


        /// <summary>
        /// Таблица > Список заказов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Orders_Part_Table(string? Date_From, string? Date_To)
        {
            return Index(Date_From: Date_From, Date_To: Date_To);
        }


        #region Добавление

        /// <summary>
        /// Добавление заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            Model_Orders_AddEdit model = new()
            {
                //List_Pay_Types = _bl.Pay_Types.GetList(),
                //DishID = DishID,
            };

            return View(model);
        }

        /// <summary>
        /// Добавление заказа
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Orders_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                int? order_id = 1;

                //int? order_id = _bl.Orders.Add(ClientID: data.ClientID, Name: "", Phone: "", Pay_TypeID: data.Pay_TypeID, RestaurantID: data.RestaurantID,
                //                               Date_To: data.Date_To, Flag_Inhouse: data.Flag_Inhouse, List_OrderData: []); //TODO: KA - расставить соответствующие поля

                if (order_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = order_id });
                }

                return RedirectToAction("Index"); //Возвращаем к списку заказов
            }

            return View();
        }

        #endregion


        #region Изменение

        /// <summary>
        /// Изменение заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            COrder? order = _bl.Orders.Get(ID: ID);
            if (order != null)
            {
                Model_Orders_AddEdit data = new()
                {
                    ID = order.ID,
                    ClientID = order.ClientID,
                    Pay_TypeID = (int)order.Pay_TypeID,
                    RestaurantID = order.RestaurantID,
                    Date_To = order.Date_To,
                    Flag_Inhouse = order.Flag_Inhouse,
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }
        }

        /// <summary>
        /// Изменение заказа
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Orders_AddEdit Data)
        {
            if (Data != null)
            {
                //int? order_id = _bl.Orders.Edit(ClientID: data.ClientID, ClientToken: data.ClientToken, Pay_TypeID: data.Pay_TypeID, RestaurantID: data.RestaurantID,
                //Date_To: data.Date_To, Flag_Inhouse: data.Flag_Inhouse)?.ID;

                int? order_id = 1;

                if (order_id > 0)
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
        /// Удаление заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                COrder? order = _bl.Orders.Get(ID: ID);

                if (order != null)
                {
                    Model_Orders_AddEdit data = new()
                    {
                        ID = order.ID
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
        /// Удаление заказа
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Orders_AddEdit Data)
        {
            if (Data != null)
            {
                int order_id = _bl.Orders.Del(ID: Data.ID, UserID: UserID);

                if (order_id > 0)
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


        #region Возврат

        /// <summary>
        /// Возврат
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Order_Refund(int ID)
        {
            if (ID > 0)
            {
                COrder? order = _bl.Orders.Get(ID: ID);

                if (order != null)
                {
                    Model_Orders_AddEdit data = new()
                    {
                        ID = order.ID
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
        /// Возврат
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Order_Refund(Model_Orders_AddEdit Data)
        {
            if (Data != null)
            {
                COrder? cOrder = _bl.Orders.Get(Data.ID);

                int result = 0;

                // Если статус изменен на "Отменить", возвращаются средства
                if (cOrder!.Order_StatusID != EOrder_Status.Cancelled && cOrder!.Order_StatusID != EOrder_Status.Complete && cOrder.Pay_TypeID == EPayType.CardInApp)
                {
                    result = _bl.Orders.Cancel_Future_Refund(ClientID: cOrder.ClientID, OrderID: cOrder.ID);

                    if (result <= 0)
                    {

                        if (_log.IsEnabled(LogLevel.Critical))
                        {
                            _log.LogCritical(message: "{}", args: $"# ADM # Не удалось вернуть средства для клиента {Data.ClientID} при отмене заказа {Data.ID}");
                        }

                    }
                }

                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(key: "Client", errorMessage: "Не удалось вернуть средства для клиента");
                    return View(model: Data);
                }
            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }

        #endregion Возврат


        #region Изменение статуса заказа

        /// <summary>
        /// Изменение статуса заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit_Status(int ID)
        {
            COrder? order = _bl.Orders.Get(ID: ID);
            if (order != null)
            {
                Model_Orders_EditStatus data = new()
                {
                    ID = order.ID,
                    Flag_Payed = order.Flag_Payed,
                    Order_StatusID = order.Order_StatusID,
                    List_Order_Status = _bl.OrderStatuses.GetList()
                };

                return View(model: data);
            }

            return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
        }

        /// <summary>
        /// Изменение статуса заказа
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit_Status(Model_Orders_EditStatus data)
        {
            if (data != null)
            {
                int? client_id = _bl.Orders.Get(ID: data.ID)?.ClientID;
                COrder? order = _bl.Orders.Get(ID: data.ID);
                EOrder_Status? order_statusID = order?.Order_StatusID;
                int? order_id = _bl.Orders.Edit_Status(ID: data.ID, Order_StatusID: data.Order_StatusID, Flag_Payed: data.Flag_Payed);
                string? restaurant = _bl.Restaurants.GetList()?.FirstOrDefault()?.Address;

                if (order_id > 0)
                {
                    if (order_statusID != data.Order_StatusID)
                    {
                        switch (data.Order_StatusID)
                        {
                            //case EOrder_Status.New:
                            //    _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.New,
                            //                                           ReHIDEText1: order_id.ToString());
                            //    break;
                            case EOrder_Status.Cancelled:
                                _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.Cancelled,
                                                                       ReHIDEText1: order_id.ToString(), OrderID: order_id.Value);

                                if (order != null && order.BookingID.HasValue)
                                {
                                    _bl.Bookings.Del(ID: order.BookingID.Value, UserID: UserID);
                                }

                                break;
                            case EOrder_Status.Cooking:
                                _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.Cooking,
                                                                       ReHIDEText1: order_id.ToString(), OrderID: order_id.Value);
                                break;
                            case EOrder_Status.ReadyToClient:
                                _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.ReadyToClient,
                                                                       ReHIDEText1: order_id.ToString(), ReHIDEText2: restaurant, OrderID: order_id.Value);
                                break;
                            case EOrder_Status.Delivery:
                                _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.Delivery,
                                                                       ReHIDEText1: order_id.ToString(), OrderID: order_id.Value);
                                break;
                            case EOrder_Status.Complete:
                                _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.Complete,
                                                                       ReHIDEText1: order_id.ToString(), OrderID: order_id.Value);
                                break;
                            default:

                                break;
                        }
                    }

                    return RedirectToAction("Index");
                }

                return View(model: data);
            }

            return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
        }

        #endregion


        /// <summary>
        /// Детали заказа
        /// </summary>
        /// <returns></returns>
        public IActionResult Order_Details(int OrderID)
        {
            if (OrderID <= 0)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }

            COrder_ForAdmin? order = _bl.Orders.Get_ForAdmin(ID: OrderID);

            //List<COrder_Data_ForAdmin>? list_OrderDatas = [];
            //if (order != null)
            //{
            //    list_OrderDatas = order.List_OrderDatas;
            //}

            return View(model: order);
        }


        /// <summary>
        /// Таблица > Список деталей заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Order_Details_Part_Table(int OrderID)
        {
            return Order_Details(OrderID: OrderID);
        }


        /// <summary>
        /// Детали заказа > Бронирование
        /// </summary>
        /// <returns></returns>
        public IActionResult Order_Booking(int BookingID, string? Booking_Price)
        {
            if (BookingID <= 0)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }

            CBooking? order_booking = _bl.Bookings.Get(BookingID);

            if (order_booking == null)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }

            string? type = _bl.Restaurant_Tables.Get(ID: order_booking.Restaurant_TableID)?.Restaurant_Table_Type;
            string? restaurant = _bl.Restaurant_Tables.Get(ID: order_booking.Restaurant_TableID)?.Restaurant;

            Model_Order_Booking model = new()
            {
                Booking = order_booking,
                Booking_Price = HIDE_Convert.ToDecimal_Nullable(Booking_Price),
                Restaurant = restaurant,
                Restaurant_Table_Type = type
            };

            return View(model: model);
        }


        /// <summary>
        /// Детали заказа > Доставка
        /// </summary>
        /// <returns></returns>
        public IActionResult Order_Delivery(int OrderID)
        {
            if (OrderID <= 0)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }

            COrder_Delivery? order_delivery = _bl.Order_Deliveries.GetByOrderID(OrderID);

            COrder_ForAdmin? order = _bl.Orders.Get_ForAdmin(ID: OrderID);

            decimal? order_summ = 0;

            if (order != null && order.Delivery_Price != null)
            {
                order_summ += order.Delivery_Price;
            }

            if (order != null && order.List_OrderDatas != null && order.List_OrderDatas.Count != 0)
            {
                foreach (var elem in order.List_OrderDatas)
                {
                    if (elem.OrderID == OrderID)
                    {
                        order_summ += elem.Dish_Summ;
                    }
                }
            }

            Model_Order_Delivery model = new()
            {
                OrderID = OrderID,
            };

            if (order_delivery != null)
            {
                model = new()
                {
                    OrderID = OrderID,
                    Delivery_Address = order_delivery.Delivery_Address,
                    Delivery_EntranceCode = order_delivery.Delivery_EntranceCode,
                    Delivery_EntranceNumber = order_delivery.Delivery_EntranceNumber,
                    Delivery_Floor = order_delivery.Delivery_Floor,
                    Delivery_Comment = order!.Comment_AddToOrder,
                    Delivery_Comment_Remove = order.Comment_RemoveFromOrder,
                    Delivery_Handing = order_delivery.Delivery_Handing,
                    Delivery_To = order_delivery.Delivery_To,
                    Delivery_Price = order_summ,

                };
            }




            return View(model: model);
        }


        /// <summary>
        /// Детали заказа > Ингридиенты блюда
        /// </summary>
        /// <returns></returns>
        public IActionResult Order_Details_Ingridients(int OrderID, int OrderDataID)
        {
            if (OrderID <= 0)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }

            COrder_ForAdmin? order = _bl.Orders.Get_ForAdmin(ID: OrderID);
            List<COrder_Data_Ingridient_ForAdmin>? list_OrderDataIngridients = [];

            if (order != null && order.List_OrderDatas != null && order.List_OrderDatas.Count != 0)
            {
                foreach (var elem in order.List_OrderDatas)
                {
                    if (elem.ID == OrderDataID)
                    {
                        if (elem.List_OrderDataIngridients != null && elem.List_OrderDataIngridients.Count != 0)
                        {
                            foreach (var item in elem.List_OrderDataIngridients)
                            {
                                list_OrderDataIngridients.Add(item);
                            }
                        }
                    }
                }
            }

            Model_Order_Details_Ingridients model = new()
            {
                OrderID = OrderID,
                OrderDataID = OrderDataID,
                List_OrderDataIngridients = list_OrderDataIngridients
            };

            return View(model: model);
        }


        /// <summary>
        /// Таблица > Список деталей заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Order_Details_Ingridients_Part_Table(int OrderID, int OrderDataID)
        {
            return Order_Details_Ingridients(OrderID: OrderID, OrderDataID: OrderDataID);
        }


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

        #endregion Получение Флага > Админ

    }
}