using System.Runtime.Serialization;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Enums
{
    /// <summary>
    /// Статус уведомлений ЮКассы
    /// </summary>
    public enum EStatusNotifications
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// Ожидает подтверждения
        /// </summary>
        [EnumMember(Value = "waiting_for_capture")]
        Waiting_For_Capture = 1,


        /// <summary>
        /// Успешно
        /// </summary>
        [EnumMember(Value = "succeeded")]
        Succeeded = 3,


        /// <summary>
        /// Отменено
        /// </summary>
        [EnumMember(Value = "canceled")]
        Canceled = 5,


        /// <summary>
        /// Ожидает действий от пользователя
        /// </summary>
        [EnumMember(Value = "pending")]
        Pending = 7,


        /// <summary>
        /// Способ оплаты сохранен
        /// </summary>
        [EnumMember(Value = "active")]
        Active = 9,


        /// <summary>
        /// Способ оплаты не сохранен
        /// </summary>
        [EnumMember(Value = "inactive")]
        Inactive = 11,
    }
}
