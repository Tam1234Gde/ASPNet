using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions_ForMenu;

namespace HIDE.RestAndRant.Database.Opers_ForMenu
{
    /// <summary>
    /// Класс для управления базой данных в части опций меню ресторана
    /// </summary>
    public class Menu_Restaurant_Option_Oper
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
        public Menu_Restaurant_Option_Oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Получение списка
        /// </summary>
        /// <returns></returns>
        public IQueryable<Menu_Restaurant_Option_Def> GetQuery()
        {
            IQueryable<Menu_Restaurant_Option_Def> query = from p in _db.Set<Menu_Restaurant_Option_Def>().Include(x => x.Menu_Restaurant).Include(x => x.Menu_Dish).Include(x => x.Menu_Ingridient).Include(x => x.Menu_Ingridient_Group).Include(x => x.Menu).AsNoTracking()
                                                       select p;

            return query;
        }

        /// <summary>
        /// Получение списка для редактирования
        /// </summary>
        /// <returns></returns>
        public IQueryable<Menu_Restaurant_Option_Def> GetQuery_Edit()
        {
            IQueryable<Menu_Restaurant_Option_Def> query = from p in _db.Set<Menu_Restaurant_Option_Def>().Include(x => x.Menu_Restaurant).Include(x => x.Menu_Dish).Include(x => x.Menu_Ingridient).Include(x => x.Menu_Ingridient_Group).Include(x => x.Menu)
                                                       select p;

            return query;
        }

        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Menu_Restaurant_Option_Def? Get(int ID)
        {
            Menu_Restaurant_Option_Def? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение по ID для редактирования
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Menu_Restaurant_Option_Def? GetEdit(int ID)
        {
            Menu_Restaurant_Option_Def? elem = (from p in GetQuery_Edit() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение списка по Menu_RestaurantID
        /// </summary>
        /// <param name="Menu_RestaurantID">Идентификатор привязки меню к ресторану</param>
        /// <returns></returns>
        public IQueryable<Menu_Restaurant_Option_Def> GetQuery_ByMenu_RestaurantID(int Menu_RestaurantID)
        {
            IQueryable<Menu_Restaurant_Option_Def> query = from p in GetQuery() where p.Menu_RestaurantID == Menu_RestaurantID select p;
            return query;
        }

        /// <summary>
        /// Добавление опции для блюда
        /// </summary>
        /// <param name="Menu_RestaurantID">Код привязки меню к ресторану</param>
        /// <param name="Menu_DishID">Код блюда</param>
        /// <param name="MenuID">Код меню</param>
        /// <returns></returns>
        public int Add_Dish(int Menu_RestaurantID, int Menu_DishID, int MenuID)
        {
            string loginfo = $"Menu_RestaurantID='{Menu_RestaurantID}' Menu_DishID='{Menu_DishID}' MenuID='{MenuID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Restaurant_Option_Def elem = new()
            {
                Menu_RestaurantID = Menu_RestaurantID,
                Menu_DishID = Menu_DishID,
                MenuID = MenuID
            };

            _db.Set<Menu_Restaurant_Option_Def>().Add(elem);

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
        /// Добавление опции для ингредиента
        /// </summary>
        /// <param name="Menu_RestaurantID">Код привязки меню к ресторану</param>
        /// <param name="Menu_IngridientID">Код ингредиента</param>
        /// <param name="MenuID">Код меню</param>
        /// <returns></returns>
        public int Add_Ingridient(int Menu_RestaurantID, int Menu_IngridientID, int MenuID)
        {
            string loginfo = $"Menu_RestaurantID='{Menu_RestaurantID}' Menu_IngridientID='{Menu_IngridientID}' MenuID='{MenuID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Restaurant_Option_Def elem = new()
            {
                Menu_RestaurantID = Menu_RestaurantID,
                Menu_IngridientID = Menu_IngridientID,
                MenuID = MenuID
            };

            _db.Set<Menu_Restaurant_Option_Def>().Add(elem);

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
        /// Добавление опции для группы ингредиентов
        /// </summary>
        /// <param name="Menu_RestaurantID">Код привязки меню к ресторану</param>
        /// <param name="Menu_Ingridient_GroupID">Код группы ингредиентов</param>
        /// <param name="MenuID">Код меню</param>
        /// <returns></returns>
        public int Add_Ingridient_Group(int Menu_RestaurantID, int Menu_Ingridient_GroupID, int MenuID)
        {
            string loginfo = $"Menu_RestaurantID='{Menu_RestaurantID}' Menu_Ingridient_GroupID='{Menu_Ingridient_GroupID}' MenuID='{MenuID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Restaurant_Option_Def elem = new()
            {
                Menu_RestaurantID = Menu_RestaurantID,
                Menu_Ingridient_GroupID = Menu_Ingridient_GroupID,
                MenuID = MenuID
            };

            _db.Set<Menu_Restaurant_Option_Def>().Add(elem);

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

            Menu_Restaurant_Option_Def? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: [Errors.Err200, Errors.Err200_str, loginfo]);
                }
                return Errors.Err200;
            }

            _db.Set<Menu_Restaurant_Option_Def>().Remove(elem);

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
        /// Удалить все опции по Menu_RestaurantID
        /// </summary>
        /// <param name="Menu_RestaurantID">Код привязки меню к ресторану</param>
        /// <returns></returns>
        public bool Delete_ByMenu_RestaurantID(int Menu_RestaurantID)
        {
            string loginfo = $"Menu_RestaurantID='{Menu_RestaurantID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            IQueryable<Menu_Restaurant_Option_Def> items = from p in GetQuery_Edit() where p.Menu_RestaurantID == Menu_RestaurantID select p;

            _db.Set<Menu_Restaurant_Option_Def>().RemoveRange(items);

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