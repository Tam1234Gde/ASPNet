using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Web_Api.Models.v4.Restaurant_PayTypes
{
    /// <summary>
    /// Получить список доступных типов оплаты в ресторане
    /// </summary>
    public class Model_Restaurant_PayTypes_v4
    {
        /// <summary>
        /// Код ресторана
        /// </summary>
        public int RestaurantID { get; set; }


        /// <summary>
        /// Тип заказа(С собой\Поесть в кафе\Доставка\Вынести к машине)
        /// </summary>
        public EOrder_Type Order_TypeID { get; set; }

    }
}