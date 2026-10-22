using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Статусы столика
    /// </summary>
    [Table("Restaurant_Table_Statuses")]
    [Comment("Статусы столика")]
    public class Restaurant_Table_Status
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Код")]
        public ERestaurant_Table_Status ID { get; set; }


        /// <summary>
        /// Наименование
        /// </summary>
        [Comment("Наименование")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Изображение стола
        /// </summary>
        [Comment("Изображение стола")]
        public byte[]? RestaurantTable_Image { get; set; }

    }
}