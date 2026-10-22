using Microsoft.EntityFrameworkCore;

namespace HIDE.RestAndRant.Web_Api.Models.v3.Bookings
{
    /// <summary>
    /// Добавление брони
    /// </summary>
    public class Model_Bookings_AddBooking_v3
    {
        /// <summary>
        /// Клиент - Код
        /// </summary>
        [Comment("Клиент - Код")]
        public int ClientID { get; set; }


        /// <summary>
        /// Стол - Код
        /// </summary>
        [Comment("Стол - Код")]
        public int Restaurant_TableID { get; set; }


        /// <summary>
        /// Дата начала
        /// </summary>
        [Comment("Дата начала")]
        public DateTime Date_Start { get; set; }


        /// <summary>
        /// Дата окончания
        /// </summary>
        [Comment("Дата окончания")]
        public DateTime Date_End { get; set; }

    }
}