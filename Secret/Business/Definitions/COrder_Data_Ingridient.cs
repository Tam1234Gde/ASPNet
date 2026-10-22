using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Детали содержимого заказа - ингридиенты
    /// </summary>
    public class COrder_Data_Ingridient
    {
        /// <summary>
        /// Ссылка на позицию заказа - код
        /// </summary>
        [JsonIgnore]
        public int Order_DataID { get; set; }


        /// <summary>
        /// Ингридиент - код
        /// </summary>
        public int IngridientID { get; set; }


        /// <summary>
        /// Цена ингридиента
        /// </summary>
        public decimal Ingridient_Price { get; set; }


        /// <summary>
        /// Количество игридиента
        /// </summary>
        public decimal Ingridient_Count { get; set; }


        /// <summary>
        /// Общая сумма ингридиента
        /// </summary>
        public decimal Ingridient_Summ { get; set; }


        /// <summary>
        /// Наименование ингредиента в момент оформления
        /// </summary>
        public string Ingridient_Name { get; set; } = null!;


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        public bool Flag_Del { get; set; }

    }
}
