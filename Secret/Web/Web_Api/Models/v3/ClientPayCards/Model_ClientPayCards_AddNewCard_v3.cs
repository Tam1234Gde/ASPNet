namespace HIDE.RestAndRant.Web_Api.Models.v3.ClientPayCards
{
    /// <summary>
    /// Модель данных для добавление новой карты и инициализационного платежа по ней
    /// </summary>
    public class Model_ClientPayCards_AddNewCard_v3
    {
        /// <summary>
        /// Тип карты: mastercard, visa,....
        /// </summary>
        public string ClientPay_Card_Type { get; set; } = null!;


        /// <summary>
        /// Идентификатор карты, полученный от платежного сервиса
        /// </summary>
        public string CardIdent { get; set; } = null!;


        /// <summary>
        /// Последние 4 номера карты
        /// </summary>
        public string CardLastNum { get; set; } = null!;


        /// <summary>
        /// Идентификатор платежа, генерируемый платежной системой
        /// </summary>
        public string PaymentIdent { get; set; } = null!;


        /// <summary>
        /// Сумма платежа
        /// </summary>
        public decimal Summ { get; set; }

    }
}