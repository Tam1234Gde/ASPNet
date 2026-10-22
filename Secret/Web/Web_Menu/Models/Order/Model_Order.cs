using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Site_Menu.Models.Order
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Model_Order
    {
        /// <summary>
        /// Код заказа
        /// </summary>
        public int? OrderID { get; set; }


        /// <summary>
        /// Искомый заказ
        /// </summary>
        public COrder? Order { get; set; }


        /// <summary>
        /// Вывод ошибки\текста
        /// </summary>
        public string? Info { get; set; }


        /// <summary>
        /// Общая сумма
        /// </summary>
        public decimal? Sum { get; set; }

    }
}