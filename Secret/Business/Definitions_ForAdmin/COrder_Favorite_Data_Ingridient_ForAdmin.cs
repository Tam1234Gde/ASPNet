using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Детали содержимого избранного заказа - ингредиенты
    /// </summary>
    public class COrder_Favorite_Data_Ingridient_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [JsonIgnore]
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Ссылка на позицию заказа - код
        /// </summary>
        [JsonIgnore]
        [DisplayName("Код позиции заказа")]
        public int Order_Favorite_DataID { get; set; }


        /// <summary>
        /// Ингредиент - код
        /// </summary>
        [DisplayName("Код ингредиента")]
        public int IngridientID { get; set; }


        /// <summary>
        /// Ингредиент
        /// </summary>
        [JsonIgnore]
        [DisplayName("Ингредиент")]
        public string Ingridient { get; set; } = null!;


        /// <summary>
        /// Цена ингредиента
        /// </summary>
        [DisplayName("Цена ингредиента")]
        public decimal Ingridient_Price { get; set; }


        /// <summary>
        /// Количество ингредиента
        /// </summary>
        [DisplayName("Количество ингредиента")]
        public decimal Ingridient_Count { get; set; }


        /// <summary>
        /// Общая сумма ингредиента
        /// </summary>
        [DisplayName("Общая сумма ингредиента")]
        public decimal Ingridient_Summ { get; set; }
    }
}