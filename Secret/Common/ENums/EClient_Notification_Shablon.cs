namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Перечисление для шаблонов уведомлений
    /// </summary>
    public enum EClient_Notification_Shablon : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// Успешная авторизация
        /// </summary>
        Auth = 1,


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


        /// <summary>
        /// Столик забронирован за вами на ХХ:ХХ.
        /// </summary>
        BookingTable = 15,


        /// <summary>
        /// Через 1 час начнется ваша бронь столика №n.
        /// </summary>
        BookingHour = 17,


        /// <summary>
        /// Ваш заказ №n начали готовить. Бронь столика начнется через 15 мин.
        /// </summary>
        BookingMinutes = 19,


        /// <summary>
        /// Ваш заказ №n ожидает вас. Столик забронирован за вами до XX:XX.
        /// </summary>
        BookingNotComplete = 21,


        /// <summary>
        /// Завтра за вами забронирован столик №n на XX:XX.
        /// </summary>
        BookingTomorrow = 23,


        /// <summary>
        /// Сегодня за вами забронирован столик №n на XX:XX.
        /// </summary>
        BookingToday = 25,
    }
}