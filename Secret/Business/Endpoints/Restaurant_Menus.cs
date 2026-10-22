using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Меню ресторана
    /// </summary>
    public class Restaurant_Menus
    {
        private readonly ILogger _log;
        private readonly Restaurant_Menu_oper _dl_Restaurant_Menu = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_Menus()
        {
            _log = AppLog.CreateLogger(GetType());
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem">Объект</param>
        /// <returns></returns>
        private static CRestaurant_Menu? Get(Restaurant_Menu? Elem)
        {
            if (Elem == null) return null;

            CRestaurant_Menu item = new()
            {
                ID = Elem.ID,
                Name = Elem.Name,
                Flag_Active = Elem.Flag_Active,
                RestaurantID = Elem.RestaurantID,
                Restaurant = Elem.Restaurant?.Adress
            };
            return item;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elems">Объект</param>
        /// <returns></returns>
        private static List<CRestaurant_Menu>? GetList(List<Restaurant_Menu>? Elems)
        {
            List<CRestaurant_Menu> result = [];

            if (Elems == null) return null;

            foreach (Restaurant_Menu elem in Elems)
            {
                CRestaurant_Menu item = new()
                {
                    ID = elem.ID,
                    Name = elem.Name,
                    Flag_Active = elem.Flag_Active,
                    RestaurantID = elem.RestaurantID,
                    Restaurant = elem.Restaurant?.Adress
                };

                result.Add(item);
            }


            return result;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public List<CRestaurant_Menu>? GetList(List<int> IDs)
        {
            return GetList(Elems: _dl_Restaurant_Menu.GetActiveList(IDs)?.ToList());
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        public CRestaurant_Menu? Get(int ID)
        {
            return Get(Elem: _dl_Restaurant_Menu.Get(ID));
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <returns></returns>
        public CRestaurant_Menu? GetActive(int RestaurantID)
        {
            return Get(Elem: _dl_Restaurant_Menu.GetActive(RestaurantID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <param name="Flag_Del_val">True - Помеченные как удаленные</param>
        /// <returns></returns>
        public List<CRestaurant_Menu> GetList(int? RestaurantID = null, bool? Flag_Del_val = false)
        {
            List<CRestaurant_Menu> list_result = [];
            foreach (Restaurant_Menu elem in _dl_Restaurant_Menu.GetQuery_ByRestaurantID(RestaurantID: RestaurantID, Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant_Menu? item = Get(Elem: elem);
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
        /// <param name="Name">Название меню</param>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <param name="Flag_Active">True - Активное меню</param>
        /// <returns></returns>
        public int Add(string Name, int RestaurantID, bool Flag_Active)
        {
            return _dl_Restaurant_Menu.Add(Name: Name, RestaurantID: RestaurantID, Flag_Active: Flag_Active);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="Name">Название меню</param>
        /// <param name="RestaurantID">Код ресторана</param>
        /// <param name="Flag_Active">True - Активное меню</param>
        /// <returns></returns>
        public int Edit(int ID, string Name, int RestaurantID, bool Flag_Active)
        {
            return _dl_Restaurant_Menu.Edit(ID: ID, Name: Name, RestaurantID: RestaurantID, Flag_Active: Flag_Active);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="UserID">Пользователь</param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            return _dl_Restaurant_Menu.Edit_FlagDel(ID: ID, Flag_Del: true, UserID: UserID);
        }

    }
}