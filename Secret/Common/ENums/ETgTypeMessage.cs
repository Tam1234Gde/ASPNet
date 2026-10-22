namespace HIDE.RestAndRant.Common.ENums;

/// <summary>
/// Тип сообщения в телеграм
/// </summary>
public enum ETgTypeMessage
{
    /// <summary>
    /// Не установлено
    /// </summary>
    _NotSet = 0,


    /// <summary>
    /// Попросить меню
    /// </summary>
    Menu = 3,


    /// <summary>
    ///  Позвать официанта
    /// </summary>
    Waiter = 6,


    /// <summary>
    ///  Комментарий официанту
    /// </summary>
    Comment = 9,


    /// <summary>
    /// Оплатить счет
    /// </summary>
    Pay = 12

}