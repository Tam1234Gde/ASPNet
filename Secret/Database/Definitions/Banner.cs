using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Банеры
    /// </summary>
    [Table("Banners")]
    [Comment("Банеры")]
    public class Banner
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Название
        /// </summary>
        [Comment("Название")]
        public string? Name { get; set; }


        /// <summary>
        /// Баннер большой
        /// </summary>
        [Comment("Баннер большой")]
        public byte[]? Image_Big { get; set; }


        /// <summary>
        /// Баннер маленький
        /// </summary>
        [Comment("Баннер маленький")]
        public byte[]? Image_Small { get; set; }


        /// <summary>
        /// Город - код
        /// </summary>
        [Comment("Город - код")]
        public int CityID { get; set; }


        /// <summary>
        /// Город
        /// </summary>
        [Comment("Город")]
        public City City { get; set; } = null!;


        /// <summary>
        /// Категория - ID
        /// </summary>
        [Comment("Категория - ID (Для ссылки)")]
        public int? Category_ID { get; set; }


        /// <summary>
        /// Продукт - ID
        /// </summary>
        [Comment("Продукт - ID (Для ссылки)")]
        public int? Product_ID { get; set; }


        /// <summary>
        /// Дата начала показа
        /// </summary>
        [Comment("Дата начала показа")]
        public DateTime? Date_Start { get; set; }


        /// <summary>
        /// Дата окончания показа
        /// </summary>
        [Comment("Дата окончания показа")]
        public DateTime? Date_End { get; set; }


        /// Дата создания
        /// </summary>
        [Comment("Дата создания")]
        public DateTime? Date_Create { get; set; }


        /// <summary>
        /// Порядок отображения
        /// </summary>
        [Comment("Порядок отображения")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        [Comment("Флаг пометки на удаление")]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата редактирования
        /// </summary>
        [Comment("Дата редактирования")]
        public DateTime Date_Edit { get; set; }


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
