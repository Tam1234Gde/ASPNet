namespace HIDE.RestAndRant.Web_Site_Menu.Models.Cart
{
    /// <summary>
    /// Корзина - Элемент
    /// </summary>
    public class Model_CartItem_Add
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Код блюда
        /// </summary>
        public int DishID { get; set; }


        /// <summary>
        /// Количество блюд
        /// </summary>
        public decimal Dish_Count { get; set; }


        /// <summary>
        /// Код объёма
        /// </summary>
        public int? Dish_VolumeID { get; set; }


        /// <summary>
        /// Ингредиенты блюда
        /// </summary>
        public List<int>? Dish_Ingredient_IDs { get; set; }

    }
}