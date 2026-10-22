using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Оценка приложения
    /// </summary>
    [Table("Ratings")]
    [Comment("Оценка приложения")]
    public class Rating
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


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
        /// Оценка
        /// </summary>
        [Comment("Оценка")]
        [Range(1, 5)]
        public int Grade { get; set; }


        /// <summary>
        /// Текст
        /// </summary>
        [Comment("Текст")]
        public string? Text { get; set; }


        /// <summary>
        /// Номер телефона
        /// </summary>
        [Comment("Номер телефона")]
        public string? Phone { get; set; }


        /// <summary>
        /// Код столика
        /// </summary>
        [Comment("Код столика")]
        public int? Restaurant_TableID { get; set; }


        /// <summary>
        /// Дата оценки
        /// </summary>
        [Comment("Дата оценки")]
        public DateTime Date_Action { get; set; }


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