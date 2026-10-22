using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Категория блюда
    /// </summary>
    public class Dish_Category_oper
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly DBContext _db = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Dish_Category_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Dish_Category? Get(int ID)
        {
            Dish_Category? elem = (from p in GetQuery().Include(x => x.Dishes).Include(x => x.Parent).AsNoTracking() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }




        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Dish_Category? GetEdit(int ID, bool? Flag_Del_val = false)
        {
            Dish_Category? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val).Include(x => x.Dishes).Include(x => x.Parent) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val">Удалено</param>
        /// <returns></returns>
        public IQueryable<Dish_Category> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Dish_Category> query = from p in _db.Dish_Categories.Include(x => x.Dishes).Include(x => x.Parent) select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Restaurant_MenuID">Идентификатор меню ресторана</param>
        /// <param name="Flag_Del_val">Удалено</param>
        /// <returns></returns>
        public IQueryable<Dish_Category> GetQueryByRestaurant_MenuID(int Restaurant_MenuID, bool? Flag_Del_val = false)
        {
            // Получаем список IDs товаров выбранного меню
            IQueryable<int> queryRestaurantDishes = from p in _db.Restaurant_Menu_Dishes where p.Restaurant_MenuID == Restaurant_MenuID && p.Flag_Available == true && !p.Flag_Del select p.DishID;

            // Получаем список категорий в которых есть указанные товары
            IQueryable<int> queryDishes = from p in _db.Dishes.Include(x => x.Dish_Category).Include(x => x.Dish_Volumes).Include(x => x.Dish_Ingridients) 
                                          where queryRestaurantDishes.ToList().Contains(p.ID) select p.Dish_CategoryID;

            // Получаем список категорий в которых есть указанные товары
            IQueryable<Dish_Category> query = from p in GetQuery().Include(x => x.Dishes)
                                              where queryDishes.Distinct().ToList().Contains(p.ID) select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="OrderNum">Номер сортировки</param>
        /// <param name="Flag_AllowColors">Разрешить цвета</param>
        /// <param name="Cooking_Minutes">Время приготовления</param>
        /// <returns></returns>
        public int Add(string Name, int OrderNum, bool Flag_AllowColors, int? Cooking_Minutes, bool Flag_IsParent, int? ParentID, int[]? Work_Hours, bool Flag_UseDefaultWorkHours)
        {
            string loginfo = $"Name='{Name}' OrderNum='{OrderNum}' Flag_AllowColors='{Flag_AllowColors}' Cooking_Minutes='{Cooking_Minutes}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Category elem = new()
            {
                Name = Name,
                OrderNum = OrderNum,
                Flag_AllowColors = Flag_AllowColors,
                Cooking_Minutes = Cooking_Minutes,
                ParentID = ParentID,
                Work_Hours = Work_Hours,
                Flag_IsParent = Flag_IsParent,
                Flag_UseDefaultWorkHours = Flag_UseDefaultWorkHours
            };

            _db.Dish_Categories.Add(elem);
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
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Name_New">Новоре имя</param>
        /// <param name="OrderNum_New">Новая сортировка</param>
        /// <param name="Flag_AllowColors_New">Разрешить цвета</param>
        /// <param name="Cooking_Minutes_New">Новое время приготовления</param>
        /// <returns></returns>
        public int   Edit(int ID, string Name_New, int OrderNum_New, bool Flag_AllowColors_New, int? Cooking_Minutes_New, bool Flag_IsParent_New, int? ParentID_New, int[]? Work_Hours_New, bool Flag_UseDefaultWorkHours)
        {
            string loginfo = $"ID='{ID}' Name_New='{Name_New}' OrderNum_New='{OrderNum_New}' Flag_AllowColors_New='{Flag_AllowColors_New}' Cooking_Minutes_New='{Cooking_Minutes_New}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Category? elem = GetEdit(ID: ID, Flag_Del_val: null);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Name = Name_New;
            elem.OrderNum = OrderNum_New;
            elem.Flag_AllowColors = Flag_AllowColors_New;
            elem.Cooking_Minutes = Cooking_Minutes_New;
            elem.Flag_IsParent = Flag_IsParent_New;
            elem.ParentID = ParentID_New;
            elem.Work_Hours = Work_Hours_New;
            elem.Flag_UseDefaultWorkHours = Flag_UseDefaultWorkHours;

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
        /// <param name="ID">Идентификатор запси</param>
        /// <param name="Flag_Del_New">Удалено</param>
        /// <param name="UserID">Кто изменил</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del_New, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Category? elem = GetEdit(ID: ID, Flag_Del_val: null);

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