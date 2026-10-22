using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Доступные типы оплаты в ресторане
    /// </summary>
    public class Restaurant_PayType_oper
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
        public Restaurant_PayType_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Restaurant_PayType? Get(int ID, int? RestaurantID = null, EOrder_Type? Order_TypeID = null, bool? Flag_Del_val = false)
        {
            Restaurant_PayType? elem = (from p in GetQuery(RestaurantID: RestaurantID, Order_TypeID: Order_TypeID, Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_PayType> GetQuery(int? RestaurantID = null, EOrder_Type? Order_TypeID = null, bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_PayType> query = from p in _db.Restaurant_PayTypes.Include(x => x.Restaurant).Include(x => x.Pay_Type).Include(x => x.Order_Type).AsNoTracking()
                                                   select p;

            if (RestaurantID.HasValue)
            {
                query = from p in query where p.RestaurantID == RestaurantID.Value select p;
            }

            if (Order_TypeID.HasValue)
            {
                query = from p in query where p.Order_TypeID == Order_TypeID.Value select p;
            }

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns></returns>
        private Restaurant_PayType? Get_Edit(int ID, int? RestaurantID = null, EOrder_Type? Order_TypeID = null, bool? Flag_Del_val = false)
        {
            Restaurant_PayType? elem = (from p in GetQuery(RestaurantID: RestaurantID, Order_TypeID: Order_TypeID, Flag_Del_val: Flag_Del_val)
                                        where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <param name="Order_TypeID">Тип заказа(С собой\Поесть в кафе\Доставка\К машине)</param>
        /// <param name="Pay_TypeID">Вид оплаты (картой, нал, ...)</param>
        /// <returns></returns>
        public int Add(int RestaurantID, EOrder_Type Order_TypeID, EPayType Pay_TypeID)
        {
            string loginfo = $"RestaurantID='{RestaurantID}' Order_TypeID='{Order_TypeID}' Pay_TypeID='{Pay_TypeID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_PayType elem = new()
            {
                Order_TypeID = Order_TypeID,
                Pay_TypeID = Pay_TypeID,
                RestaurantID = RestaurantID,
            };

            _db.Restaurant_PayTypes.Add(elem);

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
                _log.LogError(exception: ex, message: "{}", args: $"Err # ErrorCode='{Errors.Err202_str}' ## {loginfo}");
                return Errors.Err202;
            }
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <param name="Order_TypeID">Тип заказа(С собой\Поесть в кафе\Доставка\К машине)</param>
        /// <param name="Pay_TypeID">Вид оплаты (картой, нал, ...)</param>
        /// <returns></returns>
        public int Edit(int ID, int RestaurantID, EOrder_Type Order_TypeID, EPayType Pay_TypeID)
        {
            string loginfo = $"ID='{ID}' RestaurantID='{RestaurantID}' Order_TypeID='{Order_TypeID}' Pay_TypeID='{Pay_TypeID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_PayType? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode='{Errors.Err200_str}' ## {loginfo}");
                return Errors.Err200;
            }

            elem.RestaurantID = RestaurantID;
            elem.Order_TypeID = Order_TypeID;
            elem.Pay_TypeID = Pay_TypeID;

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
                _log.LogError(exception: ex, message: "{}", args: $"Err # ErrorCode='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение пометки на удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Flag_Del"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del}' UserID='{UserID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_PayType? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode='{Errors.Err200_str}' ## {loginfo}");
                return Errors.Err200;
            }

            elem.Flag_Del = Flag_Del;
            if (Flag_Del)
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
                _log.LogError(exception: ex, message: "{}", args: $"Err # ErrorCode='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }

    }
}