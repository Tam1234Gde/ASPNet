using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Бонусы клиента
    /// </summary>
    public class CBonus
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Код клиента
        /// </summary>
        [DisplayName("Код клиента")]
        public int ClientID { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        [DisplayName("Клиент")]
        public string? Client { get; set; }

        /// <summary>
        /// Код заказа
        /// </summary>
        [DisplayName("Код заказа")]
        public int OrderID { get; set; }

        /// <summary>
        /// Количество бонусов
        /// </summary>
        [DisplayName("Количество бонусов")]
        public decimal Bonuses { get; set; }

        /// <summary>
        /// Дата добавления записи
        /// </summary>
        [DisplayName("Дата добавления записи")]
        public DateTime Date_Action { get; set; }

    }
}