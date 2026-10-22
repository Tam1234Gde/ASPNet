using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Dish_Volumes
{
    /// <summary>
    /// Объёмы
    /// </summary>
    public class Model_Dish_Volumes_List
    {
        /// <summary>
        /// Список объёмов
        /// </summary>
        public List<CDish_Volume>? List_Volumes { get; set; }

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
