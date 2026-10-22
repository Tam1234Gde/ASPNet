using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Ингредиент блюда в меню ресторана
    /// </summary>
    [Table("Restaurant_Menu_Dish_Ingredients")]
    [Comment("Ингредиенты блюда в меню ресторана")]
    public class Restaurant_Menu_Dish_Ingredient
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
        public int Restaurant_Menu_DishID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        [Comment("Блюдо")]
        public Restaurant_Menu_Dish? Restaurant_Menu_Dish { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable


        /// <summary>
        /// Ингридиент - Код
        /// </summary>
        [Comment("Ингридиент - Код")]
        public int IngridientID { get; set; }


        /// <summary>
        /// Ингридиент
        /// </summary>
        [Comment("Ингридиент")]
        public Ingridient? Ingridient { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable


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
        [Comment("Группа")]
        public int? IngridientGroupID { get; set; }


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