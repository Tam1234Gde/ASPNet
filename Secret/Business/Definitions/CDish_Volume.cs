using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Обьем блюда с ценой
    /// </summary>
    public class CDish_Volume
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Блюдо
        /// </summary>
        [DisplayName("Блюдо")]
        public int DishID { get; set; }

        /// <summary>
        /// Блюдо
        /// </summary>
        [JsonIgnore]
        public string? Dish { get; set; }

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