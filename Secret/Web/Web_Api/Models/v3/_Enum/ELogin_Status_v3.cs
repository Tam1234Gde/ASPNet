namespace HIDE.RestAndRant.Web_Api.Models.v3._Enum
{
    /// <summary>
    /// Статус клиента
    /// </summary>
    public enum ELogin_Status_v3 : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// Клиент успешно найден
        /// </summary>
        Client_OK = 100,


        /// <summary>
        /// Клиента ошибка поиска клиента
        /// </summary>
        Client_Err = -100,


        /// <summary>
        /// Верный SMS-код
        /// </summary>
        SmsCodeOK = 200,


        /// <summary>
        /// Неверный SMS-код
        /// </summary>
        SmsCodeErr = -205,

    }
}