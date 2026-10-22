namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Тип уведомлений
    /// </summary>
    public enum EClient_Notification_Type : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// Через 1 час начнется ваша бронь столика №n.
        /// </summary>
        OneHour = 1,


        /// <summary>
        /// Ваш заказ №n начали готовить. Бронь столика начнется через 15 мин.
        /// </summary>
        Minuts15 = 3,


        /// <summary>
        /// Завтра за вами забронирован столик №n на XX:XX.
        /// </summary>
        Tomorrow = 5,


        /// <summary>
        /// Сегодня за вами забронирован столик №n на XX:XX.
        /// </summary>
        Today = 7,


        /// <summary>
        /// Ваш заказ №n ожидает вас. Столик забронирован за вами до XX:XX.
        /// </summary>
        Waiting = 9,
    }
}