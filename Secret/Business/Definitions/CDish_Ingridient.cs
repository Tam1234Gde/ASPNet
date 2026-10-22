using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Ингридиент
    /// </summary>
    public class CDish_Ingridient
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Блюдо - код
        /// </summary>
        public int DishID { get; set; }

        /// <summary>
        /// Блюдо
        /// </summary>
        [JsonIgnore]
        public string? Dish { get; set; }

        /// <summary>
        /// Ингридиент
        /// </summary>
        public int IngridientID { get; set; }

        /// <summary>
        /// Ингридиент
        /// </summary>
        [DisplayName("Наименование ингридиента")]
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
    }
}