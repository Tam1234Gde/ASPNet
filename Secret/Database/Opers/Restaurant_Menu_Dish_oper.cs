using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Блюда меню ресторана
    /// </summary>
    public class Restaurant_Menu_Dish_oper
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
        public Restaurant_Menu_Dish_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Restaurant_Menu_Dish? Get(int ID, bool? FlagDelVal = false)
        {
            Restaurant_Menu_Dish? elem = (from p in GetQuery(FlagDelVal: FlagDelVal) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение по списку ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu_Dish>? Get_ByListIDs(List<int> IDs, bool? FlagDelVal = false)
        {
            IQueryable<Restaurant_Menu_Dish>? elem = from p in GetQuery(FlagDelVal: FlagDelVal) where IDs.Contains(p.DishID) select p;

            return elem;
        }


        /// <summary>
        /// Получение по DishID
        /// </summary>
        /// <param name="DishID"></param>
        /// <returns></returns>
        public Restaurant_Menu_Dish? GetByDishID(int DishID, int RestaurantID, bool? FlagDelVal = false)
        {
            Restaurant_Menu_Dish? elem = (from p in GetQuery(FlagDelVal: FlagDelVal) where p.DishID == DishID && p.Restaurant_Menu != null &&
                                          p.Restaurant_Menu.RestaurantID == RestaurantID && p.Flag_Available && !p.Flag_Del select p).FirstOrDefault();              
            return elem;
        }


        /// <summary>
        /// Получение по DishID
        /// </summary>
        /// <param name="DishID"></param>
        /// <returns></returns>
        public IQueryable<int>? GetListIDsByDishID(int DishID, bool? FlagDelVal = false)
        {
            IQueryable<int>? elem = from p in GetQuery(FlagDelVal: FlagDelVal)
                                    where p.DishID == DishID && p.Restaurant_Menu != null &&
                                    p.Flag_Available && !p.Flag_Del
                                    select p.Restaurant_MenuID;
            return elem;
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu_Dish> GetQuery(bool? FlagDelVal = false, bool Edit = false) //TODO: KMI - Сделать так везде?
        {
            IQueryable<Restaurant_Menu_Dish> query = from p in _db.Restaurant_Menu_Dishes select p;

            if (FlagDelVal.HasValue)
            {
                query = from p in query where p.Flag_Del == FlagDelVal select p;
            }

            if (Edit)
            {
                return query;
            }
            else
            {
                return query.Include(x => x.Restaurant_Menu).Include(x => x.Dish).ThenInclude(d => d.Dish_Category)
                            .Include(x => x.Restaurant_Menu_Dish_Volumes).ThenInclude(v => v.Volume)
                            .Include(x => x.Restaurant_Menu_Dish_Ingredients).ThenInclude(i => i.Ingridient).AsNoTracking();
            }
        }




        /// <summary>
        /// Получение списка
        /// </summary>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu_Dish> GetQuery_ByRestarauntID(int RestaurantMenuID, bool? FlagDelVal = false)
        {
            return from p in GetQuery(FlagDelVal: FlagDelVal)
                   where p.Restaurant_MenuID == RestaurantMenuID
                      && p.Dish != null
                      && p.Dish.Dish_Category != null
                      && !p.Dish.Dish_Category.Flag_Del
                      && !p.Dish.FlagNoActive
                   select p;
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="RestaurantMenuID"></param>
        /// <param name="FlagDelVal"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu_Dish> GetQueryByDishID(int DishID, bool? FlagDelVal = false)
        {
            IQueryable<Restaurant_Menu_Dish> query = from p in GetQuery(FlagDelVal: FlagDelVal) where p.DishID == DishID select p;

            return query;
        }


        /// <summary>
        /// Получение списка блюд по Restaurant_MenuID и Dish_CategoryID
        /// </summary>
        /// <param name="RestaurantMenuID"></param>
        /// <param name="Dish_CategoryID"></param>
        /// <param name="FlagDelVal"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu_Dish> GetQueryForAdmin(int RestaurantMenuID, int? Dish_CategoryID, bool? FlagDelVal = false)
        {
            IQueryable<Restaurant_Menu_Dish> query = from p in GetQuery(FlagDelVal: FlagDelVal) where p.Restaurant_MenuID == RestaurantMenuID && p.Dish != null &&
                                                     p.Dish.Dish_CategoryID == Dish_CategoryID && !p.Dish.FlagNoActive && !p.Flag_Del select p;


            return query;
        }



        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns></returns>
        private Restaurant_Menu_Dish? Get_Edit(int ID, bool? FlagDelVal = false)
        {
            Restaurant_Menu_Dish? elem = (from p in GetQuery(FlagDelVal: FlagDelVal, Edit: true) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получить все блюда ресторана
        /// </summary>
        /// <returns></returns>
        private IQueryable<Restaurant_Menu_Dish> GetQuery_Edit_ByRestaurantMenuID(int RestaurantMenuID)
        {
            IQueryable<Restaurant_Menu_Dish> query = from p in GetQuery(Edit: true) where p.Restaurant_MenuID == RestaurantMenuID select p;

            return query;
        }


        /// <summary>
        /// Коммит
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            try
            {
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # ErrorCode='{Errors.Err205}' ## Error='{Errors.Err202_str}'");
                }
                return false;
            }
        }


        /// <summary>
        /// Коммит
        /// </summary>
        /// <param name="Restaurant_MenuID">Код меню ресторана</param>
        /// <param name="DishID">Код блюда</param>
        /// <param name="Flag_Available">True - Доступен</param>
        /// <param name="Price">Стоимость</param>
        /// <param name="OrderNum">Порядок отображения</param>
        public void Add_NoCommit(int Restaurant_MenuID, int DishID, bool Flag_Available, decimal? Price, int OrderNum, bool FlagNew)
        {
            string loginfo = $"Restaurant_MenuID='{Restaurant_MenuID}' DishID='{DishID}' Flag_Available='{Flag_Available}' Price='{Price}' OrderNum='{OrderNum}'" +
                             $"FlagNew='{FlagNew}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_Menu_Dish elem = new()
            {
                Restaurant_MenuID = Restaurant_MenuID,
                DishID = DishID,
                Flag_Available = Flag_Available,
                Price = Price,
                OrderNum = OrderNum,
                FlagNew = FlagNew,
            };

            _db.Restaurant_Menu_Dishes.Add(elem);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Restaurant_MenuID">Код меню ресторана</param>
        /// <param name="DishID">Код блюда</param>
        /// <param name="Flag_Available">True - Доступен</param>
        /// <param name="Price">Стоимость</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <returns></returns>
        public int Add(int Restaurant_MenuID, int DishID, bool Flag_Available, decimal? Price, int OrderNum, bool FlagNew)
        {
            string loginfo = $"Restaurant_MenuID='{Restaurant_MenuID}' DishID='{DishID}' Flag_Available='{Flag_Available}' Price='{Price}' OrderNum='{OrderNum}'" +
                             $"FlagNew='{FlagNew}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_Menu_Dish elem = new()
            {
                Restaurant_MenuID = Restaurant_MenuID,
                DishID = DishID,
                Flag_Available = Flag_Available,
                Price = Price,
                OrderNum = OrderNum,
                FlagNew = FlagNew,
            };

            _db.Restaurant_Menu_Dishes.Add(elem);

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
        /// <param name="Restaurant_MenuID">Код меню ресторана</param>
        /// <param name="DishID">Код блюда</param>
        /// <param name="Flag_Available">True - Доступен</param>
        /// <param name="Price">Стоимость</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <returns></returns>
        public int Edit(int ID, int Restaurant_MenuID, int DishID, bool Flag_Available, decimal? Price, int OrderNum, bool FlagNew)
        {
            string loginfo = $"ID='{ID}' Restaurant_MenuID='{Restaurant_MenuID}' DishID='{DishID}' Flag_Available='{Flag_Available}' Price='{Price}' OrderNum='{OrderNum}'" +
                             $"FlagNew='{FlagNew}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_Menu_Dish? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode='{Errors.Err200_str}' ## {loginfo}");
                return Errors.Err200;
            }

            elem.Restaurant_MenuID = Restaurant_MenuID;
            elem.DishID = DishID;
            elem.Flag_Available = Flag_Available;
            elem.Price = Price;
            elem.OrderNum = OrderNum;
            elem.FlagNew = FlagNew;

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
            
            Restaurant_Menu_Dish? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode='{Errors.Err200_str}' ## {loginfo}");
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


        /// <summary>
        /// Изменение флага Flag_Available
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Edit_Flag_Available(int ID, bool Flag_Available)
        {
            string loginfo = $"ID='{ID}' Flag_Available='{Flag_Available}''";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            
            Restaurant_Menu_Dish? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode='{Errors.Err200_str}' ## {loginfo}");
                return Errors.Err200;
            }

            elem.Flag_Available = Flag_Available;

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
        /// Изменение пометки на удаление для всех блюд ресторана
        /// </summary>
        /// <param name="Restaurant_MenuID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Edit_FlagDelAll(int Restaurant_MenuID, int UserID)
        {
            string loginfo = $"ID='{Restaurant_MenuID}' UserID='{UserID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            List<Restaurant_Menu_Dish> list = [.. GetQuery_Edit_ByRestaurantMenuID(Restaurant_MenuID)];
            list.ForEach(elem => { elem.Flag_Del = true; elem.Date_Del = DateTime.Now; elem.UserID_Del = UserID; });

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return 200;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, "Err # ErrorCode='{}' ## {}", Errors.Err203_str, loginfo);
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Удалить все товары категории
        /// </summary>
        /// <param name="Restaurant_MenuID"></param>
        /// <param name="Dish_CategoryID"></param>
        /// <returns></returns>
        public int Edit_FlagDelByDish_CategoryID(int Restaurant_MenuID, int Dish_CategoryID)
        {
            string loginfo = $"Restaurant_MenuID='{Restaurant_MenuID}' Dish_CategoryID='{Dish_CategoryID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            // Получаем список всех нужных блюд
            IQueryable<Restaurant_Menu_Dish> queryDishes = from p in _db.Restaurant_Menu_Dishes where p.Restaurant_MenuID == Restaurant_MenuID &&
                                                           p.Dish != null && p.Dish.Dish_CategoryID == Dish_CategoryID select p;
                                                          
            // Помечаем недоступными 
            queryDishes.ToList().ForEach(elem => { elem.Flag_Available = false; });

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return 200;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args: $"Err # ErrorCode='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Открыть все товары категории
        /// </summary>
        /// <param name="Restaurant_MenuID"></param>
        /// <param name="Dish_CategoryID"></param>
        /// <returns></returns>
        public int Edit_Flag_AvailableByDish_CategoryID(int Restaurant_MenuID, int Dish_CategoryID)
        {
            string loginfo = $"Restaurant_MenuID='{Restaurant_MenuID}' Dish_CategoryID='{Dish_CategoryID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            // Получаем список всех нужных блюд
            IQueryable<Restaurant_Menu_Dish> queryDishes = from p in _db.Restaurant_Menu_Dishes where p.Restaurant_MenuID == Restaurant_MenuID && p.Dish != null &&
                                                           p.Dish.Dish_CategoryID == Dish_CategoryID select p;
            // Помечаем доступными 
            queryDishes.ToList().ForEach(elem => { elem.Flag_Available = true; });

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "OK # {}", args: loginfo);
                }
                return 200;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args: $"Err # ErrorCode='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Удалить все товары категории
        /// </summary>
        /// <param name="DishID"></param>
        /// <returns></returns>
        public int Edit_PriceByDishID(int DishID, decimal? Price)
        {
            string loginfo = $"DishID='{DishID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            // Получаем список всех нужных блюд
            IQueryable<Restaurant_Menu_Dish> queryDishes = from p in _db.Restaurant_Menu_Dishes
                                                           where p.DishID == DishID
                                                           select p;

            // Меняем цену
            queryDishes.ToList().ForEach(elem => { elem.Price = Price; });

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return 200;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args: $"Err # ErrorCode='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение пометки на удаление для всех блюд ресторана
        /// </summary>
        /// <param name="DishID"></param>
        /// <returns></returns>
        public int Edit_FlagDelDishes(int DishID)
        {
            string loginfo = $"ID='{DishID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            // Получаем список всех нужных блюд
            IQueryable<Restaurant_Menu_Dish> queryDishes = from p in _db.Restaurant_Menu_Dishes where p.DishID == DishID select p;

            queryDishes.ToList().ForEach(elem => { elem.Flag_Del = true; elem.Date_Del = DateTime.Now; });

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return 200;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args: $"Err # ErrorCode='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }

    }
}