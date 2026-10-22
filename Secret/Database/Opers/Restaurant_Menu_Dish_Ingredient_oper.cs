using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Ингредиент блюда в меню ресторана
    /// </summary>
    public class Restaurant_Menu_Dish_Ingredient_oper
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
        public Restaurant_Menu_Dish_Ingredient_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Restaurant_Menu_Dish_Ingredient? Get(int ID, bool? Flag_Del_val = false)
        {
            Restaurant_Menu_Dish_Ingredient? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="Restaurant_Menu_DishID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu_Dish_Ingredient> GetQuery(bool? Flag_Del_val = false) 
        {
            IQueryable<Restaurant_Menu_Dish_Ingredient> query = from p in _db.Restaurant_Menu_Dish_Ingredients.Include(x => x.Restaurant_Menu_Dish).Include(x => x.Ingridient).AsNoTracking() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="Restaurant_Menu_DishID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu_Dish_Ingredient> GetQuery_Edit(bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Menu_Dish_Ingredient> query = from p in _db.Restaurant_Menu_Dish_Ingredients select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="Restaurant_Menu_DishID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu_Dish_Ingredient> GetQuery_ByRestarauntID(int Restaurant_Menu_DishID, bool? Flag_Del_val = false) 
        {
            IQueryable<Restaurant_Menu_Dish_Ingredient> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.Restaurant_Menu_DishID == Restaurant_Menu_DishID select p;

            return query;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns></returns>
        private Restaurant_Menu_Dish_Ingredient? Get_Edit(int ID, bool? Flag_Del_val = false)
        {
            Restaurant_Menu_Dish_Ingredient? elem = (from p in GetQuery_Edit(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Изменить цену ингредиентов во всех ресторанах
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="IngredientID"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        public int EditPricesIngredients(int DishID, int IngredientID, decimal Price)
        {
            string loginfo = $"DishID='{DishID}' IngredientID='{IngredientID}' Price='{Price}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            // Получаем список всех нужных ингредиентов
            IQueryable<Restaurant_Menu_Dish_Ingredient> queryIngredients = from p in _db.Restaurant_Menu_Dish_Ingredients where p.Restaurant_Menu_Dish != null &&
                                                        p.Restaurant_Menu_Dish.DishID == DishID && p.IngridientID == IngredientID select p;

            // Меняем цену
            queryIngredients.ToList().ForEach(elem => { elem.Price = Price; });

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: "OK # {loginfo}");
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
        /// Изменить цену ингредиентов во всех ресторанах
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="IngredientID"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        public int EditPricesIngredientsByDishedIDs(List<int> Restaurant_Menu_DishIDs, int IngredientID, decimal Price)
        {
            string loginfo = $"Restaurant_Menu_DishIDs='{Restaurant_Menu_DishIDs}' IngredientID='{IngredientID}' Price='{Price}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
           
            // Получаем список всех нужных ингредиентов
            IQueryable<Restaurant_Menu_Dish_Ingredient> queryIngredients = from p in _db.Restaurant_Menu_Dish_Ingredients where p.Restaurant_Menu_Dish != null && Restaurant_Menu_DishIDs.Contains(p.Restaurant_Menu_DishID) &&
                                                                          p.IngridientID == IngredientID select p;

            // Меняем цену
            queryIngredients.ToList().ForEach(elem => { elem.Price = Price; });

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
        /// Изменить цену ингредиентов во всех ресторанах
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="IngredientID"></param>
        /// <returns></returns>
        public int DelIngredients(int DishID, int IngredientID)
        {
            string loginfo = $"DishID='{DishID}' IngredientID='{IngredientID}''";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            
            // Получаем список всех нужных ингредиентов
            IQueryable<Restaurant_Menu_Dish_Ingredient> queryIngredients = from p in _db.Restaurant_Menu_Dish_Ingredients where p.Restaurant_Menu_Dish != null &&
                                                        p.Restaurant_Menu_Dish.DishID == DishID && p.IngridientID == IngredientID select p;
            // Удаляем
            queryIngredients.ToList().ForEach(elem => { elem.Flag_Del = true; elem.Date_Del = DateTime.Now; });

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
        /// <param name="IngridientID">Код ингредиента</param>
        /// <param name="Restaurant_Menu_DishID">Код блюда с ресторана</param>
        /// <param name="IngridientGroupID">Группа</param>
        /// <param name="Cooking_Minutes">Время приготовления</param>
        /// <param name="Price">Стоимость</param>
        /// <returns></returns>
        public void Add_NoCommit(int IngridientID, int Restaurant_Menu_DishID, int? IngridientGroupID, int? Cooking_Minutes, decimal Price)
        {
            string loginfo = $"IngridientID='{IngridientID}' Restaurant_Menu_DishID='{Restaurant_Menu_DishID}' IngridientGroupID='{IngridientGroupID}' " +
                             $"Cooking_Minutes='{Cooking_Minutes}' Price='{Price}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            
            Restaurant_Menu_Dish_Ingredient elem = new()
            {
                IngridientID = IngridientID,
                Restaurant_Menu_DishID = Restaurant_Menu_DishID,
                IngridientGroupID = IngridientGroupID,
                Cooking_Minutes = Cooking_Minutes,
                Price = Price
            };

            _db.Restaurant_Menu_Dish_Ingredients.Add(elem);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="IngridientID">Код ингредиента</param>
        /// <param name="Restaurant_Menu_DishID">Код блюда с ресторана</param>
        /// <param name="IngridientGroupID">Группа</param>
        /// <param name="Cooking_Minutes">Время приготовления</param>
        /// <param name="Price">Стоимость</param>
        /// <returns></returns>
        public int Add(int IngridientID, int Restaurant_Menu_DishID, int? IngridientGroupID, int? Cooking_Minutes, decimal Price)
        {
            string loginfo = $"IngridientID='{IngridientID}' Restaurant_Menu_DishID='{Restaurant_Menu_DishID}' IngridientGroupID='{IngridientGroupID}' " +
                             $"Cooking_Minutes='{Cooking_Minutes}' Price='{Price}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            
            Restaurant_Menu_Dish_Ingredient elem = new()
            {
                IngridientID = IngridientID,
                Restaurant_Menu_DishID = Restaurant_Menu_DishID,
                IngridientGroupID = IngridientGroupID,
                Cooking_Minutes = Cooking_Minutes,
                Price = Price
            };

            _db.Restaurant_Menu_Dish_Ingredients.Add(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "OK # {}", args: loginfo);
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
        /// <param name="IngridientID">Код ингредиента</param>
        /// <param name="Restaurant_Menu_DishID">Код блюда с ресторана</param>
        /// <param name="IngridientGroupID">Группа</param>
        /// <param name="Cooking_Minutes">Время приготовления</param>
        /// <param name="Price">Стоимость</param>
        /// <returns></returns>
        public int Edit(int ID, int IngridientID, int Restaurant_Menu_DishID, int? IngridientGroupID, int? Cooking_Minutes, decimal Price)
        {
            string loginfo = $"ID='{IngridientID}' ID='{IngridientID}' Restaurant_Menu_DishID='{Restaurant_Menu_DishID}' IngridientGroupID='{IngridientGroupID}' " +
                             $"Cooking_Minutes='{Cooking_Minutes}' Price='{Price}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            

            Restaurant_Menu_Dish_Ingredient? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode='{Errors.Err200_str}' ## {loginfo}");
                return Errors.Err200;
            }

            elem.IngridientID = IngridientID;
            elem.Restaurant_Menu_DishID = Restaurant_Menu_DishID;
            elem.IngridientGroupID = IngridientGroupID;
            elem.Cooking_Minutes = Cooking_Minutes;
            elem.Price = Price;

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
            Restaurant_Menu_Dish_Ingredient? elem = Get_Edit(ID: ID);

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
                _log.LogError(exception: ex, message: "{}", args: $"Err # ErrorCode='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }

    }
}