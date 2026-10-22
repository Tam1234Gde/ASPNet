using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Зал в ресторане
    /// </summary>
    public class CRestaurant_Hall
    {
        /// <summary>
        /// Код
        /// </summary>
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Название
        /// </summary>
        [Comment("Название")]
        public string? Name { get; set; }


        /// <summary>
        /// Код ресторана
        /// </summary>
        [Comment("Код ресторана")]
        public int RestaurantID { get; set; }


        /// <summary>
        /// Ресторан
        /// </summary>
        [Comment("Ресторан")]
        public string Restaurant { get; set; } = null!;


        /// <summary>
        /// Тип столика - код
        /// </summary>
        [Comment("Тип столика - код")]
        public ERestaurant_Table_Type? Restaurant_Table_TypeID { get; set; }


        /// <summary>
        /// Тип столика
        /// </summary>
        [Comment("Тип столика")]
        public string? Restaurant_Table_Type { get; set; }


        /// <summary>
        /// Планировка
        /// </summary>
        [Comment("Планировка")]
        [JsonIgnore]
        public byte[]? Restaurant_Image { get; set; }


        /// <summary>
        /// True - Активен
        /// </summary>
        [Comment("Активен")]
        public bool Flag_Active { get; set; }

    }
}