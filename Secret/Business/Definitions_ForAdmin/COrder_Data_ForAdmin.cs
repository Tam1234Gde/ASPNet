using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Содержимое заказа
    /// </summary>
    public class COrder_Data_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [JsonIgnore]
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Заказ - код
        /// </summary>
        [JsonIgnore]
        [DisplayName("Код заказа")]
        public int OrderID { get; set; }


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
        /// Цвет (стаканчика, ложки, и т.д.) - код
        /// </summary>
        [DisplayName("Код цвета")]
        public int? Dish_ColorID { get; set; }


        /// <summary>
        /// Цвет (стаканчика, ложки, и т.д.)
        /// </summary>
        [JsonIgnore]
        [DisplayName("Цвет")]
        public string? Dish_Color { get; set; }


        /// <summary>
        /// Стоимость позиции
        /// </summary>
        [Precision(8, 2)]
        [DisplayName("Стоимость")]
        public decimal Dish_Price { get; set; }


        /// <summary>
        /// Количество
        /// </summary>
        [Precision(8, 2)]
        [DisplayName("Количество")]
        public decimal Dish_Count { get; set; }


        /// <summary>
        /// Общая сумма позиции
        /// </summary>
        [Precision(8, 2)]
        [DisplayName("Общая сумма")]
        public decimal Dish_Summ { get; set; }


        /// <summary>
        /// Наименование товара в момент оформления
        /// </summary>
        [DisplayName("Наименование товара")]
        public string Dish_Name { get; set; } = null!;


        /// <summary>
        /// Код объёма
        /// </summary>
        [DisplayName("Код объёма")]
        public int? Dish_VolumeID { get; set; }


        /// <summary>
        /// Значение объёма
        /// </summary>
        [Precision(8, 2)]
        [DisplayName("Значение объёма")]
        public decimal? Dish_VolumeValue { get; set; }


        /// <summary>
        /// Цена объёма
        /// </summary>
        [Precision(8, 2)]
        [DisplayName("Цена объёма")]
        public decimal? Dish_VolumePrice { get; set; }


        /// <summary>
        /// Изображение блюда
        /// </summary>
        [Comment("Изображение блюда")]
        [DisplayName("Изображение")]
        public byte[]? Dish_Image { get; set; }


        /// <summary>
        /// Количество списанных бонусов
        /// </summary>
        [Precision(8, 2)]
        [DisplayName("Списано бонусов")]
        public decimal? Bonuses_Minus { get; set; }


        /// <summary>
        /// Количество начисленных бонусов
        /// </summary>
        [Precision(8, 2)]
        [DisplayName("Начислено бонусов")]
        public decimal? Bonuses_Plus { get; set; }


        /// <summary>
        /// Ингредиенты
        /// </summary>
        [DisplayName("Ингредиенты")]
        public List<COrder_Data_Ingridient_ForAdmin>? List_OrderDataIngridients { get; set; }
    }
}