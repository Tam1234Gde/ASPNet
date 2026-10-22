using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Меню ресторана
    /// </summary>
    public class Restaurant_Menu_oper
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
        public Restaurant_Menu_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Costructor");
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Restaurant_Menu? Get(int ID, bool? Flag_Del_val = false)
        {
            Restaurant_Menu? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение по RestaurantID
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        public Restaurant_Menu? GetActive(int RestaurantID, bool? Flag_Del_val = false)
        {
            Restaurant_Menu? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.RestaurantID == RestaurantID && p.Flag_Active == true select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение по RestaurantID
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu>? GetActiveList(List<int> RestaurantIDs, bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Menu>? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where RestaurantIDs.Contains(p.RestaurantID) && p.Flag_Active == true select p);

            return elem;
        }


        /// <summary>
        /// Получение по RestaurantID
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        public IQueryable<int>? GetActiveID_ByMenuID(int MenuID, bool? Flag_Del_val = false)
        {
            IQueryable<int>? elem = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == MenuID && p.Flag_Active == true select p.ID;

            return elem;
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu> GetQuery_ByRestaurantID(int? RestaurantID, bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Menu> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.RestaurantID == RestaurantID select p;

            return query;
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Menu> query = from p in _db.Restaurant_Menus.Include(x => x.Restaurant) select p;

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
        private Restaurant_Menu? Get_Edit(int ID, bool? Flag_Del_val = false)
        {
            Restaurant_Menu? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Name">Название меню</param>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <param name="Flag_Active">True - Активное меню</param>
        /// <returns></returns>
        public int Add(string Name, int RestaurantID, bool Flag_Active)
        {
            string loginfo = $"Name='{Name}' RestaurantID='{RestaurantID}' Flag_Active='{Flag_Active}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_Menu elem = new()
            {
                Name = Name,
                RestaurantID = RestaurantID,
                Flag_Active = Flag_Active
            };

            _db.Restaurant_Menus.Add(elem);

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
        /// <param name="Name">Название меню</param>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <param name="Flag_Active">True - Активное меню</param>
        /// <returns></returns>
        public int Edit(int ID, string Name, int RestaurantID, bool Flag_Active)
        {
            string loginfo = $"ID='{ID}' Name='{Name}' RestaurantID='{RestaurantID}' Flag_Active='{Flag_Active}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
                
            Restaurant_Menu? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError("Err # ErrorCode='{}' ## {}", Errors.Err200_str, loginfo);
                return Errors.Err200;
            }

            elem.Name = Name;
            elem.RestaurantID = RestaurantID;
            elem.Flag_Active = Flag_Active;

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

            Restaurant_Menu? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError("Err # ErrorCode='{}' ## {}", Errors.Err200_str, loginfo);
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
                _log.LogError(exception: ex, "Err # ErrorCode='{}' ## {}", Errors.Err203_str, loginfo);
                return Errors.Err203;
            }
        }

    }
}