using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Содержимое избранного заказа
    /// </summary>
    public class COrder_Favorite_Data
    {
        /// <summary>
        /// Код
        /// </summary>
        [JsonIgnore]
        public int ID { get; set; }


        /// <summary>
        /// Заказ - код
        /// </summary>
        [JsonIgnore]
        public int OrderFavoriteID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        public int DishID { get; set; }


        /// <summary>
        /// Цвет (стаканчика, ложки, и т.д.)
        /// </summary>
        public int? Dish_ColorID { get; set; }


        /// <summary>
        /// Стоимость позиции
        /// </summary>
        public decimal Dish_Price { get; set; }


        /// <summary>
        /// Количество 
        /// </summary>
        public decimal Dish_Count { get; set; }


        /// <summary>
        /// Общая сумма позиции
        /// </summary>
        public decimal Dish_Summ { get; set; }


        /// <summary>
        /// Наименование товара в момент оформления
        /// </summary>
        public string Dish_Name { get; set; } = null!;


        /// <summary>
        /// Код объёма
        /// </summary>
        public int? Dish_VolumeID { get; set; }


        /// <summary>
        /// Значение объёма
        /// </summary>
        public decimal? Dish_VolumeValue { get; set; }


        /// <summary>
        /// Цена
        /// </summary>
        public decimal? Dish_VolumePrice { get; set; }


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        [JsonIgnore]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Ингридиенты
        /// </summary>
        public List<COrder_Favorite_Data_Ingridient>? List_Dish_Ingridients { get; set; }


    }
}