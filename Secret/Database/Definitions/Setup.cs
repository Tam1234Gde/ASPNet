using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Установки
    /// </summary>
    [Table("Setups")]
    [Comment("Установки")]
    public class Setup
    {
        /// <summary>
        /// Код записи
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ключ")]
        public string Key { get; set; } = null!;


        /// <summary>
        /// Значение записи
        /// </summary>
        [Comment("Значение записи 1")]
        public string? Val { get; set; }


        /// <summary>
        /// Комментарий к записи 1
        /// </summary>
        [Comment("Комментарий к записи 1")]
        public string? Val_Comment { get; set; }


        /// <summary>
        /// Значение записи
        /// </summary>
        [Comment("Значение записи 2")]
        public string? Val2 { get; set; }


        /// <summary>
        /// Комментарий к записи 2
        /// </summary>
        [Comment("Комментарий к записи 2")]
        public string? Val2_Comment { get; set; }


        /// <summary>
        /// Значение записи 3
        /// </summary>
        [Comment("Значение записи 3")]
        public string? Val3 { get; set; }


        /// <summary>
        /// Комментарий к записи 3
        /// </summary>
        [Comment("Комментарий к записи 3")]
        public string? Val3_Comment { get; set; }


        /// <summary>
        /// Значение записи 4
        /// </summary>
        [Comment("Значение записи 4")]
        public string? Val4 { get; set; }


        /// <summary>
        /// Комментарий к записи 4
        /// </summary>
        [Comment("Комментарий к записи 4")]
        public string? Val4_Comment { get; set; }

    }
}