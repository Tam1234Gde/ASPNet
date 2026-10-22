using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Меню ресторана
    /// </summary>
    public class CRestaurant_Menu
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Код ресторана
        /// </summary>
        [DisplayName("Код ресторана")]
        public int RestaurantID { get; set; }


        /// <summary>
        /// Ресторан
        /// </summary>
        [DisplayName("Ресторан")]
        public string? Restaurant { get; set; }


        /// <summary>
        /// Название меню
        /// </summary>
        [DisplayName("Название меню")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Активен
        /// </summary>
        [DisplayName("Активен")]
        public bool Flag_Active { get; set; }

    }
}