namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Идентификатор системы облачных уведомлений(FCM, HCM)
    /// </summary>
    public enum EClient_Notification_TokenType : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,

        /// <summary>
        /// FCM
        /// </summary>
        FCM = 1,

        /// <summary>
        /// HCM
        /// </summary>
        HCM = 3,
    }
}