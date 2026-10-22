using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Блюдо - Цвета
    /// </summary>
    [Table("Dish_Colors")]
    [Comment("Цвета блюда")]
    public class Dish_Color
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
        public Dish? Dish { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable


        /// <summary>
        /// Цвет - Код
        /// </summary>
        [Comment("Цвет ID")]
        public int ColorID { get; set; }


        /// <summary>
        /// Цвет
        /// </summary>
        [Comment("Цвет")]
        public Color Color { get; set; } = null!;


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