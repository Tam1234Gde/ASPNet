using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Объём блюда с ценой
    /// </summary>
    [Table("Dish_Volumes")]
    [Comment("Объёмы блюда")]
    public class Dish_Volume
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Блюдо - Код
        /// </summary>
        [Comment("Блюдо - Код")]
        public int DishID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        [Comment("Блюдо")]
        public Dish Dish { get; set; } = null!;


        /// <summary>
        /// Объем - Код
        /// </summary>
        [Comment("Объем - Код")]
        public int VolumeID { get; set; }


        /// <summary>
        /// Объем
        /// </summary>
        [Comment("Объем")]
        public Volume Volume { get; set; } = null!;


        /// <summary>
        /// Цена за этот обьем
        /// </summary>
        [Precision(8, 2)]
        [Comment("Цена за этот обьем")]
        public decimal Price { get; set; }


        /// <summary>
        /// Порядковый номер
        /// </summary>
        [Comment("Порядковый номер")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        [Comment("Флаг пометки на удаление")]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        [Comment("Дата пометки на удаление")]
        public DateTime? Date_Del { get; set; }


        /// <summary>
        /// Пользователь, который поставил запись на удаление
        /// </summary>
        [Comment("Пользователь, который поставил запись на удаление")]
        public int? UserID_Del { get; set; }


    }
}