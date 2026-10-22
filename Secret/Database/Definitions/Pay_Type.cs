using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Вид оплаты (картой, нал, ...)
    /// </summary>
    [Table("Pay_Types")]
    [Comment("Виды оплаты")]
    public class Pay_Type
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Код")]
        public EPayType ID { get; set; }


        /// <summary>
        /// Наименование
        /// </summary>
        [Comment("Наименование")]
        public string Name { get; set; } = null!;

    }
}