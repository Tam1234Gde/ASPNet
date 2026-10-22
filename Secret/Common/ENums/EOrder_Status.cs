namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Статус заказа
    /// </summary>
    public enum EOrder_Status : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// Новый заказ
        /// </summary>
        New = 3,

        /// <summary>
        /// Заказ отменен
        /// </summary>
        Cancelled = 5,


        /// <summary>
        /// Заказ готовится
        /// </summary>
        Cooking = 7,


        /// <summary>
        /// Заказ готов к выдаче
        /// </summary>
        ReadyToClient = 9,


        /// <summary>
        /// Заказ передан в доставку
        /// </summary>
        Delivery = 11,


        /// <summary>
        /// Заказ завершен
        /// </summary>
        Complete = 13,

    }
}