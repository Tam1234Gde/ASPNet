using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Api.Models.v3.Orders
{
    /// <summary>
    /// Популярные заказы
    /// </summary>
    public class Model_Orders_AsUsual_v3
    {
        /// <summary>
        /// Список заказов
        /// </summary>
        public List<COrder>? List_Orders { get; set; }


        /// <summary>
        /// Список избранных заказов
        /// </summary>
        public List<COrder_Favorite>? List_Favorites { get; set; }

    }
}