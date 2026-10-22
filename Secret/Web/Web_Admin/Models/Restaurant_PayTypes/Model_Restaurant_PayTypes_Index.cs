using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Restaurant_PayTypes
{
    /// <summary>
    /// Доступные типы оплаты в ресторане > Главная
    /// </summary>
    public class Model_Restaurant_PayTypes_Index
    {
        /// <summary>
        /// Код ресторана
        /// </summary>
        public int RestaurantID { get; set; }


        /// <summary>
        /// Список доступных типов оплаты в ресторане
        /// </summary>
        public List<CRestaurant_PayType>? List_Restaurant_PayTypes { get; set; }

    }
}