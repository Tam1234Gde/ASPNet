using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.RestaurantMenu_Dish_Ingredients
{
    /// <summary>
    /// Ингредиенты
    /// </summary>
    public class Model_RestaurantMenu_Dish_Ingredients_Index
    {
        /// <summary>
        /// Список ингредиентов
        /// </summary>
        public List<CRestaurant_Menu_Dish_Ingredient>? List_Restaurant_Menu_Dish_Ingredients { get; set; }


        /// <summary>
        /// Код блюда ресторана
        /// </summary>
        public int Restaurant_Menu_DishID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        public string Restaurant_Menu_Dish { get; set; } = null!;


        /// <summary>
        /// 
        /// </summary>
        public int RestaurantMenuID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int Dish_CategoryID { get; set; }

    }
}