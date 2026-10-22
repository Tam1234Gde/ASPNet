using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Api.Models.v3.Bookings
{
    /// <summary>
    /// Общая модель
    /// </summary>
    public class Model_Booking_Time_v3
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date_Booking { get; set; }


        /// <summary>
        /// Список столиков
        /// </summary>
        public List<CRestaurant_Table>? List_Restaurant_Tables { get; set; }

    }
}