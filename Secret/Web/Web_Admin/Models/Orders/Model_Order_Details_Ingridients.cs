using HIDE.RestAndRant.Business.Definitions_ForAdmin;

namespace HIDE.RestAndRant.Web_Admin.Models.Orders
{
    /// <summary>
    /// Ингридиенты заказа
    /// </summary>
    public class Model_Order_Details_Ingridients
    {
        /// <summary>
        /// Заказ
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// Детали заказа
        /// </summary>
        public int OrderDataID { get; set; }

        /// <summary>
        /// Список ингридиентов заказа
        /// </summary>
        public List<COrder_Data_Ingridient_ForAdmin>? List_OrderDataIngridients { get; set; }

    }
}