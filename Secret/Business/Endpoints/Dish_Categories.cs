using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
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
    /// Категории блюд
    /// </summary>
    public class Dish_Categories
    {
        private readonly ILogger _log;
        private readonly Dish_Category_oper _dl_Dish_Category = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Dish_Categories()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <param name="Flag_GetDishes"></param>
        /// <param name="CategoryDict">Словарь категорий для получения родителя</param>
        /// <returns></returns>
        private static CDish_Category? Get(Dish_Category? Elem, bool? Flag_GetDishes = false, Dictionary<int, Dish_Category>? CategoryDict = null)
        {
            if (Elem != null)
            {
                CDish_Category item = new()
                {
                    ID = Elem.ID,
                    Name = Elem.Name,
                    OrderNum = Elem.OrderNum,
                    Flag_AllowColors = Elem.Flag_AllowColors,
                    Cooking_Minutes = Elem.Cooking_Minutes,
                    Flag_IsParent = Elem.Flag_IsParent,
                    ParentID = Elem.ParentID,
                    Work_Hours = Elem.Work_Hours,
                    Flag_Del = Elem.Flag_Del,
                    Flag_UseDefaultWorkHours = Elem.Flag_UseDefaultWorkHours
                };

                // Получаем название родительской категории
                if (Elem.ParentID.HasValue && CategoryDict != null)
                {
                    if (CategoryDict.TryGetValue(Elem.ParentID.Value, out Dish_Category? parent))
                    {
                        item.ParentCategory_Name = parent.Name;
                        item.ParentCategory_IsDeleted = parent.Flag_Del == true;
                    }
                    else
                    {
                        item.ParentCategory_Name = "Категория не найдена";
                        item.ParentCategory_IsDeleted = true;
                    }
                }
                else if (Elem.ParentID.HasValue)
                {
                    item.ParentCategory_Name = "Категория не найдена";
                    item.ParentCategory_IsDeleted = true;
                }
                else
                {
                    item.ParentCategory_Name = "Без родителя";
                    item.ParentCategory_IsDeleted = false;
                }

                // Получаем блюда
                if (Elem.Dishes != null && Elem.Dishes.Count != 0 &&
                    Flag_GetDishes.HasValue && Flag_GetDishes.Value)
                {
                    item.Dishes = [];
                    foreach (Dish elem in Elem.Dishes)
                    {
                        CDish? dish = GetDish(Elem: elem, Include_VolumesList: true, Include_Ingridients: true);
                        if (dish != null)
                        {
                            item.Dishes.Add(dish);
                        }
                    }
                }

                return item;
            }

            return null;
        }


        /// <summary>
        /// Возвращает список дочерних категорий для указанной родительской категории
        /// </summary>
        /// <param name="parentID">ID родительской категории</param>
        /// <param name="Flag_Del_val">Фильтр по удалённым (false - только активные, true - удалённые, null - все)</param>
        /// <returns>Отсортированный список категорий</returns>
        public List<CDish_Category> GetListByParentID(int parentID, bool? Flag_Del_val = false)
        {
            IQueryable<Dish_Category>? query = _dl_Dish_Category.GetQuery(Flag_Del_val: Flag_Del_val)
                                         .Where(c => c.ParentID == parentID);

            List<CDish_Category> result = [];
            foreach (Dish_Category elem in query.ToList())
            {
                CDish_Category? item = Get(elem); 
                if (item != null)
                    result.Add(item);
            }

            return [.. result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <param name="Flag_GetDishes"></param>
        /// <returns></returns>
        private static CDish_Category_ForAdmin? GetForSite(Dish_Category? Elem, bool? Flag_GetDishes = false)
        {
            if (Elem != null)
            {
                CDish_Category_ForAdmin item = new()
                {
                    ID = Elem.ID,
                    Name = Elem.Name,
                    OrderNum = Elem.OrderNum,
                    Flag_AllowColors = Elem.Flag_AllowColors,
                    Cooking_Minutes = Elem.Cooking_Minutes,
                };

                // Получаем блюда
                if (Elem.Dishes != null && Elem.Dishes.Count != 0 &&
                    Flag_GetDishes.HasValue && Flag_GetDishes.Value)
                {
                    item.Dishes = [];
                    foreach (Dish elem in Elem.Dishes)
                    {
                        CDish_ForAdmin? dish = GetDishForAdmin(Elem: elem, Include_VolumesList: true, Include_Ingridients: false);
                        if (dish != null && !dish.Flag_Del)
                        {
                            item.Dishes.Add(dish);
                        }
                    }
                }

                return item;
            }

            return null;
        }



        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        private static CDish? GetDish(Dish? Elem, bool Include_VolumesList, bool Include_Ingridients)
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
                };

                if (Include_VolumesList)
                {
                    item.List_Dish_Volumes = Dish_Volumes.GetList(List_from: Elem.Dish_Volumes);
                }

                if (Include_Ingridients)
                {
                    item.List_Dish_Ingridients = Dish_Ingridients.GetList(List_from: Elem.Dish_Ingridients);
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
        private static CDish_ForAdmin? GetDishForAdmin(Dish? Elem, bool Include_VolumesList, bool Include_Ingridients)
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
                    Bonuses = Elem.Bonuses,
                    Flag_Bonus = Elem.Flag_Bonus,
                    Cooking_Minutes = Elem.Cooking_Minutes,
                    Flag_Del = Elem.Flag_Del
                };

                if (Include_VolumesList)
                {
                    item.List_Dish_Volumes = Dish_Volumes.GetList(List_from: Elem.Dish_Volumes);
                }

                if (Include_Ingridients)
                {
                    item.List_Dish_Ingridients = Dish_Ingridients.GetList_ForAdmin(List_from: Elem.Dish_Ingridients);
                }

                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CDish_Category? Get(int ID)
        {
            return Get(Elem: _dl_Dish_Category.Get(ID));
        }




        /// <summary>
        /// Получение данных категории в коротком виде
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CCommon? Get_Short(Dish_Category? Elem)
        {
            if (Elem != null)
            {
                CCommon item = new()
                {
                    ID = Elem.ID,
                    Name = Elem.Name,
                };

                return item;
            }

            return null;
        }


        /// <summary>
        /// Возвращает список категорий
        /// </summary>
        /// <param name="AddCleanRow"></param>
        /// <param name="Flag_Del_val"></param>
        /// <param name="Flag_IsParent">Вернуть родительские</param>
        /// <returns></returns>
        public List<CDish_Category> GetList(bool Flag_IsParent, bool AddCleanRow = false, bool? Flag_Del_val = false)
        {
            List<CDish_Category> list_result = [];

            if (AddCleanRow)
            {
                list_result.Add(new CDish_Category { ID = Consts.NoData_Int, Name = Consts.NoData_Str });
            }

            // Получаем все категории
            List<Dish_Category> categories = _dl_Dish_Category.GetQuery(Flag_Del_val: Flag_Del_val).ToList();

            // Создаём словарь для быстрого доступа к категориям по ID
            Dictionary<int, Dish_Category> categoryDict = categories.ToDictionary(c => c.ID, c => c);

            foreach (Dish_Category elem in categories)
            {
                CDish_Category? item = Get(Elem: elem);
                if (item != null)
                {
                    // Устанавливаем флаг удаления
                    item.Flag_Del = elem.Flag_Del;

                    // Получаем название родительской категории
                    if (elem.ParentID.HasValue)
                    {
                        if (categoryDict.TryGetValue(elem.ParentID.Value, out Dish_Category? parent))
                        {
                            item.ParentCategory_Name = parent.Name;
                            item.ParentCategory_IsDeleted = parent.Flag_Del == true;
                        }
                        else
                        {
                            item.ParentCategory_Name = "Категория не найдена";
                            item.ParentCategory_IsDeleted = true;
                        }
                    }
                    else
                    {
                        item.ParentCategory_Name = "Без родителя";
                        item.ParentCategory_IsDeleted = false;
                    }

                    list_result.Add(item);
                }
            }

            return [.. list_result.Where(x => x.Flag_IsParent == Flag_IsParent).OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="AddCleanRow"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CDish_Category> GetListByRestaurantID(int RestaurantID, bool AddCleanRow = false, bool? Flag_Del_val = false)
        {
            List<CDish_Category> list_result = [];

            CRestaurant? restaurant = new Restaurants().Get(RestaurantID);

            if (restaurant == null)
            {
                return list_result;
            }

            CRestaurant_Menu? cRestaurant_Menu = new Restaurant_Menus().GetActive(RestaurantID: RestaurantID);

            if (cRestaurant_Menu == null)
            {
                return list_result;
            }

            if (AddCleanRow)
            {
                list_result.Add(new CDish_Category { ID = Consts.NoData_Int, Name = Consts.NoData_Str });
            }

            foreach (Dish_Category elem in _dl_Dish_Category.GetQueryByRestaurant_MenuID(Restaurant_MenuID: cRestaurant_Menu.ID, Flag_Del_val: Flag_Del_val).ToList())
            {
                CDish_Category? item = Get(Elem: elem);
                if (item != null && !item.Flag_IsParent)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }


        /// <summary>
        /// Возвращает список родительских категорий
        /// </summary>
        /// <param name="AddCleanRow"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CDish_Category> GetParentList(bool AddCleanRow = false, bool? Flag_Del_val = null)
        {
            List<CDish_Category> list_result = [];

            if (AddCleanRow)
            {
                list_result.Add(new CDish_Category { ID = Consts.NoData_Int, Name = Consts.NoData_Str });
            }

            // Получаем настройки
            Setups _bl_Setups = new Setups();
            CSetup? defaultWorkHoursSetup = _bl_Setups.Get(Consts.SetupKey_DefaultWorkHours);
            CSetup? useDefaultWorkHoursSetup = _bl_Setups.Get(Consts.SetupKey_UseDefaultWorkHours);

            // Проверяем, включено ли использование часов по умолчанию
            bool useDefaultWorkHours = false;
            int[]? defaultWorkHours = null;

            if (useDefaultWorkHoursSetup != null && HIDE_Convert.ToBool(useDefaultWorkHoursSetup.Val) == true)
            {
                useDefaultWorkHours = true;

                // Получаем часы работы по умолчанию
                if (defaultWorkHoursSetup != null && !string.IsNullOrWhiteSpace(defaultWorkHoursSetup.Val))
                {
                    string[] hoursArray = defaultWorkHoursSetup.Val.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    defaultWorkHours = hoursArray.Select(int.Parse).ToArray();
                }
            }

            // Получаем только родительские категории (Flag_IsParent == true)
            List<Dish_Category> categories = [.. _dl_Dish_Category.GetQuery(Flag_Del_val: Flag_Del_val).Where(x => x.Flag_IsParent == true)];

            foreach (Dish_Category elem in categories)
            {
                CDish_Category? item = Get(Elem: elem);
                if (item != null)
                {
                    // Если у категории включен флаг использования часов по умолчанию
                    if (item.Flag_UseDefaultWorkHours && useDefaultWorkHours)
                    {
                        // Подставляем часы из настроек
                        item.Work_Hours = defaultWorkHours;
                    }

                    list_result.Add(item);
                }
            }

            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="AddCleanRow"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CDish_Category_ForAdmin> GetListForSite(bool AddCleanRow = false, bool? Flag_Del_val = false)
        {
            List<CDish_Category_ForAdmin> list_result = [];

            if (AddCleanRow)
            {
                list_result.Add(new CDish_Category_ForAdmin { ID = Consts.NoData_Int, Name = Consts.NoData_Str });
            }

            foreach (Dish_Category elem in _dl_Dish_Category.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CDish_Category_ForAdmin? item = GetForSite(Elem: elem, Flag_GetDishes: true);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }



        /// <summary>
        /// Возвращает список в коротком виде
        /// </summary>
        /// <returns></returns>
        public List<CCommon> GetList_Short()
        {
            List<CCommon> list_result = [];
            foreach (Dish_Category elem in _dl_Dish_Category.GetQuery().ToList())
            {
                CCommon? item = Get_Short(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Возвращает список в коротком виде
        /// </summary>
        /// <returns></returns>
        public List<CCommon> GetList_Short_SubCategories()
        {
            List<CCommon> list_result = [];
            foreach (Dish_Category elem in _dl_Dish_Category.GetQuery().ToList())
            {
                if (elem.Flag_IsParent)
                {
                    continue;
                }

                CCommon? item = Get_Short(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="OrderNum"></param>
        /// <param name="Flag_AllowColors"></param>
        /// <param name="Cooking_Minutes"></param>
        /// <param name="Flag_IsParent"></param>
        /// <param name="ParentID"></param>
        /// <param name="Work_Hours"></param>
        /// <returns></returns>
        public int Add(string Name, int OrderNum, bool Flag_AllowColors, int? Cooking_Minutes, bool Flag_IsParent, int? ParentID, int[]? Work_Hours, bool Flag_UseDefaultWorkHours)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Dish_Category.Add(Name: Name, OrderNum: OrderNum, Flag_AllowColors: Flag_AllowColors, Cooking_Minutes: Cooking_Minutes, ParentID: ParentID, Flag_IsParent: Flag_IsParent, Work_Hours: Work_Hours, Flag_UseDefaultWorkHours: Flag_UseDefaultWorkHours);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name_New"></param>
        /// <param name="OrderNum_New"></param>
        /// <param name="Flag_AllowColors_New"></param>
        /// <param name="Cooking_Minutes_New"></param>
        /// <param name="Flag_IsParent_New"></param>
        /// <param name="ParentID_New"></param>
        /// <param name="Work_Hours_New"></param>
        /// <param name="Flag_UseDefaultWorkHours"></param>
        /// <returns></returns>
        public int Edit(int ID, string Name_New, int OrderNum_New, bool Flag_AllowColors_New, int? Cooking_Minutes_New, bool Flag_IsParent_New, int? ParentID_New, int[]? Work_Hours_New, bool Flag_UseDefaultWorkHours)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Dish_Category.Edit(ID: ID, Name_New: Name_New, OrderNum_New: OrderNum_New, Flag_AllowColors_New: Flag_AllowColors_New, Cooking_Minutes_New: Cooking_Minutes_New, Flag_IsParent_New: Flag_IsParent_New, ParentID_New: ParentID_New, Work_Hours_New: Work_Hours_New, Flag_UseDefaultWorkHours: Flag_UseDefaultWorkHours);
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
            return _dl_Dish_Category.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }


        /// <summary>
        /// Восстановление категории блюда
        /// </summary>
        /// <param name="ID">ID категории</param>
        /// <returns>Количество затронутых записей</returns>
        public int Restore(int ID)
        {
            return _dl_Dish_Category.Edit_FlagDel(ID: ID, Flag_Del_New: false, UserID: 1);
        }

    }
}