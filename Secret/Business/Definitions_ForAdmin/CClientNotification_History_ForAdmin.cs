using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// История отправки кастомных уведомлений
    /// </summary>
    public class CClientNotification_History_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [Comment("Код")]
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Заголовок уведомления
        /// </summary>
        [DisplayName("Заголовок")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Текст уведомления
        /// </summary>
        [DisplayName("Текст")]
        public string Content { get; set; } = null!;


        /// <summary>
        /// Дата создания
        /// </summary>
        [DisplayName("Дата создания")]
        public DateTime Date_Created { get; set; }


        /// <summary>
        /// Дата отправки
        /// </summary>
        [DisplayName("Дата отправки")]
        public DateTime? Date_Send { get; set; }


        /// <summary>
        /// Флаг отправка выбранным
        /// </summary>
        [DisplayName("Отправка выбранным")]
        public bool Flag_Selected { get; set; }
    }
}