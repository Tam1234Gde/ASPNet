namespace HIDE.RestAndRant.Web_Api.Models.v4.ClientPayCards
{
    /// <summary>
    /// Модель данных для добавление новой карты и инициализационного платежа по ней
    /// </summary>
    public class Model_ClientPayCards_Buy_v4
    {
        /// <summary>
        /// Тип заказа - код
        /// </summary>
        public int OrderID { get; set; }


        /// <summary>
        /// Код карты из CClientPay_Card_Data.ID
        /// </summary>
        public int CardID { get; set; }


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