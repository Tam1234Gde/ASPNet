using Microsoft.Extensions.Logging;
using Telegram.Bot;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Business.Helpers;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Работа с заказами
    /// </summary>
    public class Orders
    {
        private readonly ILogger _log;

        private readonly Order_oper _dl_Order = new();
        private readonly ClientPay_Card_Pay_oper _dl_ClientPay_Card_Pay = new();
        private readonly Order_Data_oper _dl_Order_Data = new();
        private readonly Setups _bl_Setup = new();
        private readonly Client_Notifications _bl_Client_Notifications = new();
        private readonly Dishes _bl_Dishes = new();
        private readonly Bonuses _bl_Bonuses = new();
        private readonly Bookings _bl_Bookings = new();
        private readonly Restaurant_Tables _bl_Restaurant_Tables = new();
        private readonly Clients _bl_Clients = new();
        private readonly Order_Deliveries _bl_Order_Deliveries = new();
        private readonly Order_Data_Ingridient_oper _dl_Order_Data_Ingridient = new();
        private readonly User_oper _dl_User = new();
        private readonly Helper_Telegram _h_Telegram = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Orders()
        {
            _log = AppLog.CreateLogger(GetType());
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder? Get_Order(Order? Elem)
        {
            if (Elem != null)
            {
                COrder item = new()
                {
                    ID = Elem.ID,
                    ClientID = Elem.ClientID,
                    Name = Elem.Name,
                    Phone = Elem.Phone,
                    Pay_TypeID = Elem.Pay_TypeID,
                    RestaurantID = Elem.RestaurantID,
                    Date_To = Elem.Date_To,
                    Date_Act = Elem.Date_Act,
                    Date_LastOrderStatusChange = Elem.Date_LastOrderStatusChange,
                    Flag_Inhouse = Elem.Flag_Inhouse,
                    Flag_Payed = Elem.Flag_Payed,
                    Order_StatusID = Elem.Order_StatusID,
                    Order_FavoriteID = Elem.Order_FavoriteID,
                    Order_TypeID = Elem.Order_TypeID,
                    Delivery_Price = Elem.Delivery_Price,
                    Delivery_Address = Elem.Delivery_Address,
                    Booking_Price = Elem.Booking_Price,
                    BookingID = Elem.BookingID,
                    Handing = Elem.Handing,
                    Car_RegNumber = Elem.Client?.Car_RegNumber,
                };

                if (Elem.Order_Datas != null && Elem.Order_Datas.Count != 0)
                {
                    List<COrder_Data> list_datas = [];

                    foreach (Order_Data elem_detail in Elem.Order_Datas)
                    {
                        COrder_Data? item_detail = Get_OrderData(elem_detail);
                        if (item_detail != null/* && (item_detail.Bonuses_Minus == null || item_detail.Bonuses_Minus == 0)*/)
                        {
                            list_datas.Add(item_detail);
                        }
                    }

                    item.List_OrderDatas = list_datas;
                }

                if (Elem.Booking != null)
                {
                    CBooking? item_detail = Get_Booking(Elem.Booking);
                    if (item_detail != null)
                    {
                        item_detail.Date_Start = item_detail.Date_Start.AddMilliseconds(1); // Кастыль добавлен по требованию техлида(@ed)
                        item_detail.Date_End = item_detail.Date_End.AddMilliseconds(1); // Кастыль добавлен по требованию техлида(@ed)
                        item.Booking = item_detail;
                    }
                }

                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_ForAdmin? Get_Order_ForAdmin(Order? Elem)
        {
            if (Elem != null)
            {
                COrder_ForAdmin item = new()
                {
                    ID = Elem.ID,
                    Date_Act = Elem.Date_Act,
                    Order_StatusID = Elem.Order_StatusID,
                    Order_Status = Elem.Order_Status.Name,
                    ClientID = Elem.ClientID,
                    Client = Elem.Client?.Name,
                    Client_Phone = Elem.Client?.Phone,
                    Client_Photo = Elem.Client?.Photo,
                    Name = Elem.Name,
                    Phone = Elem.Phone,
                    Comment = Elem.Comment,
                    Comment_AddToOrder = Elem.Comment_AddToOrder,
                    Comment_RemoveFromOrder = Elem.Comment_RemoveFromOrder,
                    Pay_TypeID = Elem.Pay_TypeID,
                    Pay_Type = Elem.Pay_Type.Name,
                    RestaurantID = Elem.RestaurantID,
                    Restaurant = Elem.Restaurant.Adress,
                    Date_To = Elem.Date_To,
                    Flag_Inhouse = Elem.Flag_Inhouse,
                    Flag_Payed = Elem.Flag_Payed,
                    Order_FavoriteID = Elem.Order_FavoriteID,
                    Order_TypeID = Elem.Order_TypeID,
                    //Order_Type = Elem.Order_Type.Name,
                    Delivery_Price = Elem.Delivery_Price,
                    Booking_Price = Elem.Booking_Price,
                    BookingID = Elem.BookingID,
                    Handing = Elem.Handing,
                    Car_Brand = Elem.Client?.Car_Brand,
                    Car_Color = Elem.Client?.Car_Color,
                    Car_RegNumber = Elem.Client?.Car_RegNumber,
                };

                if (Elem.Order_Datas != null && Elem.Order_Datas.Count != 0)
                {
                    List<COrder_Data_ForAdmin> list_datas = [];

                    foreach (Order_Data elem_detail in Elem.Order_Datas)
                    {
                        COrder_Data_ForAdmin? item_detail = Get_OrderData_ForAdmin(elem_detail);
                        if (item_detail != null)
                        {
                            list_datas.Add(item_detail);
                        }
                    }

                    item.List_OrderDatas = list_datas;
                }

                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных заказа
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Data? Get_OrderData(Order_Data? Elem)
        {
            if (Elem != null)
            {
                COrder_Data item = new()
                {
                    ID = Elem.ID,
                    OrderID = Elem.OrderID,
                    DishID = Elem.DishID,
                    Dish_ColorID = Elem.Dish_ColorID,
                    Dish_Price = Elem.Dish_Price,
                    Dish_Count = Elem.Dish_Count,
                    Dish_Summ = Elem.Dish_Summ,
                    Dish_Name = Elem.Dish_Name,
                    Dish_VolumeID = Elem.Dish_VolumeID,
                    Dish_VolumeValue = Elem.Dish_VolumeValue,
                    Dish_VolumePrice = Elem.Dish_VolumePrice,
                    Flag_Del = Elem.Dish?.Flag_Del ?? false,
                    Bonuses_Plus = Elem.Bonuses_Plus,
                    Bonuses_Minus = Elem.Bonuses_Minus,
                };

                if (Elem.Order_Data_Ingridients != null && Elem.Order_Data_Ingridients.Count != 0)
                {
                    List<COrder_Data_Ingridient> list_ingridients = [];

                    foreach (Order_Data_Ingridient elem_detail in Elem.Order_Data_Ingridients)
                    {
                        COrder_Data_Ingridient? item_detail = Get_OrderDataIngridient(elem_detail);
                        if (item_detail != null)
                        {
                            list_ingridients.Add(item_detail);
                        }
                    }
                    item.List_Dish_Ingridients = list_ingridients;
                }

                return item;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Получение данных доставки
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Delivery? Get_OrderDelivery(Order_Delivery? Elem)
        {
            if (Elem != null)
            {
                COrder_Delivery item = new()
                {
                    ID = Elem.ID,
                    OrderID = Elem.OrderID,
                    Delivery_Address = Elem.Delivery_Address,
                    Delivery_Comment = Elem.Delivery_Comment,
                    Delivery_EntranceCode = Elem.Delivery_EntranceCode,
                    Delivery_EntranceNumber = Elem.Delivery_EntranceNumber,
                    Delivery_Floor = Elem.Delivery_Floor,
                    Delivery_Handing = Elem.Delivery_Handing,
                    Delivery_To = Elem.Delivery_To,
                };
                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных заказа
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CBooking? Get_Booking(Booking? Elem)
        {
            if (Elem != null)
            {
                CBooking item = new()
                {
                    ID = Elem.ID,
                    ClientID = Elem.ClientID,
                    Client = Elem.Client?.Phone,
                    Date_Action = Elem.Date_Action,
                    Date_Start = Elem.Date_Start,
                    Date_End = Elem.Date_End,
                    Phone = Elem.Phone,
                    Name = Elem.Name,
                    Restaurant_Table = Elem.Restaurant_Table?.Name,
                    Restaurant_TableID = Elem.Restaurant_TableID,
                    Restaurant_Table_Type = Elem.Restaurant_Table?.Restaurant_Table_Type?.Name
                };

                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных заказа
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Data_ForAdmin? Get_OrderData_ForAdmin(Order_Data? Elem)
        {
            if (Elem != null)
            {
                COrder_Data_ForAdmin item = new()
                {
                    ID = Elem.ID,
                    OrderID = Elem.OrderID,
                    DishID = Elem.DishID,
                    Dish = Elem.Dish.Name,
                    Dish_ColorID = Elem.Dish_ColorID,
                    Dish_Color = Elem.Dish_Color?.Color.Name,
                    Dish_Price = Elem.Dish_Price,
                    Dish_Count = Elem.Dish_Count,
                    Dish_Summ = Elem.Dish_Summ,
                    Dish_Name = Elem.Dish_Name,
                    Dish_VolumeID = Elem.Dish_VolumeID,
                    Dish_VolumeValue = Elem.Dish_VolumeValue,
                    Dish_VolumePrice = Elem.Dish_VolumePrice,
                    Bonuses_Minus = Elem.Bonuses_Minus,
                    Bonuses_Plus = Elem.Bonuses_Plus,
                    Dish_Image = Elem.Dish?.Image
                };

                if (Elem.Order_Data_Ingridients != null && Elem.Order_Data_Ingridients.Count != 0)
                {
                    List<COrder_Data_Ingridient_ForAdmin> list_ingridients = [];

                    foreach (Order_Data_Ingridient elem_detail in Elem.Order_Data_Ingridients)
                    {
                        COrder_Data_Ingridient_ForAdmin? item_detail = Get_OrderDataIngridient_ForAdmin(elem_detail);
                        if (item_detail != null)
                        {
                            list_ingridients.Add(item_detail);
                        }
                    }
                    item.List_OrderDataIngridients = list_ingridients;
                }

                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение ингридиента
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Data_Ingridient? Get_OrderDataIngridient(Order_Data_Ingridient? Elem)
        {
            if (Elem != null)
            {
                COrder_Data_Ingridient item = new()
                {
                    Order_DataID = Elem.Order_DataID,
                    IngridientID = Elem.IngridientID,
                    Ingridient_Price = Elem.Ingridient_Price,
                    Ingridient_Count = Elem.Ingridient_Count,
                    Ingridient_Summ = Elem.Ingridient_Summ,
                    Ingridient_Name = Elem.Ingridient_Name,
                    Flag_Del = Elem.Ingridient?.Flag_Del ?? false
                };

                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение ингридиента
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Data_Ingridient_ForAdmin? Get_OrderDataIngridient_ForAdmin(Order_Data_Ingridient? Elem)
        {
            if (Elem != null)
            {
                COrder_Data_Ingridient_ForAdmin item = new()
                {
                    ID = Elem.ID,
                    Order_DataID = Elem.Order_DataID,
                    IngridientID = Elem.IngridientID,
                    Ingridient = Elem.Ingridient.Name,
                    Ingridient_Price = Elem.Ingridient_Price,
                    Ingridient_Count = Elem.Ingridient_Count,
                    Ingridient_Summ = Elem.Ingridient_Summ,
                };

                return item;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public COrder? Get(int ID)
        {
            return Get_Order(Elem: _dl_Order.Get(ID));
        }


        /// <summary>
        /// Получение данных для админки
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public COrder_ForAdmin? Get_ForAdmin(int ID)
        {
            return Get_Order_ForAdmin(Elem: _dl_Order.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<COrder> GetList(bool? Flag_Del_val = false)
        {
            List<COrder> list_result = [];

            foreach (Order elem in _dl_Order.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                COrder? item = Get_Order(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<COrder_ForAdmin> GetList_ForAdmin(int? RestaurantID = null)
        {
            List<COrder_ForAdmin> list_result = [];

            foreach (Order elem in _dl_Order.GetQueryForAdmin(RestaurantID: RestaurantID, User_Root: null).ToList())
            {
                COrder_ForAdmin? item = Get_Order_ForAdmin(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(p => p.Order_StatusID).ThenByDescending(p => p.Date_Act)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<COrder_ForAdmin> GetList_ForOperator(int? RestaurantID = null)
        {
            List<COrder_ForAdmin> list_result = [];

            List<Order> listOrders = [.. _dl_Order.GetQueryForOperator(RestaurantID: RestaurantID, User_Root: null)];

            foreach (Order elem in listOrders)
            {
                COrder_ForAdmin? item = Get_Order_ForAdmin(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(p => p.Order_StatusID).ThenByDescending(p => p.Date_Act)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<COrder_ForAdmin> GetList_ForAdminDates(DateTime? Date_From, DateTime? Date_To, int? RestaurantID = null)
        {
            List<COrder_ForAdmin> list_result = [];

            foreach (Order elem in _dl_Order.GetQueryForAdminDates(Date_From: Date_From, Date_To: Date_To, RestaurantID: RestaurantID, User_Root: null).ToList())
            {
                COrder_ForAdmin? item = Get_Order_ForAdmin(Elem: elem);
                if (item != null)
                {
                    item.Sum = GetOrderSum(item.ID, out decimal? delivery_Price);
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(p => p.Order_StatusID).ThenByDescending(p => p.Date_Act)];
        }


        /// <summary>
        /// Возвращает список всех заказов
        /// </summary>
        /// <returns></returns>
        public List<COrder>? GetList(int ClientID, bool? Flag_Del_val = false)
        {
            List<COrder> list_result = [];

            foreach (Order elem in _dl_Order.GetQuery(ClientID: ClientID, Flag_Del_val: Flag_Del_val).ToList())
            {
                COrder? item = Get_Order(Elem: elem);
                if (item != null)
                {
                    if (item.Date_To.HasValue)
                    {
                        item.Date_To = item.Date_To.Value.AddMilliseconds(1); // Кастыль добавлен по требованию техлида(@ed)
                    }
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderByDescending(p => p.Date_LastOrderStatusChange)]; // .OrderByDescending(p => p.Order_StatusID).ThenByDescending(p => p.Date_Act)
        }


        /// <summary>
        /// Возвращает список всех заказов
        /// </summary>
        /// <returns></returns>
        public List<COrder_ForAdmin>? GetListByUserID(int UserID)
        {
            List<COrder_ForAdmin> list_result = [];

            bool? user_root = _dl_User.Get(UserID)?.Flag_Root;

            int? RestaurantID = _dl_User.Get(UserID)?.RestaurantID ?? null;

            foreach (Order elem in _dl_Order.GetQueryForOperator(RestaurantID: RestaurantID, User_Root: user_root).ToList())
            {
                COrder_ForAdmin? item = Get_Order_ForAdmin(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderByDescending(p => p.Order_StatusID).ThenByDescending(p => p.Date_Act)];
        }


        /// <summary>
        /// Возвращает список активных заказов
        /// </summary>
        /// <param name="ClientID">Код клиента</param>
        /// <param name="Minuts_AddCompleted">Количество минут, в течении которых в список активных заказов попадают завершенные заказы</param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<COrder>? GetList_Active(int ClientID, int Minuts_AddCompleted = 0, bool? Flag_Del_val = false)
        {
            List<COrder> list_result = [];
            IQueryable<Order> query_all;
            List<Order> query_result = [];
            IQueryable<Order> query_other;
            IQueryable<Order> query_sort;

            query_all = from p in _dl_Order.GetQuery(ClientID: ClientID, Flag_Del_val: Flag_Del_val)
                        where p.Order_StatusID != EOrder_Status.Complete && p.Order_StatusID != EOrder_Status.Cancelled && p.Order_StatusID == EOrder_Status.ReadyToClient
                        orderby p.Date_LastOrderStatusChange descending
                        select p;

            query_other = from p in _dl_Order.GetQuery(ClientID: ClientID, Flag_Del_val: Flag_Del_val)
                          where p.Order_StatusID != EOrder_Status.Complete && p.Order_StatusID != EOrder_Status.Cancelled && p.Order_StatusID != EOrder_Status.ReadyToClient
                          orderby p.Date_LastOrderStatusChange descending
                          select p;

            foreach (Order item in query_all)
            {
                query_result.Add(item);
            }

            foreach (Order item in query_other)
            {
                query_result.Add(item);
            }

            foreach (Order elem in query_result)
            {
                COrder? item = Get_Order(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }

            // p.Order_Status.OrderNum, 
            if (Minuts_AddCompleted > 0)
            {
                query_sort = from p in _dl_Order.GetQuery(ClientID: ClientID, Flag_Del_val: Flag_Del_val)
                             where (p.Order_StatusID == EOrder_Status.Complete || p.Order_StatusID == EOrder_Status.Cancelled)
                                    && p.Date_LastOrderStatusChange >= DateTime.Now.AddMinutes(-Minuts_AddCompleted)
                             orderby p.Date_LastOrderStatusChange descending
                             select p;

                foreach (Order elem in query_sort.ToList())
                {
                    COrder? item = Get_Order(Elem: elem);
                    if (item != null)
                    {
                        if (item.Date_To.HasValue)
                        {
                            item.Date_To = item.Date_To.Value.AddMilliseconds(1); // Кастыль добавлен по требованию техлида(@ed)
                        }
                        list_result.Add(item);
                    }
                }

                DateTime? val = _bl_Setup.Get_Date(Key: Consts.SetupKey_Orders_DateChange);

                if (!query_all.Equals(query_sort) && val <= DateTime.Now.AddMinutes(-Minuts_AddCompleted))
                {
                    _bl_Setup.Set(Key: Consts.SetupKey_Orders_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления
                }
            }

            return [.. list_result.OrderByDescending(p => p.Date_LastOrderStatusChange)]; // .OrderByDescending(p => p.Order_StatusID).ThenByDescending(p => p.Date_Act)
        }


        /// <summary>
        /// Обновление даты для активных заказов
        /// </summary>
        /// <param name="ClientID">Код клиента</param>
        /// <param name="Minuts_AddCompleted">Количество минут, в течении которых в список активных заказов попадают завершенные заказы</param>
        /// <returns></returns>
        public void GetLast(int ClientID, int Minuts_AddCompleted = 0)
        {
            IQueryable<Order> query_all;
            IQueryable<Order> query_sort;

            query_all = from p in _dl_Order.GetQuery(ClientID: ClientID, Flag_Del_val: false)
                        where p.Order_StatusID != EOrder_Status.Complete && p.Order_StatusID != EOrder_Status.Cancelled
                        orderby p.Order_Status.OrderNum descending, p.Date_Act descending
                        select p;

            if (Minuts_AddCompleted > 0)
            {
                query_sort = from p in _dl_Order.GetQuery(ClientID: ClientID, Flag_Del_val: false)
                             where (p.Order_StatusID == EOrder_Status.Complete || p.Order_StatusID == EOrder_Status.Cancelled)
                                    && p.Date_LastOrderStatusChange >= DateTime.Now.AddMinutes(-Minuts_AddCompleted)
                             orderby p.Order_Status.OrderNum, p.Date_Act descending
                             select p;

                DateTime? val = _bl_Setup.Get_Date(Key: Consts.SetupKey_Orders_DateChange);

                if (!query_all.Equals(query_sort) && val <= DateTime.Now.AddMinutes(-Minuts_AddCompleted))
                {
                    _bl_Setup.Set(Key: Consts.SetupKey_Orders_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления
                }
            }
        }


        /// <summary>
        /// Возвращает список активных заказов
        /// </summary>
        /// <param name="ClientID">Код клиента</param>
        /// <param name="Minuts_AddCompleted">Количество минут, в течении которых в список активных заказов попадают завершенные заказы</param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<COrder_ForAdmin>? GetList_ForAdmin_Active(int ClientID, int Minuts_AddCompleted = 0, bool? Flag_Del_val = false)
        {
            List<COrder_ForAdmin> list_result = [];

            IQueryable<Order> query;


            query = from p in _dl_Order.GetQuery(ClientID: ClientID, Flag_Del_val: Flag_Del_val)
                    where p.Order_StatusID != EOrder_Status.Complete && p.Order_StatusID != EOrder_Status.Cancelled
                    orderby p.Order_Status.OrderNum descending, p.Date_Act descending
                    select p;

            foreach (Order elem in query.ToList())
            {
                COrder_ForAdmin? item = Get_Order_ForAdmin(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }

            if (Minuts_AddCompleted > 0)
            {
                query = from p in _dl_Order.GetQuery(ClientID: ClientID, Flag_Del_val: Flag_Del_val)
                        where (p.Order_StatusID == EOrder_Status.Complete || p.Order_StatusID == EOrder_Status.Cancelled)
                               && p.Date_LastOrderStatusChange >= DateTime.Now.AddMinutes(-Minuts_AddCompleted)
                        orderby p.Order_Status.OrderNum, p.Date_Act descending
                        select p;

                foreach (Order elem in query.ToList())
                {
                    COrder_ForAdmin? item = Get_Order_ForAdmin(Elem: elem);
                    if (item != null)
                    {
                        list_result.Add(item);
                    }
                }
            }

            return [.. list_result];
        }


        /// <summary>
        /// Возвращает список истории заказов
        /// </summary>
        /// <returns></returns>
        public List<COrder>? GetList_History(int ClientID, bool? Flag_Del_val = false)
        {
            List<COrder> list_result = [];

            IQueryable<Order> query = from p in _dl_Order.GetQuery(ClientID: ClientID, Flag_Del_val: Flag_Del_val)
                                      where (p.Order_StatusID == EOrder_Status.Complete || p.Order_StatusID == EOrder_Status.Cancelled)
                                      orderby p.Order_Status.OrderNum, p.Date_Act descending
                                      select p;

            foreach (Order elem in query.ToList())
            {
                COrder? item = Get_Order(Elem: elem);
                if (item != null)
                {
                    if (item.Date_To.HasValue)
                    {
                        item.Date_To = item.Date_To.Value.AddMilliseconds(1); // Кастыль добавлен по требованию техлида(@ed)
                    }
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderByDescending(p => p.Date_LastOrderStatusChange)]; // .OrderByDescending(p => p.Order_StatusID).ThenByDescending(p => p.Date_Act)
        }


        /// <summary>
        /// Получение данных по Order_FavoriteID
        /// </summary>
        /// <param name="Order_FavoriteID"></param>
        /// <returns></returns>
        public COrder? GetByOrder_Favorite(int Order_FavoriteID)
        {
            return Get_Order(Elem: _dl_Order.GetByOrder_Favorite(Order_FavoriteID));
        }


        /// <summary>
        /// Получение данных по BookingID
        /// </summary>
        /// <param name="BookingID"></param>
        /// <returns></returns>
        public COrder? GetByBookingID(int BookingID)
        {
            return Get_Order(Elem: _dl_Order.GetByBookingID(BookingID));
        }


        /// <summary>
        /// Получение суммы заказа
        /// </summary>
        /// <param name="OrderID">Код заказа</param>
        /// <param name="Delivery_Price">Стоимость доставки</param>
        /// <returns></returns>
        public decimal? GetOrderSum(int OrderID, out decimal? Delivery_Price)
        {
            return _dl_Order.GetOrderSum(OrderID, out Delivery_Price);
        }


        /// <summary>
        /// Удалить избранный заказ
        /// </summary>
        /// <param name="Order_FavoriteID"></param>
        /// <returns></returns>
        public int Delete_Order_Favorite(int Order_FavoriteID)
        {
            COrder? order = GetByOrder_Favorite(Order_FavoriteID: Order_FavoriteID);

            if (order == null)
            {
                _log.LogError(message: "ERR # Не удалось найти заказ по избранному заказу # {} ## {}", args: [Order_FavoriteID, Errors.GetErrorString(Order_FavoriteID)]);
                return -1;
            }

            return _dl_Order.Edit_OrderFavoriteID(ID: order.ID, Order_FavoriteID: null);
        }


        /// <summary>
        /// Добавляет заказ с товарами заказа
        /// </summary>
        /// <returns></returns>
        public static COrder AddWithData()
        {
            COrder result = new();
            return result;
        }


        /// <summary>
        /// Добавление нового заказа(Оплата картой)
        /// </summary>
        /// <returns></returns>
        public int Add(int ClientID, string Name, string Phone, EPayType Pay_TypeID, int RestaurantID, DateTime? Date_To, bool Flag_Inhouse, int CardID,
                       string? Comment, string? PaySystemIdent, decimal? Summ, string? Comment_AddToOrder, string? Comment_RemoveFromOrder,
                       EOrder_Type Order_TypeID, string? Delivery_Address, string? Delivery_Apartment, string? Delivery_EntranceCode,
                       string? Delivery_EntranceNumber, string? Delivery_Floor, decimal? Delivery_Price, string? Car_Brand, string? Car_RegNumber,
                       string? Car_Color, int? Restaurant_TableID, DateTime? Date_BookingStart, DateTime? Date_BookingEnd, decimal? Booking_Price,
                       List<COrder_Data> List_OrderData)
        {
            if (List_OrderData.Count == 0)
            {
                _log.LogError(message: "ERR # {} ## {}", args: [Errors.Err102, Errors.Err102_str]); 
                return Errors.Err200;
            }

            int? booking_id = null;

            if (Order_TypeID == EOrder_Type.Incafe)
            {
                if (Date_BookingStart > DateTime.MinValue && Date_BookingEnd > DateTime.MinValue && Restaurant_TableID.HasValue)
                {
                    // Проверяем столик
                    CBooking? CBooking = _bl_Bookings.CheckBooking(Restaurant_TableID: (int)Restaurant_TableID, Date_Booking: (DateTime)Date_BookingStart);

                    CRestaurant_Table? Restaurant_Table = _bl_Restaurant_Tables.Get((int)Restaurant_TableID);

                    if (CBooking != null || Restaurant_Table?.Restaurant_Table_StatusID == ERestaurant_Table_Status.Disabled)
                    {
                        _log.LogWarning(message: "Warn # Столик уже занят # {}", args: Restaurant_TableID);
                        return Errors.Err202;
                    }

                    booking_id = _bl_Bookings.AddInApp(ClientID: ClientID, Date_Start: (DateTime)Date_BookingStart, Date_End: (DateTime)Date_BookingEnd,
                                                       Restaurant_TableID: HIDE_Convert.ToInt(Restaurant_TableID));

                    if (booking_id < 0)
                    {
                        _log.LogError(message: "ERR # Не удалось создать бронь # {}", args: booking_id);
                        return HIDE_Convert.ToInt(booking_id);
                    }

                    CRestaurant_Table? table = new Restaurant_Tables().Get(Restaurant_TableID.Value);
                    if (table != null && table.Restaurant_Table_TypeID == ERestaurant_Table_Type.VIP)
                    {
                        int booking_dop_id = _bl_Bookings.AddReserve(Date_Start: Date_BookingStart.Value, Date_End: Date_BookingEnd.Value,
                                                                     Restaurant_TableID: HIDE_Convert.ToInt(Restaurant_TableID));

                        if (booking_dop_id < 0)
                        {
                            _log.LogError(message: "ERR # Не удалось создать бронь # {}", args: booking_dop_id);
                            return HIDE_Convert.ToInt(booking_dop_id);
                        }
                    }

                    string text = $"на {Date_BookingStart.Value:t}";
                    if (DateTime.Now.Date != Date_BookingStart.Value.Date)
                    {
                        text = $"{Date_BookingStart.Value:d} в {Date_BookingStart.Value:t}";
                    }

                    _bl_Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(ClientID), ShablonID: EClient_Notification_Shablon.BookingTable,
                                                           ReHIDEText1: text, OrderID: null);
                }

                else if (Date_BookingStart > DateTime.MinValue && Date_BookingEnd > DateTime.MinValue && Restaurant_TableID == null)
                {

                }
                else
                {
                    _log.LogError(message: "ERR # Данные для бронирования столика некорректны## {}", args: Errors.Err200);
                    return Errors.Err200;
                }
            }
            foreach (COrder_Data elem in List_OrderData)
            {
                if (elem.Bonuses_Minus > 0)
                {
                    if (_bl_Bonuses.Check_Bonuses(ClientID: ClientID) / Consts.Bonus_Count < elem.Bonuses_Minus)
                    {
                        _log.LogError(message: "ERR # Недостаточно бонусов для списания # Клиент # {} ## {}", args: [ClientID, Errors.GetErrorString(ClientID)]);
                        return Errors.Err200;
                    }
                }
            }

            int paydata_id = _dl_ClientPay_Card_Pay.Add(ClientID: ClientID, OrderID: null, ClientPay_Card_Pay_TypeID: EClientPay_Card_Pay_Type.AddPay,
                                                    ClientPay_Card_DataID: CardID, PaymentIdent: PaySystemIdent, Summ: Summ);

            if (paydata_id < 0)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", args: Errors.GetErrorString(ErrorVal: paydata_id));
                }
                return paydata_id;
            }

            int orderid = _dl_Order.Add(Order_StatusID: EOrder_Status.New, ClientID: ClientID, Name: Name, Phone: Phone, Pay_TypeID: Pay_TypeID,
                                        RestaurantID: RestaurantID, Date_To: Date_To, ClientPay_Card_PayID: paydata_id, Flag_Inhouse: Flag_Inhouse, Flag_Payed: true,
                                        Comment: Comment, Delivery_Price: Delivery_Price, Order_TypeID: Order_TypeID, Handing: null, BookingID: booking_id,
                                        Comment_AddToOrder: Comment_AddToOrder, Comment_RemoveFromOrder: Comment_RemoveFromOrder, Booking_Price: Booking_Price);

            if (orderid < 0)
            {
                _log.LogError(message: "ERR # Не удалось создать заказ # {} ## {}", args: [orderid, Errors.GetErrorString(orderid)]);
                return orderid;
            }

            decimal? dish_for_bonuses_count;
            string data_order = "";
            int data_order_index = 0;

            foreach (COrder_Data elem in List_OrderData)
            {
                int orderdata_id = 0;
                int bonus_sub_id = 0;
                int bonus_add_id = 0;
                dish_for_bonuses_count = 0;

                data_order_index++;
                data_order += $"{data_order_index}. \"{elem.Dish_Name}\", {elem.Dish_Count} шт. \n";

                if (elem.Bonuses_Minus > 0)
                {
                    if (_bl_Bonuses.Check_Bonuses(ClientID: ClientID) < elem.Bonuses_Minus)
                    {
                        _log.LogError(message: "ERR # Недостаточно бонусов для списания # Клиент # {} ## {}", args: [ClientID, Errors.GetErrorString(ClientID)]);
                        return bonus_sub_id;
                    }

                    // Отдельная запись - Списано бонусов
                    orderdata_id = _dl_Order_Data.Add(OrderID: orderid, DishID: elem.DishID, Dish_ColorID: elem.Dish_ColorID, Dish_Price: elem.Dish_Price,
                                      Dish_Count: elem.Dish_Count, Dish_Summ: elem.Dish_Summ, Dish_Name: elem.Dish_Name,
                                      Dish_VolumeID: elem.Dish_VolumeID, Dish_VolumeValue: elem.Dish_VolumeValue,
                                      Dish_VolumePrice: elem.Dish_VolumePrice, Bonuses_Plus: null, Bonuses_Minus: elem.Bonuses_Minus * Consts.Bonus_Count);

                    bonus_sub_id = _bl_Bonuses.Add(ClientID: ClientID, Bonuses: -HIDE_Convert.ToDecimal(elem.Bonuses_Minus * Consts.Bonus_Count), OrderID: orderid);

                    if (bonus_sub_id < 0)
                    {
                        _log.LogError(message: "ERR # Не удалось добавить запись в Bonuses # {} ## {}", args: [bonus_sub_id, Errors.GetErrorString(bonus_sub_id)]);
                        return bonus_sub_id;
                    }

                    if (elem.List_Dish_Ingridients != null && elem.List_Dish_Ingridients.Count != 0)
                    {
                        foreach (COrder_Data_Ingridient elem_ingridient in elem.List_Dish_Ingridients)
                        {
                            int orderdataingridient_id = _dl_Order_Data_Ingridient.Add(Order_DataID: orderdata_id, IngridientID: elem_ingridient.IngridientID,
                                                                                       Ingridient_Price: elem_ingridient.Ingridient_Price, Ingridient_Count: elem_ingridient.Ingridient_Count,
                                                                                       Ingridient_Summ: elem_ingridient.Ingridient_Summ, Ingridient_Name: elem_ingridient.Ingridient_Name);
                            if (orderdataingridient_id < 0)
                            {
                                _log.LogError(message: "ERR # Не удалось создать ингридиенты# {} ## {}", args: [orderdataingridient_id, Errors.GetErrorString(orderdataingridient_id)]);
                                return orderdataingridient_id;
                            }
                        }
                    }

                }

                decimal? bonuses_for_dish = _bl_Dishes.Check_Dish_ForBonuses(ID: elem.DishID);

                if (bonuses_for_dish > 0)
                {
                    dish_for_bonuses_count = ((int)elem.Dish_Count - (elem.Bonuses_Minus ?? 0)) * bonuses_for_dish;
                }

                orderdata_id = _dl_Order_Data.Add(OrderID: orderid, DishID: elem.DishID, Dish_ColorID: elem.Dish_ColorID, Dish_Price: elem.Dish_Price,
                                  Dish_Count: elem.Dish_Count, Dish_Summ: elem.Dish_Summ, Dish_Name: elem.Dish_Name,
                                  Dish_VolumeID: elem.Dish_VolumeID, Dish_VolumeValue: elem.Dish_VolumeValue,
                                  Dish_VolumePrice: elem.Dish_VolumePrice, Bonuses_Plus: dish_for_bonuses_count > 0 ? dish_for_bonuses_count : null, Bonuses_Minus: null);

                if (orderdata_id < 0)
                {
                    _log.LogError(message: "ERR # Не удалось создать содержимое заказ # {} ## {}", args: [orderdata_id, Errors.GetErrorString(orderdata_id)]);
                    return orderdata_id;
                }

                if (dish_for_bonuses_count != null && dish_for_bonuses_count > 0)
                {
                    bonus_add_id = _bl_Bonuses.Add(ClientID: ClientID, Bonuses: HIDE_Convert.ToDecimal(dish_for_bonuses_count), OrderID: orderid);

                    if (bonus_add_id < 0)
                    {
                        _log.LogError(message: "ERR # Не удалось добавить запись в Bonuses # {} ## {}", args: [bonus_add_id, Errors.GetErrorString(bonus_add_id)]);
                        return bonus_add_id;
                    }
                }

                if (elem.List_Dish_Ingridients != null && elem.List_Dish_Ingridients.Count != 0)
                {
                    foreach (COrder_Data_Ingridient elem_ingridient in elem.List_Dish_Ingridients)
                    {
                        int orderdataingridient_id = _dl_Order_Data_Ingridient.Add(Order_DataID: orderdata_id, IngridientID: elem_ingridient.IngridientID,
                                                                                   Ingridient_Price: elem_ingridient.Ingridient_Price, Ingridient_Count: elem_ingridient.Ingridient_Count,
                                                                                   Ingridient_Summ: elem_ingridient.Ingridient_Summ, Ingridient_Name: elem_ingridient.Ingridient_Name);
                        if (orderdataingridient_id < 0)
                        {
                            _log.LogError(message: "ERR # Не удалось создать ингридиенты# {} ## {}", args: [orderdataingridient_id, Errors.GetErrorString(orderdataingridient_id)]);
                            return orderdataingridient_id;
                        }

                        data_order += $"Ингредиент: \"{elem_ingridient.Ingridient_Name}\", {elem_ingridient.Ingridient_Count} шт. \n";
                    }
                }
            }

            // Доставка
            if (Order_TypeID == EOrder_Type.Delivery)
            {
                int result_delivery = _bl_Order_Deliveries.Add(OrderID: orderid, Delivery_Apartment: Delivery_Apartment, Delivery_EntranceNumber: Delivery_EntranceNumber,
                                                                   Delivery_EntranceCode: Delivery_EntranceCode, Delivery_Floor: Delivery_Floor, Delivery_Handing: null,
                                                                   Delivery_Address: Delivery_Address, Delivery_Comment: Comment, Delivery_To: Date_To);

                if (result_delivery < 0)
                {
                    _log.LogError(message: "ERR # Не удалось создать доставку # {} ## {}", args: [orderid, Errors.GetErrorString(result_delivery)]);
                    return result_delivery;
                }

                int edit_client = _bl_Clients.Edit_Delivery(ID: ClientID, Delivery_Address: Delivery_Address, Delivery_Apartment: Delivery_Apartment,
                                                            Delivery_EntranceCode: Delivery_EntranceCode, Delivery_EntranceNumber: Delivery_EntranceNumber,
                                                            Delivery_Floor: Delivery_Floor);

                if (edit_client < 0)
                {
                    _log.LogError(message: "ERR # Не удалось изменить данные доставки # {} ## {}", args: [orderid, Errors.Err203_str]);
                    return result_delivery;
                }
            }

            // Вынести к машине
            if (Order_TypeID == EOrder_Type.Car)
            {
                int edit_client = _bl_Clients.Edit_Car(ID: ClientID, Car_Brand: Car_Brand, Car_RegNumber: Car_RegNumber, Car_Color: Car_Color);

                if (edit_client < 0)
                {
                    _log.LogError(message: "ERR # Не удалось изменить данные машины # {} ## {}", args: [orderid, Errors.Err203_str]);
                    return Errors.Err203;
                }
            }

            // Заказ другому человеку
            if (!string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(Phone))
            {
                int edit_client = _bl_Clients.Edit_Another(ID: ClientID, Name_Another: Name, Phone_Another: Phone);

                if (edit_client < 0)
                {
                    _log.LogError(message: "ERR # Не удалось изменить данные для другого человека # {} ## {}", args: [orderid, Errors.Err203_str]);
                    return Errors.Err203;
                }
            }

            _bl_Setup.Set(Key: Consts.SetupKey_Orders_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            if (orderid > 0)
            {
                CCommon? order_type = new Order_Types().Get((int)Order_TypeID);
                CRestaurant? restaurant = new Restaurants().Get(RestaurantID);
                CClient? client = _bl_Clients.Get(ClientID);
                int? count_orders = new Orders().GetList(ClientID)?.Count;
                decimal? sum = GetOrderSum(orderid, out decimal? delivery_Price);

                //Проверка на null для Sum
                if(sum == null)
                {
                    _log.LogError(message: "ERR # {} ## {}", args: [Errors.Err103, Errors.Err103_str]);
                    return Errors.Err103;
                }

                string sum_delivery = "";
                if (Order_TypeID == EOrder_Type.Delivery)
                {
                    sum_delivery = delivery_Price.HasValue && delivery_Price.Value > 0 && sum.HasValue && sum.Value < HIDE_Convert.ToDecimal(new Setups().Get(Key: Consts.SetupKey_Dop_Delivery_PriceFree)?.Val) ? "Доставка: " + delivery_Price.Value.ToString("0.##") + "\n" : "Доставка: Бесплатно\n";
                }
                string client_status = "новый";
                if (count_orders.HasValue && count_orders.Value > 1)
                {
                    if (count_orders.Value >= 2 && count_orders <= 10)
                    {
                        client_status = "повторный";
                    }

                    if (count_orders.Value > 10)
                    {
                        client_status = "постоянный";
                    }
                }

                // Бронирование столиков
                if (Order_TypeID == EOrder_Type.Incafe)
                {
                    if (Date_BookingStart > DateTime.MinValue && Date_BookingEnd > DateTime.MinValue && Restaurant_TableID.HasValue)
                    {
                        CRestaurant_Table? cRestaurant_Table = new Restaurant_Tables().Get(Restaurant_TableID.Value);

                        // Отправка оповещения о новом заказе по бронированию в Telegram бот
                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: "Telegram: {Message} - Details: {Order_TypeID}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот", Order_TypeID]);
                        }

                        string MessageText = $"Новый заказ № <a href=\"https://oper.RestAndRant.ru\">{orderid}</a> \n" +
                                                 $"Адрес кофейни: {restaurant?.Address} \n" +
                                                 $"Тип заказа: {order_type?.Name} \n" +
                                                 $"Столик: №{cRestaurant_Table?.Name} \n" +
                                                 $"Клиент: \"{client?.Name ?? "Имя не указано"}\"({client_status}) \n" +
                                                 $"Телефон: <a href=\"tel:+{client?.Phone}\">+{client?.Phone}</a> \n \n" +
                                                 $"Данные о заказе: \n" +
                                                 $"{data_order} \n \n" +
                                                 $"Сумма заказа: {sum.Value:0.##} \n" +
                                                 $"{sum_delivery} \n" +
                                                 $"<a href=\"https://oper.RestAndRant.ru\">Перейти к заказу</a>";

                        if(_h_Telegram.SendMessageTelegram(ChatID: restaurant?.TgChatBookingID ?? AppConfig.TG_ChatBookingID, MessageText: MessageText, out string? ErrMessage))
                        {
                            if (_log.IsEnabled(LogLevel.Information))
                            {
                                _log.LogInformation(message: "Telegram: {Message}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Успешно"]);
                            }
                        }
                        else
                        {
                            _log.LogWarning(message: "Telegram: {Message} - Details: {ErrMessage}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Ошибка", ErrMessage]);
                        }

                    }
                    else if (Date_BookingStart > DateTime.MinValue && Date_BookingEnd > DateTime.MinValue && !Restaurant_TableID.HasValue)
                    {


                        // Отправка оповещения о новом заказе по бронированию в Telegram бот
                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: "Telegram: {Message} - Details: {Order_TypeID}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот", Order_TypeID]);
                        }

                        string MessageText = $"Новый заказ № <a href=\"https://oper.RestAndRant.ru\">{orderid}</a> \n" +
                                                 $"Адрес кофейни: {restaurant?.Address} \n" +
                                                 $"Тип заказа: {order_type?.Name} \n" +
                                                 $"Клиент: \"{client?.Name ?? "Имя не указано"}\"({client_status}) \n" +
                                                 $"Телефон: <a href=\"tel:+{client?.Phone}\">+{client?.Phone}</a> \n \n" +
                                                 $"Данные о заказе: \n" +
                                                 $"{data_order} \n \n" +
                                                 $"Сумма заказа: {sum.Value:0.##} \n" +
                                                 $"{sum_delivery} \n" +
                                                 $"<a href=\"https://oper.RestAndRant.ru\">Перейти к заказу</a>";

                        if (_h_Telegram.SendMessageTelegram(ChatID: restaurant?.TgChatBookingID ?? AppConfig.TG_ChatBookingID, MessageText: MessageText, out string? ErrMessage))
                        {
                            if (_log.IsEnabled(LogLevel.Information))
                            {
                                _log.LogInformation(message: "Telegram: {Message}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Успешно"]);
                            }
                        }
                        else
                        {
                            _log.LogWarning(message: "Telegram: {Message} - Details: {ErrMessage}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Ошибка", ErrMessage]);
                        }
                    }
                }
                else if (Order_TypeID == EOrder_Type.Delivery)
                {
                    // Отправка оповещения о новом заказе с доставкой в Telegram бот
                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(message: "Telegram: {Message} - Details: {Order_TypeID}", args: ["Отправка оповещения о новом заказе с доставкой в Telegram бот", Order_TypeID]);
                    }

                    string MessageText = $"Новый заказ № <a href=\"https://oper.RestAndRant.ru\">{orderid}</a> \n" +
                                             $"Адрес кофейни: {restaurant?.Address} \n" +
                                             $"Тип заказа: {order_type?.Name} \n" +
                                             $"Клиент: \"{client?.Name ?? "Имя не указано"}\"({client_status}) \n" +
                                             $"Телефон: <a href=\"tel:+{client?.Phone}\">+{client?.Phone}</a> \n \n" +
                                             $"Данные о заказе: \n" +
                                             $"{data_order} \n \n" +
                                             $"Сумма заказа: {sum.Value:0.##} \n" +
                                             $"{sum_delivery} \n" +
                                             $"Адрес доставки: {Delivery_Address}, \n" +
                                             $"Дом: {Delivery_Apartment}, \n" +
                                             $"Код домофона: {Delivery_EntranceCode}, \n" +
                                             $"Номер квартиры: {Delivery_EntranceNumber} \n" +
                                             $"<a href=\"https://oper.RestAndRant.ru\">Перейти к заказу</a>";

                    if (_h_Telegram.SendMessageTelegram(ChatID: AppConfig.TG_ChatDeliveryID, MessageText: MessageText, out string? ErrMessage))
                    {
                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: "Telegram: {Message}", args: ["Отправка оповещения о новом заказе с доставкой в Telegram бот - Успешно"]);
                        }
                    }
                    else
                    {
                        _log.LogWarning(message: "Telegram: {Message} - Details: {ErrMessage}", args: ["Отправка оповещения о новом заказе с доставкой в Telegram бот - Ошибка", ErrMessage]);
                    }
                }
                else
                {
                    // Отправка оповещения о новом заказе в Telegram бот
                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(message: "Telegram: {Message} - Details: {Order_TypeID}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот", Order_TypeID]);
                    }

                    string MessageText = $"Новый заказ № <a href=\"https://oper.RestAndRant.ru\">{orderid}</a> \n" +
                                             $"Адрес кофейни: {restaurant?.Address} \n" +
                                             $"Тип заказа: {order_type?.Name} \n" +
                                             $"Клиент: \"{client?.Name ?? "Имя не указано"}\"({client_status}) \n" +
                                             $"Телефон: <a href=\"tel:+{client?.Phone}\">+{client?.Phone}</a> \n \n" +
                                             $"Данные о заказе: \n" +
                                             $"{data_order} \n \n" +
                                             $"Сумма заказа: {sum.Value:0.##} \n" +
                                             $"{sum_delivery} \n" +
                                             $"<a href=\"https://oper.RestAndRant.ru\">Перейти к заказу</a>";

                    if(_h_Telegram.SendMessageTelegram(ChatID: restaurant?.TgChatID ?? AppConfig.TG_ChatID, MessageText: MessageText, out string? ErrMessage))
                    {
                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: "Telegram: {Message}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Успешно"]);
                        }
                    }
                    else
                    {
                        _log.LogWarning(message: "Telegram: {Message} - Details: {ErrMessage}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Ошибка", ErrMessage]);
                    }
                }
            }
            return orderid;
        }

        /// <summary>
        /// Добавление нового заказа(Оплата наличными)
        /// </summary>
        /// <returns></returns>
        public int AddMoney(int ClientID, string Name, string Phone, EPayType Pay_TypeID, int RestaurantID, DateTime? Date_To, bool Flag_Inhouse,
                            EOrder_Type Order_TypeID, string? Delivery_Address, string? Comment, string? Delivery_Apartment, string? Delivery_EntranceCode,
                            string? Delivery_EntranceNumber, string? Delivery_Floor, decimal? Delivery_Price, int? Restaurant_TableID,
                            DateTime? Date_BookingStart, DateTime? Date_BookingEnd, decimal? Handing, string? Car_Brand, string? Car_RegNumber,
                            string? Car_Color, string? Comment_AddToOrder, string? Comment_RemoveFromOrder, decimal? Booking_Price, List<COrder_Data> List_OrderData)
        {
            int? booking_id = null;
            string data_order = "";
            int data_order_index = 0;

            if (Order_TypeID == EOrder_Type.Incafe)
            {
                if (Date_BookingStart > DateTime.MinValue && Date_BookingEnd > DateTime.MinValue && Restaurant_TableID.HasValue)
                {
                    // Проверяем столик
                    CBooking? CBooking = _bl_Bookings.CheckBooking(Restaurant_TableID: (int)Restaurant_TableID, Date_Booking: (DateTime)Date_BookingStart);

                    CRestaurant_Table? Restaurant_Table = _bl_Restaurant_Tables.Get((int)Restaurant_TableID);



                    if (CBooking != null || Restaurant_Table?.Restaurant_Table_StatusID == ERestaurant_Table_Status.Disabled)
                    {

                        _log.LogWarning(message: "Warn # Столик уже занят # {}", args: Restaurant_TableID);

                        return Errors.Err202;
                    }

                    booking_id = _bl_Bookings.AddInApp(ClientID: ClientID, Date_Start: (DateTime)Date_BookingStart, Date_End: (DateTime)Date_BookingEnd,
                                                       Restaurant_TableID: HIDE_Convert.ToInt(Restaurant_TableID));

                    if (booking_id < 0)
                    {

                        _log.LogError(message: "ERR # Не удалось создать бронь # {}", args: booking_id);
                        return HIDE_Convert.ToInt(booking_id);
                    }
                }
                else if (Date_BookingStart > DateTime.MinValue && Date_BookingEnd > DateTime.MinValue && Restaurant_TableID == null)
                {
                    //TODO: MAE - Обрабатывать - Когда?
                }

                else
                {
                    _log.LogError(message: "ERR # {} ## {}", args: [Errors.Err104, Errors.Err104_str]);
                    return Errors.Err104;
                }
            }

            if (List_OrderData.Count == 0)
            {
                _log.LogError(message: "ERR # {} ## {}", args: [Errors.Err102, Errors.Err102_str]);
                return Errors.Err102;
            }

            int orderid = _dl_Order.Add(Order_StatusID: EOrder_Status.New, ClientID: ClientID, Name: Name, Phone: Phone, Pay_TypeID: Pay_TypeID,
                                        RestaurantID: RestaurantID, Date_To: Date_To, ClientPay_Card_PayID: null, Flag_Inhouse: Flag_Inhouse, Flag_Payed: false,
                                        Comment: Comment, Delivery_Price: Delivery_Price, Order_TypeID: Order_TypeID, BookingID: booking_id, Handing: Handing,
                                        Comment_AddToOrder: Comment_AddToOrder, Comment_RemoveFromOrder: Comment_RemoveFromOrder, Booking_Price: Booking_Price);
            if (orderid < 0)
            {
                _log.LogError(message: "ERR # Не удалось создать заказ # {} ## {}", args: [orderid, Errors.GetErrorString(orderid)]);
                return orderid;
            }

            decimal? dish_for_bonuses_count;

            foreach (COrder_Data elem in List_OrderData)
            {
                int orderdata_id = 0;
                int bonus_sub_id = 0;

                dish_for_bonuses_count = 0;

                data_order_index++;
                data_order += $"{data_order_index}. \"{elem.Dish_Name}\", {elem.Dish_Count} шт. \n";

                if (elem.Bonuses_Minus > 0)
                {
                    if (_bl_Bonuses.Check_Bonuses(ClientID: ClientID) < elem.Bonuses_Minus)
                    {

                        _log.LogError(message: "ERR # Недостаточно бонусов для списания # Клиент # {} ## {}", args: [ClientID,Errors.GetErrorString(ClientID) ]);
                        return bonus_sub_id;
                    }

                    // Отдельная запись - Списано бонусов
                    orderdata_id = _dl_Order_Data.Add(OrderID: orderid, DishID: elem.DishID, Dish_ColorID: elem.Dish_ColorID, Dish_Price: elem.Dish_Price,
                                      Dish_Count: elem.Dish_Count, Dish_Summ: elem.Dish_Summ, Dish_Name: elem.Dish_Name,
                                      Dish_VolumeID: elem.Dish_VolumeID, Dish_VolumeValue: elem.Dish_VolumeValue,
                                      Dish_VolumePrice: elem.Dish_VolumePrice, Bonuses_Plus: null, Bonuses_Minus: elem.Bonuses_Minus * Consts.Bonus_Count);

                    if (elem.List_Dish_Ingridients != null && elem.List_Dish_Ingridients.Count != 0)
                    {
                        foreach (COrder_Data_Ingridient elem_ingridient in elem.List_Dish_Ingridients)
                        {
                            int orderdataingridient_id = _dl_Order_Data_Ingridient.Add(Order_DataID: orderdata_id, IngridientID: elem_ingridient.IngridientID,
                                                                                       Ingridient_Price: elem_ingridient.Ingridient_Price, Ingridient_Count: elem_ingridient.Ingridient_Count,
                                                                                       Ingridient_Summ: elem_ingridient.Ingridient_Summ, Ingridient_Name: elem_ingridient.Ingridient_Name);
                            if (orderdataingridient_id < 0)
                            {
                                _log.LogError(message: "ERR # Не удалось создать ингридиенты# {} ## {}", args: [orderdataingridient_id, Errors.GetErrorString(orderdataingridient_id)]);
                                return orderdataingridient_id;
                            }

                            data_order += $"Ингредиент: \"{elem_ingridient.Ingridient_Name}\", {elem_ingridient.Ingridient_Count} шт. \n";
                        }
                    }

                }

                decimal? bonuses_for_dish = _bl_Dishes.Check_Dish_ForBonuses(ID: elem.DishID);

                if (bonuses_for_dish > 0)
                {
                    dish_for_bonuses_count = ((int)elem.Dish_Count - (elem.Bonuses_Minus ?? 0)) * bonuses_for_dish;
                }

                if (dish_for_bonuses_count >= 0 && (elem.Bonuses_Minus <= 0 || elem.Bonuses_Minus == null))
                {
                    orderdata_id = _dl_Order_Data.Add(OrderID: orderid, DishID: elem.DishID, Dish_ColorID: elem.Dish_ColorID, Dish_Price: elem.Dish_Price,
                      Dish_Count: elem.Dish_Count, Dish_Summ: elem.Dish_Summ, Dish_Name: elem.Dish_Name,
                      Dish_VolumeID: elem.Dish_VolumeID, Dish_VolumeValue: elem.Dish_VolumeValue,
                      Dish_VolumePrice: elem.Dish_VolumePrice, Bonuses_Plus: dish_for_bonuses_count > 0 ? dish_for_bonuses_count : null, Bonuses_Minus: null);

                    if (orderdata_id < 0)
                    {
                        _log.LogError(message: "ERR # Не удалось создать содержимое заказ # {} ## {}", args: [orderdata_id, Errors.GetErrorString(orderdata_id)]);
                        return orderdata_id;
                    }
                }

                // Список ингредиентов
                if (elem.Bonuses_Minus <= 0 || elem.Bonuses_Minus == null)
                {
                    if (elem.List_Dish_Ingridients != null && elem.List_Dish_Ingridients.Count != 0)
                    {
                        foreach (COrder_Data_Ingridient elem_ingridient in elem.List_Dish_Ingridients)
                        {
                            int orderdataingridient_id = _dl_Order_Data_Ingridient.Add(Order_DataID: orderdata_id, IngridientID: elem_ingridient.IngridientID,
                                                                                       Ingridient_Price: elem_ingridient.Ingridient_Price, Ingridient_Count: elem_ingridient.Ingridient_Count,
                                                                                       Ingridient_Summ: elem_ingridient.Ingridient_Summ, Ingridient_Name: elem_ingridient.Ingridient_Name);
                            if (orderdataingridient_id < 0)
                            {

                                _log.LogError(message: "ERR # Не удалось создать ингридиенты# {} ## {}", args: [orderdataingridient_id, Errors.GetErrorString(orderdataingridient_id)]);
                                return orderdataingridient_id;
                            }

                            data_order += $"Ингредиент: \"{elem_ingridient.Ingridient_Name}\", {elem_ingridient.Ingridient_Count} шт. \n ";
                        }
                    }
                }

            }

            // Доставка
            if (Order_TypeID == EOrder_Type.Delivery)
            {
                int result_delivery = _bl_Order_Deliveries.Add(OrderID: orderid, Delivery_EntranceNumber: Delivery_EntranceNumber,
                                                               Delivery_EntranceCode: Delivery_EntranceCode, Delivery_Floor: Delivery_Floor, Delivery_Handing: Handing,
                                                               Delivery_Address: Delivery_Address, Delivery_Comment: Comment, Delivery_To: Date_To,
                                                               Delivery_Apartment: Delivery_Apartment);

                if (result_delivery < 0)
                {
                    _log.LogError(message: "ERR # Не удалось создать доставку # {} ## {}", args: [orderid, Errors.GetErrorString(result_delivery)]);
                    return result_delivery;
                }

                int edit_client = _bl_Clients.Edit_Delivery(ID: ClientID, Delivery_Address: Delivery_Address, Delivery_Apartment: Delivery_Apartment,
                                                            Delivery_EntranceCode: Delivery_EntranceCode, Delivery_EntranceNumber: Delivery_EntranceNumber,
                                                            Delivery_Floor: Delivery_Floor);

                if (edit_client < 0)
                {
                    _log.LogError(message: "ERR # Не удалось изменить данные доставки # {} ## {}", args: [orderid, Errors.Err203_str]);
                    return result_delivery;
                }
            }

            // Вынести к машине
            if (Order_TypeID == EOrder_Type.Car)
            {
                int edit_client = _bl_Clients.Edit_Car(ID: ClientID, Car_Brand: Car_Brand, Car_RegNumber: Car_RegNumber, Car_Color: Car_Color);

                if (edit_client < 0)
                {
                    _log.LogError(message: "ERR # Не удалось изменить данные машины # {} ## {}", args: [orderid, Errors.Err203_str]);
                    return Errors.Err203;
                }
            }

            // Заказ другому человеку
            if (!string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(Phone))
            {
                int edit_client = _bl_Clients.Edit_Another(ID: ClientID, Name_Another: Name, Phone_Another: Phone);

                if (edit_client < 0)
                {
                    _log.LogError(message: "ERR # Не удалось изменить данные для другого человека # {} ## {}", args: [orderid, Errors.Err203_str]);
                    return Errors.Err203;
                }
            }

            //_bl_Clients.Flag_IsPolicy_Set(ID: ClientID, Flag_IsPolicy: true);

            _bl_Setup.Set(Key: Consts.SetupKey_Orders_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            if (orderid > 0)
            {
                CCommon? order_type = new Order_Types().Get((int)Order_TypeID);
                CRestaurant? restaurant = new Restaurants().Get(RestaurantID);
                CClient? client = _bl_Clients.Get(ClientID);
                int? count_orders = new Orders().GetList(ClientID)?.Count;
                decimal? sum = GetOrderSum(orderid, out decimal? delivery_Price);

                //Проверка на Null для Sum
                if (sum == null)
                {

                    _log.LogError(message: $"ERR # Сумма имеет значение null");

                    return Errors.Err203; //TODO: MAE -Сделать обработку ошибок
                }

                string sum_delivery = "";
                if (Order_TypeID == EOrder_Type.Delivery)
                {
                    sum_delivery = delivery_Price.HasValue && delivery_Price.Value > 0 && sum.HasValue && sum.Value < HIDE_Convert.ToDecimal(new Setups().Get(Key: Consts.SetupKey_Dop_Delivery_PriceFree)?.Val) ? "Доставка: " + delivery_Price.Value.ToString("0.##") + "\n" : "Доставка: Бесплатно\n";
                }
                string client_status = "новый";
                if (count_orders.HasValue && count_orders.Value > 1)
                {
                    if (count_orders.Value >= 2 && count_orders <= 10)
                    {
                        client_status = "повторный";
                    }

                    if (count_orders.Value > 10)
                    {
                        client_status = "постоянный";
                    }
                }

                // Бронирование столиков
                if (Order_TypeID == EOrder_Type.Incafe)
                {
                    if (Date_BookingStart > DateTime.MinValue && Date_BookingEnd > DateTime.MinValue && Restaurant_TableID.HasValue)
                    {
                        CRestaurant_Table? cRestaurant_Table = new Restaurant_Tables().Get(Restaurant_TableID.Value);

                        // Отправка оповещения о новом заказе по бронированию в Telegram бот
                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: "Telegram: {Message} - Details: {Order_TypeID}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот", Order_TypeID]);
                        }

                        string MessageText = $"Новый заказ № <a href=\"https://oper.RestAndRant.ru\">{orderid}</a> \n" +
                                                 $"Адрес кофейни: {restaurant?.Address} \n" +
                                                 $"Тип заказа: {order_type?.Name} \n" +
                                                 $"Столик: №{cRestaurant_Table?.Name} \n" +
                                                 $"Клиент: \"{client?.Name ?? "Имя не указано"}\"({client_status}) \n" +
                                                 $"Телефон: <a href=\"tel:+{client?.Phone}\">+{client?.Phone}</a> \n \n" +
                                                 $"Данные о заказе: \n" +
                                                 $"{data_order} \n \n" +
                                                 $"Сумма заказа: {sum.Value:0.##} \n" +
                                                 $"{sum_delivery} \n" +
                                                 $"<a href=\"https://oper.RestAndRant.ru\">Перейти к заказу</a>";

                        if(_h_Telegram.SendMessageTelegram(ChatID: restaurant?.TgChatBookingID ?? AppConfig.TG_ChatBookingID, MessageText: MessageText, out string? ErrMessage))
                        {
                            if (_log.IsEnabled(LogLevel.Information))
                            {
                                _log.LogInformation(message: "Telegram: {Message}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Успешно"]);
                            }
                        }
                        else
                        {
                            _log.LogWarning(message: "Telegram: {Message} - Details: {ErrMessage}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Ошибка", ErrMessage]);
                        }


                    }
                    else if (Date_BookingStart > DateTime.MinValue && Date_BookingEnd > DateTime.MinValue && !Restaurant_TableID.HasValue)
                    {


                        // Отправка оповещения о новом заказе по бронированию в Telegram бот
                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: "Telegram: {Message} - Details: {Order_TypeID}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот", Order_TypeID]);
                        }

                        string MessageText = $"Новый заказ № <a href=\"https://oper.RestAndRant.ru\">{orderid}</a> \n" +
                                                 $"Адрес кофейни: {restaurant?.Address} \n" +
                                                 $"Тип заказа: {order_type?.Name} \n" +
                                                 $"Клиент: \"{client?.Name ?? "Имя не указано"}\"({client_status}) \n" +
                                                 $"Телефон: <a href=\"tel:+{client?.Phone}\">+{client?.Phone}</a> \n \n" +
                                                 $"Данные о заказе: \n" +
                                                 $"{data_order} \n \n" +
                                                 $"Сумма заказа: {sum.Value:0.##} \n" +
                                                 $"{sum_delivery} \n" +
                                                 $"<a href=\"https://oper.RestAndRant.ru\">Перейти к заказу</a>";

                        if (_h_Telegram.SendMessageTelegram(ChatID: restaurant?.TgChatBookingID ?? AppConfig.TG_ChatBookingID, MessageText: MessageText, out string? ErrMessage))
                        {
                            if (_log.IsEnabled(LogLevel.Information))
                            {
                                _log.LogInformation(message: "Telegram: {Message}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Успешно"]);
                            }
                        }
                        else
                        {
                            _log.LogWarning(message: "Telegram: {Message} - Details: {ErrMessage}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Ошибка", ErrMessage]);
                        }
                    }
                }
                else if (Order_TypeID == EOrder_Type.Delivery)
                {
                    // Отправка оповещения о новом заказе с доставкой в Telegram бот
                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(message: "Telegram: {Message} - Details: {Order_TypeID}", args: ["Отправка оповещения о новом заказе с доставкой в Telegram бот", Order_TypeID]);
                    }

                    string MessageText = $"Новый заказ № <a href=\"https://oper.RestAndRant.ru\">{orderid}</a> \n" +
                                             $"Адрес кофейни: {restaurant?.Address} \n" +
                                             $"Тип заказа: {order_type?.Name} \n" +
                                             $"Клиент: \"{client?.Name ?? "Имя не указано"}\"({client_status}) \n" +
                                             $"Телефон: <a href=\"tel:+{client?.Phone}\">+{client?.Phone}</a> \n \n" +
                                             $"Данные о заказе: \n" +
                                             $"{data_order} \n \n" +
                                             $"Сумма заказа: {sum.Value:0.##} \n" +
                                             $"{sum_delivery} \n" +
                                             $"Адрес доставки: {Delivery_Address}, \n" +
                                             $"Дом: {Delivery_Apartment}, \n" +
                                             $"Код домофона: {Delivery_EntranceCode}, \n" +
                                             $"Номер квартиры: {Delivery_EntranceNumber} \n" +
                                             $"<a href=\"https://oper.RestAndRant.ru\">Перейти к заказу</a>";

                    if (_h_Telegram.SendMessageTelegram(ChatID: AppConfig.TG_ChatDeliveryID, MessageText: MessageText, out string? ErrMessage))
                    {
                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: "Telegram: {Message}", args: ["Отправка оповещения о новом заказе с доставкой в Telegram бот - Успешно"]);
                        }
                    }
                    else
                    {
                        _log.LogWarning(message: "Telegram: {Message} - Details: {ErrMessage}", args: ["Отправка оповещения о новом заказе с доставкой в Telegram бот - Ошибка", ErrMessage]);
                    }
                }
                else
                {
                    // Отправка оповещения о новом заказе в Telegram бот
                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(message: "Telegram: {Message} - Details: {Order_TypeID}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот", Order_TypeID]);
                    }

                    string MessageText = $"Новый заказ № <a href=\"https://oper.RestAndRant.ru\">{orderid}</a> \n" +
                                             $"Адрес кофейни: {restaurant?.Address} \n" +
                                             $"Тип заказа: {order_type?.Name} \n" +
                                             $"Клиент: \"{client?.Name ?? "Имя не указано"}\"({client_status}) \n" +
                                             $"Телефон: <a href=\"tel:+{client?.Phone}\">+{client?.Phone}</a> \n \n" +
                                             $"Данные о заказе: \n" +
                                             $"{data_order} \n \n" +
                                             $"Сумма заказа: {sum.Value:0.##} \n" +
                                             $"{sum_delivery} \n" +
                                             $"<a href=\"https://oper.RestAndRant.ru\">Перейти к заказу</a>";

                    if(_h_Telegram.SendMessageTelegram(ChatID: restaurant?.TgChatID ?? AppConfig.TG_ChatID, MessageText: MessageText, out string? ErrMessage))
                    {
                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: "Telegram: {Message}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Успешно"]);
                        }
                    }
                    else
                    {
                        _log.LogWarning(message: "Telegram: {Message} - Details: {ErrMessage}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Ошибка", ErrMessage]);
                    }
                }
            }
            return orderid;
        }


        /// <summary>
        /// Добавление нового заказа(Оплата наличными)
        /// </summary>
        /// <returns></returns>
        public int AddByOperator(int ClientID, string Name, string Phone, EPayType Pay_TypeID, int RestaurantID, DateTime? Date_To, bool Flag_Inhouse, List<COrder_Data> List_OrderData, string? Comment)
        {
            if (List_OrderData.Count == 0)
            {
                _log.LogError(message: "ERR # Список состава заказа пустой## {}", args: [Errors.Err200]); //TODO: Привести в порядок код ошибки
                return Errors.Err200;
            }

            int orderid = _dl_Order.AddByOperator(Order_StatusID: EOrder_Status.New, ClientID: ClientID, Name: Name, Phone: Phone, Pay_TypeID: Pay_TypeID,
                                        RestaurantID: RestaurantID, Date_To: Date_To, ClientPay_Card_PayID: null, Flag_Inhouse: Flag_Inhouse, Flag_Payed: false, Comment: Comment);
            if (orderid < 0)
            {
                _log.LogError(message: "ERR # Не удалось создать заказ # {} ## {}", args: [orderid, Errors.GetErrorString(orderid)]);
                return orderid;
            }

            decimal? dish_for_bonuses_count = 0;

            foreach (COrder_Data elem in List_OrderData)
            {
                int orderdata_id = 0;
                int bonus_sub_id = 0;

                if (elem.Bonuses_Minus > 0)
                {
                    if (_bl_Bonuses.Check_Bonuses(ClientID: ClientID) < elem.Bonuses_Minus)
                    {
                        _log.LogError(message: "ERR # Недостаточно бонусов для списания # Клиент # {} ## {}", args: [ClientID, Errors.GetErrorString(ClientID)]);
                        return bonus_sub_id;
                    }

                    // Отдельная запись - Списано бонусов
                    orderdata_id = _dl_Order_Data.Add(OrderID: orderid, DishID: elem.DishID, Dish_ColorID: elem.Dish_ColorID, Dish_Price: elem.Dish_Price,
                                      Dish_Count: elem.Dish_Count, Dish_Summ: elem.Dish_Summ, Dish_Name: elem.Dish_Name,
                                      Dish_VolumeID: elem.Dish_VolumeID, Dish_VolumeValue: elem.Dish_VolumeValue,
                                      Dish_VolumePrice: elem.Dish_VolumePrice, Bonuses_Plus: null, Bonuses_Minus: elem.Bonuses_Minus * Consts.Bonus_Count);

                    if (elem.List_Dish_Ingridients != null && elem.List_Dish_Ingridients.Count != 0)
                    {
                        foreach (COrder_Data_Ingridient elem_ingridient in elem.List_Dish_Ingridients)
                        {
                            int orderdataingridient_id = _dl_Order_Data_Ingridient.Add(Order_DataID: orderdata_id, IngridientID: elem_ingridient.IngridientID,
                                                                                       Ingridient_Price: elem_ingridient.Ingridient_Price, Ingridient_Count: elem_ingridient.Ingridient_Count,
                                                                                       Ingridient_Summ: elem_ingridient.Ingridient_Summ, Ingridient_Name: elem_ingridient.Ingridient_Name);
                            if (orderdataingridient_id < 0)
                            {
                                _log.LogError(message: "ERR # Не удалось создать ингридиенты# {} ## {}", args: [orderdataingridient_id, Errors.GetErrorString(orderdataingridient_id)]);
                                return orderdataingridient_id;
                            }
                        }
                    }

                }

                decimal? bonuses_for_dish = _bl_Dishes.Check_Dish_ForBonuses(ID: elem.DishID);

                if (bonuses_for_dish > 0)
                {
                    dish_for_bonuses_count = ((int)elem.Dish_Count - (elem.Bonuses_Minus ?? 0)) * bonuses_for_dish;
                }

                if (dish_for_bonuses_count >= 0 && (elem.Bonuses_Minus <= 0 || elem.Bonuses_Minus == null))
                {
                    orderdata_id = _dl_Order_Data.Add(OrderID: orderid, DishID: elem.DishID, Dish_ColorID: elem.Dish_ColorID, Dish_Price: elem.Dish_Price,
                      Dish_Count: elem.Dish_Count, Dish_Summ: elem.Dish_Summ, Dish_Name: elem.Dish_Name,
                      Dish_VolumeID: elem.Dish_VolumeID, Dish_VolumeValue: elem.Dish_VolumeValue,
                      Dish_VolumePrice: elem.Dish_VolumePrice, Bonuses_Plus: dish_for_bonuses_count > 0 ? dish_for_bonuses_count : null, Bonuses_Minus: null);

                    if (orderdata_id < 0)
                    {
                        _log.LogError(message: "ERR # Не удалось создать содержимое заказ # {} ## {}", args: [orderdata_id, Errors.GetErrorString(orderdata_id)]);
                        return orderdata_id;
                    }
                }

                // Список ингредиентов
                if (elem.Bonuses_Minus <= 0 || elem.Bonuses_Minus == null)
                {
                    if (elem.List_Dish_Ingridients != null && elem.List_Dish_Ingridients.Count != 0)
                    {
                        foreach (COrder_Data_Ingridient elem_ingridient in elem.List_Dish_Ingridients)
                        {
                            int orderdataingridient_id = _dl_Order_Data_Ingridient.Add(Order_DataID: orderdata_id, IngridientID: elem_ingridient.IngridientID,
                                                                                       Ingridient_Price: elem_ingridient.Ingridient_Price, Ingridient_Count: elem_ingridient.Ingridient_Count,
                                                                                       Ingridient_Summ: elem_ingridient.Ingridient_Summ, Ingridient_Name: elem_ingridient.Ingridient_Name);
                            if (orderdataingridient_id < 0)
                            {
                                _log.LogError(message: "ERR # Не удалось создать ингридиенты# {} ## {}", args: [orderdataingridient_id, Errors.GetErrorString(orderdataingridient_id)]);
                                return orderdataingridient_id;
                            }
                        }
                    }
                }

            }

            //_bl_Clients.Flag_IsPolicy_Set(ID: ClientID, Flag_IsPolicy: true);

            _bl_Setup.Set(Key: Consts.SetupKey_Orders_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return orderid;
        }


        /// <summary>
        /// Добавление нового заказа(На сайте-меню)
        /// </summary>
        /// <returns></returns>
        public int AddSiteMenu(int ClientID, string Name, string Phone, EPayType Pay_TypeID, int RestaurantID, DateTime? Date_To, EOrder_Type Order_TypeID,
                               string? Comment_AddToOrder, string? Comment_RemoveFromOrder, int? Restaurant_TableID, DateTime? Date_BookingStart,
                               DateTime? Date_BookingEnd, decimal? Handing, int? NumberQuests, List<COrder_Data> List_OrderData)
        {
            int? booking_id = null;

            if (Order_TypeID == EOrder_Type.Incafe && Date_BookingStart > DateTime.MinValue && Date_BookingEnd > DateTime.MinValue && Restaurant_TableID.HasValue)
            {
                // Проверяем столик
                CBooking? CBooking = _bl_Bookings.CheckBooking(Restaurant_TableID: (int)Restaurant_TableID, Date_Booking: (DateTime)Date_BookingStart);

                CRestaurant_Table? Restaurant_Table = _bl_Restaurant_Tables.Get((int)Restaurant_TableID);



                if (CBooking != null || Restaurant_Table?.Restaurant_Table_StatusID == ERestaurant_Table_Status.Disabled)
                {
                    _log.LogWarning(message: "Warn # Столик уже занят # {}", args: Restaurant_TableID);
                    return Errors.Err202;
                }

                booking_id = _bl_Bookings.AddInSite(ClientID: ClientID, Date_Start: (DateTime)Date_BookingStart, Date_End: (DateTime)Date_BookingEnd,
                                                   Restaurant_TableID: HIDE_Convert.ToInt(Restaurant_TableID), NumberQuests: NumberQuests);

                if (booking_id < 0)
                { 
                    _log.LogError(message: "ERR # Не удалось создать бронь # {}", args: [booking_id]);
                    return HIDE_Convert.ToInt(booking_id);
                }

                string text = $"на {Date_BookingStart.Value:t}";
                if (DateTime.Now.Date != Date_BookingStart.Value.Date)
                {
                    text = $"{Date_BookingStart.Value:d} в {Date_BookingStart.Value:t}";
                }

                _bl_Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(ClientID), ShablonID: EClient_Notification_Shablon.BookingTable,
                                                       ReHIDEText1: text, OrderID: null);
            }

            else if (Date_BookingStart > DateTime.MinValue && Date_BookingEnd > DateTime.MinValue && Restaurant_TableID == null)
            {

            }

            if (List_OrderData.Count == 0)
            {
                _log.LogError(message: "ERR # Список состава заказа пустой## {}", args: Errors.Err200); //TODO: Привести в порядок код ошибки
                return Errors.Err200;
            }

            int orderid = _dl_Order.Add(Order_StatusID: EOrder_Status.New, ClientID: ClientID, Name: Name, Phone: Phone, Pay_TypeID: Pay_TypeID,
                                        RestaurantID: RestaurantID, Date_To: Date_To, ClientPay_Card_PayID: null, Flag_Inhouse: true, Flag_Payed: false,
                                        Comment: null, Delivery_Price: null, Order_TypeID: Order_TypeID, BookingID: booking_id, Handing: Handing,
                                        Comment_AddToOrder: Comment_AddToOrder, Comment_RemoveFromOrder: Comment_RemoveFromOrder, Booking_Price: null);
            if (orderid < 0)
            {
                _log.LogError(message: "ERR # Не удалось создать заказ # {} ## {}", args: [orderid, Errors.GetErrorString(orderid)]);
                return orderid;
            }

            decimal? dish_for_bonuses_count;
            string data_order = "";
            int data_order_index = 0;

            foreach (COrder_Data elem in List_OrderData)
            {
                int orderdata_id = 0;
                int bonus_sub_id = 0;
                dish_for_bonuses_count = 0;

                data_order_index++;
                data_order += $"{data_order_index}. \"{elem.Dish_Name}\", {elem.Dish_Count:#,#} шт. \n";

                if (elem.Bonuses_Minus > 0)
                {
                    if (_bl_Bonuses.Check_Bonuses(ClientID: ClientID) < elem.Bonuses_Minus)
                    {
                        _log.LogError(message: "ERR # Недостаточно бонусов для списания # Клиент # {} ## {}", args: [ClientID, Errors.GetErrorString(ClientID)]);
                        return bonus_sub_id;
                    }

                    // Отдельная запись - Списано бонусов
                    orderdata_id = _dl_Order_Data.Add(OrderID: orderid, DishID: elem.DishID, Dish_ColorID: elem.Dish_ColorID, Dish_Price: elem.Dish_Price,
                                      Dish_Count: elem.Dish_Count, Dish_Summ: elem.Dish_Summ, Dish_Name: elem.Dish_Name,
                                      Dish_VolumeID: elem.Dish_VolumeID, Dish_VolumeValue: elem.Dish_VolumeValue,
                                      Dish_VolumePrice: elem.Dish_VolumePrice, Bonuses_Plus: null, Bonuses_Minus: elem.Bonuses_Minus * Consts.Bonus_Count);

                    if (elem.List_Dish_Ingridients != null && elem.List_Dish_Ingridients.Count != 0)
                    {
                        foreach (COrder_Data_Ingridient elem_ingridient in elem.List_Dish_Ingridients)
                        {
                            int orderdataingridient_id = _dl_Order_Data_Ingridient.Add(Order_DataID: orderdata_id, IngridientID: elem_ingridient.IngridientID,
                                                                                       Ingridient_Price: elem_ingridient.Ingridient_Price, Ingridient_Count: elem_ingridient.Ingridient_Count,
                                                                                       Ingridient_Summ: elem_ingridient.Ingridient_Summ, Ingridient_Name: elem_ingridient.Ingridient_Name);
                            if (orderdataingridient_id < 0)
                            {
                                _log.LogError(message: "ERR # Не удалось создать ингридиенты# {} ## {}", args: [orderdataingridient_id, Errors.GetErrorString(orderdataingridient_id)]);
                                return orderdataingridient_id;
                            }
                        }
                    }

                }

                decimal? bonuses_for_dish = _bl_Dishes.Check_Dish_ForBonuses(ID: elem.DishID);

                if (bonuses_for_dish > 0)
                {
                    dish_for_bonuses_count = ((int)elem.Dish_Count - (elem.Bonuses_Minus ?? 0)) * bonuses_for_dish;
                }

                if (dish_for_bonuses_count >= 0 && (elem.Bonuses_Minus <= 0 || elem.Bonuses_Minus == null))
                {
                    orderdata_id = _dl_Order_Data.Add(OrderID: orderid, DishID: elem.DishID, Dish_ColorID: elem.Dish_ColorID, Dish_Price: elem.Dish_Price,
                      Dish_Count: elem.Dish_Count, Dish_Summ: elem.Dish_Summ, Dish_Name: elem.Dish_Name,
                      Dish_VolumeID: elem.Dish_VolumeID, Dish_VolumeValue: elem.Dish_VolumeValue,
                      Dish_VolumePrice: elem.Dish_VolumePrice, Bonuses_Plus: dish_for_bonuses_count > 0 ? dish_for_bonuses_count : null, Bonuses_Minus: null);

                    if (orderdata_id < 0)
                    {
                        _log.LogError(message: "ERR # Не удалось создать содержимое заказ # {} ## {}", args: [orderdata_id, Errors.GetErrorString(orderdata_id)]);
                        return orderdata_id;
                    }
                }

                // Список ингредиентов
                if (elem.Bonuses_Minus <= 0 || elem.Bonuses_Minus == null)
                {
                    if (elem.List_Dish_Ingridients != null && elem.List_Dish_Ingridients.Count != 0)
                    {
                        foreach (COrder_Data_Ingridient elem_ingridient in elem.List_Dish_Ingridients)
                        {
                            int orderdataingridient_id = _dl_Order_Data_Ingridient.Add(Order_DataID: orderdata_id, IngridientID: elem_ingridient.IngridientID,
                                                                                       Ingridient_Price: elem_ingridient.Ingridient_Price, Ingridient_Count: elem_ingridient.Ingridient_Count,
                                                                                       Ingridient_Summ: elem_ingridient.Ingridient_Summ, Ingridient_Name: elem_ingridient.Ingridient_Name);
                            if (orderdataingridient_id < 0)
                            {
                                _log.LogError(message: "ERR # Не удалось создать ингридиенты# {} ## {}", args: [orderdataingridient_id, Errors.GetErrorString(orderdataingridient_id)]);
                                return orderdataingridient_id;
                            }

                            data_order += $"Ингредиент: \"{elem_ingridient.Ingridient_Name}\", {elem_ingridient.Ingridient_Count} шт. \n";
                        }
                    }
                }

            }

            if (orderid > 0)
            {
                CCommon? order_type = new Order_Types().Get((int)Order_TypeID);
                CRestaurant? restaurant = new Restaurants().Get(RestaurantID);
                CClient? client = _bl_Clients.Get(ClientID);
                int? count_orders = new Orders().GetList(ClientID)?.Count;
                decimal? sum = GetOrderSum(orderid, out decimal? delivery_Price);

                //Проверка на Null для Sum
                if (sum == null)
                {
                    _log.LogError(message: "ERR # Сумма имеет значение null");
                    return Errors.Err203; //TODO: MAE -Сделать обработку ошибок
                }

                string sum_delivery = "";
                if (Order_TypeID == EOrder_Type.Delivery)
                {
                    sum_delivery = delivery_Price.HasValue && delivery_Price.Value > 0 && sum.HasValue && sum.Value < HIDE_Convert.ToDecimal(new Setups().Get(Key: Consts.SetupKey_Dop_Delivery_PriceFree)?.Val) ? "Доставка: " + delivery_Price.Value.ToString("0.##") + "\n" : "Доставка: Бесплатно\n";
                }
                string client_status = "новый";
                if (count_orders.HasValue && count_orders.Value > 1)
                {
                    if (count_orders.Value >= 2 && count_orders <= 10)
                    {
                        client_status = "повторный";
                    }

                    if (count_orders.Value > 10)
                    {
                        client_status = "постоянный";
                    }
                }

                // Бронирование столиков
                if (Order_TypeID == EOrder_Type.Incafe)
                {
                    if (Date_BookingStart > DateTime.MinValue && Date_BookingEnd > DateTime.MinValue && Restaurant_TableID.HasValue)
                    {
                        CRestaurant_Table? cRestaurant_Table = new Restaurant_Tables().Get(Restaurant_TableID.Value);

                        // Отправка оповещения о новом заказе по бронированию в Telegram бот
                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: "Telegram: {Message} - Details: {Order_TypeID}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот", Order_TypeID]);
                        }

                        string MessageText = $"Новый заказ № <a href=\"https://oper.RestAndRant.ru\">{orderid}</a> \n" +
                                                 $"Адрес кофейни: {restaurant?.Address} \n" +
                                                 $"Тип заказа: {order_type?.Name} \n" +
                                                 $"Столик: №{cRestaurant_Table?.Name} \n" +
                                                 $"Клиент: \"{client?.Name ?? "Имя не указано"}\"({client_status}) \n" +
                                                 $"Телефон: <a href=\"tel:+{client?.Phone}\">+{client?.Phone}</a> \n \n" +
                                                 $"Данные о заказе: \n" +
                                                 $"{data_order.ReHIDE("<br />", "").ReHIDE("<br>", "").ReHIDE("<p>", "").ReHIDE("</p>", "")} \n \n" +
                                                 $"Сумма заказа: {sum.Value:0.##} \n" +
                                                 $"{sum_delivery} \n" +
                                                 $"<a href=\"https://oper.RestAndRant.ru\">Перейти к заказу</a>";
                         
                        if(_h_Telegram.SendMessageTelegram(ChatID: restaurant?.TgChatBookingID ?? AppConfig.TG_ChatBookingID, MessageText: MessageText, out string? ErrMessage))
                        {
                            if (_log.IsEnabled(LogLevel.Information))
                            {
                                _log.LogInformation(message: "Telegram: {Message}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Успешно"]);
                            }
                        }
                        else
                        {
                            _log.LogWarning(message: "Telegram: {Message} - Details: {ErrMessage}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Ошибка", ErrMessage]);
                        }
                    }
                }
                else if (Order_TypeID == EOrder_Type.Delivery)
                {
                    // Отправка оповещения о новом заказе с доставкой в Telegram бот
                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(message: "Telegram: {Message} - Details: {Order_TypeID}", args: ["Отправка оповещения о новом заказе с доставкой в Telegram бот", Order_TypeID]);
                    }

                    string MessageText = $"Новый заказ № <a href=\"https://oper.RestAndRant.ru\">{orderid}</a> \n" +
                                             $"Адрес кофейни: {restaurant?.Address} \n" +
                                             $"Тип заказа: {order_type?.Name} \n" +
                                             $"Клиент: \"{client?.Name ?? "Имя не указано"}\"({client_status}) \n" +
                                             $"Телефон: <a href=\"tel:+{client?.Phone}\">+{client?.Phone}</a> \n \n" +
                                             $"Данные о заказе: \n" +
                                             $"{data_order} \n \n" +
                                             $"Сумма заказа: {sum.Value:0.##} \n" +
                                             $"{sum_delivery} \n" +
                                             $"<a href=\"https://oper.RestAndRant.ru\">Перейти к заказу</a>";

                    if (_h_Telegram.SendMessageTelegram(ChatID: AppConfig.TG_ChatDeliveryID, MessageText: MessageText, out string? ErrMessage))
                    {
                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: "Telegram: {Message}", args: ["Отправка оповещения о новом заказе с доставкой в Telegram бот - Успешно"]);
                        }
                    }
                    else
                    {
                        _log.LogWarning(message: "Telegram: {Message} - Details: {ErrMessage}", args: ["Отправка оповещения о новом заказе с доставкой в Telegram бот - Ошибка", ErrMessage]);
                    }
                }
                else
                {
                    // Отправка оповещения о новом заказе в Telegram бот
                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(message: "Telegram: {Message} - Details: {Order_TypeID}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот", Order_TypeID]);
                    }

                    string MessageText = $"Новый заказ № <a href=\"https://oper.RestAndRant.ru\">{orderid}</a> \n" +
                                             $"Адрес кофейни: {restaurant?.Address} \n" +
                                             $"Тип заказа: {order_type?.Name} \n" +
                                             $"Клиент: \"{client?.Name ?? "Имя не указано"}\"({client_status}) \n" +
                                             $"Телефон: <a href=\"tel:+{client?.Phone}\">+{client?.Phone}</a> \n \n" +
                                             $"Данные о заказе: \n" +
                                             $"{data_order.ReHIDE("<br />", "").ReHIDE("<br>", "").ReHIDE("<p>", "").ReHIDE("</p>", "")} \n \n" +
                                             $"Сумма заказа: {sum.Value:0.##} \n" +
                                             $"{sum_delivery} \n" +
                                             $"<a href=\"https://oper.RestAndRant.ru\">Перейти к заказу</a>";

                    if(_h_Telegram.SendMessageTelegram(ChatID: restaurant?.TgChatID ?? AppConfig.TG_ChatID, MessageText: MessageText, out string? ErrMessage))
                    {
                        if (_log.IsEnabled(LogLevel.Information))
                        {
                            _log.LogInformation(message: "Telegram: {Message}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Успешно"]);
                        }
                    }
                    else
                    {
                        _log.LogWarning(message: "Telegram: {Message} - Details: {ErrMessage}", args: ["Отправка оповещения о новом заказе по бронированию в Telegram бот - Ошибка", ErrMessage]);
                    }

                }

            }

            _bl_Setup.Set(Key: Consts.SetupKey_Orders_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return orderid;
        }


        /// <summary>
        /// Отмена заказа c возвратом оплаченных средств
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public int Cancel_Future_Refund(int ClientID, int OrderID)
        {
            int result;

            Order? elem = _dl_Order.Get(ID: OrderID);

            if (elem == null || elem.ClientID != ClientID) //Если заказ не найден, или заказ не от того клиента
            {
                _log.LogError(message: Errors.Err200_str);
                return Errors.Err200;
            }

            CRestaurant? restaurant = new Restaurants().Get(elem.RestaurantID);

            if (restaurant == null)
            {
                _log.LogError(message: Errors.Err200_str);
                return Errors.Err200;
            }

            if (elem.ClientPay_Card_PayID == null ||
                elem.ClientPay_Card_Pay == null ||
                elem.ClientPay_Card_Pay.ClientPay_Card_Pay_TypeID != EClientPay_Card_Pay_Type.AddPay)
            {
                _log.LogError(message: Errors.Err200_str);
                return Errors.Err200;
            }

            // Осуществляем возврат
            bool result_refund = Helper_YooKassa.YooKassa.Refund(Payment_id: elem.ClientPay_Card_Pay?.PaymentIdent!,
                                                                 Summ: (decimal)elem.ClientPay_Card_Pay?.Summ!,
                                                                 RestaurantLogin: restaurant.YouKassaLogin,
                                                                 RestaurantPassw: restaurant.YouKassaPassword,
                                                                 out string? _,
                                                                 out string? Log);

            if (result_refund)
            {
                // Если возврат получился, то добавляем запись о возврате
                int paydata_id = _dl_ClientPay_Card_Pay.Add(ClientID: ClientID, OrderID: OrderID, ClientPay_Card_Pay_TypeID: EClientPay_Card_Pay_Type.ReturnPay,
                                                            ClientPay_Card_DataID: elem.ClientPay_Card_Pay.ClientPay_Card_DataID,
                                                            PaymentIdent: elem.ClientPay_Card_Pay.PaymentIdent, Summ: elem.ClientPay_Card_Pay.Summ);
                if (paydata_id < 0)
                {
                    if (_log.IsEnabled(LogLevel.Critical))
                    {
                        _log.LogCritical(message: "Ошибка добавления записи о возврате для заказа {}", args: OrderID);
                    }
                    return Errors.Err202;
                }

                // Изменяем ClientPay_Card_PayID
                result = _dl_Order.Edit_ClientPayCardPayID(ID: OrderID, ClientPay_Card_PayID: paydata_id);

                if (result < 0)
                {
                    _log.LogError(message: "Ошибка изменения ClientPay_Card_PayID для заказа {}", args: OrderID);
                    return result;
                }
            }
            else
            {
                // Если возврат не удался

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", args: Log);
                }
                return Errors.Err910;
            }

            // Изменяем статус заказа
            result = _dl_Order.Edit_Status(ID: OrderID, Order_StatusID: EOrder_Status.Cancelled, Flag_Payed: false);

            if (result < 0)
            {
                _log.LogError(message: "Ошибка изменения статуса для заказа {}", args: OrderID);
                return result;
            }

            return result;
        }


        /// <summary>
        /// Изменение статуса
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Order_StatusID"></param>
        /// <param name="Flag_Payed"></param>
        /// <returns></returns>
        public int Edit_Status(int ID, EOrder_Status Order_StatusID, bool Flag_Payed)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Orders_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            int? client_id = Get(ID)?.ClientID;
            COrder_ForAdmin? order = Get_ForAdmin(ID: ID);

            if (order == null || order.List_OrderDatas == null)
            {
                return -1;
            }

            if (!order.Flag_Payed)
            {
                foreach (var elem in order.List_OrderDatas)
                {
                    if (elem.Bonuses_Plus > 0 && Flag_Payed && Order_StatusID == EOrder_Status.Complete && order.Order_StatusID != Order_StatusID)
                    {
                        int bonus_add_id = _bl_Bonuses.Add(ClientID: HIDE_Convert.ToInt(client_id), Bonuses: HIDE_Convert.ToDecimal(elem.Bonuses_Plus), OrderID: ID);

                        if (bonus_add_id < 0)
                        {
                            _log.LogError(message: "ERR # Не удалось добавить запись в Bonuses # {} ## {}", args: [bonus_add_id, Errors.GetErrorString(bonus_add_id)]);
                            return bonus_add_id;
                        }
                    }

                    if (elem.Bonuses_Minus > 0 && Flag_Payed && Order_StatusID == EOrder_Status.Complete && order.Order_StatusID != Order_StatusID)
                    {
                        int bonus_add_id = _bl_Bonuses.Add(ClientID: HIDE_Convert.ToInt(client_id), Bonuses: -HIDE_Convert.ToDecimal(elem.Bonuses_Minus), OrderID: ID);

                        if (bonus_add_id < 0)
                        {

                            _log.LogError(message: "ERR # Не удалось добавить запись в Bonuses # {} ## {}", args: [bonus_add_id, Errors.GetErrorString(bonus_add_id)]);

                            return bonus_add_id;
                        }
                    }
                }
            }

            return _dl_Order.Edit_Status(ID: ID, Order_StatusID: Order_StatusID, Flag_Payed: Flag_Payed);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Orders_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Order.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }


        /// <summary>
        /// Отмена заказа
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public int Cancel(int ID, int ClientID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Orders_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            _bl_Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(ClientID), ShablonID: EClient_Notification_Shablon.Cancelled,
                                                   ReHIDEText1: ID.ToString(), OrderID: null);

            // Удалить бронь
            int? booking_id = Get(ID)?.BookingID;

            if (booking_id.HasValue && booking_id.Value > 0)
            {
                int result = _bl_Bookings.Del(booking_id.Value, 1);

                if (result < 0)
                {
                    _log.LogError(message: "{}", args: Errors.GetErrorString(result));
                    return result;
                }
            }

            return _dl_Order.Edit_Status(ID: ID, Order_StatusID: EOrder_Status.Cancelled, Flag_Payed: false);
        }

        /// <summary>
        /// Удаление всех заказов клиента (нужно при удалении аккаунта)
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public int Del_All(int ClientID)
        {
            int result = _dl_Order.SetAll_FlagDel(ClientID: ClientID);

            if (result < 0)
            {
                _log.LogError(message: "{}", args: Errors.GetErrorString(result));
                return result;
            }

            return result;
        }

    }
}