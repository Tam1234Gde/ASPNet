using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Работа с заказами
    /// </summary>
    public class Order_Favorites
    {
        private readonly ILogger _log;
        private readonly Order_Favorite_oper _dl_Order_Favorite = new();
        private readonly Orders _db_Orders = new();
        private readonly Order_oper _dl_Orders = new();
        private readonly Order_Favorite_Data_oper _dl_Order_Favorite_Data = new();
        private readonly Order_Favorite_Data_Ingridient_oper _dl_Order_Favorite_Data_Ingridient = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Order_Favorites()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Favorite? Get_Order(Order_Favorite? Elem)
        {
            if (Elem != null)
            {
                COrder_Favorite item = new()
                {
                    ID = Elem.ID,
                    ClientID = Elem.ClientID,
                    Name = Elem.Name,
                    Phone = Elem.Phone,
                    Pay_TypeID = Elem.Pay_TypeID,
                    RestaurantID = Elem.RestaurantID,
                    Date_To = Elem.Date_To,
                    Flag_Inhouse = Elem.Flag_Inhouse,
                    Flag_Payed = Elem.Flag_Payed
                };

                if (Elem.Order_Datas != null && Elem.Order_Datas.Count != 0)
                {
                    List<COrder_Favorite_Data> list_datas = [];

                    foreach (Order_Favorite_Data elem_detail in Elem.Order_Datas)
                    {
                        COrder_Favorite_Data? item_detail = Get_OrderData(elem_detail);
                        if (item_detail != null && !item_detail.Flag_Del)
                        {
                            list_datas.Add(item_detail);
                        }
                    }

                    item.List_OrderDatas = list_datas;
                }

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
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Favorite_ForAdmin? Get_Order_ForAdmin(Order_Favorite? Elem)
        {
            if (Elem != null)
            {
                COrder_Favorite_ForAdmin item = new()
                {
                    ID = Elem.ID,
                    Date_Act = Elem.Date_Act,
                    Order_StatusID = Elem.Order_StatusID,
                    Order_Status = Elem.Order_Status.Name,
                    ClientID = Elem.ClientID,
                    Client = Elem.Client?.Name,
                    Name = Elem.Name,
                    Phone = Elem.Phone,
                    Pay_TypeID = Elem.Pay_TypeID,
                    Pay_Type = Elem.Pay_Type.Name,
                    RestaurantID = Elem.RestaurantID,
                    Restaurant = Elem.Restaurant.Adress,
                    Date_To = Elem.Date_To,
                    Flag_Inhouse = Elem.Flag_Inhouse,
                    Flag_Payed = Elem.Flag_Payed,
                };

                if (Elem.Order_Datas != null && Elem.Order_Datas.Count != 0)
                {
                    List<COrder_Favorite_Data_ForAdmin> list_datas = [];

                    foreach (Order_Favorite_Data elem_detail in Elem.Order_Datas)
                    {
                        COrder_Favorite_Data_ForAdmin? item_detail = Get_OrderData_ForAdmin(elem_detail);
                        if (item_detail != null)
                        {
                            list_datas.Add(item_detail);
                        }
                    }

                    item.List_OrderDatas = list_datas;
                }

                return item;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Получение данных заказа
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Favorite_Data? Get_OrderData(Order_Favorite_Data? Elem)
        {
            if (Elem != null)
            {
                COrder_Favorite_Data item = new()
                {
                    ID = Elem.ID,
                    OrderFavoriteID = Elem.OrderFavoriteID,
                    DishID = Elem.DishID,
                    Dish_ColorID = Elem.Dish_ColorID,
                    Dish_Price = Elem.Dish_Price,
                    Dish_Count = Elem.Dish_Count,
                    Dish_Summ = Elem.Dish_Summ,
                    Dish_Name = Elem.Dish_Name,
                    Dish_VolumeID = Elem.Dish_VolumeID,
                    Dish_VolumeValue = Elem.Dish_VolumeValue,
                    Dish_VolumePrice = Elem.Dish_VolumePrice,
                    Flag_Del = Elem.Flag_Del
                };

                if (Elem.Order_Data_Ingridients != null && Elem.Order_Data_Ingridients.Count != 0)
                {
                    List<COrder_Favorite_Data_Ingridient> list_ingridients = [];

                    foreach (Order_Favorite_Data_Ingridient elem_detail in Elem.Order_Data_Ingridients)
                    {
                        COrder_Favorite_Data_Ingridient? item_detail = Get_OrderDataIngridient(elem_detail);
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
        /// Получение данных заказа
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Favorite_Data_ForAdmin? Get_OrderData_ForAdmin(Order_Favorite_Data? Elem)
        {
            if (Elem != null)
            {
                COrder_Favorite_Data_ForAdmin item = new()
                {
                    ID = Elem.ID,
                    OrderFavoriteID = Elem.OrderFavoriteID,
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
                };

                if (Elem.Order_Data_Ingridients != null && Elem.Order_Data_Ingridients.Count != 0)
                {
                    List<COrder_Favorite_Data_Ingridient_ForAdmin> list_ingridients = [];

                    foreach (Order_Favorite_Data_Ingridient elem_detail in Elem.Order_Data_Ingridients)
                    {
                        COrder_Favorite_Data_Ingridient_ForAdmin? item_detail = Get_OrderDataIngridient_ForAdmin(elem_detail);
                        if (item_detail != null)
                        {
                            list_ingridients.Add(item_detail);
                        }
                    }
                    item.List_OrderDataIngridients = list_ingridients;
                }

                return item;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Получение ингридиента
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Favorite_Data_Ingridient? Get_OrderDataIngridient(Order_Favorite_Data_Ingridient? Elem)
        {
            if (Elem != null)
            {
                COrder_Favorite_Data_Ingridient item = new()
                {
                    Order_Favorite_DataID = Elem.Order_Favorite_DataID,
                    IngridientID = Elem.IngridientID,
                    Ingridient_Price = Elem.Ingridient_Price,
                    Ingridient_Count = Elem.Ingridient_Count,
                    Ingridient_Summ = Elem.Ingridient_Summ,
                    Ingridient_Name = Elem.Ingridient_Name,
                    Flag_Del = Elem.Ingridient?.Flag_Del ?? false
                };

                return item;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Получение ингридиента
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Favorite_Data_Ingridient_ForAdmin? Get_OrderDataIngridient_ForAdmin(Order_Favorite_Data_Ingridient? Elem)
        {
            if (Elem != null)
            {
                COrder_Favorite_Data_Ingridient_ForAdmin item = new()
                {
                    ID = Elem.ID,
                    Order_Favorite_DataID = Elem.Order_Favorite_DataID,
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
        public COrder_Favorite? Get(int ID)
        {
            return Get_Order(Elem: _dl_Order_Favorite.Get(ID));
        }


        /// <summary>
        /// Получение данных для админки
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public COrder_Favorite_ForAdmin? Get_ForAdmin(int ID)
        {
            return Get_Order_ForAdmin(Elem: _dl_Order_Favorite.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<COrder_Favorite> GetList(bool? Flag_Del_val = false)
        {
            List<COrder_Favorite> list_result = [];

            foreach (Order_Favorite elem in _dl_Order_Favorite.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                COrder_Favorite? item = Get_Order(Elem: elem);
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
        public List<COrder_Favorite_ForAdmin> GetList_ForAdmin(bool? Flag_Del_val = false)
        {
            List<COrder_Favorite_ForAdmin> list_result = [];

            foreach (Order_Favorite elem in _dl_Order_Favorite.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                COrder_Favorite_ForAdmin? item = Get_Order_ForAdmin(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result];
        }


        /// <summary>
        /// Возвращает список избранных заказов
        /// </summary>
        /// <returns></returns>
        public List<COrder_Favorite>? GetList(int ClientID, bool? Flag_Del_val = false)
        {
            List<COrder_Favorite> list_result = [];

            foreach (Order_Favorite elem in _dl_Order_Favorite.GetQueryByClientID(ClientID: ClientID, Flag_Del_val: Flag_Del_val).ToList())
            {
                COrder_Favorite? item = Get_Order(Elem: elem);
                if (item != null && item.List_OrderDatas != null && item.List_OrderDatas.Count != 0)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result];
        }


        /// <summary>
        /// Добавляет заказ с товарами заказа
        /// </summary>
        /// <returns></returns>
        public static COrder_Favorite AddWithData()
        {
            COrder_Favorite result = new();
            return result;
        }


        /// <summary>
        /// Получение даты последнего редактирование избранных заказов
        /// </summary>
        /// <param name="ClientID">Код клиента</param>
        /// <returns></returns>
        public DateTime? GetMaxLastEditDate(int ClientID)
        {
            DateTime? date_max = null;

            IQueryable<Order_Favorite> query = _dl_Order_Favorite.GetQueryByClientID(ClientID: ClientID);

            if (query != null && query.Any())
            {
                date_max = query.Max(x => x.Date_LastEdit);
            }

            return date_max;
        }


        /// <summary>
        /// Добавление заказа в избранное
        /// </summary>
        /// <returns></returns>
        public int Add(int OrderID)
        {
            COrder? order = _db_Orders.Get(OrderID);

            if (order == null)
            {

                _log.LogError(message: "{}", args: $"ERR # Заказ с данным ID отсутствует## {Errors.Err200}"); //TODO: Привести в порядок код ошибки

                return Errors.Err200;
            }

            if (order.List_OrderDatas == null && order?.List_OrderDatas?.Count == 0)
            {

                _log.LogError(message: "{}", args: $"ERR # Список состава заказа пустой## {Errors.Err200}"); //TODO: Привести в порядок код ошибки

                return Errors.Err200;
            }

            int order_favoriteid = _dl_Order_Favorite.Add(Order_StatusID: EOrder_Status.New, ClientID: order!.ClientID, Name: order.Name, Phone: order.Phone,
                                                          Pay_TypeID: order.Pay_TypeID, RestaurantID: order.RestaurantID, Date_To: order.Date_To,
                                                          Flag_Inhouse: order.Flag_Inhouse);

            if (order_favoriteid < 0)
            {

                _log.LogError(message: "{}", args: $"ERR # Не удалось создать заказ # {order_favoriteid} ## {Errors.GetErrorString(order_favoriteid)}");

                return order_favoriteid;
            }

            int orderid = _dl_Orders.Edit_OrderFavoriteID(ID: OrderID, Order_FavoriteID: order_favoriteid);

            if (orderid < 0)
            {

                _log.LogError(message: "{}", args: $"ERR # Не удалось изменить заказ # {orderid} ## {Errors.GetErrorString(order_favoriteid)}");

                return order_favoriteid;
            }

            _bl_Setup.Set(Key: Consts.SetupKey_Orders_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            _bl_Setup.Set(Key: Consts.SetupKey_OrdersFavorite_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            if (order.List_OrderDatas != null && order.List_OrderDatas.Count != 0)
            {
                foreach (COrder_Data elem in order.List_OrderDatas)
                {
                    int orderdata_id = _dl_Order_Favorite_Data.Add(OrderID: order_favoriteid, DishID: elem.DishID, Dish_ColorID: elem.Dish_ColorID,
                                                                   Dish_Price: elem.Dish_Price, Dish_Count: elem.Dish_Count, Dish_Summ: elem.Dish_Summ,
                                                                   Dish_Name: elem.Dish_Name, Dish_VolumeID: elem.Dish_VolumeID, Dish_VolumeValue: elem.Dish_VolumeValue,
                                                                   Dish_VolumePrice: elem.Dish_VolumePrice);

                    if (orderdata_id < 0)
                    {

                        _log.LogError(message: "{}", args: $"ERR # Не удалось создать содержимое заказ # {orderdata_id} ## {Errors.GetErrorString(orderdata_id)}");

                        return orderdata_id;
                    }

                    if (elem.List_Dish_Ingridients != null && elem.List_Dish_Ingridients.Count != 0)
                    {
                        foreach (COrder_Data_Ingridient elem_ingridient in elem.List_Dish_Ingridients)
                        {
                            int orderdataingridient_id = _dl_Order_Favorite_Data_Ingridient.Add(Order_Favorite_DataID: orderdata_id, IngridientID: elem_ingridient.IngridientID,
                                                                                       Ingridient_Price: elem_ingridient.Ingridient_Price,
                                                                                       Ingridient_Count: elem_ingridient.Ingridient_Count,
                                                                                       Ingridient_Summ: elem_ingridient.Ingridient_Summ,
                                                                                       Ingridient_Name: elem_ingridient.Ingridient_Name);
                            if (orderdataingridient_id < 0)
                            {

                                _log.LogError(message: "{}", args: $"ERR # Не удалось создать ингридиенты# {orderdataingridient_id} ## {Errors.GetErrorString(orderdataingridient_id)}");

                                return orderdataingridient_id;
                            }
                        }
                    }
                }
            }

            return order_favoriteid;
        }



        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Order_StatusID"></param>
        /// <param name="ClientID"></param>
        /// <param name="Name"></param>
        /// <param name="Phone"></param>
        /// <param name="Pay_TypeID"></param>
        /// <param name="RestaurantID"></param>
        /// <param name="Date_To"></param>
        /// <param name="Flag_Inhouse"></param>
        /// <param name="List_Order_Favorite_Datas"></param>
        /// <returns></returns>
        public int Edit(int ID, EOrder_Status Order_StatusID, int ClientID, string Name, string Phone, EPayType Pay_TypeID, int RestaurantID, DateTime? Date_To,
                        bool Flag_Inhouse, List<COrder_Favorite_Data> List_Order_Favorite_Datas)
        {
            COrder_Favorite? order_favorite = Get(ID);

            if (order_favorite == null)
            {

                _log.LogError(message: "{}", args: $"ERR # Избранный заказ с данным ID отсутствует## {Errors.Err200}"); //TODO: Привести в порядок код ошибки

                return Errors.Err200;
            }

            if (order_favorite.List_OrderDatas == null && order_favorite?.List_OrderDatas?.Count == 0)
            {

                _log.LogError(message: "{}", args: $"ERR # Список состава избранного заказа пустой## {Errors.Err200}"); //TODO: Привести в порядок код ошибки

                return Errors.Err200;
            }

            int order_favoriteid = _dl_Order_Favorite.Edit(ID: ID, Order_StatusID: Order_StatusID, ClientID: ClientID, Name: Name, Phone: Phone, Pay_TypeID: Pay_TypeID,
                                                           RestaurantID: RestaurantID, Date_To: Date_To, Flag_Inhouse: Flag_Inhouse);

            bool order_favoritedata_delete = _dl_Order_Favorite_Data.Del_All(order_favoriteid);

            if (!order_favoritedata_delete)
            {

                _log.LogError(message: "{}", args: $"ERR # {Errors.Err204} Ошибка удаления данных для избранного заказа # {order_favoriteid}");

                return Errors.Err204;
            }

            bool order_favoritedata_ingridient_delete = _dl_Order_Favorite_Data_Ingridient.Del_All(order_favoriteid);

            if (!order_favoritedata_ingridient_delete)
            {

                _log.LogError(message: "{}", args: $"ERR # Ошибка удаления ингредиентов для избранного заказа # {order_favoriteid}");

                return Errors.Err204;
            }

            foreach (COrder_Favorite_Data elem in List_Order_Favorite_Datas)
            {
                int orderfavoritedata_id = _dl_Order_Favorite_Data.Add(OrderID: order_favoriteid, DishID: elem.DishID, Dish_ColorID: elem.Dish_ColorID,
                                                                       Dish_Price: elem.Dish_Price, Dish_Count: elem.Dish_Count, Dish_Summ: elem.Dish_Summ,
                                                                       Dish_Name: elem.Dish_Name, Dish_VolumeID: elem.Dish_VolumeID, Dish_VolumeValue: elem.Dish_VolumeValue,
                                                                       Dish_VolumePrice: elem.Dish_VolumePrice);

                if (orderfavoritedata_id < 0)
                {

                    _log.LogError(message: "{}", args: $"ERR # Не удалось создать содержимое для избранного заказа # {orderfavoritedata_id} ## {Errors.GetErrorString(Errors.Err202)}");

                    return Errors.Err202;
                }

                if (elem.List_Dish_Ingridients != null && elem.List_Dish_Ingridients.Count != 0)
                {
                    foreach (COrder_Favorite_Data_Ingridient elem_ingridient in elem.List_Dish_Ingridients)
                    {
                        int orderdataingridient_id = _dl_Order_Favorite_Data_Ingridient.Add(Order_Favorite_DataID: orderfavoritedata_id, IngridientID: elem_ingridient.IngridientID,
                                                                                            Ingridient_Price: elem_ingridient.Ingridient_Price,
                                                                                            Ingridient_Count: elem_ingridient.Ingridient_Count,
                                                                                            Ingridient_Summ: elem_ingridient.Ingridient_Summ,
                                                                                            Ingridient_Name: elem_ingridient.Ingridient_Name);
                        if (orderdataingridient_id < 0)
                        {

                            _log.LogError(message: "{}", args: $"ERR # Не удалось создать ингридиенты# {orderdataingridient_id} ## {Errors.GetErrorString(Errors.Err202)}");

                            return Errors.Err202;
                        }
                    }
                }
            }

            return order_favoriteid;
        }





        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public int Del(int OrderID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Orders_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            _bl_Setup.Set(Key: Consts.SetupKey_OrdersFavorite_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Order_Favorite.Edit_FlagDel(ID: OrderID, Flag_Del_New: true);
        }


        /// <summary>
        /// Удаление всех заказов клиента (нужно при удалении аккаунта)
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public int Del_All(int ClientID)
        {
            int result = _dl_Order_Favorite.SetAll_FlagDel(ClientID: ClientID);

            if (result < 0)
            {

                _log.LogError(message: "{}", args: Errors.GetErrorString(result));

                return result;
            }

            return result;
        }

    }
}