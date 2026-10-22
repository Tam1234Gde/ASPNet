namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Тип платежа (платеж чтоб привязать карту, рекурентный платеж, возврат...)
    /// </summary>
    public enum EClientPay_Card_Pay_Type : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,

        /// <summary>
        /// Инициализационный платеж (при привязке карты)
        /// </summary>
        AddInitialPay = 1,

        /// <summary>
        /// Возврат инициализационного платежа (при привязке карты)
        /// </summary>
        ReturnInitialPay = 3,

        /// <summary>
        /// Платеж
        /// </summary>
        AddPay = 7,

        /// <summary>
        /// Возврат платежа
        /// </summary>
        ReturnPay = 9,

        /// <summary>
        /// Рекурентный платеж
        /// </summary>
        AddRecurrentPay = 13,

        /// <summary>
        /// Возврат рекурентного платежа
        /// </summary>
        ReturnRecurrentPay = 15,

    }
}