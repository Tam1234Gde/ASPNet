using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Доставки
    /// </summary>
    public class Order_Deliveries
    {
        private readonly ILogger _log;
        private readonly Order_Delivery_oper _dl_Order_Delivery = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Order_Deliveries()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static COrder_Delivery? Get(Order_Delivery? Elem)
        {
            if (Elem != null)
            {
                COrder_Delivery item = new()
                {
                    ID = Elem.ID,
                    OrderID = Elem.OrderID,
                    Delivery_Address = Elem.Delivery_Address,
                    Delivery_Comment = Elem.Delivery_Comment,
                    Delivery_EntranceCode = Elem.Delivery_EntranceCode,
                    Delivery_EntranceNumber = Elem.Delivery_EntranceNumber,
                    Delivery_Floor = Elem.Delivery_Floor,
                    Delivery_Handing = Elem.Delivery_Handing,
                    Delivery_To = Elem.Delivery_To,
                    Delivery_Apartment = Elem.Delivery_Apartment
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
        public COrder_Delivery? Get(int ID)
        {
            return Get(Elem: _dl_Order_Delivery.Get(ID));
        }


        /// <summary>
        /// Получение данных по OrderID
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public COrder_Delivery? GetByOrderID(int OrderID)
        {
            return Get(Elem: _dl_Order_Delivery.GetByOrderID(OrderID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<COrder_Delivery> GetList(bool? Flag_Del_val = false)
        {
            List<COrder_Delivery> list_result = [];
            foreach (Order_Delivery elem in _dl_Order_Delivery.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                COrder_Delivery? item = Get(Elem: elem);
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
        /// <param name="OrderID"></param>
        /// <param name="Delivery_EntranceNumber"></param>
        /// <param name="Delivery_EntranceCode"></param>
        /// <param name="Delivery_Floor"></param>
        /// <param name="Delivery_Handing"></param>
        /// <param name="Delivery_Address"></param>
        /// <param name="Delivery_Comment"></param>
        /// <param name="Delivery_To"></param>
        /// <param name="Delivery_Apartment"></param>
        /// <returns></returns>
        public int Add(int OrderID, string? Delivery_Apartment, string? Delivery_EntranceNumber, string? Delivery_EntranceCode, string? Delivery_Floor,
                       decimal? Delivery_Handing, string? Delivery_Address, string? Delivery_Comment, DateTime? Delivery_To)
        {
            return _dl_Order_Delivery.Add(OrderID: OrderID, Delivery_Apartment: Delivery_Apartment, Delivery_EntranceNumber: Delivery_EntranceNumber,
                                          Delivery_EntranceCode: Delivery_EntranceCode, Delivery_Floor: Delivery_Floor, Delivery_Handing: Delivery_Handing,
                                          Delivery_Address: Delivery_Address, Delivery_Comment: Delivery_Comment, Delivery_To: Delivery_To);
        }

    }
}