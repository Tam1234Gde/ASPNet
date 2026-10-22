using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Web_Site_Menu.Models.Cart;

namespace HIDE.RestAndRant.Web_Site_Menu.Models.Menu
{
    /// <summary>
    /// Меню
    /// </summary>
    public class Model_Menu
    {
        /// <summary>
        /// Список категорий
        /// </summary>
        public List<CDish_Category>? List_Dish_Categories { get; set; }


        /// <summary>
        /// Список блюд
        /// </summary>
        public List<CSiteMenu>? List_Dishes { get; set; }


        /// <summary>
        /// Список блюд в корзине
        /// </summary>
        public List<Model_CartItem>? List_Dishes_Cart { get; set; }


        /// <summary>
        /// Поиск
        /// </summary>
        public string? Search { get; set; }

    }
}