using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Статусы столика
    /// </summary>
    public class Restaurant_Table_Statuses
    {
        private readonly ILogger _log;
        private readonly Restaurant_Table_Status_oper _dl_Restaurant_Table_Status = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_Table_Statuses()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CRestaurant_Table_Status? Get(Restaurant_Table_Status? Elem)
        {
            if (Elem != null)
            {
                CRestaurant_Table_Status item = new()
                {
                    ID = (int)Elem.ID,
                    Name = Elem.Name,
                    RestaurantTable_Image = Elem.RestaurantTable_Image
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
        public CRestaurant_Table_Status? Get(int ID)
        {
            return Get(Elem: _dl_Restaurant_Table_Status.Get(ID));
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CRestaurant_Table_Status>? GetList()
        {
            List<CRestaurant_Table_Status> list_result = [];
            foreach (Restaurant_Table_Status elem in _dl_Restaurant_Table_Status.GetQuery().ToList())
            {
                CRestaurant_Table_Status? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Получение изображения статуса заказа по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public byte[]? Get_Image(int ID)
        {
            return _dl_Restaurant_Table_Status.Get(ID: ID)?.RestaurantTable_Image;
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="RestaurantTable_Image"></param>
        /// <returns></returns>
        public int Edit(int ID, string Name, byte[]? RestaurantTable_Image)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Restaurant_Table_Statuses_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Restaurant_Table_Status.Edit(ID: ID, Name: Name, RestaurantTable_Image: RestaurantTable_Image);
        }

    }
}