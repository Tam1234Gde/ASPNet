using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.RestaurantMenus
{
    /// <summary>
    /// Меню ресторана
    /// </summary>
    public class Model_RestaurantMenus_Index
    {
        /// <summary>
        /// Список меню
        /// </summary>
        public List<CRestaurant_Menu>? List_Restaurant_Menus { get; set; }


        /// <summary>
        /// Код ресторана
        /// </summary>
        public int RestaurantID { get; set; }

    }
}