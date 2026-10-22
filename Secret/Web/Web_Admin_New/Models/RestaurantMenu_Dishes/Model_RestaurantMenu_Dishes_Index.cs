using Microsoft.AspNetCore.Mvc.Rendering;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.RestaurantMenu_Dishes
{
    /// <summary>
    /// Блюда ресторана
    /// </summary>
    public class Model_RestaurantMenu_Dishes_Index
    {
        /// <summary>
        /// Список меню
        /// </summary>
        public List<CRestaurant_Menu_Dish>? List_CRestaurant_Menu_Dishes { get; set; }


        /// <summary>
        /// Код меню ресторана
        /// </summary>
        public int RestaurantMenuID { get; set; }


        /// <summary>
        /// Категория
        /// </summary>
        public int? Dish_CategoryID { get; set; }


        /// <summary>
        /// Список категорий блюд
        /// </summary>
        public List<CCommon>? List_Categories { get; set; }


        /// <summary>
        /// Список категорий блюд - SelectListItem
        /// </summary>
        public List<SelectListItem>? List_Categories_ForSelect { get; set; }

    }
}