using Microsoft.EntityFrameworkCore;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Установки
    /// </summary>
    public class CSetup
    {
        /// <summary>
        /// Код записи
        /// </summary>
        [Comment("Код записи")]
        public string Key { get; set; } = null!;

        /// <summary>
        /// Значение записи
        /// </summary>
        [Comment("Значение записи")]
        public string? Val { get; set; }

        /// <summary>
        /// Комментарий к записи 1
        /// </summary>
        [Comment("Комментарий к записи 1")]
        public string? Val_Comment { get; set; }

        /// <summary>
        /// Значение записи
        /// </summary>
        [Comment("Значение записи")]
        public string? Val2 { get; set; }

        /// <summary>
        /// Комментарий к записи 2
        /// </summary>
        [Comment("Комментарий к записи 2")]
        public string? Val2_Comment { get; set; }

        /// <summary>
        /// Значение записи
        /// </summary>
        [Comment("Значение записи")]
        public string? Val3 { get; set; }

        /// <summary>
        /// Комментарий к записи 3
        /// </summary>
        [Comment("Комментарий к записи 3")]
        public string? Val3_Comment { get; set; }

        /// <summary>
        /// Значение записи
        /// </summary>
        [Comment("Значение записи")]
        public string? Val4 { get; set; }

        /// <summary>
        /// Комментарий к записи 4
        /// </summary>
        [Comment("Комментарий к записи 4")]
        public string? Val4_Comment { get; set; }

    }
}