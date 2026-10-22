using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Dish_Colors
{
    public class Model_Dish_Colors_List
    {
        /// <summary>
        /// Список цветов
        /// </summary>
        public List<CDish_Color>? List_Colors { get; set; }

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
