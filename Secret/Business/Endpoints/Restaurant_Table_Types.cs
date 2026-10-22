using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Типы столиков
    /// </summary>
    public class Restaurant_Table_Types
    {
        private readonly ILogger _log;
        private readonly Restaurant_Table_Type_oper _dl_Restaurant_Table_Type = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_Table_Types()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CCommon? Get(Restaurant_Table_Type? Elem)
        {
            if (Elem != null)
            {
                CCommon item = new()
                {
                    ID = (int)Elem.ID,
                    Name = Elem.Name,
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
        public CCommon? Get(int ID)
        {
            return Get(Elem: _dl_Restaurant_Table_Type.Get(ID));
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CCommon>? GetList()
        {
            List<CCommon> list_result = [];
            foreach (Restaurant_Table_Type elem in _dl_Restaurant_Table_Type.GetQuery().ToList())
            {
                CCommon? item = Get(Elem: elem);
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
        /// <param name="RestaurantID">Код ресторана</param>
        /// <returns></returns>
        public List<CCommon>? GetListByRestaurantID(int RestaurantID)
        {
            List<CCommon> list_result = [];
            foreach (Restaurant_Table_Type elem in _dl_Restaurant_Table_Type.GetQueryByRestaurantID(RestaurantID: RestaurantID).ToList())
            {
                CCommon? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

    }
}