namespace HIDE.RestAndRant.Web_Api.Models.v4.ClientPayCards
{
    /// <summary>
    /// Модель данных для отправки смс
    /// </summary>
    public class Model_ClientPayCards_ChekSend_v4
    {
        /// <summary>
        /// Телефон, куда отправляем. Должен совпадать с телефоном клиента
        /// </summary>
        public string Phone { get; set; } = null!;


        /// <summary>
        /// Ссылка на чек
        /// </summary>
        public string Chek_Url { get; set; } = null!;

    }
}