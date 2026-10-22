using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Dish_Ingridients
{
    /// <summary>
    /// Ингридиенты
    /// </summary>
    public class Model_Dish_Ingridients_List
    {
        /// <summary>
        /// Список ингридиентов
        /// </summary>
        public List<CDish_Ingridient>? List_Ingridients { get; set; }

        /// <summary>
        /// Код выбранного блюда
        /// </summary>
        public int? DishID { get; set; }

        /// <summary>
        /// Блюдо
        /// </summary>
        public string? DishName { get; set; }

        /// <summary>
        /// Код выбранной категории
        /// </summary>
        public int? Dish_CategoryID { get; set; }

    }
}
