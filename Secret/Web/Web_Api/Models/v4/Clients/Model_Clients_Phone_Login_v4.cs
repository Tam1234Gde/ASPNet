namespace HIDE.RestAndRant.Web_Api.Models.v4.Clients
{
    /// <summary>
    /// Модель
    /// </summary>
    public class Model_Clients_Phone_Login_v4
    {
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; } = null!;


        /// <summary>
        /// Код подтверждения
        /// </summary>
        public string Code { get; set; } = null!;

    }
}