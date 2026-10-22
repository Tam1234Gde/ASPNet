using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Orders
{
    /// <summary>
    /// Редактирование статуса заказа
    /// </summary>
    public class Model_Orders_EditStatus
    {
        /// <summary>
        /// Код заказа
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Код статуса
        /// </summary>
        public EOrder_Status Order_StatusID { get; set; }

        /// <summary>
        /// Оплачен
        /// </summary>
        public bool Flag_Payed { get; set; }

        /// <summary>
        /// Список статусов
        /// </summary>
        public List<CCommon_Ordered>? List_Order_Status { get; set; }

    }
}