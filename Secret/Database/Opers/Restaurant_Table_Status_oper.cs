using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Статусы столиков
    /// </summary>
    public class Restaurant_Table_Status_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_Table_Status_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Restaurant_Table_Status? Get(int ID)
        {
            Restaurant_Table_Status? elem = (from p in _db.Restaurant_Table_Statuses where p.ID == (ERestaurant_Table_Status)ID select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        private IQueryable<Restaurant_Table_Status> GetQuery_NoTracking()
        {
            IQueryable<Restaurant_Table_Status> query = from p in _db.Restaurant_Table_Statuses.AsNoTracking() select p;
            return query;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        public IQueryable<Restaurant_Table_Status> GetQuery()
        {
            IQueryable<Restaurant_Table_Status> query = from p in GetQuery_NoTracking() select p;
            return query;
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="RestaurantTable_Image"></param>
        /// <returns></returns>
        public int Edit(int ID, string Name, byte[]? RestaurantTable_Image)
        {
            string loginfo = $"ID='{ID}' Name='{Name}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_Table_Status? elem = Get(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Name = Name;
            elem.RestaurantTable_Image = RestaurantTable_Image;

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