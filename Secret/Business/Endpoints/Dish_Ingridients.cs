using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Ингридиенты блюда
    /// </summary>
    public class Dish_Ingridients
    {
        private readonly ILogger _log;
        private readonly Dish_Ingridient_oper _dl_Dish_Ingridient = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Dish_Ingridients()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogDebug("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CDish_Ingridient? Get(Dish_Ingridient? Elem)
        {
            if (Elem != null)
            {
                CDish_Ingridient item = new()
                {
                    ID = Elem.ID,
                    DishID = Elem.DishID,
                    Dish = Elem.Dish?.Name,
                    Price = Elem.Price,
                    Ingridient = Elem.Ingridient?.Name,
                    IngridientID = Elem.IngridientID,
                    IngridientGroupID = Elem.Ingridient_Category_ID,
                    Cooking_Minutes = Elem.Cooking_Minutes,
                };
                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CDish_Ingridient_ForAdmin? Get_ForAdmin(Dish_Ingridient? Elem)
        {
            if (Elem != null)
            {
                CDish_Ingridient_ForAdmin item = new()
                {
                    ID = Elem.ID,
                    DishID = Elem.DishID,
                    Dish = Elem.Dish?.Name,
                    Price = Elem.Price,
                    Ingridient = Elem.Ingridient?.Name,
                    IngridientID = Elem.IngridientID,
                    IngridientGroupID = Elem.Ingridient_Category_ID,
                    Cooking_Minutes = Elem.Cooking_Minutes,
                    Image = Elem.Ingridient?.Image
                };
                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CDish_Ingridient? Get(int ID)
        {
            return Get(Elem: _dl_Dish_Ingridient.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="List_from"></param>
        /// <returns></returns>
        public static List<CDish_Ingridient>? GetList(List<Dish_Ingridient>? List_from)
        {
            if (List_from != null)
            {
                List<CDish_Ingridient> list_result = [];

                foreach (Dish_Ingridient elem in List_from.Where(p => p.Ingridient != null && !p.Ingridient.Flag_Del))
                {
                    CDish_Ingridient? item = Get(Elem: elem);
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
        /// Возвращает список
        /// </summary>
        /// <param name="List_from"></param>
        /// <returns></returns>
        public static List<CDish_Ingridient_ForAdmin>? GetList_ForAdmin(List<Dish_Ingridient>? List_from)
        {
            if (List_from != null)
            {
                List<CDish_Ingridient_ForAdmin> list_result = [];

                foreach (Dish_Ingridient elem in List_from)
                {
                    CDish_Ingridient_ForAdmin? item = Get_ForAdmin(Elem: elem);
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
        /// Возвращает список
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CDish_Ingridient>? GetList(int DishID, bool? Flag_Del_val = false)
        {
            return GetList(List_from: [.. _dl_Dish_Ingridient.GetQuery_ByDishID(DishID: DishID, Flag_Del_val: Flag_Del_val)]);
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CDish_Ingridient_ForAdmin>? GetList_ForAdmin(int DishID, bool? Flag_Del_val = false)
        {
            return GetList_ForAdmin(List_from: [.. _dl_Dish_Ingridient.GetQuery_ByDishID(DishID: DishID, Flag_Del_val: Flag_Del_val)]);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="IngridientID"></param>
        /// <param name="Price"></param>
        /// <param name="IngridientGroupID"></param>
        /// <param name="Cooking_Minutes"></param>
        /// <returns></returns>
        public int Add(int DishID, int IngridientID, decimal Price, int? IngridientGroupID, int? Cooking_Minutes)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Dish_Ingridient.Add(DishID: DishID, IngridientID: IngridientID, Price: Price, IngridientGroupID: IngridientGroupID, Cooking_Minutes: Cooking_Minutes);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DishID_New"></param>
        /// <param name="IngridientID_New"></param>
        /// <param name="Price_New"></param>
        /// <param name="IngridientGroupID_New"></param>
        /// <param name="Cooking_Minutes_New"></param>
        /// <returns></returns>
        public int Edit(int ID, int DishID_New, int IngridientID_New, decimal Price_New, int? IngridientGroupID_New, int? Cooking_Minutes_New)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Dish_Ingridient.Edit(ID: ID, DishID_New: DishID_New, IngridientID_New: IngridientID_New, Price_New: Price_New,
                                            IngridientGroupID_New: IngridientGroupID_New, Cooking_Minutes_New: Cooking_Minutes_New);
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
            return _dl_Dish_Ingridient.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }

    }
}