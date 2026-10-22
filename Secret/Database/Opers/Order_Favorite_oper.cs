using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Избранный заказ
    /// </summary>
    public class Order_Favorite_oper
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст БД
        /// </summary>
        private readonly DBContext _db = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Order_Favorite_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Order_Favorite? Get_Edit(int ID)
        {
            Order_Favorite? elem = (from p in _db.Order_Favorites where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Order_Favorite? Get(int ID)
        {
            Order_Favorite? elem = (from p in GetQuery_NoTracking() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        private IQueryable<Order_Favorite> GetQuery_NoTracking(bool? Flag_Del_val = false)
        {
            IQueryable<Order_Favorite> query = from p in _db.Order_Favorites.Include(x => x.Order_Status).Include(x => x.Client).Include(x => x.Pay_Type).Include(x => x.Restaurant).Include(x => x.Order_Datas).AsNoTracking() select p; //TODO: MAE - GetQuery

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
        /// <returns></returns>
        public IQueryable<Order_Favorite> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Order_Favorite> query = from p in GetQuery_NoTracking() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }

        /// <summary>
        /// Получение запроса для редактирования
        /// </summary>
        /// <returns></returns>
        public IQueryable<Order_Favorite> GetQuery_Edit(int ClientID)
        {
            IQueryable<Order_Favorite> query = from p in _db.Order_Favorites.Include(x => x.Order_Status).Include(x => x.Client).Include(x => x.Pay_Type).Include(x => x.Restaurant).Include(x => x.Order_Datas) //TODO: MAE - GetQuery
                                               where p.ClientID == ClientID
                                               select p;

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Order_Favorite> GetQueryByClientID(int ClientID, bool? Flag_Del_val = false)
        {
            IQueryable<Order_Favorite> query = from p in GetQuery_NoTracking()
                                               where p.ClientID == ClientID
                                               select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public int Add(EOrder_Status Order_StatusID, int ClientID, string Name, string Phone, EPayType Pay_TypeID, int RestaurantID, DateTime? Date_To,
                       bool Flag_Inhouse)
        {
            string loginfo = $"Order_StatusID='{Order_StatusID}' ClientID='{ClientID}' Pay_TypeID='{Pay_TypeID}' RestaurantID='{RestaurantID}' Date_To='{Date_To}' " +
                             $"Flag_Inhouse='{Flag_Inhouse}' ";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order_Favorite elem = new()
            {
                Date_Act = DateTime.Now,
                Date_LastEdit = DateTime.Now,
                Order_StatusID = Order_StatusID,
                ClientID = ClientID,
                Name = Name,
                Phone = Phone,
                Pay_TypeID = Pay_TypeID,
                RestaurantID = RestaurantID,
                Date_To = Date_To,
                Flag_Inhouse = Flag_Inhouse,
            };

            _db.Order_Favorites.Add(elem);

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
        /// <returns></returns>
        public int Edit(int ID, EOrder_Status Order_StatusID, int ClientID, string Name, string Phone, EPayType Pay_TypeID, int RestaurantID, DateTime? Date_To,
                        bool Flag_Inhouse)
        {
            string loginfo = $"ID='{ID}' Order_StatusID='{Order_StatusID}' ClientID='{ClientID}' Name='{Name}' " +
                             $"Phone='{Phone}' Pay_TypeID='{Pay_TypeID}' RestaurantID='{RestaurantID}' Date_To='{Date_To}' Flag_Inhouse='{Flag_Inhouse}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order_Favorite? elem = Get_Edit(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Date_LastEdit = DateTime.Now;
            elem.Order_StatusID = Order_StatusID;
            elem.ClientID = ClientID;
            elem.Name = Name;
            elem.Phone = Phone;
            elem.Pay_TypeID = Pay_TypeID;
            elem.RestaurantID = RestaurantID;
            elem.Date_To = Date_To;
            elem.Flag_Inhouse = Flag_Inhouse;

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
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение пометки на удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Flag_Del_New"></param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del_New)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order_Favorite? elem = Get_Edit(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Flag_Del = Flag_Del_New;
            if (Flag_Del_New)
            {
                elem.Date_Del = DateTime.Now;
            }
            else
            {
                elem.Date_Del = null;
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
        /// Помечает на удаление все избранные заказы пользователя
        /// </summary>
        /// <returns>Код клиента если все прошло успешно или код ошибки</returns>
        public int SetAll_FlagDel(int ClientID)
        {
            string loginfo = $"ClientID='{ClientID}' ";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            List<Order_Favorite> list = [.. GetQuery_Edit(ClientID: ClientID)];
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


    }
}
