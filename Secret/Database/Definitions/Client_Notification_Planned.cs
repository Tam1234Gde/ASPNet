using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Запланированные уведомления
    /// </summary>
    [Table("Client_Notification_Planned")]
    [Comment("Запланированные уведомления")]
    public class Client_Notification_Planned
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Заголовок уведомления
        /// </summary>
        [Comment("Заголовок уведомления")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Текст уведомления
        /// </summary>
        [Comment("Текст уведомления")]
        public string Content { get; set; } = null!;


        /// <summary>
        /// Дата создания
        /// </summary>
        [Comment("Дата создания")]
        public DateTime Date_Created { get; set; }


        /// <summary>
        /// Дата отправки
        /// </summary>
        [Comment("Дата отправки")]
        public DateTime Date_Send { get; set; }


        /// <summary>
        /// Флаг отправка выбранным из Client_Notification_Planned_Data
        /// </summary>
        [Comment("Флаг отправка выбранным из Client_Notification_Planned_Data")]
        public bool Flag_Selected { get; set; }


        /// <summary>
        /// Код шаблона
        /// </summary>
        [Comment("Код шаблона")]
        public EClient_Notification_Shablon? Client_Notification_ShablonID { get; set; }


        /// <summary>
        /// Флаг помеки на удаление
        /// </summary>
        [Comment("Флаг отправлено")]
        public bool Flag_Sended { get; set; }


        /// <summary>
        /// Флаг помеки на удаление
        /// </summary>
        [Comment("Флаг помеки на удаление")]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        [Comment("Дата пометки на удаление")]
        public DateTime? Date_Del { get; set; }

    }
}
