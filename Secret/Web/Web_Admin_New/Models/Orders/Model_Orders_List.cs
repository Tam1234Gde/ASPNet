using HIDE.RestAndRant.Business.Definitions_ForAdmin;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Orders
{
    /// <summary>
    /// Заказы
    /// </summary>
    public class Model_Orders_List
    {
        /// <summary>
        /// Дата начала для журнала событий
        /// </summary>
        public DateTime Date_From { get; set; }


        /// <summary>
        /// Дата окончания для журнала событий
        /// </summary>
        public DateTime Date_To { get; set; }


        /// <summary>
        /// Список заказов
        /// </summary>
        public List<COrder_ForAdmin>? List_Orders { get; set; }

    }
}