using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Цвета блюд
    /// </summary>
    public class Dish_Colors
    {
        private readonly ILogger _log;
        private readonly Dish_Color_oper _dl_Dish_Color = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Dish_Colors()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogDebug("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CDish_Color? Get(Dish_Color? Elem)
        {
            if (Elem != null)
            {
                CDish_Color item = new()
                {
                    ID = Elem.ID,
                    ColorID = Elem.ColorID,
                    DishID = Elem.DishID,
                    Dish = Elem.Dish?.Name,
                    Color = Elem.Color?.Name
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
        public CDish_Color? Get(int ID)
        {
            return Get(Elem: _dl_Dish_Color.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="List_from"></param>
        /// <returns></returns>
        public static List<CDish_Color>? GetList(List<Dish_Color>? List_from)
        {
            if (List_from != null)
            {
                List<CDish_Color> list_result = [];

                foreach (Dish_Color elem in List_from)
                {
                    CDish_Color? item = Get(Elem: elem);
                    if (item != null)
                    {
                        list_result.Add(item);
                    }
                }
                return [.. list_result];
            }

            return null;
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CDish_Color>? GetList(int DishID, bool? Flag_Del_val = false)
        {
            return GetList(List_from: [.. _dl_Dish_Color.GetQuery_ByDishID(DishID: DishID, Flag_Del_val: Flag_Del_val)]);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="ColorID"></param>
        /// <returns></returns>
        public int Add(int DishID, int ColorID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Dish_Color.Add(DishID: DishID, ColorID: ColorID);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DishID_New"></param>
        /// <param name="ColorID_New"></param>
        /// <returns></returns>
        public int Edit(int ID, int DishID_New, int ColorID_New)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Dish_Color.Edit(ID: ID, DishID_New: DishID_New, ColorID_New: ColorID_New);
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
            return _dl_Dish_Color.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }

    }
}