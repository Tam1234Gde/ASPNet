using HIDE.RestAndRant.Business.Definitions_ForAdmin;

namespace HIDE.RestAndRant.Web_Site_Menu.Models.Cart
{
    /// <summary>
    /// Корзина - Подробнее
    /// </summary>
    public class Model_Cart_More
    {
        /// <summary>
        /// Блюдо - Код
        /// </summary>
        public CDish_ForAdmin? Dish { get; set; }


        /// <summary>
        /// Блюдо в корзине - Код
        /// </summary>
        public Model_CartItem? Dish_Cart { get; set; }


        /// <summary>
        /// Список выбранных ингредиентов
        /// </summary>
        public List<int>? List_Ingredients { get; set; }

    }
}