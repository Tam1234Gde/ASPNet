using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Объем блюда в меню ресторана
    /// </summary>
    public class CRestaurant_Menu_Dish_Volume
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Блюдо - Код
        /// </summary>
        [DisplayName("Блюдо - Код")]
        public int Restaurant_Menu_DishID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        [DisplayName("Блюдо")]
        public string? Restaurant_Menu_Dish { get; set; }


        /// <summary>
        /// Объём
        /// </summary>
        [DisplayName("Объём")]
        public decimal? VolumeValue { get; set; }


        /// <summary>
        /// Объём
        /// </summary>
        [DisplayName("Объём")]
        public int VolumeID { get; set; }


        /// <summary>
        /// Дополнительное наменование
        /// </summary>
        public string? VolumeName { get; set; }


        /// <summary>
        /// Цена за этот обьем
        /// </summary>
        [DisplayName("Цена за этот обьем")]
        public decimal VolumePrice { get; set; }


        /// <summary>
        /// Порядковый номер
        /// </summary>
        [DisplayName("Порядковый номер")]
        public int OrderNum { get; set; }

    }
}