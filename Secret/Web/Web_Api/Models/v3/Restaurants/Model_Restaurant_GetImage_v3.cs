using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Web_Api.Models.v3.Restaurants
{
    /// <summary>
    /// Получить планировку помещения
    /// </summary>
    public class Model_Restaurant_GetImage_v3
    {
        /// <summary>
        /// Код ресторана
        /// </summary>
        public int RestaurantID { get; set; }


        /// <summary>
        /// Тип стола
        /// </summary>
        public ERestaurant_Table_Type Restaurant_Table_TypeID { get; set; }

    }
}