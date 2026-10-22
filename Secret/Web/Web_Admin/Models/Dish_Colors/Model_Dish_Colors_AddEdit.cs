using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Dish_Colors
{
    /// <summary>
    /// Добавление\Изменение цвета
    /// </summary>
    public class Model_Dish_Colors_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        public int DishID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        public string? Dish { get; set; }

        /// <summary>
        /// Цвет
        /// </summary>
        public int ColorID { get; set; }

        /// <summary>
        /// Цвет
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Список цветов
        /// </summary>
        public List<CColor>? List_Colors { get; set; }

    }
}
