using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Ингредиент блюда в меню ресторана
    /// </summary>
    public class Restaurant_Menu_Dish_Ingredients
    {
        private readonly ILogger _log;
        private readonly Restaurant_Menu_Dish_Ingredient_oper _dl_Restaurant_Menu_Dish_Ingredient = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_Menu_Dish_Ingredients()
        {
            _log = AppLog.CreateLogger(GetType());
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem">Объект</param>
        /// <returns></returns>
        private static CRestaurant_Menu_Dish_Ingredient? Get(Restaurant_Menu_Dish_Ingredient? Elem)
        {
            if (Elem == null) return null;

            CRestaurant_Menu_Dish_Ingredient item = new()
            {
                ID = Elem.ID,
                IngridientID = Elem.IngridientID,
                Ingridient = Elem.Ingridient?.Name,
                Cooking_Minutes = Elem.Cooking_Minutes,
                IngridientGroupID = Elem.IngridientGroupID,
                Price = Elem.Price,
                Restaurant_Menu_DishID = Elem.Restaurant_Menu_DishID,
                Restaurant_Menu_Dish = Elem.Restaurant_Menu_Dish?.Dish?.Name,
                Category_ID = Elem.Ingridient?.Category_ID
            };
            return item;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        public CRestaurant_Menu_Dish_Ingredient? Get(int ID)
        {
            return Get(Elem: _dl_Restaurant_Menu_Dish_Ingredient.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Restaurant_Menu_DishID">Блюдо в ресторане</param>
        /// <param name="Flag_Del_val">True - Помеченные как удаленные</param>
        /// <returns></returns>
        public List<CRestaurant_Menu_Dish_Ingredient> GetList(int Restaurant_Menu_DishID, bool? Flag_Del_val = false)
        {
            List<CRestaurant_Menu_Dish_Ingredient> list_result = [];
            foreach (Restaurant_Menu_Dish_Ingredient elem in _dl_Restaurant_Menu_Dish_Ingredient.GetQuery_ByRestarauntID(Restaurant_Menu_DishID: Restaurant_Menu_DishID, Flag_Del_val: Flag_Del_val).ToList())

            {
                CRestaurant_Menu_Dish_Ingredient? item = Get(Elem: elem);
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
        /// <param name="List_from"></param>
        /// <returns></returns>
        public static List<CRestaurant_Menu_Dish_Ingredient>? GetList(List<Restaurant_Menu_Dish_Ingredient>? List_from)
        {
            if (List_from != null)
            {
                List<CRestaurant_Menu_Dish_Ingredient> list_result = [];


                List<Restaurant_Menu_Dish_Ingredient> list_Ingridients_Filtered = [.. (from p in List_from where p.Ingridient != null && !p.Ingridient.Flag_Del && !p.Flag_Del select p)];

                foreach (Restaurant_Menu_Dish_Ingredient elem in list_Ingridients_Filtered)
                {
                    CRestaurant_Menu_Dish_Ingredient? item = Get(Elem: elem);
                    if (item != null)
                    {
                        list_result.Add(item);
                    }
                }
                return [.. list_result.OrderBy(x => x.Ingridient)];
            }

            return null;
        }


        /// <summary>
        /// Массовое добавление товаров во все рестораны
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="Dish_IngridientID"></param>
        /// <returns></returns>
        public bool MassAddDishIngredients(int DishID, int Dish_IngridientID)
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

            CDish_Ingridient? ingredient = new Dish_Ingridients().Get(Dish_IngridientID);

            if (ingredient == null)
            {
                return false;
            }

            // Получаем список всех блюд в ресторанах
            List<CRestaurant_Menu_Dish>? listDishes = new Restaurant_Menu_Dishes().GetListByDishID(DishID);

            if (listDishes != null && listDishes.Count == 0 || listDishes == null)
            {
                return false;
            }

            foreach (CRestaurant_Menu_Dish item in listDishes)
            {
                _dl_Restaurant_Menu_Dish_Ingredient.Add_NoCommit(IngridientID: ingredient.IngridientID, Restaurant_Menu_DishID: item.ID,
                                                                 IngridientGroupID: ingredient.IngridientGroupID, Cooking_Minutes: ingredient.Cooking_Minutes,
                                                                 Price: ingredient.Price);
            }

            if (_dl_Restaurant_Menu_Dish_Ingredient.Commit())
            {
                return true;
            }

            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return false;
        }


        /// <summary>
        /// Изменить цену для всех ингредиентов в других ресторанах
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="IngredientID"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        public int EditIngredientPrices(int DishID, int IngredientID, decimal Price)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Restaurant_Menu_Dish_Ingredient.EditPricesIngredients(DishID: DishID, IngredientID: IngredientID, Price: Price);
        }


        /// <summary>
        /// Удалить ингредиент в других ресторанах
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="IngredientID"></param>
        /// <returns></returns>
        public int DelIngredients(int DishID, int IngredientID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Restaurant_Menu_Dish_Ingredient.DelIngredients(DishID: DishID, IngredientID: IngredientID);
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
        public void Add_NoCommit(int IngridientID, int Restaurant_Menu_DishID, int? IngridientGroupID, int? Cooking_Minutes, decimal Price)
        {
            _dl_Restaurant_Menu_Dish_Ingredient.Add_NoCommit(IngridientID: IngridientID, Restaurant_Menu_DishID: Restaurant_Menu_DishID,
                                                           IngridientGroupID: IngridientGroupID, Cooking_Minutes: Cooking_Minutes, Price: Price);
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
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Restaurant_Menu_Dish_Ingredient.Add(IngridientID: IngridientID, Restaurant_Menu_DishID: Restaurant_Menu_DishID,
                                                           IngridientGroupID: IngridientGroupID, Cooking_Minutes: Cooking_Minutes, Price: Price);
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
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Restaurant_Menu_Dish_Ingredient.Edit(ID: ID, IngridientID: IngridientID, Restaurant_Menu_DishID: Restaurant_Menu_DishID,
                                                            IngridientGroupID: IngridientGroupID, Cooking_Minutes: Cooking_Minutes, Price: Price);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="UserID">Пользователь</param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Restaurant_Menu_Dish_Ingredient.Edit_FlagDel(ID: ID, Flag_Del: true, UserID: UserID);
        }

    }
}