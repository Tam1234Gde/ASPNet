using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.RestaurantMenu_Dish_Ingredients
{
    /// <summary>
    /// CRUD - Ингредиенты
    /// </summary>
    public class Model_RestaurantMenu_Dish_Ingredients_CRUD
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Dish_CategoryID { get; set; }

        /// <summary>
        /// Блюдо
        /// </summary>
        public int Restaurant_Menu_DishID { get; set; }

        /// <summary>
        /// Блюдо
        /// </summary>
        public string? Restaurant_Menu_Dish { get; set; }

        /// <summary>
        /// Ингридиент
        /// </summary>
        public int IngridientID { get; set; }

        /// <summary>
        /// Ингридиент
        /// </summary>
        public string? Ingridient { get; set; }

        /// <summary>
        /// Количество минут для приготовления
        /// </summary>
        public int? Cooking_Minutes { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public int? IngridientGroupID { get; set; }

        /// <summary>
        /// Флаг - Группа
        /// </summary>
        public bool Flag_IsGroup { get; set; }

        /// <summary>
        /// Список ингридиентов
        /// </summary>
        public List<CIngridient>? List_Ingridients { get; set; }

    }
}