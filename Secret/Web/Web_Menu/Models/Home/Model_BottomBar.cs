namespace HIDE.RestAndRant.Web_Site_Menu.Models.Home
{
    /// <summary>
    /// Нижняя панель
    /// </summary>
    public class Model_BottomBar
    {
        /// <summary>
        /// Количество блюд в корзине
        /// </summary>
        public decimal Dish_Count { get; set; }


        /// <summary>
        /// Выбранный раздел
        /// </summary>
        public string? Menu { get; set; }

    }
}