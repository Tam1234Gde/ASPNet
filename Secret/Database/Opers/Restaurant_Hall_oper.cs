using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Залы в ресторане
    /// </summary>
    public class Restaurant_Hall_oper
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
        public Restaurant_Hall_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Restaurant_Hall? Get(int ID)
        {
            Restaurant_Hall? elem = (from p in _db.Restaurant_Halls.Include(x => x.Restaurant).Include(x => x.Restaurant_Table_Type) where p.ID == ID select p).FirstOrDefault();//TODO: MAE - GetQuery
            return elem;
        }


        /// <summary>
        /// Получение по RestaurantID
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        public Restaurant_Hall? GetByRestaurantID(int RestaurantID, ERestaurant_Table_Type Restaurant_Table_TypeID)
        {
            Restaurant_Hall? elem = (from p in _db.Restaurant_Halls.Include(x => x.Restaurant).Include(x => x.Restaurant_Table_Type)  //TODO: MAE - GetQuery
                                     where p.RestaurantID == RestaurantID && p.Restaurant_Table_TypeID == Restaurant_Table_TypeID && p.Flag_Del == false select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по RestaurantID и Restaurant_Table_TypeID
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <returns></returns>
        public Restaurant_Hall? Get(int RestaurantID, ERestaurant_Table_Type Restaurant_Table_TypeID, bool? Flag_Del_val = false)
        {
            Restaurant_Hall? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.RestaurantID == RestaurantID && p.Restaurant_Table_TypeID == Restaurant_Table_TypeID select p).FirstOrDefault();
            
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Hall> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Hall> query = from p in _db.Restaurant_Halls.Include(x => x.Restaurant).Include(x => x.Restaurant_Table_Type) select p;

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
        public IQueryable<Restaurant_Hall> GetQueryByRestaurantID(int RestaurantID, bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Hall> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.RestaurantID == RestaurantID select p;

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Hall> GetQueryByRestaurantID(int RestaurantID, ERestaurant_Table_Type? Restaurant_Table_TypeID, bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Hall> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.RestaurantID == RestaurantID select p;

            if (Restaurant_Table_TypeID.HasValue)
            {
                query = from p in query where p.Restaurant_Table_TypeID == Restaurant_Table_TypeID select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <param name="Restaurant_Image"></param>
        /// <param name="Flag_Active"></param>
        /// <returns></returns>
        public int Add(int RestaurantID, ERestaurant_Table_Type? Restaurant_Table_TypeID, byte[]? Restaurant_Image, bool Flag_Active, string? Name)
        {
            string loginfo = $"RestaurantID='{RestaurantID}' Restaurant_Table_TypeID='{Restaurant_Table_TypeID}' Restaurant_Image='{Restaurant_Image}'" +
                             $"Flag_Active='{Flag_Active}' Name='{Name}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_Hall elem = new()
            {
                RestaurantID = RestaurantID,
                Restaurant_Image = Restaurant_Image,
                Restaurant_Table_TypeID = Restaurant_Table_TypeID,
                Flag_Active = Flag_Active,
                Name = Name
            };

            _db.Restaurant_Halls.Add(elem);

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
        /// <param name="RestaurantID"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <param name="Restaurant_Image"></param>
        /// <param name="Flag_Active"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int Edit(int ID, int RestaurantID, ERestaurant_Table_Type? Restaurant_Table_TypeID, byte[]? Restaurant_Image, bool Flag_Active, string? Name)
        {
            string loginfo = $"ID='{ID}' RestaurantID='{RestaurantID}' Restaurant_Table_TypeID='{Restaurant_Table_TypeID}' Restaurant_Image='{Restaurant_Image}'" +
                             $"Flag_Active='{Flag_Active}' Name='{Name}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            Restaurant_Hall? elem = Get(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.RestaurantID = RestaurantID;
            elem.Restaurant_Table_TypeID = Restaurant_Table_TypeID;
            elem.Restaurant_Image = Restaurant_Image;
            elem.Flag_Active = Flag_Active;
            elem.Name = Name;

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
        public int Edit_FlagDel(int ID, bool Flag_Del_New, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' UserID='{UserID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            Restaurant_Hall? elem = Get(ID: ID);

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
                    _log.LogCritical(message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}", exception: ex);
                }
                return Errors.Err203;
            }
        }

    }
}