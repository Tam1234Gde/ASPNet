using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Ratings
{
    /// <summary>
    /// CRUD - Оценка приложения
    /// </summary>
    public class Model_Ratings_AddEdit
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

    }
}