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
    /// Запланированные уведомления данные отправки выборочно.
    /// </summary>
    [Table("Client_Notification_Planned_Data")]
    [Comment("Запланированные уведомления данные отправки выборочно.")]
    public class Client_Notification_Planned_Data
    {

        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Заказ - Код
        /// </summary>
        [Comment("Заказ - Код")]
        [ForeignKey(nameof(Client_Notification_Planned))]
        public int Client_Notification_Planned_ID { get; set; }


        /// <summary>
        /// Заказ
        /// </summary>
        [Comment("Заказ")]
        public Client_Notification_Planned Client_Notification_Planned { get; set; } = null!;


        /// <summary>
        /// Заказ - Код
        /// </summary>
        [Comment("Клиент - Код")]
        public int ClientID { get; set; }


        /// <summary>
        /// Заказ
        /// </summary>
        [Comment("Клиент")]
        public Client Client { get; set; } = null!; 


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
