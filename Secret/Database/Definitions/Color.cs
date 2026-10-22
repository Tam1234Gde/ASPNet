using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Цвет стакана
    /// </summary>
    [Table("Colors")]
    [Comment("Цвет стакана")]
    public class Color
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Наименование цвета
        /// </summary>
        [Comment("Наименование цвета")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Наименование цвета - англ
        /// </summary>
        [Comment("Наименование цвета - англ")]
        public string Name_Eng { get; set; } = null!;


        /// <summary>
        /// Цвет стакана
        /// </summary>
        [Comment("Цвет стакана")]
        public string HexColor { get; set; } = null!;


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