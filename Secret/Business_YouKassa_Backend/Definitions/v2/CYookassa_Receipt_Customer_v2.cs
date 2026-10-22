using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// Информация о пользователе
    /// </summary>
    public class CYookassa_Receipt_Customer_v2
    {
        /// <summary>
        /// Электронная почта пользователя для отправки чека
        /// </summary>
        [JsonProperty("email")]
        public string? Email { get; set; }


        /// <summary>
        /// Телефон пользователя для отправки чека
        /// </summary>
        [JsonProperty("phone")]
        public string? Phone { get; set; }
    }

}
