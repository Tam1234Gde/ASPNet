using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Ингридиенты
    /// </summary>
    public class Dish_Ingridient_oper
    {
        /// <summary>
        /// Логер
        /// </summary>
        private readonly ILogger _log;

        
        /// <summary>
        /// Контекст Базы данных
        /// </summary>
        private readonly DBContext _db = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Dish_Ingridient_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Dish_Ingridient? Get(int ID, bool? Flag_Del_val = false)
        {
            Dish_Ingridient? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Dish_Ingridient? GetEdit(int ID, bool? Flag_Del_val = false)
        {
            Dish_Ingridient? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Dish_Ingridient> GetQuery_ByDishID(int DishID, bool? Flag_Del_val = false)
        {
            IQueryable<Dish_Ingridient> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.DishID == DishID select p;

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Dish_Ingridient> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Dish_Ingridient> query = from p in _db.Dish_Ingridients.Include(x => x.Dish).Include(x => x.Ingridient).AsNoTracking() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="IngridientID"></param>
        /// <param name="Price"></param>
        /// <param name="IngridientGroupID"></param>
        /// <returns></returns>
        public int Add(int DishID, int IngridientID, decimal Price, int? IngridientGroupID, int? Cooking_Minutes)
        {
            string loginfo = $"DishID='{DishID}' IngridientID='{IngridientID}' Price='{Price}' IngridientGroupID='{IngridientGroupID}' Cooking_Minutes='{Cooking_Minutes}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Ingridient elem = new()
            {
                DishID = DishID,
                IngridientID = IngridientID,
                Ingridient_Category_ID = IngridientGroupID,
                Price = Price,
                Cooking_Minutes = Cooking_Minutes,
            };

            _db.Dish_Ingridients.Add(elem);
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
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="DishID_New">Идентификатор блюда</param>
        /// <param name="IngridientID_New"></param>
        /// <param name="Price_New"></param>
        /// <param name="IngridientGroupID_New"></param>
        /// <returns></returns>
        public int Edit(int ID, int DishID_New, int IngridientID_New, decimal Price_New, int? IngridientGroupID_New, int? Cooking_Minutes_New)
        {
            string loginfo = $"ID='{ID}' DishID_New='{DishID_New}' IngridientID_New='{IngridientID_New}' Price_New='{Price_New}' " +
                             $"IngridientGroupID_New='{IngridientGroupID_New}' Cooking_Minutes_New='{Cooking_Minutes_New}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Ingridient? elem = Get(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.DishID = DishID_New;
            elem.Price = Price_New;
            elem.IngridientID = IngridientID_New;
            elem.Ingridient_Category_ID = IngridientGroupID_New;
            elem.Cooking_Minutes = Cooking_Minutes_New;

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
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Flag_Del_New">Удалено</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del_New, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Ingridient? elem = Get(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Flag_Del = Flag_Del_New;
            elem.Date_Del = DateTime.Now;
            elem.UserID_Del = UserID;

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