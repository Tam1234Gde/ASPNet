using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Статус оплаты заказа (Ожидает оплаты, Оплачен, Попытка повторной оплаты, Оплата не удалась, Отменен)
    /// </summary>
    [Table("Order_Pay_Statuses")]
    [Comment("Статусы оплаты")]
    public class Order_Pay_Status
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public EOrder_Pay_Status ID { get; set; }


        /// <summary>
        /// Наименование
        /// </summary>
        [Comment("Наименование")]
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Комментарий
        /// </summary>
        [Comment("Комментарий")]
        [StringLength(300)]
        public string? Comment { get; set; }


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        [Comment("Флаг пометки на удаление")]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Пользователь, установивший пометку на удаление - код
        /// </summary>
        [Comment("Пользователь, установивший пометку на удаление - код")]
        public int? UserID_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        [Comment("Дата пометки на удаление")]
        public DateTime? Date_Del { get; set; }

    }
}