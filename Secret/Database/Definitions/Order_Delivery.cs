using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Доставка
    /// </summary>
    [Table("Order_Deliveries")]
    [Comment("Доставка")]
    public class Order_Delivery
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Заказ - Код
        /// </summary>
        [Comment("Заказ - Код")]
        public int OrderID { get; set; }


        /// <summary>
        /// Заказ
        /// </summary>
        [Comment("Заказ")]
        public Order? Order { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable


        /// <summary>
        /// Адрес доставки
        /// </summary>
        [Comment("Адрес доставки")]
        public string? Delivery_Address { get; set; }


        /// <summary>
        /// Квартира
        /// </summary>
        [Comment("Квартира")]
        public string? Delivery_Apartment { get; set; }


        /// <summary>
        /// Код подъезда
        /// </summary>
        [Comment("Код подъезда")]
        public string? Delivery_EntranceCode { get; set; }


        /// <summary>
        /// Домофон
        /// </summary>
        [Comment("Домофон")]
        public string? Delivery_EntranceNumber { get; set; }


        /// <summary>
        /// Номер этажа
        /// </summary>
        [Comment("Номер этажа")]
        public string? Delivery_Floor { get; set; }


        /// <summary>
        /// Комментарий к доставке
        /// </summary>
        [Comment("Комментарий к доставке")]
        public string? Delivery_Comment { get; set; }


        /// <summary>
        /// Дать сдачи с ...
        /// </summary>
        [Comment("Дать сдачи с ...")]
        [Precision(8, 2)]
        public decimal? Delivery_Handing { get; set; }


        /// <summary>
        /// Предзаказ доставки(Доставить к ...)
        /// </summary>
        [Comment("Предзаказ доставки(Доставить к ...)")]
        public DateTime? Delivery_To { get; set; }


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        [Comment("Флаг пометки на удаление")]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        [Comment("Дата пометки на удаление")]
        public DateTime? Date_Del { get; set; }


        /// <summary>
        /// Пользователь, который поставил запись на удаление
        /// </summary>
        [Comment("Пользователь, который поставил запись на удаление")]
        public int? UserID_Del { get; set; }

    }
}