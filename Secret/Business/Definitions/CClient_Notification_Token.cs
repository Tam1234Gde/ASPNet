using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Регистрация токенов уведомлений клиентов
    /// </summary>
    public class CClient_Notification_Token
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        public int ID { get; set; }


        /// <summary>
        /// Код клиента
        /// </summary>
        public int ClientID { get; set; }


        /// <summary>
        /// Идентификатор системы облачных уведомлений
        /// </summary>
        public string TokenType { get; set; } = null!;


        /// <summary>
        /// Идентификатор системы облачных уведомлений
        /// </summary>
        public int TokenTypeID { get; set; }


        /// <summary>
        /// Токен
        /// </summary>
        [StringLength(200)]
        public string Token { get; set; } = null!;


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        public DateTime? Date_Del { get; set; }


        /// <summary>
        /// Идентификатор устройства
        /// </summary>
        public string DeviceID { get; set; } = null!;

    }
}