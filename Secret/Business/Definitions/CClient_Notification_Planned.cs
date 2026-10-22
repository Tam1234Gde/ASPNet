using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Запланированные уведомления
    /// </summary>
    public class CClient_Notification_Planned
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Заголовок уведомления
        /// </summary>
        public string Name { get; set; } = null!;


        /// <summary>
        /// Текст уведомления
        /// </summary>
        public string Content { get; set; } = null!;


        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Date_Created { get; set; }


        /// <summary>
        /// Дата отправки
        /// </summary>
        public DateTime Date_Send { get; set; }


        /// <summary>
        /// Флаг отправка выбранным из Client_Notification_Planned_Data
        /// </summary>
        public bool Flag_Selected { get; set; }


        /// <summary>
        /// Код шаблона
        /// </summary>
        public EClient_Notification_Shablon? Client_Notification_ShablonID { get; set; }


        /// <summary>
        /// Флаг помеки на удаление
        /// </summary>
        public bool Flag_Sended { get; set; }


        /// <summary>
        /// Флаг помеки на удаление
        /// </summary>
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        public DateTime? Date_Del { get; set; }
    }
}
