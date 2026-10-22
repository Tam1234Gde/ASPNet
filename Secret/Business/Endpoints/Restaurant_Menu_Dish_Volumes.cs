using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Объем блюда в меню ресторана
    /// </summary>
    public class Restaurant_Menu_Dish_Volumes
    {
        private readonly ILogger _log;
        private readonly Restaurant_Menu_Dish_Volume_oper _dl_Restaurant_Menu_Dish_Volume = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_Menu_Dish_Volumes()
        {
            _log = AppLog.CreateLogger(GetType());
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem">Объект</param>
        /// <returns></returns>
        private static CRestaurant_Menu_Dish_Volume? Get(Restaurant_Menu_Dish_Volume? Elem)
        {
            if (Elem == null) return null;

            CRestaurant_Menu_Dish_Volume item = new()
            {
                ID = Elem.ID,
                VolumePrice = Elem.Price,
                VolumeID = Elem.VolumeID,
                VolumeValue = Elem.Volume?.Value,
                VolumeName = Elem.Volume?.Name,
                OrderNum = Elem.OrderNum,
                Restaurant_Menu_DishID = Elem.Restaurant_Menu_DishID,
                Restaurant_Menu_Dish = Elem.Restaurant_Menu_Dish?.Dish?.Name,
            };
            return item;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        public CRestaurant_Menu_Dish_Volume? Get(int ID)
        {
            return Get(Elem: _dl_Restaurant_Menu_Dish_Volume.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Restaurant_Menu_DishID"></param>
        /// <param name="Flag_Del_val">True - Помеченные как удаленные</param>
        /// <returns></returns>
        public List<CRestaurant_Menu_Dish_Volume> GetList(int Restaurant_Menu_DishID, bool? Flag_Del_val = false)
        {
            List<CRestaurant_Menu_Dish_Volume> list_result = [];
            foreach (Restaurant_Menu_Dish_Volume elem in _dl_Restaurant_Menu_Dish_Volume.GetQuery_ByRestaurantID(Restaurant_Menu_DishID: Restaurant_Menu_DishID,
                                                                                                  Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant_Menu_Dish_Volume? item = Get(Elem: elem);
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
        public static List<CRestaurant_Menu_Dish_Volume>? GetList(List<Restaurant_Menu_Dish_Volume>? List_from)
        {
            if (List_from != null)
            {
                List<CRestaurant_Menu_Dish_Volume> list_result = [];

                foreach (Restaurant_Menu_Dish_Volume elem in List_from.Where(p => !p.Flag_Del))
                {
                    CRestaurant_Menu_Dish_Volume? item = Get(Elem: elem);
                    if (item != null)
                    {
                        list_result.Add(item);
                    }
                }
                return [.. list_result.OrderBy(x => x.OrderNum)];
            }

            return null;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Restaurant_Menu_DishID">Код блюда ресторана</param>
        /// <param name="VolumeID">Код объема</param>
        /// <param name="Price">Стоимость</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <returns></returns>
        public int Add(int Restaurant_Menu_DishID, int VolumeID, decimal Price, int OrderNum)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Restaurant_Menu_Dish_Volume.Add(Restaurant_Menu_DishID: Restaurant_Menu_DishID, VolumeID: VolumeID, Price: Price, OrderNum: OrderNum);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="Restaurant_Menu_DishID">Код блюда ресторана</param>
        /// <param name="VolumeID">Код объема</param>
        /// <param name="Price">Стоимость</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <returns></returns>
        public int Edit(int ID, int Restaurant_Menu_DishID, int VolumeID, decimal Price, int OrderNum)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Restaurant_Menu_Dish_Volume.Edit(ID: ID, Restaurant_Menu_DishID: Restaurant_Menu_DishID, VolumeID: VolumeID, Price: Price,
                                                        OrderNum: OrderNum);
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
            return _dl_Restaurant_Menu_Dish_Volume.Edit_FlagDel(ID: ID, Flag_Del: true, UserID: UserID);
        }

    }
}