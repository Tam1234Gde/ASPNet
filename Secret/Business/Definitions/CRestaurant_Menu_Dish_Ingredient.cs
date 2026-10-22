using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Ингредиент блюда в меню ресторана
    /// </summary>
    public class CRestaurant_Menu_Dish_Ingredient
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
        /// Ингридиент - Код
        /// </summary>
        [DisplayName("Ингридиент - Код")]
        public int IngridientID { get; set; }


        /// <summary>
        /// Ингридиент
        /// </summary>
        [DisplayName("Ингридиент")]
        public string? Ingridient { get; set; }


        /// <summary>
        /// Количество минут для приготовления
        /// </summary>
        [DisplayName("Количество минут для приготовления")]
        public int? Cooking_Minutes { get; set; }


        /// <summary>
        /// Стоимость
        /// </summary>
        [DisplayName("Стоимость")]
        public decimal Price { get; set; }


        /// <summary>
        /// Группа
        /// </summary>
        [DisplayName("Группа")]
        public int? IngridientGroupID { get; set; }


        /// <summary>
        /// Идентификатор категории
        /// </summary>
        [DisplayName("Идентификатор категории")]
        public int? Category_ID { get; set; }


        /// <summary>
        /// Изображение ингредиента
        /// </summary>
        [Comment("Изображение ингредиента")]
        public byte[]? Image { get; set; }

    }
}