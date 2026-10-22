using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Ингридиенты блюд
    /// </summary>
    [Table("Dish_Ingridients")]
    [Comment("Ингридиенты блюд")]
    public class Dish_Ingridient
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
        /// Ингридиент - Код
        /// </summary>
        [Comment("Ингридиент - Код")]
        public int IngridientID { get; set; }


        /// <summary>
        /// Ингридиент
        /// </summary>
        [Comment("Ингридиент")]
        public Ingridient Ingridient { get; set; } = null!;


        /// <summary>
        /// Количество минут для приготовления
        /// </summary>
        [Comment("Количество минут для приготовления")]
        public int? Cooking_Minutes { get; set; }


        /// <summary>
        /// Стоимость
        /// </summary>
        [Precision(8, 2)]
        [Comment("Стоимость")]
        public decimal Price { get; set; }


        /// <summary>
        /// Группа
        /// </summary>
        [Comment("Категория ингридиента")]
        public int? Ingridient_Category_ID { get; set; }


        /// <summary>
        /// Категория ингридиента
        /// </summary>
        [Comment("Категория ингридиента")]
        public Dish_Ingridient_Category? Dish_Ingridient_Category { get; set; }


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