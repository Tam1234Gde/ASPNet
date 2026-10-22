using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Записи статистики отправлений по токенам
    /// </summary>
    [Table("Client_Notification_Stats")]
    [Comment("Записи статистики отправлений по токенам")]
    public class Client_Notification_Stats
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Код клиента
        /// </summary>
        [Comment("Код клиента")]
        public int TokenID { get; set; }


        /// <summary>
        /// Количество безошибочных отправлений
        /// </summary>
        [Comment("Ошибок нет")]
        public int None_Counts { get; set; }


        /// <summary>
        /// Количество ошибок PermissionDenied
        /// </summary>
        [Comment("Ошибка Firebase PermissionDenied")]
        public int PermissionDenied_Counts { get; set; }


        /// <summary>
        /// Количество ошибок InvalidArgument
        /// </summary>
        [Comment("Ошибка Firebase InvalidArgument")]
        public int InvalidArgument_Counts { get; set; }


        /// <summary>
        /// Количество ошибок Unauthenticated
        /// </summary>
        [Comment("Ошибка Firebase Unauthenticated")]
        public int Unauthenticated_Counts { get; set; }


        /// <summary>
        /// Количество ошибок InvalidError
        /// </summary>
        [Comment("Ошибка Firebase InvalidError")]
        public int InvalidError_Counts { get; set; }


        /// <summary>
        /// Количество ошибок NotFound
        /// </summary>
        [Comment("Ошибка Firebase NotFound")]
        public int NotFound_Counts { get; set; }


        /// <summary>
        /// Количество ошибок AlreadyExists
        /// </summary>
        [Comment("Ошибка Firebase AlreadyExists")]
        public int AlreadyExists_Counts { get; set; }
    }
}
