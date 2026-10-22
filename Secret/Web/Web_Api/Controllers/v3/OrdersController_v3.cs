using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Web_Api.Auth;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v3._Common;
using HIDE.RestAndRant.Web_Api.Models.v3._Enum;
using HIDE.RestAndRant.Web_Api.Models.v3.Orders;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Api.Controllers.v3
{
    /// <summary>
    /// Контроллер - Заказы
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("Orders_v3 - Заказы")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/Orders/[action]")]
    [ApiController]
    [BasicAuth]
    public class OrdersController_v3(ILogger<OrdersController_v3> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает дату последнего изменения заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetOrdersChangeDate()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            // Обновление даты для активных заказов
            _bl.Orders.GetLast(ClientID: client.ID, Minuts_AddCompleted: 1);

            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Orders_DateChange);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Возвращает дату последнего изменения избранного заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DateTime?))]
        public JsonResult GetOrdersFavoriteChangeDate()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            DateTime? date_max = _bl.Order_Favorites.GetMaxLastEditDate(ClientID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: date_max);
        }


        /// <summary>
        /// Получает данные обо всех заказах
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<COrder>))]
        public JsonResult GetOrders()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            List<COrder>? list_orders = _bl.Orders.GetList(ClientID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_orders);
        }


        /// <summary>
        /// Получает данные об активных заказах
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<COrder>))]
        public JsonResult GetActiveOrders()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            List<COrder>? list_orders = _bl.Orders.GetList_Active(ClientID: client.ID, Minuts_AddCompleted: 1);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_orders);
        }


        /// <summary>
        /// Получает данные об истории заказов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<COrder>))]
        public JsonResult GetOrderHistory()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            List<COrder>? list_orders = _bl.Orders.GetList_History(ClientID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_orders);
        }


        /// <summary>
        /// Получает данные об избранных заказах
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<COrder_Favorite>))]
        public JsonResult GetFavoriteOrders()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            List<COrder_Favorite>? list_favorites_orders = _bl.Order_Favorites.GetList(ClientID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_favorites_orders);
        }


        /// <summary>
        /// Получает данные о популярных заказах
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<COrder_Favorite>))]
        [Obsolete]
        public JsonResult GetPopularOrders()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            List<COrder_Favorite>? list_favorites_orders = _bl.Order_Favorites.GetList(ClientID: client.ID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_favorites_orders);
        }


        /// <summary>
        /// Получает данные о заказах для блока "Сделать как обычно"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Model_Orders_AsUsual_v3))]
        [Obsolete]
        public JsonResult GetAsUsualOrders()
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            Model_Orders_AsUsual_v3 model = new()
            {
                List_Orders = _bl.Orders.GetList(ClientID: client.ID)?.Take(1).ToList(),
                List_Favorites = _bl.Order_Favorites.GetList(ClientID: client.ID)
            };

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: model);
        }


        /// <summary>
        /// Добавление нового заказа(без оплаты в приложении)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Type = typeof(int), Description = "Код добавленного заказа")]
        public JsonResult OrderAdd(Model_Orders_AddOrder_v3 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            DateTime? date_to = DateTime.Now;
            if (Data.Date_To_Unix.HasValue)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(Data.Date_To_Unix.Value);
                date_to = dateTimeOffset.DateTime;
            }
            else
            {
                if (Data.Date_To != null)
                {
                    date_to = Data.Date_To;
                }
            }

            DateTime? date_BookingStart = DateTime.Now;
            if (Data.Date_BookingStart_Unix.HasValue)
            {
                DateTimeOffset dateTimeOffsetBookingStart = DateTimeOffset.FromUnixTimeMilliseconds(Data.Date_BookingStart_Unix.Value);
                date_BookingStart = dateTimeOffsetBookingStart.DateTime;
            }
            else
            {
                if (Data.Date_BookingStart != null)
                {
                    date_BookingStart = Data.Date_BookingStart;
                }
            }

            DateTime? date_BookingEnd = DateTime.Now;
            if (Data.Date_BookingEnd_Unix.HasValue)
            {
                DateTimeOffset dateTimeOffsetBookingEnd = DateTimeOffset.FromUnixTimeMilliseconds(Data.Date_BookingEnd_Unix.Value);
                date_BookingEnd = dateTimeOffsetBookingEnd.DateTime;
            }
            else
            {
                if (Data.Date_BookingEnd != null)
                {
                    date_BookingEnd = Data.Date_BookingEnd;
                }
            }

            int order_id = _bl.Orders.AddMoney(ClientID: client.ID, Name: Data.Name, Phone: Data.Phone, Pay_TypeID: Data.Pay_TypeID,
                                               RestaurantID: Data.RestaurantID, Date_To: date_to, Flag_Inhouse: Data.Flag_Inhouse,
                                               List_OrderData: Data.List_OrderData, Order_TypeID: Data.Order_TypeID, Comment: Data.Comment,
                                               Delivery_Address: Data.Delivery_Address, Delivery_Apartment: Data.Delivery_Apartment,
                                               Delivery_EntranceCode: Data.Delivery_EntranceCode, Delivery_EntranceNumber: Data.Delivery_EntranceNumber,
                                               Delivery_Floor: Data.Delivery_Floor, Delivery_Price: Data.Delivery_Price,
                                               Restaurant_TableID: Data.Restaurant_TableID, Date_BookingStart: date_BookingStart, Date_BookingEnd: date_BookingEnd,
                                               Handing: Data.Handing, Car_Brand: Data.Car_Brand, Car_RegNumber: Data.Car_RegNumber, Car_Color: Data.Car_Color,
                                               Comment_AddToOrder: Data.Comment_AddToOrder, Comment_RemoveFromOrder: Data.Comment_RemoveFromOrder,
                                               Booking_Price: Data.Booking_Price);

            if (order_id < 0)
            {

                _log.LogError(message: "{}", args: $"Ошибка добавления заказа для клиента {client.ID}");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: order_id, Message: Errors.GetErrorString(ErrorVal: order_id));
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: order_id);
        }


        /// <summary>
        /// Добавление нового заказа(оплаченный)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Type = typeof(int), Description = "Код добавленного заказа")]
        public JsonResult OrderAddPayment(Model_Orders_AddOrder_Card_v3 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            CClientPay_Card_Data? card_data = _bl.ClientPay_Cards.Get_Card(ClientID: client.ID);

            if (card_data == null)
            {
                _log.LogError(message: "Не удалось найти карту клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)Errors.Err201);
            }

            DateTime? date_to = DateTime.Now;
            if (Data.Date_To_Unix.HasValue)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(Data.Date_To_Unix.Value);
                date_to = dateTimeOffset.DateTime;
            }
            else
            {
                if (Data.Date_To != null)
                {
                    date_to = Data.Date_To;
                }
            }

            DateTime? date_BookingStart = DateTime.Now;
            if (Data.Date_BookingStart_Unix.HasValue)
            {
                DateTimeOffset dateTimeOffsetBookingStart = DateTimeOffset.FromUnixTimeMilliseconds(Data.Date_BookingStart_Unix.Value);
                date_BookingStart = dateTimeOffsetBookingStart.DateTime;
            }
            else
            {
                if (Data.Date_BookingStart != null)
                {
                    date_BookingStart = Data.Date_BookingStart;
                }
            }

            DateTime? date_BookingEnd = DateTime.Now;
            if (Data.Date_BookingEnd_Unix.HasValue)
            {
                DateTimeOffset dateTimeOffsetBookingEnd = DateTimeOffset.FromUnixTimeMilliseconds(Data.Date_BookingEnd_Unix.Value);
                date_BookingEnd = dateTimeOffsetBookingEnd.DateTime;
            }
            else
            {
                if (Data.Date_BookingEnd != null)
                {
                    date_BookingEnd = Data.Date_BookingEnd;
                }
            }

            int order_id = _bl.Orders.Add(ClientID: client.ID, Name: Data.Name, Phone: Data.Phone, Pay_TypeID: Data.Pay_TypeID, CardID: card_data.ID,
                                          Summ: Data.Summ, RestaurantID: Data.RestaurantID, Date_To: date_to, Flag_Inhouse: Data.Flag_Inhouse,
                                          PaySystemIdent: Data.PaySystemIdent, List_OrderData: Data.List_OrderData, Order_TypeID: Data.Order_TypeID,
                                          Delivery_Apartment: Data.Delivery_Apartment, Delivery_Address: Data.Delivery_Address,
                                          Delivery_EntranceCode: Data.Delivery_EntranceCode, Delivery_EntranceNumber: Data.Delivery_EntranceNumber,
                                          Delivery_Floor: Data.Delivery_Floor, Delivery_Price: Data.Delivery_Price, Car_Brand: Data.Car_Brand,
                                          Restaurant_TableID: Data.Restaurant_TableID, Date_BookingStart: date_BookingStart, Date_BookingEnd: date_BookingEnd,
                                          Car_RegNumber: Data.Car_RegNumber, Car_Color: Data.Car_Color, Comment: Data.Comment,
                                          Comment_AddToOrder: Data.Comment_AddToOrder, Comment_RemoveFromOrder: Data.Comment_RemoveFromOrder,
                                          Booking_Price: Data.Booking_Price);

            if (order_id < 0)
            {

                _log.LogError(message: "{}", args: $"Ошибка добавления заказа для клиента {client.ID}");

                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: order_id, Message: Errors.GetErrorString(ErrorVal: order_id));
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: order_id);
        }


        /// <summary>
        /// Редактирование избранного заказа
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Type = typeof(int), Description = "Код измененного избранного заказа")]
        public JsonResult Order_FavoriteEdit(Model_Order_Favorites_Edit_v3 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            int order_favorite_id = _bl.Order_Favorites.Edit(ID: Data.ID, ClientID: client.ID, Name: Data.Name, Phone: Data.Phone, Pay_TypeID: Data.Pay_TypeID,
                                                             RestaurantID: Data.RestaurantID, Date_To: Data.Date_To, Flag_Inhouse: Data.Flag_Inhouse,
                                                             Order_StatusID: EOrder_Status.Complete, List_Order_Favorite_Datas: Data.List_Order_Favorite_Datas);

            if (order_favorite_id < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: order_favorite_id, Message: Errors.GetErrorString(ErrorVal: order_favorite_id));
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: order_favorite_id);
        }


        /// <summary>
        /// Добавление заказа в избранное
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Type = typeof(int), Description = "Код добавленного в избранное заказа")]
        public JsonResult Order_FavoriteAdd(Model_Order_Favorites_Add_v3 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            COrder? order = _bl.Orders.Get(Data.OrderID);

            if (order == null || order != null && order.Order_FavoriteID.HasValue)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: Errors.Err200, Message: Errors.GetErrorString(ErrorVal: Errors.Err200));
            }

            int order_favorite_id = _bl.Order_Favorites.Add(OrderID: Data.OrderID);

            if (order_favorite_id < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: order_favorite_id, Message: Errors.GetErrorString(ErrorVal: order_favorite_id));
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: order_favorite_id);
        }


        /// <summary>
        /// Удаление заказа из избранного
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Type = typeof(int), Description = "Код удаленного из избранного заказа")]
        public JsonResult Order_FavoriteDelete(Model_Order_Favorites_Del_v3 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            int order_favorite_id = _bl.Order_Favorites.Del(OrderID: Data.OrderFavoriteID);

            if (order_favorite_id < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: order_favorite_id, Message: Errors.GetErrorString(ErrorVal: order_favorite_id));
            }

            int order_favorite_del = _bl.Orders.Delete_Order_Favorite(Order_FavoriteID: order_favorite_id);

            if (order_favorite_del < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: order_favorite_id, Message: Errors.GetErrorString(ErrorVal: order_favorite_id));
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: order_favorite_id);
        }


        /// <summary>
        /// Отмена заказа
        /// </summary>
        /// <param name="Data">Код заказа</param>
        /// <returns>Код заказа или код ошибки</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult Order_Cancel(Model_Common_Int_v3 Data)
        {
            CCommon? client = Helper_Client.GetClient(HttpContext.User);
            if (client == null)
            {
                _log.LogWarning(message: "Не удалось идентифицировать клиента");
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v3.Err);
            }

            // Получаем заказ
            COrder? order = _bl.Orders.Get(Data.Data_Int);

            if (order == null || order.Order_StatusID != EOrder_Status.New)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: Errors.Err200, Message: Errors.Err200_str);
            }

            int result;

            result = _bl.Orders.Cancel(ID: Data.Data_Int, ClientID: client.ID);

            //if (order.Pay_TypeID == EPayType.CardInApp)
            //{
            //    // Если оплачен картой, то возвращаем средства
            //    result = _bl.Orders.Cancel_Future_Refund(ClientID: client.ID, OrderID: Data.Data_Int);
            //}
            //else
            //{
            //    result = _bl.Orders.Cancel(ID: Data.Data_Int, ClientID: client.ID);
            //}

            if (result < 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: Errors.Err203, Message: Errors.Err203_str);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: result);
        }


        /// <summary>
        /// Возвращает возможных статусов заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CCommon_Ordered>))]
        public JsonResult GetOrderStatusesList()
        {
            List<CCommon_Ordered>? list_result = _bl.OrderStatuses.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Возвращает возможных способов получения заказа(С собой\Поесть в кафе\Доставка\Вынести к машине)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CCommon>))]
        public JsonResult GetOrderTypesList()
        {
            List<CCommon>? list_result = _bl.Order_Types.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Возвращает сумму при которой доставка становится бесплатной
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(decimal))]
        public JsonResult GetDelivery_PriceFree()
        {
            decimal? val = HIDE_Convert.ToDecimal(_bl.Setups.Get(Key: Consts.SetupKey_Dop_Delivery_PriceFree)?.Val);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }

    }
}