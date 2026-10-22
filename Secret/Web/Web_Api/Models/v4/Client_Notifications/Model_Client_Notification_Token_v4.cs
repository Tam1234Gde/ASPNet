using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Api.Models.v4.Client_Notifications
{
    /// <summary>
    /// Регистрация токена
    /// </summary>
    public class Model_Client_Notification_Token_v4
    {
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
        /// Идентификатор устройства
        /// </summary>
        public string DeviceID { get; set; } = null!;

    }
}