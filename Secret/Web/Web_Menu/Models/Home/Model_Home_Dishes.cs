using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Web_Site_Menu.Models.Cart;

namespace HIDE.RestAndRant.Web_Site_Menu.Models.Home
{
    /// <summary>
    /// Блюда категории
    /// </summary>
    public class Model_Home_Dishes
    {
        /// <summary>
        /// Категория
        /// </summary>
        public CDish_Category Dish_Category { get; set; } = null!;


        /// <summary>
        /// Список категорий
        /// </summary>
        public List<CDish_Category_ForAdmin>? List_Dish_Categories { get; set; }


        /// <summary>
        /// Список блюд
        /// </summary>
        public List<CDish_ForAdmin>? List_Dishes { get; set; }


        /// <summary>
        /// Список блюд в корзине
        /// </summary>
        public List<Model_CartItem>? List_Dishes_Cart { get; set; }

    }
}