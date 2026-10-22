using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions_ForMenu;

namespace HIDE.RestAndRant.Database.Opers_ForMenu
{
    /// <summary>
    /// Класс для управления базой данных в части ингредиентов блюд меню
    /// </summary>
    public class Menu_Dish_Ingridient_Oper
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;

        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Menu_Dish_Ingridient_Oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Получение списка
        /// </summary>
        /// <returns></returns>
        public IQueryable<Menu_Dish_Ingridient_Def> GetQuery()
        {
            IQueryable<Menu_Dish_Ingridient_Def> query = from p in _db.Set<Menu_Dish_Ingridient_Def>().Include(x => x.Menu_Dish).Include(x => x.Menu_Ingridient).AsNoTracking()
                                                     select p;

            return query;
        }

        /// <summary>
        /// Получение списка для редактирования
        /// </summary>
        /// <returns></returns>
        public IQueryable<Menu_Dish_Ingridient_Def> GetQuery_Edit()
        {
            IQueryable<Menu_Dish_Ingridient_Def> query = from p in _db.Set<Menu_Dish_Ingridient_Def>().Include(x => x.Menu_Dish).Include(x => x.Menu_Ingridient)
                                                     select p;

            return query;
        }

        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Menu_Dish_Ingridient_Def? Get(int ID)
        {
            Menu_Dish_Ingridient_Def? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение по ID для редактирования
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Menu_Dish_Ingridient_Def? GetEdit(int ID)
        {
            Menu_Dish_Ingridient_Def? elem = (from p in GetQuery_Edit() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение списка по Menu_DishID
        /// </summary>
        /// <param name="Menu_DishID">Идентификатор блюда</param>
        /// <returns></returns>
        public IQueryable<Menu_Dish_Ingridient_Def> GetQuery_ByMenu_DishID(int Menu_DishID)
        {
            IQueryable<Menu_Dish_Ingridient_Def> query = from p in GetQuery() where p.Menu_DishID == Menu_DishID select p;
            return query;
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Menu_DishID">Код блюда</param>
        /// <param name="Menu_IngridientID">Код ингредиента</param>
        /// <returns></returns>
        public int Add(int Menu_DishID, int Menu_IngridientID)
        {
            string loginfo = $"Menu_DishID='{Menu_DishID}' Menu_IngridientID='{Menu_IngridientID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Dish_Ingridient_Def elem = new()
            {
                Menu_DishID = Menu_DishID,
                Menu_IngridientID = Menu_IngridientID
            };

            _db.Set<Menu_Dish_Ingridient_Def>().Add(elem);

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
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(exception: ex, message: "Err # Error='{0}' ## {1}", args: [Errors.Err202_str, loginfo]);
                }
                return Errors.Err202;
            }
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public int Delete(int ID)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Dish_Ingridient_Def? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: [Errors.Err200, Errors.Err200_str, loginfo]);
                }
                return Errors.Err200;
            }

            _db.Set<Menu_Dish_Ingridient_Def>().Remove(elem);

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
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}", args: [Errors.Err203_str, loginfo]);
                }
                return Errors.Err203;
            }
        }

        /// <summary>
        /// Удалить все ингредиенты блюда
        /// </summary>
        /// <param name="Menu_DishID">Код блюда</param>
        /// <returns></returns>
        public bool Delete_ByMenu_DishID(int Menu_DishID)
        {
            string loginfo = $"Menu_DishID='{Menu_DishID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            IQueryable<Menu_Dish_Ingridient_Def> items = from p in GetQuery_Edit() where p.Menu_DishID == Menu_DishID select p;

            _db.Set<Menu_Dish_Ingridient_Def>().RemoveRange(items);

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
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(exception: ex, message: "Err # {0} ## {1}", args: [Errors.Err203_str, loginfo]);
                }
                return false;
            }
        }
    }
}