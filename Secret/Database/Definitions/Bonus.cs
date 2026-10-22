using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Бонусы клиента
    /// TODO: HIDE
    /// KA - Добавить поле с типом движения бонусов: добавление, списание, списание по времени, добавление подарочных бонусов, и т.д.
    /// KA - Добавить виртуальное поле Order
    /// </summary>
    [Table("Bonuses")]
    [Comment("Бонусы клиента")]
    public class Bonus
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Дата добавления записи
        /// </summary>
        [Comment("Дата добавления записи")]
        public DateTime Date_Action { get; set; }


        /// <summary>
        /// Клиент - Код
        /// </summary>
        [Comment("Клиент - Код")]
        public int ClientID { get; set; }


        /// <summary>
        /// Клиент
        /// </summary>
        [Comment("Клиент")]
        public Client Client { get; set; } = null!;


        /// <summary>
        /// Заказ - Код
        /// </summary>
        [Comment("Заказ - Код")]
        public int OrderID { get; set; }


        /// <summary>
        /// Количество бонусов
        /// </summary>
        [Precision(8, 2)]
        [Comment("Количество бонусов")]
        public decimal Bonuses { get; set; }

    }
}