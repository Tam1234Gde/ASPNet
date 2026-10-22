namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Тип столика(Обычный\VIP\Веранда)
    /// </summary>
    public enum ERestaurant_Table_Type : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// Зал
        /// </summary>
        Ordinary = 3,


        /// <summary>
        /// VIP
        /// </summary>
        VIP = 6,


        /// <summary>
        /// Веранда
        /// </summary>
        Veranda = 9,


        /// <summary>
        /// Двор
        /// </summary>
        Dvor = 12,

    }
}