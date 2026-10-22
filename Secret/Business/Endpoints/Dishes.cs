using Microsoft.Extensions.Logging;
using System.Diagnostics;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Блюда
    /// </summary>
    public class Dishes
    {
        private readonly ILogger _log;
        private readonly Dish_oper _dl_Dish = new();
        private readonly Dish_Categories _bl_Dish_Categories = new();
        private readonly Restaurant_Menu_Dish_oper _dl_Restaurant_Menu_Dish = new();
        private readonly Restaurant_Menu_oper _dl_Restaurant_Menu = new();
        private readonly Restaurant_oper _dl_Restaurant = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Dishes()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        private CDish? Get(Dish? Elem, bool Include_VolumesList, bool Include_Ingridients)
        {
            if (Elem != null)
            {
                CDish item = new()
                {
                    ID = Elem.ID,
                    Dish_CategoryID = Elem.Dish_CategoryID,
                    Dish_Category = Elem.Dish_Category?.Name ?? string.Empty,
                    Name = Elem.Name,
                    Teaser = Elem.Teaser,
                    Text = Elem.Text,
                    Flag_AllowColors = Elem.Flag_AllowColors,
                    OrderNum = Elem.OrderNum,
                    Price = Elem.Price,
                    Image_ChangeDate = Elem.Image_ChangeDate,
                    Bonuses = Elem.Bonuses,
                    Flag_Bonus = Elem.Flag_Bonus,
                    Cooking_Minutes = Elem.Cooking_Minutes,
                    FlagNew = Elem.FlagNew,
                    FlagNoActive = Elem.FlagNoActive,
                    New_DateFrom = Elem.New_DateFrom,
                };

                if (Include_VolumesList)
                {
                    item.List_Dish_Volumes = Dish_Volumes.GetList(List_from: Elem.Dish_Volumes);
                }

                if (Include_Ingridients)
                {
                    item.List_Dish_Ingridients = Dish_Ingridients.GetList(List_from: Elem.Dish_Ingridients);
                }

                CDish_Category? category = _bl_Dish_Categories.Get(item.Dish_CategoryID);
                if (category != null && category.Cooking_Minutes > 0 && (item.Cooking_Minutes == null || item.Cooking_Minutes == 0))
                {
                    item.Cooking_Minutes = category.Cooking_Minutes;
                }

                if (!string.IsNullOrWhiteSpace(Elem.RecommendDishIDs))
                {
                    List<int> ints = [.. Elem.RecommendDishIDs.Split(";").Select(p => HIDE_Convert.ToInt(p))];
                    item.List_RecommendDishIDs = ints;
                }

                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        private static CDish? Get_V2(Dish? Elem, bool Include_VolumesList, bool Include_Ingridients, List<CDish_Category> List_Categories)
        {
            if (Elem != null)
            {
                CDish item = new()
                {
                    ID = Elem.ID,
                    Dish_CategoryID = Elem.Dish_CategoryID,
                    Dish_Category = Elem.Dish_Category?.Name ?? string.Empty,
                    Name = Elem.Name,
                    Teaser = Elem.Teaser,
                    Text = Elem.Text,
                    Flag_AllowColors = Elem.Flag_AllowColors,
                    OrderNum = Elem.OrderNum,
                    Price = Elem.Price,
                    Image_ChangeDate = Elem.Image_ChangeDate,
                    Bonuses = Elem.Bonuses,
                    Flag_Bonus = Elem.Flag_Bonus,
                    Cooking_Minutes = Elem.Cooking_Minutes,
                    FlagNew = Elem.FlagNew,
                    FlagNoActive = Elem.FlagNoActive,
                    New_DateFrom = Elem.New_DateFrom,
                };

                if (Include_VolumesList)
                {
                    item.List_Dish_Volumes = Dish_Volumes.GetList(List_from: Elem.Dish_Volumes);
                }

                if (Include_Ingridients)
                {
                    item.List_Dish_Ingridients = Dish_Ingridients.GetList(List_from: Elem.Dish_Ingridients);
                }

                CDish_Category? category = List_Categories.Where(p => p.ID == Elem.Dish_CategoryID).FirstOrDefault();
                if (category != null && category.Cooking_Minutes > 0 && (item.Cooking_Minutes == null || item.Cooking_Minutes == 0))
                {
                    item.Cooking_Minutes = category.Cooking_Minutes;
                }

                if (!string.IsNullOrWhiteSpace(Elem.RecommendDishIDs))
                {
                    List<int> ints = [.. Elem.RecommendDishIDs.Split(";").Select(p => HIDE_Convert.ToInt(p))];
                    item.List_RecommendDishIDs = ints;
                }

                return item;
            }

            return null;
        }


        /// <summary>
        /// Получить список городов в котором используются блюда с ID
        /// </summary>
        /// <param name="DishID">ID Блюда</param>
        /// <returns></returns>
        public List<string>? Get_UsedCity(int DishID)
        {
            List<string>? result = [];

            List<int>? Restaurant_Menu_Dishes = _dl_Restaurant_Menu_Dish.GetListIDsByDishID(DishID: DishID)?.Distinct().ToList();

            if (Restaurant_Menu_Dishes == null)
            {
                return null;
            }

            foreach (int elem in Restaurant_Menu_Dishes)
            {
                List<int>? Restaurant_Menus = _dl_Restaurant_Menu.GetActiveID_ByMenuID(MenuID: elem)?.Distinct().ToList();

                if (Restaurant_Menus == null)
                {
                    continue;
                }

                foreach (int item in Restaurant_Menus)
                {
                    Restaurant? restaurant = _dl_Restaurant.Get(item);

                    if (restaurant != null)
                    {
                        result.Add(restaurant.Adress);
                    }
                }
            }

            return [.. result.Distinct()];

        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        private static CDish_ForAdmin? Get_ForAdmin(Dish? Elem, bool Include_VolumesList, bool Include_Ingridients)
        {
            if (Elem != null)
            {
                CDish_ForAdmin item = new()
                {
                    ID = Elem.ID,
                    Dish_CategoryID = Elem.Dish_CategoryID,
                    Dish_Category = Elem.Dish_Category?.Name ?? string.Empty,
                    CategoryOrderNum = Elem.Dish_Category?.OrderNum ?? 0,
                    Name = Elem.Name,
                    Teaser = Elem.Teaser,
                    Text = Elem.Text,
                    Flag_AllowColors = Elem.Flag_AllowColors,
                    OrderNum = Elem.OrderNum,
                    Price = Elem.Price,
                    Image = Elem.Image,
                    Bonuses = Elem.Bonuses,
                    Flag_Bonus = Elem.Flag_Bonus,
                    Cooking_Minutes = Elem.Cooking_Minutes,
                    RecommendDishIDs = Elem.RecommendDishIDs,
                    FlagNew = Elem.FlagNew,
                    FlagNoActive = Elem.FlagNoActive,
                    New_DateFrom = Elem.New_DateFrom,
                };

                if (Include_VolumesList)
                {
                    item.List_Dish_Volumes = Dish_Volumes.GetList(List_from: Elem.Dish_Volumes);
                }

                if (Include_Ingridients)
                {
                    item.List_Dish_Ingridients = Dish_Ingridients.GetList_ForAdmin(List_from: Elem.Dish_Ingridients);
                }

                if (!string.IsNullOrWhiteSpace(Elem.RecommendDishIDs))
                {

                    List<string> strings = [.. Elem.RecommendDishIDs.Split(";")];

                    List<int> ints = [];
                    foreach (string str in strings)
                    {
                        if (int.TryParse(str, out int result))
                        {
                            ints.Add(result);
                        }
                    }

                    item.List_RecommendDishIDs = ints;
                }

                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        public CDish? Get(int ID, bool Include_VolumesList = false, bool Include_Ingridients = false)
        {
            return Get(Elem: _dl_Dish.Get(ID), Include_VolumesList: Include_VolumesList, Include_Ingridients: Include_Ingridients); //TODO: MAE - Зачем тут Get_Edit?
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        public CDish_ForAdmin? Get_ForAdmin(int ID, bool Include_VolumesList = false, bool Include_Ingridients = false)
        {
            return Get_ForAdmin(Elem: _dl_Dish.Get(ID), Include_VolumesList: Include_VolumesList, Include_Ingridients: Include_Ingridients);
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        public CDish_ForAdmin? Get_ForAdmin_v2(int ID, int RestaurantID, bool Include_VolumesList = false, bool Include_Ingridients = false)
        {
            // Получаем блюдо из ресторана
            CRestaurant_Menu_Dish? restaurantDish = new Restaurant_Menu_Dishes().GetByDishID(ID, RestaurantID);

            // Получаем блюдо
            CDish_ForAdmin? dishMain = Get_ForAdmin(Elem: _dl_Dish.Get(ID), Include_VolumesList: Include_VolumesList, Include_Ingridients: Include_Ingridients);

            // Результат
            CDish_ForAdmin? dish = new();

            if (restaurantDish != null && dishMain != null)
            {
                List<CDish_Volume>? List_Dish_Volumes = [];
                List<CDish_Ingridient_ForAdmin>? List_Dish_Ingridients = [];

                // Ингредиенты
                if (restaurantDish.Restaurant_Menu_Dish_Ingredients != null && restaurantDish.Restaurant_Menu_Dish_Ingredients.Count != 0)
                {
                    foreach (CRestaurant_Menu_Dish_Ingredient ingredient in restaurantDish.Restaurant_Menu_Dish_Ingredients.OrderBy(p => p.ID))
                    {
                        byte[]? image = new Ingridients().Image_Get(ID: ingredient.IngridientID);

                        CDish_Ingridient_ForAdmin ingr = new()
                        {
                            ID = ingredient.ID,
                            Cooking_Minutes = ingredient.Cooking_Minutes,
                            DishID = restaurantDish.DishID,
                            Dish = restaurantDish.Dish,
                            Ingridient = ingredient.Ingridient,
                            IngridientGroupID = ingredient.IngridientGroupID,
                            IngridientID = ingredient.IngridientID,
                            Price = ingredient.Price,
                            Image = image
                        };

                        List_Dish_Ingridients.Add(ingr);
                    }
                }

                // Объемы
                if (restaurantDish.Restaurant_Menu_Dish_Volumes != null && restaurantDish.Restaurant_Menu_Dish_Volumes.Count != 0)
                {
                    foreach (CRestaurant_Menu_Dish_Volume volume in restaurantDish.Restaurant_Menu_Dish_Volumes.OrderBy(p => p.ID))
                    {
                        CDish_Volume vol = new()
                        {
                            ID = volume.ID,
                            DishID = restaurantDish.DishID,
                            OrderNum = volume.OrderNum,
                            VolumeID = volume.VolumeID,
                            VolumeName = volume.VolumeName,
                            VolumePrice = volume.VolumePrice,
                            VolumeValue = volume.VolumeValue
                        };

                        List_Dish_Volumes.Add(vol);
                    }
                }

                dish = new()
                {
                    ID = dishMain.ID,
                    Bonuses = dishMain.Bonuses,
                    Cooking_Minutes = dishMain.Cooking_Minutes,
                    Dish_CategoryID = dishMain.Dish_CategoryID,
                    Flag_AllowColors = dishMain.Flag_AllowColors,
                    Flag_Bonus = dishMain.Flag_Bonus,
                    Image = dishMain.Image,
                    Name = dishMain.Name,
                    OrderNum = dishMain.OrderNum,
                    Price = restaurantDish.Price ?? 0,
                    Teaser = dishMain.Teaser,
                    Text = dishMain.Text,
                    Dish_Category = dishMain.Dish_Category,
                    List_Dish_Ingridients = List_Dish_Ingridients,
                    List_Dish_Volumes = List_Dish_Volumes
                };

            }

            return dish;
        }


        /// <summary>
        /// Получение изображения блюда по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public byte[]? Image_Get(int ID)
        {
            return _dl_Dish.Get(ID: ID)?.Image;
        }


        /// <summary>
        /// Проверка удалено ли блюдо
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Check_Dish(int ID)
        {
            return _dl_Dish.Get(ID: ID)?.Flag_Del ?? true;
        }


        /// <summary>
        /// Проверка блюдо кофе
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Check_Dish_Coffie(int ID)
        {
            return _dl_Dish.Get(ID: ID)?.Flag_AllowColors ?? true;
        }


        /// <summary>
        /// Получить количество бонусов за покупку блюда
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Количество начисляемых бонусов за блюдо</returns>
        public decimal? Check_Dish_ForBonuses(int ID)
        {
            return _dl_Dish.Get(ID: ID)?.Bonuses;
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CDish_ForAdmin> GetListByIDs(List<int> IDs, bool Include_VolumesList = false, bool Include_Ingridients = false, bool AddCleanRow = false, bool? Flag_Del_val = false)
        {
            List<CDish_ForAdmin> list_result = [];

            if (AddCleanRow)
            {
                list_result.Add(new CDish_ForAdmin { ID = Consts.NoData_Int, Name = Consts.NoData_Str });
            }

            IQueryable<Dish> query = _dl_Dish.GetQueryByIDs_Edit(IDs: IDs, Flag_Del_val: Flag_Del_val);

            foreach (Dish elem in query.ToList())
            {
                CDish_ForAdmin? item = Get_ForAdmin(Elem: elem, Include_VolumesList: Include_VolumesList, Include_Ingridients: Include_Ingridients);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }



        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CDish> GetList(int? DishCategoryID = null, bool Include_VolumesList = false, bool Include_Ingridients = false, bool AddCleanRow = false, bool? Flag_Del_val = false)
        {
            List<CDish> list_result = [];

            if (AddCleanRow)
            {
                list_result.Add(new CDish { ID = Consts.NoData_Int, Name = Consts.NoData_Str });
            }

            IQueryable<Dish> query = _dl_Dish.GetQuery_Edit(Flag_Del_val: Flag_Del_val);

            if (DishCategoryID != null)
            {
                query = from p in query where p.Dish_CategoryID == DishCategoryID select p;
            }


            List<CDish_Category> list_Categories = _bl_Dish_Categories.GetList(Flag_IsParent: false);


            List<Dish> list_Dishes = [.. query];

            foreach (Dish elem in list_Dishes)
            {
                CDish? item = Get_V2(Elem: elem, Include_VolumesList: Include_VolumesList, Include_Ingridients: Include_Ingridients, List_Categories: list_Categories);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }


        /// <summary>
        /// Возвращает список по коду ресторана v2
        /// </summary>
        /// <returns></returns>
        public List<CSiteMenu> GetListForSite_v2(int RestaurantID, out List<CDish_Category> listCategories, int? DishCategoryID = null, bool Include_VolumesList = false,
                                                      bool Include_Ingridients = false, bool? Flag_Del_val = false)
        {
            List<CDish_ForAdmin> list_dishes = [];
            List<CDish_ForAdmin> list_result = [];
            listCategories = [];

            // Получить активное меню ресторана
            CRestaurant_Menu? restaurantMenu = new Restaurant_Menus().GetActive(RestaurantID);

            if (restaurantMenu != null)
            {
                // Получаем список блюд ресторана
                List<CRestaurant_Menu_Dish>? list_restaurant_dishes = new Restaurant_Menu_Dishes().GetList(restaurantMenu.ID);

                IQueryable<Dish> query = _dl_Dish.GetQueryByIDs_Edit(IDs: [.. list_restaurant_dishes.Select(p => p.DishID)], Flag_Del_val: Flag_Del_val);

                if (DishCategoryID != null)
                {
                    query = from p in query where p.Dish_CategoryID == DishCategoryID select p;
                }

                foreach (Dish elem in query.ToList())
                {
                    CDish_ForAdmin? item = Get_ForAdmin(Elem: elem, Include_VolumesList: Include_VolumesList, Include_Ingridients: Include_Ingridients);
                    if (item != null)
                    {
                        list_dishes.Add(item);
                    }
                }

                // Соотносим блюда ресторана и блюда
                if (list_restaurant_dishes != null && list_restaurant_dishes.Count != 0)
                {


                    foreach (CRestaurant_Menu_Dish elem in list_restaurant_dishes.OrderBy(p => p.DishID))
                    {

                        //Проверка на доступность
                        if (!elem.Flag_Available)
                        {
                            continue;
                        }

                        foreach (CDish_ForAdmin item in list_dishes.OrderBy(p => p.ID))
                        {
                            if (elem.DishID == item.ID)
                            {
                                List<CDish_Volume>? List_Dish_Volumes = [];
                                List<CDish_Ingridient_ForAdmin>? List_Dish_Ingridients = [];

                                // Ингредиенты
                                if (elem.Restaurant_Menu_Dish_Ingredients != null && elem.Restaurant_Menu_Dish_Ingredients.Count != 0)
                                {
                                    foreach (CRestaurant_Menu_Dish_Ingredient ingredient in elem.Restaurant_Menu_Dish_Ingredients.OrderBy(p => p.ID))
                                    {
                                        CDish_Ingridient_ForAdmin ingr = new()
                                        {
                                            ID = ingredient.ID,
                                            Cooking_Minutes = ingredient.Cooking_Minutes,
                                            DishID = item.ID,
                                            Dish = elem.Dish,
                                            Ingridient = ingredient.Ingridient,
                                            IngridientGroupID = ingredient.IngridientGroupID,
                                            IngridientID = ingredient.IngridientID,
                                            Price = ingredient.Price
                                        };

                                        List_Dish_Ingridients.Add(ingr);
                                    }
                                }

                                // Объемы
                                if (elem.Restaurant_Menu_Dish_Volumes != null && elem.Restaurant_Menu_Dish_Volumes.Count != 0)
                                {
                                    foreach (CRestaurant_Menu_Dish_Volume volume in elem.Restaurant_Menu_Dish_Volumes.OrderBy(p => p.ID))
                                    {
                                        CDish_Volume vol = new()
                                        {
                                            ID = volume.ID,
                                            DishID = item.ID,
                                            OrderNum = volume.OrderNum,
                                            VolumeID = volume.VolumeID,
                                            VolumeName = volume.VolumeName,
                                            VolumePrice = volume.VolumePrice,
                                            VolumeValue = volume.VolumeValue
                                        };

                                        List_Dish_Volumes.Add(vol);
                                    }
                                }

                                CDish_ForAdmin dish = new()
                                {
                                    ID = item.ID,
                                    Bonuses = item.Bonuses,
                                    Cooking_Minutes = item.Cooking_Minutes,
                                    Dish_CategoryID = item.Dish_CategoryID,
                                    CategoryOrderNum = item.CategoryOrderNum,
                                    Flag_AllowColors = item.Flag_AllowColors,
                                    Flag_Bonus = item.Flag_Bonus,
                                    Image = item.Image,
                                    Name = item.Name,
                                    OrderNum = item.OrderNum,
                                    Price = elem.Price ?? 0,
                                    Teaser = item.Teaser,
                                    Text = item.Text,
                                    Dish_Category = item.Dish_Category,
                                    FlagNew = item.FlagNew,
                                    List_Dish_Ingridients = List_Dish_Ingridients,
                                    List_Dish_Volumes = List_Dish_Volumes
                                };
                                list_result.Add(dish);
                            }
                        }
                    }
                }

            }


            List<CSiteMenu> listCategoriesIDs = [.. list_result.GroupBy(p => p.Dish_CategoryID)
                                                    .Select(g => new CSiteMenu()
                                                    {
                                                        CategoryID = g.Key,
                                                        OrderNum = g.Select(p => p.CategoryOrderNum).FirstOrDefault(),
                                                        Category = g.Select(p => p.Dish_Category).Distinct()
                                                    .FirstOrDefault(),
                                                        ListDishes = [.. g.Select(p => p).OrderBy(x => x.OrderNum).ThenBy(x => x.Name)]
                                                    }).OrderBy(p => p.OrderNum)];

            return listCategoriesIDs;
        }


        /// <summary>
        /// Возвращает список по коду ресторана v2
        /// </summary>
        /// <returns></returns>
        public List<CDish> GetList_v2(int RestaurantID, int? DishCategoryID = null, bool Include_VolumesList = false, bool Include_Ingridients = false,
                                      bool AddCleanRow = false, bool? Flag_Del_val = false)
        {
            List<CDish> list_dishes = [];
            List<CDish> list_result = [];

            // Получить активное меню ресторана
            CRestaurant_Menu? restaurantMenu = new Restaurant_Menus().GetActive(RestaurantID);

            if (restaurantMenu != null)
            {
                // Получаем список блюд ресторана
                List<CRestaurant_Menu_Dish>? list_restaurant_dishes = new Restaurant_Menu_Dishes().GetList(restaurantMenu.ID);

                if (AddCleanRow)
                {
                    list_dishes.Add(new CDish { ID = Consts.NoData_Int, Name = Consts.NoData_Str });
                }

                IQueryable<Dish> query = _dl_Dish.GetQueryByIDs(IDs: [.. list_restaurant_dishes.Select(p => p.DishID)], Flag_Del_val: Flag_Del_val);

                if (DishCategoryID != null)
                {
                    query = from p in query where p.Dish_CategoryID == DishCategoryID select p;
                }

                List<CDish_Category> list_Categories = _bl_Dish_Categories.GetList(Flag_IsParent: false);

                foreach (Dish elem in query.ToList())
                {
                    CDish? item = Get_V2(Elem: elem, Include_VolumesList: Include_VolumesList, Include_Ingridients: Include_Ingridients, List_Categories: list_Categories);
                    if (item != null)
                    {
                        list_dishes.Add(item);
                    }
                }

                // Соотносим блюда ресторана и блюда
                if (list_restaurant_dishes != null && list_restaurant_dishes.Count != 0)
                {
                    foreach (CRestaurant_Menu_Dish elem in list_restaurant_dishes.OrderBy(p => p.DishID))
                    {

                        //Проверка на доступность
                        if (!elem.Flag_Available)
                        {
                            continue;
                        }

                        foreach (CDish item in list_dishes.OrderBy(p => p.ID))
                        {
                            if (elem.DishID == item.ID)
                            {
                                List<CDish_Volume>? List_Dish_Volumes = [];
                                List<CDish_Ingridient>? List_Dish_Ingridients = [];

                                // Ингредиенты
                                if (elem.Restaurant_Menu_Dish_Ingredients != null && elem.Restaurant_Menu_Dish_Ingredients.Count != 0)
                                {
                                    foreach (CRestaurant_Menu_Dish_Ingredient ingredient in elem.Restaurant_Menu_Dish_Ingredients.OrderBy(p => p.ID))
                                    {
                                        CDish_Ingridient ingr = new()
                                        {
                                            ID = ingredient.ID,
                                            Cooking_Minutes = ingredient.Cooking_Minutes,
                                            DishID = item.ID,
                                            Dish = elem.Dish,
                                            Ingridient = ingredient.Ingridient,
                                            IngridientGroupID = ingredient.IngridientGroupID,
                                            IngridientID = ingredient.IngridientID,
                                            Price = ingredient.Price,
                                            Category_ID = ingredient.Category_ID
                                        };

                                        List_Dish_Ingridients.Add(ingr);
                                    }
                                }


                                // Объемы
                                if (elem.Restaurant_Menu_Dish_Volumes != null && elem.Restaurant_Menu_Dish_Volumes.Count != 0)
                                {
                                    foreach (CRestaurant_Menu_Dish_Volume volume in elem.Restaurant_Menu_Dish_Volumes.OrderBy(p => p.ID))
                                    {
                                        CDish_Volume vol = new()
                                        {
                                            ID = volume.ID,
                                            DishID = item.ID,
                                            OrderNum = volume.OrderNum,
                                            VolumeID = volume.VolumeID,
                                            VolumeName = volume.VolumeName,
                                            VolumePrice = volume.VolumePrice,
                                            VolumeValue = volume.VolumeValue
                                        };

                                        List_Dish_Volumes.Add(vol);
                                    }
                                }


                                decimal resultPrice = 0;


                                //Если не выставлена цена в конкретном меню, то нужно получить данные с общего меню и подтянуть цену.
                                if (!elem.Price.HasValue || elem.Price == 0)
                                {
                                    Dish? dish_Global = _dl_Dish.Get(ID: elem.DishID);

                                    if (dish_Global != null)
                                    {
                                        resultPrice = dish_Global.Price;
                                    }
                                }
                                else
                                {
                                    resultPrice = elem.Price.Value;
                                }

                                CDish dish = new()
                                {
                                    ID = item.ID,
                                    Bonuses = item.Bonuses,
                                    Cooking_Minutes = item.Cooking_Minutes,
                                    Dish_CategoryID = item.Dish_CategoryID,
                                    Flag_AllowColors = item.Flag_AllowColors,
                                    Image_ChangeDate = item.Image_ChangeDate,
                                    Flag_Bonus = item.Flag_Bonus,
                                    Image = item.Image,
                                    Name = item.Name,
                                    OrderNum = elem.OrderNum,
                                    Price = resultPrice,
                                    Teaser = item.Teaser,
                                    Text = item.Text,
                                    FlagNew = elem.FlagNew,
                                    Dish_Category = item.Dish_Category,
                                    List_Dish_Ingridients = List_Dish_Ingridients,
                                    List_Dish_Volumes = List_Dish_Volumes
                                };
                                list_result.Add(dish);
                            }
                        }
                    }
                }

            }

            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }



       


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CDish_ForAdmin> GetList_ForAdmin_v3(int RestaurantMenuID, bool Include_VolumesList = false, bool Include_Ingridients = false)
        {
            List<CDish_ForAdmin> list_result = [];

            IQueryable<Dish> query = _dl_Dish.GetQueryForAdmin(RestaurantMenuID: RestaurantMenuID);

            foreach (Dish elem in query.ToList())
            {
                CDish_ForAdmin? item = Get_ForAdmin(Elem: elem, Include_VolumesList: Include_VolumesList, Include_Ingridients: Include_Ingridients);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CDish_ForAdmin> GetList_ForAdmin_v2(int RestaurantMenuID, bool Include_VolumesList = false, bool Include_Ingridients = false)
        {
            List<CDish_ForAdmin> list_result = [];

            IQueryable<Dish> query = _dl_Dish.GetQueryForAdmin(RestaurantMenuID: RestaurantMenuID);

            foreach (Dish elem in query.ToList())
            {
                CDish_ForAdmin? item = Get_ForAdmin(Elem: elem, Include_VolumesList: Include_VolumesList, Include_Ingridients: Include_Ingridients);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CDish_ForAdmin> GetList_ForAdmin(int? DishCategoryID = null, bool Include_VolumesList = false, bool Include_Ingridients = false, bool AddCleanRow = false, bool? Flag_Del_val = false)
        {
            List<CDish_ForAdmin> list_result = [];

            if (AddCleanRow)
            {
                list_result.Add(new CDish_ForAdmin { ID = Consts.NoData_Int, Name = Consts.NoData_Str });
            }

            IQueryable<Dish> query = _dl_Dish.GetQuery_Edit(Flag_Del_val: Flag_Del_val);

            if (DishCategoryID != null)
            {
                query = from p in query where p.Dish_CategoryID == DishCategoryID select p;
            }

            foreach (Dish elem in query.ToList())
            {
                CDish_ForAdmin? item = Get_ForAdmin(Elem: elem, Include_VolumesList: Include_VolumesList, Include_Ingridients: Include_Ingridients);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }


        /// <summary>
        /// Поиск по блюдам
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Include_VolumesList"></param>
        /// <param name="Include_Ingridients"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CDish_ForAdmin> Search(string Name, bool Include_VolumesList = false, bool Include_Ingridients = false, bool? Flag_Del_val = false)
        {
            List<CDish_ForAdmin> list_result = [];

            IQueryable<Dish> query = _dl_Dish.GetQuerySearch(Name: Name, Flag_Del_val: Flag_Del_val);

            foreach (Dish elem in query.ToList())
            {
                CDish_ForAdmin? item = Get_ForAdmin(Elem: elem, Include_VolumesList: Include_VolumesList, Include_Ingridients: Include_Ingridients);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Dish_CategoryID"></param>
        /// <param name="Name"></param>
        /// <param name="Price"></param>
        /// <param name="Flag_AllowColors"></param>
        /// <param name="OrderNum"></param>
        /// <param name="Image"></param>
        /// <param name="Teaser"></param>
        /// <param name="Text"></param>
        /// <param name="Bonuses"></param>
        /// <param name="Flag_Bonus"></param>
        /// <param name="Cooking_Minutes"></param>
        /// <param name="RecommendDishIDs"></param>
        /// <param name="FlagNew"></param>
        /// <param name="FlagNoActive"></param>
        /// <returns></returns>
        public int Add(int Dish_CategoryID, string Name, decimal Price, bool Flag_AllowColors, int OrderNum, byte[]? Image, string? Teaser, string? Text,
                       decimal? Bonuses, bool Flag_Bonus, int? Cooking_Minutes, string? RecommendDishIDs, bool FlagNew, bool FlagNoActive, DateTime? New_DateFrom)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Dish.Add(Dish_CategoryID: Dish_CategoryID, Name: Name, Price: Price, Flag_AllowColors: Flag_AllowColors, OrderNum: OrderNum,
                                Image: Image, Teaser: Teaser, Text: Text, Bonuses: Bonuses, Flag_Bonus: Flag_Bonus, Cooking_Minutes: Cooking_Minutes,
                                RecommendDishIDs: RecommendDishIDs, FlagNew: FlagNew, FlagNoActive: FlagNoActive, New_DateFrom: New_DateFrom);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Номер блюда</param>
        /// <param name="Dish_CategoryID_New">номер категории блюда</param>
        /// <param name="Name_New">Новое имя</param>
        /// <param name="Price_New">Новая цена</param>
        /// <param name="Flag_AllowColors_New">Флаг цветов</param>
        /// <param name="OrderNum_New">Номер заказа</param>
        /// <param name="Image_New">Новая картинка</param>
        /// <param name="Teaser_New">Тизер</param>
        /// <param name="Text_New">Текст</param>
        /// <param name="Bonuses_New">Бонусы</param>
        /// <param name="Flag_Bonus_New">Флаг бонусов</param>
        /// <param name="Cooking_Minutes_New">Время на готовку в минутах</param>
        /// <param name="FlagNoActive_New">Флаг активности</param>
        /// <param name="RecommendDishIDs_New">Рекомендации</param>
        /// <param name="FlagNew_New">Флаг</param>
        /// <returns></returns>
        public int Edit(int ID, int Dish_CategoryID_New, string Name_New, decimal Price_New, bool Flag_AllowColors_New, int OrderNum_New, byte[]? Image_New,
                        string? Teaser_New, string? Text_New, decimal? Bonuses_New, bool Flag_Bonus_New, int? Cooking_Minutes_New, string? RecommendDishIDs_New,
                        bool FlagNew_New, bool FlagNoActive_New, DateTime? New_DateFrom_New)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Dish.Edit(ID: ID, Dish_CategoryID_New: Dish_CategoryID_New, Name_New: Name_New, Price_New: Price_New,
                                 Flag_AllowColors_New: Flag_AllowColors_New, OrderNum_New: OrderNum_New, Image_New: Image_New, Teaser_New: Teaser_New,
                                 Text_New: Text_New, Bonuses_New: Bonuses_New, Flag_Bonus_New: Flag_Bonus_New, Cooking_Minutes_New: Cooking_Minutes_New,
                                 RecommendDishIDs_New: RecommendDishIDs_New, FlagNew_New: FlagNew_New, FlagNoActive_New: FlagNoActive_New, New_DateFrom_New: New_DateFrom_New);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Dish.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }


        /// <summary>
        /// Удаление изображения
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Del_Image(int ID)
        {
            return _dl_Dish.Del_Image(ID: ID);
        }


        /// <summary>
        /// Изменение даты изменения изображения
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Edit_Image_ChangeData(int ID)
        {
            return _dl_Dish.Edit_Image_ChangeData(ID: ID);
        }


        /// <summary>
        /// Изменение изображения
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Dish_Image"></param>
        /// <returns></returns>
        public int Edit_Image(int ID, byte[] Dish_Image)
        {
            return _dl_Dish.Edit_Image(ID: ID, Dish_Image: Dish_Image);
        }

    }
}