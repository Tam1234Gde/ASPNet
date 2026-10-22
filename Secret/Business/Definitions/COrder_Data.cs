using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Содержимое заказа
    /// </summary>
    public class COrder_Data
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
        public int OrderID { get; set; }


        /// <summary>
        /// Количество списанных бонусов
        /// </summary>
        public decimal? Bonuses_Minus { get; set; }


        /// <summary>
        /// Количество начисленных бонусов
        /// </summary>
        [JsonIgnore]
        public decimal? Bonuses_Plus { get; set; }


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
        /// Флаг пометки на удаление
        /// </summary>
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Код объёма
        /// </summary>
        public int? Dish_VolumeID { get; set; }


        /// <summary>
        /// Значение объёма
        /// </summary>
        [Precision(8, 2)]
        public decimal? Dish_VolumeValue { get; set; }


        /// <summary>
        /// Цена
        /// </summary>
        public decimal? Dish_VolumePrice { get; set; }


        /// <summary>
        /// Ингридиенты
        /// </summary>
        public List<COrder_Data_Ingridient>? List_Dish_Ingridients { get; set; }

    }
}
