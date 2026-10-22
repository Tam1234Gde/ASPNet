using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Столики в ресторане
    /// </summary>
    public class Restaurant_Table_oper
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
        public Restaurant_Table_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Table> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Table> query = from p in _db.Restaurant_Tables.Include(x => x.Restaurant).Include(x => x.Restaurant_Table_Type).Include(x => x.Restaurant_Table_Status).Include(x => x.Restaurant_Hall)
                                                 select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Restaurant_Table? Get(int ID, bool? Flag_Del_val = false)
        {
            Restaurant_Table? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }

        /// <summary>
        /// Получение по QR_Code
        /// </summary>
        /// <param name="QR_Code"></param>
        /// <returns></returns>
        public Restaurant_Table? Get(string QR_Code, bool? Flag_Del_val = false)
        {
            Restaurant_Table? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.QR_Code == QR_Code select p).FirstOrDefault();

            return elem;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Table> GetQuery_ByRestarantID(int RestaurantID, bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Table> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.RestaurantID == RestaurantID select p;

            return query;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Table> GetQueryByRestaurant_HallID(int RestaurantID, int Restaurant_HallID, bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Table> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.RestaurantID == RestaurantID && p.Restaurant_HallID.HasValue &&
                                                p.Restaurant_HallID == Restaurant_HallID select p;

            return query;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Table> GetQueryByRestaurant_TableTypeID(int RestaurantID, int? Restaurant_HallID, int Restaurant_Table_TypeID, bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Table> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.RestaurantID == RestaurantID && p.Restaurant_Table_TypeID == (ERestaurant_Table_Type)Restaurant_Table_TypeID select p;
                                                 
            if (Restaurant_HallID.HasValue)
            {
                query = from p in query where p.Restaurant_HallID == Restaurant_HallID select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Name"></param>
        /// <param name="Coordinates"></param>
        /// <param name="Flag_IsAvailableApp"></param>
        /// <param name="Flag_IsAvailable"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <param name="Coordinates_X"></param>
        /// <param name="Coordinates_Y"></param>
        /// <param name="Coordinates_Rotation"></param>
        /// <param name="Dop_Text"></param>
        /// <param name="QR_Code"></param>
        /// <returns></returns>
        public int Add(int RestaurantID, string Name, string? Coordinates, bool Flag_IsAvailableApp, bool Flag_IsAvailable, ERestaurant_Table_Type? Restaurant_Table_TypeID,
                       decimal? Coordinates_X, decimal? Coordinates_Y, decimal? Coordinates_Rotation, string? Dop_Text, decimal? Price,
                       ERestaurant_Table_Status? Restaurant_Table_StatusID, string? QR_Code)
        {
            string loginfo = $"RestaurantID='{RestaurantID}' Name='{Name}' Coordinates='{Coordinates}' Flag_IsAvailableApp='{Flag_IsAvailableApp}'" +
                             $"Flag_IsAvailable='{Flag_IsAvailable}' Coordinates_X='{Coordinates_X}' Coordinates_Y='{Coordinates_Y}' Coordinates_Rotation='{Coordinates_Rotation}'" +
                             $"Dop_Text='{Dop_Text}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_Table elem = new()
            {
                Name = Name,
                Coordinates = Coordinates,
                RestaurantID = RestaurantID,
                Flag_IsAvailableApp = Flag_IsAvailableApp,
                Flag_IsAvailable = Flag_IsAvailable,
                Restaurant_Table_TypeID = Restaurant_Table_TypeID,
                Coordinates_X = Coordinates_X,
                Coordinates_Y = Coordinates_Y,
                Coordinates_Rotation = Coordinates_Rotation,
                Dop_Text = Dop_Text,
                Price = Price,
                Restaurant_Table_StatusID = Restaurant_Table_StatusID,
                QR_Code = QR_Code,
            };

            _db.Restaurant_Tables.Add(elem);

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
        /// <param name="Name"></param>
        /// <param name="Coordinates"></param>
        /// <param name="Flag_IsAvailableApp"></param>
        /// <param name="Flag_IsAvailable"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <param name="Coordinates_X"></param>
        /// <param name="Coordinates_Y"></param>
        /// <param name="Coordinates_Rotation"></param>
        /// <param name="Dop_Text"></param>
        /// <param name="QR_Code"></param>
        /// <returns></returns>
        public int Edit(int ID, int RestaurantID, string Name, string? Coordinates, bool Flag_IsAvailableApp, bool Flag_IsAvailable, ERestaurant_Table_Type? Restaurant_Table_TypeID,
                        decimal? Coordinates_X, decimal? Coordinates_Y, decimal? Coordinates_Rotation, string? Dop_Text, decimal? Price,
                        ERestaurant_Table_Status? Restaurant_Table_StatusID, string? QR_Code, int? Restaurant_HallID)
        {
            string loginfo = $"ID='{ID}' RestaurantID='{RestaurantID}' Name='{Name}' Coordinates='{Coordinates}' Flag_IsAvailableApp='{Flag_IsAvailableApp}'" +
                             $"Flag_IsAvailable='{Flag_IsAvailable}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_Table? elem = Get(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.RestaurantID = RestaurantID;
            elem.Name = Name;
            elem.Coordinates = Coordinates;
            elem.Flag_IsAvailableApp = Flag_IsAvailableApp;
            elem.Flag_IsAvailable = Flag_IsAvailable;
            elem.Restaurant_Table_TypeID = Restaurant_Table_TypeID;
            elem.Coordinates_X = Coordinates_X;
            elem.Coordinates_Y = Coordinates_Y;
            elem.Coordinates_Rotation = Coordinates_Rotation;
            elem.Dop_Text = Dop_Text;
            elem.Price = Price;
            elem.Restaurant_Table_StatusID = Restaurant_Table_StatusID;
            elem.QR_Code = QR_Code;
            elem.Restaurant_HallID = Restaurant_HallID;

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

            Restaurant_Table? elem = Get(ID: ID);

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

        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Flag_IsAvailable"></param>
        /// <returns></returns>
        public int Edit_Flag_IsAvailable(int ID, bool Flag_IsAvailable)
        {
            string loginfo = $"ID='{ID}' Flag_IsAvailable='{Flag_IsAvailable}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            
            Restaurant_Table? elem = Get(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Flag_IsAvailable = Flag_IsAvailable;

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

    }
}