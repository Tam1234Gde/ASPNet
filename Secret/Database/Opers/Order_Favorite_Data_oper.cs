using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Данные заказа - БД
    /// </summary>
    public class Order_Favorite_Data_oper
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
        public Order_Favorite_Data_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Order_FavoriteID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns>Запланированные консультации клиента</returns>
        public IQueryable<Order_Favorite_Data> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Order_Favorite_Data> query = from p in _db.Order_Favorite_Datas.Include(x => x.OrderFavorite).Include(x => x.Dish).Include(x => x.Dish_Color)
                                                    .Include(x => x.Order_Data_Ingridients).AsNoTracking() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Order_FavoriteID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns>Запланированные консультации клиента</returns>
        public IQueryable<Order_Favorite_Data> GetQueryByOrder_FavoriteID(int Order_FavoriteID, bool? Flag_Del_val = false)
        {
            IQueryable<Order_Favorite_Data> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.OrderFavoriteID == Order_FavoriteID select p;

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
        public int Add(int OrderID, int DishID, int? Dish_ColorID, decimal Dish_Price, decimal Dish_Count, decimal Dish_Summ, string Dish_Name, int? Dish_VolumeID,
                       decimal? Dish_VolumeValue, decimal? Dish_VolumePrice)
        {
            string loginfo = $"OrderID='{OrderID}' DishID='{DishID}' Dish_ColorID='{Dish_ColorID}' Dish_Price='{Dish_Price}' Dish_Count='{Dish_Count}' " +
                             $"Dish_Summ='{Dish_Summ}' Dish_Name='{Dish_Name}' Dish_VolumeID='{Dish_VolumeID}' Dish_VolumeValue='{Dish_VolumeValue}' " +
                             $"Dish_VolumePrice='{Dish_VolumePrice}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            
            Order_Favorite_Data elem = new()
            {
                OrderFavoriteID = OrderID,
                DishID = DishID,
                Dish_ColorID = Dish_ColorID,
                Dish_Price = Dish_Price,
                Dish_Count = Dish_Count,
                Dish_Summ = Dish_Summ,
                Dish_Name = Dish_Name,
                Dish_VolumeID = Dish_VolumeID,
                Dish_VolumeValue = Dish_VolumeValue,
                Dish_VolumePrice = Dish_VolumePrice,
            };

            _db.Order_Favorite_Datas.Add(elem);

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
        /// Удалить все данные
        /// </summary>
        /// <param name="Order_FavoriteID">Код избранного заказа</param>
        /// <returns></returns>
        public bool Del_All(int Order_FavoriteID)
        {
            string loginfo = $"Order_FavoriteID='{Order_FavoriteID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            List<Order_Favorite_Data> order_favorite_datas = [.. GetQueryByOrder_FavoriteID(Order_FavoriteID: Order_FavoriteID)];
            if (order_favorite_datas.Count == 0)
            {
                return false;
            }

            order_favorite_datas.ForEach(x =>
            {
                x.Flag_Del = true;
            });

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");
                return false;
            }
        }

    }
}