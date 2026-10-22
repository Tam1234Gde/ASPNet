using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Регистрация токенов уведомлений клиентов
    /// </summary>
    [Table("Client_Notification_Tokens")]
    [Comment("Токены уведомлений клиентов")]
    [Index(nameof(DeviceID), IsUnique = true)]
    public class Client_Notification_Token
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
        public int ClientID { get; set; }


        /// <summary>
        /// Идентификатор системы облачных уведомлений
        /// </summary>
        [Comment("Идентификатор системы облачных уведомлений")]
        public int TokenTypeID { get; set; }


        /// <summary>
        /// Токен
        /// </summary>
        [StringLength(200)]
        [Comment("Токен")]
        public string Token { get; set; } = null!;


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
        /// Идентификатор устройства
        /// </summary>
        [Comment("Идентификатор устройства")]
        public string DeviceID { get; set; } = null!;

    }
}