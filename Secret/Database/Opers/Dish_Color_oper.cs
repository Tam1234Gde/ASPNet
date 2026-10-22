using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Цвета
    /// </summary>
    public class Dish_Color_oper
    {
        /// <summary>
        /// Логер
        /// </summary>
        private readonly ILogger _log;
        
        
        /// <summary>
        /// Контекст БД 
        /// </summary>
        private readonly DBContext _db = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Dish_Color_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентифиткатор записи</param>
        /// <returns></returns>
        public Dish_Color? Get(int ID, bool? Flag_Del_val = false)
        {
            Dish_Color? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault(); //TODO: MAE - GetQuery
            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентифиткатор записи</param>
        /// <returns></returns>
        public Dish_Color? GetEdit(int ID, bool? Flag_Del_val = false)
        {
            Dish_Color? elem = (from p in GetQuery_Edit(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="DishID">Идентификатор блюда</param>
        /// <param name="Flag_Del_val">Удалено</param>
        /// <returns></returns>
        public IQueryable<Dish_Color> GetQuery_ByDishID(int DishID, bool? Flag_Del_val = false)
        {
            IQueryable<Dish_Color> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.DishID == DishID select p;

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val">Удалено</param>
        /// <returns></returns>
        public IQueryable<Dish_Color> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Dish_Color> query = from p in _db.Dish_Colors.Include(x => x.Dish).Include(x => x.Color).AsNoTracking() select p;


            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        public IQueryable<Dish_Color> GetQuery_Edit(bool? Flag_Del_val = false)
        {
            IQueryable<Dish_Color> query = from p in _db.Dish_Colors select p;


            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="DishID">Идентификатор блюда</param>
        /// <param name="ColorID">Идентфикатор цвета</param>
        /// <returns></returns>
        public int Add(int DishID, int ColorID)
        {
            string loginfo = $"DishID='{DishID}' ColorID='{ColorID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Color elem = new()
            {
                DishID = DishID,
                ColorID = ColorID
            };

            _db.Dish_Colors.Add(elem);
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
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err202_str}' ## {loginfo}");
                }
                return Errors.Err202;
            }
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Идентфикатор записи</param>
        /// <param name="DishID_New">Идентфиткатор блюда</param>
        /// <param name="ColorID_New">Идентификатор цвета</param>
        /// <returns></returns>
        public int Edit(int ID, int DishID_New, int ColorID_New)
        {
            string loginfo = $"ID='{ID}' DishID_New='{DishID_New}' ColorID_New='{ColorID_New}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Color? elem = Get(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.DishID = DishID_New;
            elem.ColorID = ColorID_New;

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
        /// <param name="ID">Идентфикатор записи</param>
        /// <param name="Flag_Del_New">Удалено</param>
        /// <param name="UserID">Кто Изменил</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del_New, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Color? elem = Get(ID: ID);

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
    }
}