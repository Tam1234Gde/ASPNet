using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Статус заказа (оформлен, готовится, готов, отказ, и т.д.)
    /// </summary>
    [Table("Order_Statuses")]
    [Comment("Статусы заказа")]
    public class Order_Status
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] //Отключена автоинкрементация поля
        [Comment("Код")]
        public EOrder_Status ID { get; set; }


        /// <summary>
        /// Наименование
        /// </summary>
        [Comment("Наименование")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Порядок следования статуса при отображении
        /// </summary>
        [Comment("Порядок следования")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Цвет
        /// </summary>
        [Comment("Цвет")]
        public string? ColorHex { get; set; }

    }
}