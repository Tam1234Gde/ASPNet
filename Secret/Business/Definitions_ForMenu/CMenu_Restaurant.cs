using System;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Привязка меню к ресторану
    /// </summary>
    public class CMenu_Restaurant
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Код меню
        /// </summary>
        public int MenuID { get; set; }

        /// <summary>
        /// Код ресторана
        /// </summary>
        public int RestaurantID { get; set; }

        /// <summary>
        /// Дата начала показа
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Дата окончания показа
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Код состояния активности
        /// </summary>
        public int Menu_OptionStateID { get; set; }
    }
}