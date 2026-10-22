using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Order_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Order_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Order? Get(int ID)
        {
            Order? elem = (from p in GetQuery_NoTracking() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="Minuts_AddCompleted"></param>
        /// <returns></returns>
        public DateTime? OrderCancelled_Date(int ClientID, int Minuts_AddCompleted = 0)
        {
            DateTime? result = (from p in GetQuery_NoTracking()
                                where p.ClientID == ClientID && p.Order_StatusID == EOrder_Status.Cancelled
                                select p.Date_LastOrderStatusChange).Max();

            if (result != null && result.Value.AddMinutes(Minuts_AddCompleted) <= DateTime.Now)
            {
                return result;
            }

            return null;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private Order? Get_Edit(int ID)
        {
            Order? elem = (from p in _db.Orders.Include(x => x.Booking).Include(x => x.Order_Status).Include(x => x.Client).Include(x => x.Pay_Type)
                                      .Include(x => x.Restaurant).Include(x => x.ClientPay_Card_Pay).Include(x => x.Order_Deliveries).Include(x => x.Order_Datas)
                           where p.ID == ID select p).FirstOrDefault(); //TODO: MAE - GetQuery_EDit, Убрать Iclude
            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="Order_FavoriteID"></param>
        /// <returns></returns>
        public Order? GetByOrder_Favorite(int Order_FavoriteID)
        {
            Order? elem = (from p in _db.Orders.Include(x => x.Booking).Include(x => x.Order_Status).Include(x => x.Client).Include(x => x.Pay_Type)
                                      .Include(x => x.Restaurant).Include(x => x.ClientPay_Card_Pay).Include(x => x.Order_Deliveries).Include(x => x.Order_Datas)
                           where p.Order_FavoriteID != null && p.Order_FavoriteID == Order_FavoriteID select p).FirstOrDefault(); //TODO: MAE - GetQuery
            return elem;
        }


        /// <summary>
        /// Получение суммы заказа
        /// </summary>
        /// <param name="OrderID">Код заказа</param>
        /// <returns></returns>
        public decimal? GetOrderSum(int OrderID, out decimal? Delivery_Price)
        {
            Delivery_Price = (from p in _db.Orders.Include(x => x.Booking).Include(x => x.Order_Status).Include(x => x.Client).Include(x => x.Pay_Type)
                                      .Include(x => x.Restaurant).Include(x => x.ClientPay_Card_Pay).Include(x => x.Order_Deliveries).Include(x => x.Order_Datas)
                              where p.ID == OrderID select p.Delivery_Price).FirstOrDefault();
            IQueryable<Order_Data> order_dishes = from p in _db.Order_Datas where p.OrderID == OrderID select p;
            decimal? sum_dishes = order_dishes.Sum(p => p.Dish_Summ);
            //decimal? sum_ingredients = (from p in _db.Order_Data_Ingridients where order_dishes.Select(p => p.ID).Contains(p.Order_DataID) select p).Sum(p => p.Ingridient_Summ);
            return sum_dishes;
        }


        /// <summary>
        /// Получение по BookingID
        /// </summary>
        /// <param name="BookingID"></param>
        /// <returns></returns>
        public Order? GetByBookingID(int BookingID)
        {
            Order? elem = (from p in _db.Orders.Include(x => x.Booking).Include(x => x.Order_Status).Include(x => x.Client).Include(x => x.Pay_Type)
                                      .Include(x => x.Restaurant).Include(x => x.ClientPay_Card_Pay).Include(x => x.Order_Deliveries).Include(x => x.Order_Datas)
                           where p.BookingID != null && p.BookingID == BookingID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса (без отслеживания - для производительности)
        /// </summary>
        /// <returns></returns>
        private IQueryable<Order> GetQuery_NoTracking()
        {
            IQueryable<Order> query = _db.Orders.AsNoTracking().Include(x => x.Booking).Include(x => x.Order_Status).Include(x => x.Client).Include(x => x.Pay_Type).Include(x => x.Restaurant)
                                    .ThenInclude(r => r.City).Include(x => x.ClientPay_Card_Pay).Include(x => x.Order_Deliveries).Include(x => x.Order_Datas).ThenInclude(od => od.Dish)
                                    .Include(x => x.Order_Datas).ThenInclude(od => od.Dish_Color).Include(x => x.Order_Datas).ThenInclude(od => od.Order_Data_Ingridients).ThenInclude(odi => odi.Ingridient);
            //TODO: MAE+KMI - проработать 
            return query;

        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns>Все заказы</returns>
        public IQueryable<Order> GetQuery(int? ClientID = null, bool? Flag_Del_val = false)
        {
            IQueryable<Order> query = from p in GetQuery_NoTracking()
                                      select p;

            if (ClientID.HasValue)
            {
                query = from p in query where p.ClientID == ClientID select p;
            }

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns>Все заказы</returns>
        public IQueryable<Order> GetQueryConsole(bool? Flag_Del_val = false)
        {
            IQueryable<Order> query = from p in _db.Orders.Include(x => x.Booking).Include(x => x.Order_Status).Include(x => x.Client).Include(x => x.Pay_Type)
                                      .Include(x => x.Restaurant).Include(x => x.ClientPay_Card_Pay).Include(x => x.Order_Deliveries).Include(x => x.Order_Datas).AsNoTracking()
                                      where p.Order_StatusID != EOrder_Status.Cancelled && p.Order_StatusID != EOrder_Status.Complete && p.BookingID.HasValue
                                      select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns>Все заказы</returns>
        public IQueryable<Order> GetQueryForAdmin(int? RestaurantID, bool? User_Root)
        {
            IQueryable<Order> query = from p in GetQuery_NoTracking()
                                      where p.Flag_Del == false
                                      select p;

            if (User_Root.HasValue && User_Root == true)
            {
                return query;
            }

            if (RestaurantID.HasValue)
            {
                query = from p in query where p.RestaurantID == RestaurantID select p;
            }

            return query;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns>Все заказы</returns>
        public IQueryable<Order> GetQueryForOperator(int? RestaurantID, bool? User_Root)
        {
            IQueryable<Order> query = from p in (from p in _db.Orders.Include(x => x.Booking).Include(x => x.Order_Status).Include(x => x.Client).Include(x => x.Pay_Type)
                                      .Include(x => x.Restaurant).Include(x => x.ClientPay_Card_Pay).Include(x => x.Order_Deliveries).Include(x => x.Order_Datas)
                                                 select p)
                                      where p.Flag_Del == false &&
                                       p.Order_StatusID != EOrder_Status.Cancelled &&
                                       p.Order_StatusID != EOrder_Status.Complete &&
                                       p.Order_Datas != null && p.Order_Datas.Count != 0
                                      select p;


            if (User_Root.HasValue && User_Root == true)
            {
                return query;
            }

            if (RestaurantID.HasValue)
            {
                query = from p in query where p.RestaurantID == RestaurantID select p;
            }

            return query;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns>Все заказы</returns>
        public IQueryable<Order> GetQueryForAdminDates(DateTime? Date_From, DateTime? Date_To, int? RestaurantID, bool? User_Root)
        {
            IQueryable<Order> query = from p in GetQuery_NoTracking()
                                      where p.Flag_Del == false
                                      select p;

            if (Date_From.HasValue)
            {
                DateTime date_from = HIDE_Dates.GetDate_Start(Date_From.Value);
                query = from p in query where p.Date_Act >= date_from select p;
            }

            if (Date_To.HasValue)
            {
                DateTime date_to = HIDE_Dates.GetDate_End(Date_To.Value);
                query = from p in query where p.Date_Act <= date_to select p;
            }

            if (User_Root.HasValue && User_Root == true)
            {
                return query;
            }

            if (RestaurantID.HasValue)
            {
                query = from p in query where p.RestaurantID == RestaurantID select p;
            }

            return query;
        }

        /// <summary>
        /// Получение запроса для редактирования
        /// </summary>
        /// <returns></returns>
        public IQueryable<Order> GetQuery_Edit(int ClientID)
        {
            IQueryable<Order> query = from p in _db.Orders.Include(x => x.Booking).Include(x => x.Order_Status).Include(x => x.Client).Include(x => x.Pay_Type)
                                      .Include(x => x.Restaurant).Include(x => x.ClientPay_Card_Pay).Include(x => x.Order_Deliveries).Include(x => x.Order_Datas)
                                      where p.ClientID == ClientID
                                      select p;

            return query;
        }


        /// <summary>
        /// Добавление заказа
        /// </summary>
        /// <param name="Order_StatusID"></param>
        /// <param name="ClientID"></param>
        /// <param name="Name"></param>
        /// <param name="Phone"></param>
        /// <param name="Pay_TypeID"></param>
        /// <param name="RestaurantID"></param>
        /// <param name="Date_To"></param>
        /// <param name="ClientPay_Card_PayID"></param>
        /// <param name="Flag_Inhouse"></param>
        /// <returns></returns>
        public int Add(EOrder_Status Order_StatusID, int ClientID, string Name, string Phone, EPayType Pay_TypeID, int RestaurantID, DateTime? Date_To,
                       int? ClientPay_Card_PayID, bool Flag_Inhouse, bool Flag_Payed, string? Comment, string? Comment_AddToOrder, string? Comment_RemoveFromOrder,
                       decimal? Delivery_Price, EOrder_Type Order_TypeID, int? BookingID, decimal? Handing, decimal? Booking_Price)
        {
            string loginfo = $"Order_StatusID='{Order_StatusID}' ClientID='{ClientID}' Pay_TypeID='{Pay_TypeID}' RestaurantID='{RestaurantID}' Date_To='{Date_To}' " +
                             $"Flag_Inhouse='{Flag_Inhouse}' ClientPay_Card_PayID='{ClientPay_Card_PayID}' Delivery_Price='{Delivery_Price}' Order_TypeID='{Order_TypeID}' " +
                             $"Handing='{Handing}' Comment_AddToOrder='{Comment_AddToOrder}' Comment_RemoveFromOrder='{Comment_RemoveFromOrder}'" +
                             $"Booking_Price='{Booking_Price}'";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order elem = new()
            {
                Date_Act = DateTime.Now,
                Order_StatusID = Order_StatusID,
                Date_LastOrderStatusChange = DateTime.Now,
                ClientID = ClientID,
                Name = Name,
                Phone = Phone,
                Pay_TypeID = Pay_TypeID,
                RestaurantID = RestaurantID,
                Date_To = Date_To,
                Flag_Inhouse = Flag_Inhouse,
                ClientPay_Card_PayID = ClientPay_Card_PayID,
                Order_FavoriteID = null, //TODO: KA - Использовать это поле
                Flag_Payed = Flag_Payed,
                Comment = Comment,
                Comment_AddToOrder = Comment_AddToOrder,
                Comment_RemoveFromOrder = Comment_RemoveFromOrder,
                Delivery_Price = Delivery_Price,
                Booking_Price = Booking_Price,
                Order_TypeID = Order_TypeID,
                BookingID = BookingID,
                Handing = Handing

            };

            _db.Orders.Add(elem);

            try
            {
                _db.SaveChanges();

                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {

                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err202_str}' ## {loginfo}");

                return Errors.Err202;
            }

        }


        /// <summary>
        /// Добавление заказа
        /// </summary>
        /// <param name="Order_StatusID"></param>
        /// <param name="ClientID"></param>
        /// <param name="Name"></param>
        /// <param name="Phone"></param>
        /// <param name="Pay_TypeID"></param>
        /// <param name="RestaurantID"></param>
        /// <param name="Date_To"></param>
        /// <param name="ClientPay_Card_PayID"></param>
        /// <param name="Flag_Inhouse"></param>
        /// <returns></returns>
        public int AddByOperator(EOrder_Status Order_StatusID, int ClientID, string Name, string Phone, EPayType Pay_TypeID, int RestaurantID, DateTime? Date_To,
                       int? ClientPay_Card_PayID, bool Flag_Inhouse, bool Flag_Payed, string? Comment)
        {
            string loginfo = $"Order_StatusID='{Order_StatusID}' ClientID='{ClientID}' Pay_TypeID='{Pay_TypeID}' RestaurantID='{RestaurantID}' Date_To='{Date_To}' " +
                             $"Flag_Inhouse='{Flag_Inhouse}' ClientPay_Card_PayID='{ClientPay_Card_PayID}' ";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order elem = new()
            {
                Date_Act = DateTime.Now,
                Order_StatusID = Order_StatusID,
                Date_LastOrderStatusChange = DateTime.Now,
                ClientID = ClientID,
                Name = Name,
                Phone = Phone,
                Pay_TypeID = Pay_TypeID,
                RestaurantID = RestaurantID,
                Date_To = Date_To,
                Flag_Inhouse = Flag_Inhouse,
                ClientPay_Card_PayID = ClientPay_Card_PayID,
                Order_FavoriteID = null,
                Flag_Payed = Flag_Payed,
                Comment = Comment

            };

            _db.Orders.Add(elem);

            try
            {
                _db.SaveChanges();

                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {

                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err202_str}' ## {loginfo}");

                return Errors.Err202;
            }

        }


        /// <summary>
        /// Изменение пометки на удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Order_StatusID"></param>
        /// <param name="Flag_Payed"></param>
        /// <returns></returns>
        public int Edit_Status(int ID, EOrder_Status Order_StatusID, bool? Flag_Payed)
        {
            string loginfo = $"ID='{ID}' Order_StatusID_New='{Order_StatusID}' Flag_Payed_New='{Flag_Payed}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order? elem = Get_Edit(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Order_StatusID = Order_StatusID;
            elem.Date_LastOrderStatusChange = DateTime.Now;

            if (Flag_Payed.HasValue)
            {
                elem.Flag_Payed = Flag_Payed.Value;
            }

            try
            {
                _db.SaveChanges();

                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", exception: ex, args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение Order_FavoriteID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Order_FavoriteID"></param>
        /// <returns></returns>
        public int Edit_OrderFavoriteID(int ID, int? Order_FavoriteID)
        {
            string loginfo = $"ID='{ID}' Order_FavoriteID='{Order_FavoriteID}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order? elem = Get_Edit(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Order_FavoriteID = Order_FavoriteID;

            try
            {
                _db.SaveChanges();

                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", exception: ex, args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение пометки на удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Flag_Del_New"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del_New, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order? elem = Get_Edit(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Flag_Del = Flag_Del_New;
            if (Flag_Del_New)
            {
                elem.Date_Del = DateTime.Now;
                elem.UserID_Del = UserID;
            }
            else
            {
                elem.Date_Del = null;
                elem.UserID_Del = null;
            }

            try
            {
                _db.SaveChanges();

                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", exception: ex, args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }


        /// <summary>
        /// Помечает на удаление все заказы пользователя
        /// </summary>
        /// <returns>Код клиента если все прошло успешно или код ошибки</returns>
        public int SetAll_FlagDel(int ClientID)
        {
            string loginfo = $"ClientID='{ClientID}' ";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            List<Order> list = [.. GetQuery_Edit(ClientID: ClientID)];
            list.ForEach(a => { a.Flag_Del = true; a.Date_Del = DateTime.Now; });

            try
            {
                _db.SaveChanges();

                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return ClientID;
            }
            catch (Exception ex)
            {

                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");

                return Errors.Err203;
            }
        }

        /// <summary>
        /// Редактирование ClientPay_Card_PayID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ClientPay_Card_PayID"></param>
        /// <returns></returns>
        public int Edit_ClientPayCardPayID(int ID, int? ClientPay_Card_PayID)
        {
            string loginfo = $"ID='{ID}' ClientPay_Card_PayID='{ClientPay_Card_PayID}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order? elem = Get_Edit(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.ClientPay_Card_PayID = ClientPay_Card_PayID;

            try
            {
                _db.SaveChanges();

                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return (int)elem.ID;
            }
            catch (Exception ex)
            {

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }


    }
}