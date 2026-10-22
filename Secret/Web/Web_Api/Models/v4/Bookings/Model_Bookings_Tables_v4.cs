namespace HIDE.RestAndRant.Web_Api.Models.v4.Bookings
{
    /// <summary>
    /// Список столиков в указанное время
    /// </summary>
    public class Model_Bookings_Tables_v4
    {
        /// <summary>
        /// Желаемая дата и время 
        /// </summary>
        public DateTime Date_Booking { get; set; }

        /// <summary>
        /// Тип столика
        /// </summary>
        public int Restaurant_Table_TypeID { get; set; }

        /// <summary>
        /// Ресторан
        /// </summary>
        public int RestaurantID { get; set; }

    }
}