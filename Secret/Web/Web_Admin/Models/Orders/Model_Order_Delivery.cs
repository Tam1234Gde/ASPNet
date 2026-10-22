using Microsoft.EntityFrameworkCore;

namespace HIDE.RestAndRant.Web_Admin.Models.Orders
{
    /// <summary>
    /// Ингридиенты заказа
    /// </summary>
    public class Model_Order_Delivery
    {

        /// <summary>
        /// Заказ - код
        /// </summary>
        [Comment("Заказ - код")]
        public int OrderID { get; set; }

        /// <summary>
        /// Стоимость заказа
        /// </summary>
        [Comment("Стоимость заказа")]
        public decimal? Delivery_Price { get; set; }


        /// <summary>
        /// Адрес доставки
        /// </summary>
        [Comment("Адрес доставки")]
        public string? Delivery_Address { get; set; }


        /// <summary>
        /// Код подъезда
        /// </summary>
        [Comment("Код подъезда")]
        public string? Delivery_EntranceCode { get; set; }


        /// <summary>
        /// Номер подъезда
        /// </summary>
        [Comment("Номер подъезда")]
        public string? Delivery_EntranceNumber { get; set; }


        /// <summary>
        /// Номер этажа
        /// </summary>
        [Comment("Номер этажа")]
        public string? Delivery_Floor { get; set; }


        /// <summary>
        /// Комментарий - Добавить к заказу
        /// </summary>
        [Comment("Комментарий - Добавить к заказу")]
        public string? Delivery_Comment { get; set; }


        /// <summary>
        /// Комментарий - Убрать из заказа
        /// </summary>
        [Comment("Комментарий - Убрать из заказа")]
        public string? Delivery_Comment_Remove { get; set; }


        /// <summary>
        /// Дать сдачи с ...
        /// </summary>
        [Comment("Дать сдачи с ...")]
        public decimal? Delivery_Handing { get; set; }


        /// <summary>
        /// Предзаказ доставки(Доставить к ...)
        /// </summary>
        [Comment("Предзаказ доставки(Доставить к ...)")]
        public DateTime? Delivery_To { get; set; }

    }
}