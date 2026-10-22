using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Оценка приложения
    /// </summary>
    public class CRating
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
        [DisplayName("Клиент - Код")]
        public int ClientID { get; set; }


        /// <summary>
        /// Клиент
        /// </summary>
        [DisplayName("Клиент")]
        public string? Client { get; set; }


        /// <summary>
        /// Оценка
        /// </summary>
        [DisplayName("Оценка")]
        [Range(1, 5)]
        public int Grade { get; set; }


        /// <summary>
        /// Текст
        /// </summary>
        [DisplayName("Текст")]
        public string? Text { get; set; }


        /// <summary>
        /// Номер телефона
        /// </summary>
        [Comment("Номер телефона")]
        [JsonIgnore]
        public string? Phone { get; set; }


        /// <summary>
        /// Код столика
        /// </summary>
        [Comment("Код столика")]
        [JsonIgnore]
        public int? Restaurant_TableID { get; set; }


        /// <summary>
        /// Дата оценки
        /// </summary>
        [DisplayName("Дата оценки")]
        public DateTime Date_Action { get; set; }

    }
}