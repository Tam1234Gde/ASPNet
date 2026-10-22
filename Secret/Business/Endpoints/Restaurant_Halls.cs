using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Залы в ресторане
    /// </summary>
    public class Restaurant_Halls
    {
        private readonly ILogger _log;
        private readonly Restaurant_Hall_oper _dl_Restaurant_Hall = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_Halls()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CRestaurant_Hall? Get(Restaurant_Hall? Elem)
        {
            if (Elem != null)
            {
                CRestaurant_Hall item = new()
                {
                    ID = Elem.ID,
                    Restaurant = Elem.Restaurant.Adress,
                    RestaurantID = Elem.RestaurantID,
                    Restaurant_Image = Elem.Restaurant_Image,
                    Restaurant_Table_Type = Elem.Restaurant_Table_Type?.Name,
                    Restaurant_Table_TypeID = Elem.Restaurant_Table_TypeID,
                    Flag_Active = Elem.Flag_Active,
                    Name = Elem.Name
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
        public CRestaurant_Hall? Get(int ID)
        {
            return Get(Elem: _dl_Restaurant_Hall.Get(ID));
        }


        /// <summary>
        /// Получение данных по RestaurantID и Restaurant_Table_TypeID
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <returns></returns>
        public CRestaurant_Hall? Get(int RestaurantID, ERestaurant_Table_Type Restaurant_Table_TypeID)
        {
            return Get(Elem: _dl_Restaurant_Hall.GetByRestaurantID(RestaurantID, Restaurant_Table_TypeID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant_Hall>? GetList(bool? Flag_Del_val = false)
        {
            List<CRestaurant_Hall> list_result = [];

            foreach (Restaurant_Hall elem in _dl_Restaurant_Hall.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant_Hall? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <returns></returns>
        public CRestaurant_Hall? Get_Image(int ID, ERestaurant_Table_Type Restaurant_Table_TypeID)
        {
            return Get(Elem: _dl_Restaurant_Hall.Get(RestaurantID: ID, Restaurant_Table_TypeID: Restaurant_Table_TypeID));
        }


        /// <summary>
        /// Возвращает список по RestaurantID
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant_Hall>? GetListByRestaurantID(int RestaurantID, bool? Flag_Del_val = false)
        {
            List<CRestaurant_Hall> list_result = [];

            foreach (Restaurant_Hall elem in _dl_Restaurant_Hall.GetQueryByRestaurantID(RestaurantID: RestaurantID, Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant_Hall? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Возвращает список по RestaurantID и Restaurant_Table_TypeID
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant_Hall>? GetListByRestaurantID(int RestaurantID, ERestaurant_Table_Type? Restaurant_Table_TypeID, bool? Flag_Del_val = false)
        {
            List<CRestaurant_Hall> list_result = [];

            foreach (Restaurant_Hall elem in _dl_Restaurant_Hall.GetQueryByRestaurantID(RestaurantID: RestaurantID, Restaurant_Table_TypeID: Restaurant_Table_TypeID,
                                                                                        Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant_Hall? item = Get(Elem: elem);
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
        /// <param name="RestaurantID"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <param name="Restaurant_Image"></param>
        /// <param name="Flag_Active"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int Add(int RestaurantID, ERestaurant_Table_Type? Restaurant_Table_TypeID, byte[]? Restaurant_Image, bool Flag_Active, string? Name)
        {
            return _dl_Restaurant_Hall.Add(RestaurantID: RestaurantID, Restaurant_Table_TypeID: Restaurant_Table_TypeID,
                                           Restaurant_Image: Restaurant_Image, Flag_Active: Flag_Active, Name: Name);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="RestaurantID"></param>
        /// <param name="Restaurant_Table_TypeID"></param>
        /// <param name="Restaurant_Image"></param>
        /// <param name="Flag_Active"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int Edit(int ID, int RestaurantID, ERestaurant_Table_Type? Restaurant_Table_TypeID, byte[]? Restaurant_Image, bool Flag_Active, string? Name)
        {
            return _dl_Restaurant_Hall.Edit(ID: ID, RestaurantID: RestaurantID, Restaurant_Table_TypeID: Restaurant_Table_TypeID,
                                            Restaurant_Image: Restaurant_Image, Flag_Active: Flag_Active, Name: Name);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            return _dl_Restaurant_Hall.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }

    }
}