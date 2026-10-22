namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Статус столика(Не забронирован\Забронирован\Недоступен)
    /// </summary>
    public enum ERestaurant_Table_Status : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// Не забронирован
        /// </summary>
        Available = 3,


        /// <summary>
        /// Забронирован
        /// </summary>
        Unavailable = 6,


        /// <summary>
        /// Недоступен
        /// </summary>
        Disabled = 9,

    }
}