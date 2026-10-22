using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Город
    /// </summary>
    [Table("Cities")]
    [Comment("Город>")]
    public class City
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Название города
        /// </summary>
        [Comment("Название города")]
        [StringLength(40)]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Название города(склонение)
        /// </summary>
        [Comment("Название города(склонение)")]
        [StringLength(40)]
        public string? NameDeclension { get; set; }


        /// <summary>
        /// Порядок отображения
        /// </summary>
        [Comment("Порядок отображения")]
        public int OrderNum { get; set; }


        /// <summary>
        /// True - Отключен
        /// </summary>
        [Comment("Отключен")]
        public bool Flag_Hidden { get; set; }


        /// <summary>
        /// Заголовок сообщения при отключенном ресторане
        /// </summary>
        [Comment("Заголовок сообщения при отключенном ресторане")]
        public string? HiddenTitle { get; set; }


        /// <summary>
        /// Тело сообщения при отключенном ресторане
        /// </summary>
        [Comment("Тело сообщения при отключенном ресторане")]
        public string? HiddenText { get; set; }


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