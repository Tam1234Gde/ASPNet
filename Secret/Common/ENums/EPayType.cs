namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Вид оплаты
    /// </summary>
    public enum EPayType : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// Наличные
        /// </summary>
        Money = 3,


        /// <summary>
        /// Картой в приложении
        /// </summary>
        CardInApp = 6,


        /// <summary>
        /// Картой через терминал
        /// </summary>
        CardByTerminal = 9,

    }
}