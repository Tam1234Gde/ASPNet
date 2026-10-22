namespace HIDE.RestAndRant.Web_Site_Menu.Models.Cart
{
    /// <summary>
    /// Корзина - Главная страница
    /// </summary>
    public class Model_Cart_Index
    {
        /// <summary>
        /// Столик
        /// </summary>
        public int? RestaurantTableID { get; set; }


        /// <summary>
        /// Общая сумма
        /// </summary>
        public decimal? Sum_Itog { get; set; }


        /// <summary>
        /// Добавить к заказу
        /// </summary>
        public string? AddToOrder { get; set; }


        /// <summary>
        /// Убрать с заказа
        /// </summary>
        public string? RemoveToOrder { get; set; }


        /// <summary>
        /// Список блюд
        /// </summary>
        public List<Model_CartItem>? List_Dishes { get; set; }

    }
}