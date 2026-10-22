using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Цвет блюда
    /// </summary>
    public class CDish_Color
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
        /// Цвет
        /// </summary>
        public int ColorID { get; set; }

        /// <summary>
        /// Цвет
        /// </summary>
        public string? Color { get; set; }

    }
}