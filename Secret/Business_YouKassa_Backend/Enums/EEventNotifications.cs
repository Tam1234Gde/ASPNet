using System.Runtime.Serialization;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Enums
{
    /// <summary>
    /// События уведомлений ЮКассы
    /// </summary>
    public enum EEventNotifications
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// Платежи - ожидает подтверждения
        /// </summary>
        [EnumMember(Value = "payment.waiting_for_capture")]
        Payment_Waiting_For_Capture = 1,


        /// <summary>
        /// Платежи - успешно
        /// </summary>
        [EnumMember(Value = "payment.succeeded")]
        Payment_Succeeded = 3,


        /// <summary>
        /// Платежи - отменено
        /// </summary>
        [EnumMember(Value = "payment.canceled")]
        Payment_Canceled = 5,


        /// <summary>
        /// Возвраты - успешно
        /// </summary>
        [EnumMember(Value = "refund.succeeded")]
        Refund_Succeeded = 7,


        /// <summary>
        /// Выплаты - успешно
        /// </summary>
        [EnumMember(Value = "payout.succeeded")]
        Payout_Succeeded = 9,


        /// <summary>
        /// Выплаты - отмена
        /// </summary>
        [EnumMember(Value = "payout.canceled")]
        Payout_Canceled = 11,


        /// <summary>
        /// Сделки - закрыто
        /// </summary>
        [EnumMember(Value = "deal.closed")]
        Deal_Closed = 13,


        /// <summary>
        /// Способы оплаты — используются для привязки на нулевую сумму
        /// </summary>
        [EnumMember(Value = "payment_method.active")]
        Payment_Method_Active = 15,
    }
}
