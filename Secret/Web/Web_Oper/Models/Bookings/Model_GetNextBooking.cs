namespace HIDE.RestAndRant.Web_Oper.Models.Bookings
{
    /// <summary>
    /// Получение следующей брони
    /// </summary>
    public class Model_GetNextBooking
    {
        /// <summary>
        /// Дата начала следующей брони
        /// </summary>
        public string? Date_Next { get; set; }


        /// <summary>
        /// True - Столик занят
        /// </summary>
        public bool Flag_Unavailable { get; set; }
    }
}