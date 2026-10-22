using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Site_Menu.Models.Rating
{
    /// <summary>
    /// Оценка
    /// </summary>
    public class Model_Rating
    {
        /// <summary>
        /// Оценка
        /// </summary>
        [Comment("Оценка")]
        [Range(1, 5)]
        [Required]
        public int Grade { get; set; }


        /// <summary>
        /// Телефон
        /// </summary>
        [Comment("Телефон")]
        public string? Phone { get; set; }


        /// <summary>
        /// Текст
        /// </summary>
        [Comment("Текст")]
        public string? Text { get; set; }


        /// <summary>
        /// Код заказа
        /// </summary>
        [Comment("Код заказа")]
        public int? OrderID { get; set; }

    }
}