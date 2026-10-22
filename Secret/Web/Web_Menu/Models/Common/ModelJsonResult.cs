namespace HIDE.RestAndRant.Web_Site_Menu.Models.Common
{
    /// <summary>
    /// Перечисление значений результата(JSON)
    /// </summary>
    public enum ModelJsonResult : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,

        /// <summary>
        /// Успешно
        /// </summary>
        OK = 200,

        /// <summary>
        /// Ошибка
        /// </summary>
        Err = -200,

    }
}