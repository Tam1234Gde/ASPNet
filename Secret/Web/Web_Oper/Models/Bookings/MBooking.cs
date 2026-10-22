using System.ComponentModel;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Oper.Models.Bookings
{
    public class MBooking : CBooking
    {
        /// <summary>
        /// Код заказа
        /// </summary>
        [DisplayName("Код заказа")]
        public COrder? Order { get; set; }

    }
}