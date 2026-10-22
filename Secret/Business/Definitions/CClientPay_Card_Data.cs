namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Данные карты клиента
    /// </summary>
    public class CClientPay_Card_Data
    {
        /// <summary>
        /// Код карты
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Тип карты - код
        /// </summary>
        public string? ClientPay_Card_TypeID { get; set; }


        /// <summary>
        /// Тип карты
        /// </summary>
        public string? ClientPay_Card_Type { get; set; }


        /// <summary>
        /// Идентификатор карты, полученный от платежного сервиса
        /// </summary>
        public string CardIdent { get; set; } = null!;


        /// <summary>
        /// Последние 4 номера карты
        /// </summary>
        public string CardLastNum { get; set; } = null!;

    }
}