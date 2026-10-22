using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    public class Order_Favorite_Data_Ingridient_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Order_Favorite_Data_Ingridient_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Order_Favorite_Data_Ingridient> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Order_Favorite_Data_Ingridient> query = from p in _db.Order_Favorites_Data_Ingridients.Include(x => x.Order_Favorite_Data).Include(x => x.Ingridient).AsNoTracking() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Order_Favorite_DataID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Order_Favorite_Data_Ingridient> GetQueryByOrder_Favorite_DataID(int Order_Favorite_DataID, bool? Flag_Del_val = false)
        {
            IQueryable<Order_Favorite_Data_Ingridient> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.Order_Favorite_DataID == Order_Favorite_DataID select p;
                                                               
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
        public int Add(int Order_Favorite_DataID, int IngridientID, decimal Ingridient_Price, decimal Ingridient_Count, decimal Ingridient_Summ, string Ingridient_Name)
        {
            string loginfo = $"Order_Favorite_DataID='{Order_Favorite_DataID}' IngridientID='{IngridientID}' Ingridient_Price='{Ingridient_Price}' " +
                             $"Ingridient_Count='{Ingridient_Count}' Ingridient_Summ='{Ingridient_Summ}' Ingridient_Name='{Ingridient_Name}' ";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order_Favorite_Data_Ingridient elem = new()
            {
                Order_Favorite_DataID = Order_Favorite_DataID,
                IngridientID = IngridientID,
                Ingridient_Price = Ingridient_Price,
                Ingridient_Count = Ingridient_Count,
                Ingridient_Summ = Ingridient_Summ,
                Ingridient_Name = Ingridient_Name
            };

            _db.Order_Favorites_Data_Ingridients.Add(elem);

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
        /// <param name="Order_Favorite_DataID">Код содержимого избранного заказа</param>
        /// <returns></returns>
        public bool Del_All(int Order_Favorite_DataID)
        {
            string loginfo = $"Order_Favorite_DataID='{Order_Favorite_DataID}'";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            List<Order_Favorite_Data_Ingridient> order_favorite_ingredient_datas = [.. GetQueryByOrder_Favorite_DataID(Order_Favorite_DataID: Order_Favorite_DataID)];
            if (order_favorite_ingredient_datas.Count == 0)
            {
                return true;
            }

            order_favorite_ingredient_datas.ForEach(x =>
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
            catch (Exception e)
            {

                _log.LogError(exception: e, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");

                return false;
            }
        }

    }
}