namespace HIDE.RestAndRant.Web_Api.Models.v3.Bookings
{
    /// <summary>
    /// Время когда можно забронировать столик
    /// </summary>
    public class Model_Bookings_Times_v3
    {
        /// <summary>
        /// День бронирования
        /// </summary>
        public DateTime Date_DayBooking { get; set; }

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