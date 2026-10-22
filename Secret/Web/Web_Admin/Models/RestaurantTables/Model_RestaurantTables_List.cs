using Microsoft.AspNetCore.Mvc.Rendering;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.RestaurantTables
{
    /// <summary>
    /// Столики в ресторане
    /// </summary>
    public class Model_RestaurantTables_List
    {
        /// <summary>
        /// Список ресторанов
        /// </summary>
        public List<CCommon>? List_Restaurants { get; set; }

        /// <summary>
        /// Список ресторанов - SelectListItem
        /// </summary>
        public List<SelectListItem>? List_Restaurants_ForSelect { get; set; }

        /// <summary>
        /// Список столов
        /// </summary>
        public List<CDish>? List_RestaurantTables { get; set; }

        /// <summary>
        /// Код выбранного ресторана
        /// </summary>
        public int? RestaurantID { get; set; }

    }
}
