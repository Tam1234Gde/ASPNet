namespace HIDE.RestAndRant.Web_Api.Models.v3.Orders
{
    /// <summary>
    /// Модель данных для добавление новой карты и инициализационного платежа по ней
    /// </summary>
    public class Model_Orders_AddNewCard_v3
    {
        /// <summary>
        /// Заказ - код
        /// </summary>
        public int OrderID { get; set; }


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