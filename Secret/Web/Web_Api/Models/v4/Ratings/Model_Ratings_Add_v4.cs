using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Api.Models.v4.Ratings
{
    /// <summary>
    /// Оценка приложения
    /// </summary>
    public class Model_Ratings_Add_v4
    {
        /// <summary>
        /// Оценка
        /// </summary>
        [Comment("Оценка")]
        [Range(1, 5)]
        [Required]
        public int Grade { get; set; }


        /// <summary>
        /// Текст
        /// </summary>
        [Comment("Текст")]
        public string? Text { get; set; }

    }
}