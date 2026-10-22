using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Web_Site_Menu.Models.Home
{
    /// <summary>
    /// Главная страница
    /// </summary>
    public class Model_Home_Index
    {
        /// <summary>
        /// Время начала следующей брони
        /// </summary>
        public DateTime? Date_NextBooking { get; set; }


        /// <summary>
        /// Время окончания следующей брони
        /// </summary>
        public DateTime? Date_NextBookingEnd { get; set; }


        /// <summary>
        /// Забронирован вами до ...
        /// </summary>
        public DateTime? BookingEnd { get; set; }


        /// <summary>
        /// Тип устройства
        /// </summary>
        public EAppType AppType { get; set; }


        /// <summary>
        /// True - Отобразить модальное окно "Отсканируйте QR-код"
        /// </summary>
        public bool Flag_isQR { get; set; }

    }
}