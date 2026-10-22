namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Тип заказа(С собой\Поесть в кафе\Доставка\Вынести к машине)
    /// </summary>
    public enum EOrder_Type : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,


        /// <summary>
        /// С собой
        /// </summary>
        Myself = 3,


        /// <summary>
        /// Поесть в кафе
        /// </summary>
        Incafe = 6,


        /// <summary>
        /// Доставка
        /// </summary>
        Delivery = 9,


        /// <summary>
        /// Вынести к машине
        /// </summary>
        Car = 12,

    }
}