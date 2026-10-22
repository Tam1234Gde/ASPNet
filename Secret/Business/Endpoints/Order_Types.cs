using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Helpers;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Типы заказа(С собой\Поесть в кафе\Доставка\Вынести к машине)
    /// </summary>
    public class Order_Types
    {
        private readonly ILogger _log;
        private readonly Order_Type_oper _dl_Order_Type = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Order_Types()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
            _dl_Order_Type = new();
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CCommon? Get(Order_Type? Elem)
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
            return Get(Elem: _dl_Order_Type.Get(ID));
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CCommon>? GetList()
        {
            List<CCommon> list_result = [];
            foreach (Order_Type elem in _dl_Order_Type.GetQuery().ToList())
            {
                CCommon? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }

            list_result.Swap(1, 2);

            return list_result;
        }

    }
}