using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Статусы столика
    /// </summary>
    public class CRestaurant_Table_Status
    {
        /// <summary>
        /// Код
        /// </summary>
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Наименование
        /// </summary>
        [Comment("Наименование")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Изображение стола
        /// </summary>
        [Comment("Изображение стола")]
        [JsonIgnore]
        public byte[]? RestaurantTable_Image { get; set; }

    }
}