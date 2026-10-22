using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Orders
{
    /// <summary>
    /// Бронирование
    /// </summary>
    public class Model_Order_Booking
    {
        /// <summary>
        /// Стоимость брони
        /// </summary>
        public decimal? Booking_Price { get; set; }


        /// <summary>
        /// Бронирование
        /// </summary>
        public CBooking? Booking { get; set; }


        /// <summary>
        /// Ресторан
        /// </summary>
        public string? Restaurant { get; set; }


        /// <summary>
        /// Тип столика
        /// </summary>
        public string? Restaurant_Table_Type { get; set; }

    }
}