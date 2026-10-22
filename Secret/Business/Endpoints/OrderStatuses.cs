using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Статусы заказа
    /// </summary>
    public class OrderStatuses
    {
        private readonly ILogger _log;
        private readonly Order_Status_oper _dl_Order_Status = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public OrderStatuses()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CCommon_Ordered? Get(Order_Status? Elem)
        {
            if (Elem != null)
            {
                CCommon_Ordered item = new()
                {
                    ID = (int)Elem.ID,
                    Name = Elem.Name,
                    OrderNum = Elem.OrderNum,
                    ColorHex = Elem.ColorHex
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
        public CCommon_Ordered? Get(int ID)
        {
            return Get(Elem: _dl_Order_Status.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CCommon_Ordered>? GetList()
        {
            List<CCommon_Ordered> list_result = [];

            foreach (Order_Status elem in _dl_Order_Status.GetQuery().ToList())
            {
                CCommon_Ordered? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="ColorHex"></param>
        /// <param name="OrderNum"></param>
        /// <returns></returns>
        public int Edit(int ID, string Name, string? ColorHex, int OrderNum)
        {
            return _dl_Order_Status.Edit(ID: ID, Name: Name, ColorHex: ColorHex, OrderNum: OrderNum);
        }


    }
}