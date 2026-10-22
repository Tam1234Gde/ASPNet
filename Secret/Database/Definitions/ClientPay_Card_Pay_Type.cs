using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Тип платежа (платеж чтоб привязать карту, рекурентный платеж, возврат...)
    /// </summary>
    [Table("ClientPay_Card_Pay_Type")]
    [Comment("Типы платежа")]
    public class ClientPay_Card_Pay_Type
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Код")]
        public EClientPay_Card_Pay_Type ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Comment("Наименование")]
        public string Name { get; set; } = null!;

    }
}