namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Статус заказа (Ожидает оплаты, Оплачен, Попытка повторной оплаты, оплата не удалась, отменен)
    /// </summary>
    public enum EOrder_Pay_Status : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// Ожидает оплаты
        /// </summary>
        WaitPay = 1,


        /// <summary>
        /// Оплачен
        /// </summary>
        Payed = 3,


        /// <summary>
        /// Попытка повторной оплаты
        /// </summary>
        NeedRetry = 5,


        /// <summary>
        /// Ошибка при оплате
        /// </summary>
        Error = 7,


        /// <summary>
        /// Заказ отменен
        /// </summary>
        Canceled = 9,

    }
}