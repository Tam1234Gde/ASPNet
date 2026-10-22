using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Доставка
    /// </summary>
    public class COrder_Delivery
    {
        /// <summary>
        /// Код
        /// </summary>
        [Comment("Код")]
        [JsonIgnore]
        public int ID { get; set; }


        /// <summary>
        /// Заказ - код
        /// </summary>
        [Comment("Заказ - код")]
        [JsonIgnore]
        public int OrderID { get; set; }


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
        /// Квартира
        /// </summary>
        [Comment("Квартира")]
        public string? Delivery_Apartment { get; set; }


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
        /// Комментарий к доставке
        /// </summary>
        [Comment("Комментарий к доставке")]
        public string? Delivery_Comment { get; set; }


        /// <summary>
        /// Требуется сдача
        /// </summary>
        [Comment("Требуется сдача")]
        public decimal? Delivery_Handing { get; set; }


        /// <summary>
        /// Предзаказ доставки(Доставить к ...)
        /// </summary>
        [Comment("Предзаказ доставки(Доставить к ...)")]
        public DateTime? Delivery_To { get; set; }

    }
}