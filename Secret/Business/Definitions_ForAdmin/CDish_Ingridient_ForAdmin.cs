using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Ингредиент блюда
    /// </summary>
    public class CDish_Ingridient_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Блюдо - код
        /// </summary>
        [DisplayName("Код блюда")]
        public int DishID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        [JsonIgnore]
        [DisplayName("Блюдо")]
        public string? Dish { get; set; }


        /// <summary>
        /// Ингредиент - код
        /// </summary>
        [DisplayName("Код ингредиента")]
        public int IngridientID { get; set; }


        /// <summary>
        /// Ингредиент
        /// </summary>
        [DisplayName("Наименование ингредиента")]
        public string? Ingridient { get; set; }


        /// <summary>
        /// Изображение ингредиента
        /// </summary>
        [Comment("Изображение ингредиента")]
        [DisplayName("Изображение")]
        public byte[]? Image { get; set; }


        /// <summary>
        /// Количество минут для приготовления
        /// </summary>
        [DisplayName("Минут для приготовления")]
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
    }
}