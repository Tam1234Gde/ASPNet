using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Типы столиков
    /// </summary>
    public class Restaurant_Table_Type_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_Table_Type_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Restaurant_Table_Type? Get(int ID)
        {
            Restaurant_Table_Type? elem = (from p in _db.Restaurant_Table_Types where p.ID == (ERestaurant_Table_Type)ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        private IQueryable<Restaurant_Table_Type> GetQuery_NoTracking()
        {
            IQueryable<Restaurant_Table_Type> query = from p in _db.Restaurant_Table_Types.AsNoTracking() select p;
            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        public IQueryable<Restaurant_Table_Type> GetQuery()
        {
            IQueryable<Restaurant_Table_Type> query = from p in GetQuery_NoTracking() select p;
            return query;
        }


        /// <summary>
        /// Получение запроса по RestaurantID
        /// </summary>
        /// <returns></returns>
        public IQueryable<Restaurant_Table_Type> GetQueryByRestaurantID(int RestaurantID)
        {
            IQueryable<ERestaurant_Table_Type?>? elem = from p in _db.Restaurant_Tables
                                                        where p.RestaurantID == RestaurantID &&
                                                        p.Flag_IsAvailableApp &&
                                                        p.Restaurant_Hall != null && p.Restaurant_Hall.Flag_Active || p.Restaurant_Hall == null
                                                        select p.Restaurant_Table_TypeID;

            IQueryable<Restaurant_Table_Type> query = from p in GetQuery_NoTracking() where elem.Contains(p.ID) select p;
            return query;
        }

    }
}