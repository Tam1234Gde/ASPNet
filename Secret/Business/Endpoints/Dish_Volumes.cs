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
    /// Объёмы блюда
    /// </summary>
    public class Dish_Volumes
    {
        private readonly ILogger _log;
        private readonly Dish_Volume_oper _dl_Dish_Volume = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Dish_Volumes()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CDish_Volume? Get(Dish_Volume? Elem)
        {
            if (Elem != null)
            {
                CDish_Volume item = new()
                {
                    ID = Elem.ID,
                    DishID = Elem.DishID,
                    Dish = Elem.Dish?.Name,
                    VolumePrice = Elem.Price,
                    VolumeID = Elem.VolumeID,
                    VolumeValue = Elem.Volume?.Value,
                    VolumeName = Elem.Volume?.Name,
                    OrderNum = Elem.OrderNum,
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
        public CDish_Volume? Get(int ID)
        {
            return Get(Elem: _dl_Dish_Volume.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="List_from"></param>
        /// <returns></returns>
        public static List<CDish_Volume>? GetList(List<Dish_Volume>? List_from)
        {
            if (List_from != null)
            {
                List<CDish_Volume> list_result = [];

                foreach (Dish_Volume elem in List_from.Where(p => !p.Flag_Del))
                {
                    CDish_Volume? item = Get(Elem: elem);
                    if (item != null)
                    {
                        list_result.Add(item);
                    }
                }
                return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.VolumeValue)];
            }

            return null;
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CDish_Volume>? GetList(int DishID, bool? Flag_Del_val = false)
        {
            return GetList(List_from: [.. _dl_Dish_Volume.GetQuery_ByDishID(DishID: DishID, Flag_Del_val: Flag_Del_val)]);
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="Price"></param>
        /// <param name="VolumeID"></param>
        /// <param name="OrderNum"></param>
        /// <returns></returns>
        public int Add(int DishID, int VolumeID, decimal Price, int OrderNum)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Dish_Volume.Add(DishID: DishID, VolumeID: VolumeID, Price: Price, OrderNum: OrderNum);
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DishID_New"></param>
        /// <param name="VolumeID_New"></param>
        /// <param name="Price_New"></param>
        /// <param name="OrderNum_New"></param>
        /// <returns></returns>
        public int Edit(int ID, int DishID_New, int VolumeID_New, decimal Price_New, int OrderNum_New)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Dish_Volume.Edit(ID: ID, DishID_New: DishID_New, VolumeID_New: VolumeID_New, Price_New: Price_New, OrderNum_New: OrderNum_New);
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
            return _dl_Dish_Volume.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }

    }
}