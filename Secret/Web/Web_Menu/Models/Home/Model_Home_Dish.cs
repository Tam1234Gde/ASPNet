using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Web_Site_Menu.Models.Cart;

namespace HIDE.RestAndRant.Web_Site_Menu.Models.Home
{
    /// <summary>
    /// Блюдо - Подробности
    /// </summary>
    public class Model_Home_Dish
    {
        /// <summary>
        /// Блюдо
        /// </summary>
        public CDish_ForAdmin? Dish { get; set; }


        /// <summary>
        /// Список рекомедуемых товаров
        /// </summary>
        public List<CDish_ForAdmin>? List_DishRecommended { get; set; }


        /// <summary>
        /// Список блюд в корзине
        /// </summary>
        public List<Model_CartItem>? List_Dishes { get; set; }


        /// <summary>
        /// Url
        /// </summary>
        public string? Url { get; set; }
    }
}