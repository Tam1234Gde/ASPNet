using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Блюда меню ресторана
    /// </summary>
    public class Restaurant_Menu_Dishes
    {
        private readonly ILogger _log;
        private readonly Restaurant_Menu_Dish_oper _dl_Restaurant_Menu_Dish = new();
        private readonly Restaurant_Menu_Dish_Ingredient_oper _dl_Restaurant_Menu_Dish_Ingredient = new();
        private readonly Restaurant_Menu_Dish_Volume_oper _dl_Restaurant_Menu_Dish_Volume = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_Menu_Dishes()
        {
            _log = AppLog.CreateLogger(GetType());
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem">Объект</param>
        /// <returns></returns>
        private static CRestaurant_Menu_Dish? Get(Restaurant_Menu_Dish? Elem)
        {
            if (Elem == null) return null;

            CRestaurant_Menu_Dish item = new()
            {
                ID = Elem.ID,
                DishID = Elem.DishID,
                Dish = Elem.Dish?.Name,
                Price = Elem.Price,
                Flag_Available = Elem.Flag_Available,
                Restaurant_MenuID = Elem.Restaurant_MenuID,
                Restaurant_Menu = Elem.Restaurant_Menu?.Name,
                OrderNum = Elem.OrderNum,
                FlagNew = Elem.FlagNew,
                // Объемы
                Restaurant_Menu_Dish_Volumes = Restaurant_Menu_Dish_Volumes.GetList(List_from: Elem.Restaurant_Menu_Dish_Volumes),

                // Ингредиенты
                Restaurant_Menu_Dish_Ingredients = Restaurant_Menu_Dish_Ingredients.GetList(List_from: Elem.Restaurant_Menu_Dish_Ingredients)
            };

            return item;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ListElems">Объект</param>
        /// <returns></returns>
        private static List<CRestaurant_Menu_Dish>? GetList(List<Restaurant_Menu_Dish>? ListElems)
        {
            List<CRestaurant_Menu_Dish>? result = [];

            if (ListElems == null)
            {
                return null;
            }

            foreach (Restaurant_Menu_Dish elem in ListElems)
            {
                CRestaurant_Menu_Dish item = new()
                {
                    ID = elem.ID,
                    DishID = elem.DishID,
                    Dish = elem.Dish?.Name,
                    Price = elem.Price,
                    Flag_Available = elem.Flag_Available,
                    Restaurant_MenuID = elem.Restaurant_MenuID,
                    Restaurant_Menu = elem.Restaurant_Menu?.Name,
                    OrderNum = elem.OrderNum,
                    FlagNew = elem.FlagNew,
                    // Объемы
                    Restaurant_Menu_Dish_Volumes = Restaurant_Menu_Dish_Volumes.GetList(List_from: elem.Restaurant_Menu_Dish_Volumes),

                    // Ингредиенты
                    Restaurant_Menu_Dish_Ingredients = Restaurant_Menu_Dish_Ingredients.GetList(List_from: elem.Restaurant_Menu_Dish_Ingredients)
                };

                result.Add(item);
            }

            return result;

        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        public CRestaurant_Menu_Dish? Get(int ID)
        {
            return Get(Elem: _dl_Restaurant_Menu_Dish.Get(ID));
        }


        /// <summary>
        /// Метод который одним запросом для каждого ID с базы получает списки меню 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<CRestaurant_Menu_Dish>>? Get_ByListIDs(List<int> List_IDs)
        {
            Dictionary<int, List<CRestaurant_Menu_Dish>> result = [];

            //Получил сразу все с айдишниками List_IDs
            List<CRestaurant_Menu_Dish>? list_Menus = GetList(ListElems: _dl_Restaurant_Menu_Dish.Get_ByListIDs(List_IDs)?.ToList());



            if (list_Menus == null)
            {
                return null;
            }

            //Заполнение по ID
            foreach (int elem in List_IDs)
            {
                result.Add(key: elem, value: [.. list_Menus.Where(p => p.DishID == elem)]);
            }


            return result;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="DishID">Код блюда</param>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <returns></returns>
        public CRestaurant_Menu_Dish? GetByDishID(int DishID, int RestaurantID)
        {
            return Get(Elem: _dl_Restaurant_Menu_Dish.GetByDishID(DishID, RestaurantID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="RestaurantMenuID">Код меню ресторана</param>
        /// <param name="Flag_Del_val">True - Помеченные как удаленные</param>
        /// <returns></returns>
        public List<CRestaurant_Menu_Dish> GetList(int RestaurantMenuID, bool? Flag_Del_val = false)
        {
            List<CRestaurant_Menu_Dish> list_result = [];

            List<Restaurant_Menu_Dish> list_Restaurant_Menu_Dish = [.. (from dish in _dl_Restaurant_Menu_Dish.GetQuery_ByRestarauntID(RestaurantMenuID: RestaurantMenuID, FlagDelVal: Flag_Del_val) select dish)]; //TODO: MAE NOW - ТУТ БАГ С ДОЛГОЙ ЗАГРУЗКОЙ

            foreach (Restaurant_Menu_Dish elem in list_Restaurant_Menu_Dish)
            {
                CRestaurant_Menu_Dish? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }

            return list_result;
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="DishID">Код блюда</param>
        /// <param name="Flag_Del_val">True - Помеченные как удаленные</param>
        /// <returns></returns>
        public List<CRestaurant_Menu_Dish> GetListByDishID(int DishID, bool? Flag_Del_val = false)
        {
            List<CRestaurant_Menu_Dish> list_result = [];
            foreach (Restaurant_Menu_Dish elem in _dl_Restaurant_Menu_Dish.GetQueryByDishID(DishID: DishID, FlagDelVal: Flag_Del_val).ToList())
            {
                CRestaurant_Menu_Dish? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="RestaurantMenuID">Код меню ресторана</param>
        /// <param name="Dish_CategoryID">Код категории меню</param>
        /// <param name="Flag_Del_val">True - Помеченные как удаленные</param>
        /// <returns></returns>
        public List<CRestaurant_Menu_Dish> GetListForAdmin(int RestaurantMenuID, int? Dish_CategoryID, bool? Flag_Del_val = false)
        {
            List<CRestaurant_Menu_Dish> list_result = [];
            foreach (Restaurant_Menu_Dish elem in _dl_Restaurant_Menu_Dish.GetQueryForAdmin(RestaurantMenuID: RestaurantMenuID, Dish_CategoryID: Dish_CategoryID,
                                                                                            FlagDelVal: Flag_Del_val).ToList())
            {
                CRestaurant_Menu_Dish? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Мигратор
        /// </summary>
        /// <param name="RestaurantMenuID">Код меню</param>
        /// <param name="UserID">Код пользователя</param>
        /// <returns></returns>
        public int Migrator(int RestaurantMenuID, int UserID)
        {
            // Получаем все блюда с главного меню для всех ресторанов
            List<CDish>? listDishes = new Dishes().GetList(Include_VolumesList: true, Include_Ingridients: true);
            int result = 0;

            if (listDishes != null && listDishes.Count != 0)
            {
                // Удаляем все блюда с меню ресторана
                int delDishes = _dl_Restaurant_Menu_Dish.Edit_FlagDelAll(RestaurantMenuID, UserID);

                if (delDishes < 0)
                {
                    return Errors.Err203;
                }

                foreach (CDish dish in listDishes)
                {
                    // Добавляем блюдо
                    result = Add(Restaurant_MenuID: RestaurantMenuID, DishID: dish.ID, Flag_Available: true, Price: dish.Price, OrderNum: dish.OrderNum, FlagNew: dish.FlagNew);

                    // Добавляем ингредиенты
                    if (dish.List_Dish_Ingridients != null && dish.List_Dish_Ingridients.Count != 0)
                    {
                        foreach (CDish_Ingridient dishIngredient in dish.List_Dish_Ingridients)
                        {
                            _dl_Restaurant_Menu_Dish_Ingredient.Add(IngridientID: dishIngredient.IngridientID, Restaurant_Menu_DishID: result,
                                                                    IngridientGroupID: dishIngredient.IngridientGroupID, Cooking_Minutes: dishIngredient.Cooking_Minutes,
                                                                    Price: dishIngredient.Price);
                        }
                    }

                    // Добавляем объемы
                    if (dish.List_Dish_Volumes != null && dish.List_Dish_Volumes.Count != 0)
                    {
                        foreach (CDish_Volume dishVolume in dish.List_Dish_Volumes)
                        {
                            _dl_Restaurant_Menu_Dish_Volume.Add(Restaurant_Menu_DishID: result, VolumeID: dishVolume.VolumeID, Price: dishVolume.VolumePrice,
                                                                OrderNum: dishVolume.OrderNum);
                        }
                    }

                }
            }

            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return result;
        }


        /// <summary>
        /// Изменить стоимость ингредиента в ресторане
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <param name="IngredientID">Код ресторана</param>
        /// <param name="Price">Код ресторана</param>
        /// <returns></returns>
        public int EditPriceForIngredients(int RestaurantID, int IngredientID, decimal Price)
        {
            CRestaurant_Menu? restaurantMenu = new Restaurant_Menus().GetActive(RestaurantID);

            if (restaurantMenu == null)
            {
                return Errors.Err203;
            }

            // Получаем все блюда с главного меню для всех ресторанов
            List<CRestaurant_Menu_Dish>? listDishes = new Restaurant_Menu_Dishes().GetList(RestaurantMenuID: restaurantMenu.ID);

            int result = 0;

            if (listDishes != null && listDishes.Count != 0)
            {
                result = _dl_Restaurant_Menu_Dish_Ingredient.EditPricesIngredientsByDishedIDs([.. listDishes.Select(p => p.ID)], IngredientID: IngredientID, Price: Price);
            }

            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return result;
        }


        /// <summary>
        /// Массовое добавление товаров во все рестораны
        /// </summary>
        /// <param name="DishID"></param>
        /// <returns></returns>
        public bool MassAddDishes(int DishID)
        {
            // Получаем список всех меню ресторанов
            List<CRestaurant_Menu> listMenus = new Restaurant_Menus().GetList();

            if (listMenus != null && listMenus.Count == 0 || listMenus == null)
            {
                return false;
            }

            CDish? dish = new Dishes().Get(DishID);

            if (dish == null)
            {
                return false;
            }

            foreach (var menu in listMenus)
            {
                // Добавляем блюдо
                Add_NoCommit(Restaurant_MenuID: menu.ID, DishID: dish.ID, Flag_Available: true, Price: dish.Price, OrderNum: dish.OrderNum, FlagNew: dish.FlagNew);

            }

            if (_dl_Restaurant_Menu_Dish.Commit())
            {
                return true;
            }

            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return false;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Restaurant_MenuID">Код меню ресторана</param>
        /// <param name="DishID">Код блюда</param>
        /// <param name="Flag_Available">True - Доступен</param>
        /// <param name="Price">Стоимость</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <param name="FlagNew">Новинка</param>
        /// <returns></returns>
        public void Add_NoCommit(int Restaurant_MenuID, int DishID, bool Flag_Available, decimal? Price, int OrderNum, bool FlagNew)
        {
            _dl_Restaurant_Menu_Dish.Add_NoCommit(Restaurant_MenuID: Restaurant_MenuID, DishID: DishID, Flag_Available: Flag_Available, Price: Price, OrderNum: OrderNum,
                                                  FlagNew: FlagNew);
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Restaurant_MenuID">Код меню ресторана</param>
        /// <param name="DishID">Код блюда</param>
        /// <param name="Flag_Available">True - Доступен</param>
        /// <param name="Price">Стоимость</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <param name="FlagNew">Новинка</param>
        /// <returns></returns>
        public int Add(int Restaurant_MenuID, int DishID, bool Flag_Available, decimal? Price, int OrderNum, bool FlagNew)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_Menu_Dish.Add(Restaurant_MenuID: Restaurant_MenuID, DishID: DishID, Flag_Available: Flag_Available, Price: Price, OrderNum: OrderNum,
                                                FlagNew: FlagNew);
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
        /// <param name="FlagNew">Новинка</param>
        /// <returns></returns>
        public int Edit(int ID, int Restaurant_MenuID, int DishID, bool Flag_Available, decimal? Price, int OrderNum, bool FlagNew)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_Menu_Dish.Edit(ID: ID, Restaurant_MenuID: Restaurant_MenuID, DishID: DishID, Flag_Available: Flag_Available, Price: Price, OrderNum: OrderNum,
                                                 FlagNew: FlagNew);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="UserID">Пользователь</param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_Menu_Dish.Edit_FlagDel(ID: ID, Flag_Del: true, UserID: UserID);
        }


        /// <summary>
        /// Изменения флага доступности
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="Flag_Available">True - Доступен</param>
        /// <returns></returns>
        public int Edit_Flag_Available(int ID, bool Flag_Available)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_Menu_Dish.Edit_Flag_Available(ID: ID, Flag_Available: Flag_Available);
        }


        /// <summary>
        /// Удалить товар во всех ресторанах
        /// </summary>
        /// <param name="DishID"></param>
        /// <returns></returns>
        public int DelDishes(int DishID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_Menu_Dish.Edit_FlagDelDishes(DishID: DishID);
        }



        /// <summary>
        /// Изменить цену для всех товаров в других ресторанах
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        public int EditPrices(int DishID, decimal? Price)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_Menu_Dish.Edit_PriceByDishID(DishID: DishID, Price: Price);
        }


        /// <summary>
        /// Удалить все товары категории
        /// </summary>
        /// <param name="Restaurant_MenuID"></param>
        /// <param name="Dish_CategoryID"></param>
        /// <returns></returns>
        public int DelAllCategoryDishes(int Restaurant_MenuID, int Dish_CategoryID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_Menu_Dish.Edit_FlagDelByDish_CategoryID(Restaurant_MenuID: Restaurant_MenuID, Dish_CategoryID: Dish_CategoryID);
        }


        /// <summary>
        /// Открыть все товары категории
        /// </summary>
        /// <param name="Restaurant_MenuID"></param>
        /// <param name="Dish_CategoryID"></param>
        /// <returns></returns>
        public int OpenAllCategoryDishes(int Restaurant_MenuID, int Dish_CategoryID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_Menu_Dish.Edit_Flag_AvailableByDish_CategoryID(Restaurant_MenuID: Restaurant_MenuID, Dish_CategoryID: Dish_CategoryID);
        }

    }
}