using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Данные о платеже
    /// </summary>
    [Table("ClientPay_Card_Pays")]
    [Comment("Данные о платежах")]
    public class ClientPay_Card_Pay
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Дата создания платежа
        /// </summary>
        [Comment("Дата создания платежа")]
        public DateTime Date_Create { get; set; }


        /// <summary>
        /// Клиент - Код
        /// </summary>
        [Comment("Клиент - Код")]
        public int? ClientID { get; set; }


        /// <summary>
        /// Клиент
        /// </summary>
        [Comment("Клиент")]
        public Client? Client { get; set; }


        /// <summary>
        /// Заказ - Код
        /// </summary>
        [Comment("Заказ - Код")]
        public int? OrderID { get; set; }


        /// <summary>
        /// Заказ
        /// </summary>
        [Comment("Заказ")]
        public Order? Order { get; set; }


        /// <summary>
        /// Тип платежа (платеж чтоб привязать карту, рекурентный платеж, возврат...) - Код
        /// </summary>
        [Comment("Тип платежа (платеж чтоб привязать карту, рекурентный платеж, возврат...) - Код")]
        public EClientPay_Card_Pay_Type ClientPay_Card_Pay_TypeID { get; set; }


        /// <summary>
        /// Тип платежа (платеж чтоб привязать карту, рекурентный платеж, возврат...)
        /// </summary>
        [Comment("Тип платежа (платеж чтоб привязать карту, рекурентный платеж, возврат...)")]
        public ClientPay_Card_Pay_Type ClientPay_Card_Pay_Type { get; set; } = null!;


        /// <summary>
        /// Карта, которой совершался платеж - Код
        /// </summary>
        [Comment("Карта, которой совершался платеж - Код")]
        public int ClientPay_Card_DataID { get; set; }


        /// <summary>
        /// Карта, которой совершался платеж
        /// </summary>
        [Comment("Карта, которой совершался платеж")]
        public virtual ClientPay_Card_Data? ClientPay_Card_Data { get; set; }


        /// <summary>
        /// Идентификатор платежа, генерируемый платежной системой
        /// </summary>
        [Comment("Идентификатор платежа, генерируемый платежной системой")]
        public string? PaymentIdent { get; set; }


        /// <summary>
        /// Сумма платежа
        /// </summary>
        [Precision(9, 2)]
        [Comment("Сумма платежа")]
        public decimal? Summ { get; set; }


        /// <summary>
        /// Флаг того, что операция завершена успешно, если он не стоит - то нужно повторять попытки на API
        /// </summary>
        [Comment("Флаг того, что операция завершена успешно, если он не стоит - то нужно повторять попытки на API")]
        public bool Flag_Complete { get; set; }


        /// <summary>
        /// Дата удаления
        /// </summary>
        [Comment("Дата удаления")]
        public DateTime? Date_Complete { get; set; }

    }
}