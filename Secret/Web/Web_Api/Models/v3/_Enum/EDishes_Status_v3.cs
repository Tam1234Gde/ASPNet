namespace HIDE.RestAndRant.Web_Api.Models.v3._Enum
{
    /// <summary>
    /// Статус блюда
    /// </summary>
    public enum EDishes_Status_v3 : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// Операция успешна
        /// </summary>
        OK = 100,


        /// <summary>
        /// Ошибка: не передан код категории блюда
        /// </summary>
        Dishcategory_Err = -100,


        /// <summary>
        /// Ошибка получения списка блюд
        /// </summary>
        DishesList_Err = -110,

    }
}