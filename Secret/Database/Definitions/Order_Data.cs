using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Содержимое заказа
    /// </summary>
    [Table("Order_Datas")]
    [Comment("Содержимое заказа")]
    public class Order_Data
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Заказ - Код
        /// </summary>
        [Comment("Заказ - Код")]
        public int OrderID { get; set; }


        /// <summary>
        /// Заказ
        /// </summary>
        [Comment("Заказ")]
        public Order Order { get; set; } = null!;


        /// <summary>
        /// Количество списанных бонусов
        /// </summary>
        [Precision(8, 2)]
        [Comment("Количество списанных бонусов")]
        public decimal? Bonuses_Minus { get; set; }


        /// <summary>
        /// Количество начисленных бонусов
        /// </summary>
        [Precision(8, 2)]
        [Comment("Количество начисленных бонусов")]
        public decimal? Bonuses_Plus { get; set; }


        /// <summary>
        /// людо - Код
        /// </summary>
        [Comment("Блюдо - Код")]
        public int DishID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        [Comment("Блюдо")]
        public Dish Dish { get; set; } = null!;


        /// <summary>
        /// Цвет (стаканчика, ложки, и т.д.) - Код
        /// </summary>
        [Comment("Цвет - Код")]
        public int? Dish_ColorID { get; set; }


        /// <summary>
        /// Цвет (стаканчика, ложки, и т.д.)
        /// </summary>
        [Comment("Цвет")]
        public Dish_Color? Dish_Color { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable


        /// <summary>
        /// Стоимость позиции
        /// </summary>
        [Precision(8, 2)]
        [Comment("Стоимость позиции")]
        public decimal Dish_Price { get; set; }


        /// <summary>
        /// Количество 
        /// </summary>
        [Precision(8, 2)]
        [Comment("Количество")]
        public decimal Dish_Count { get; set; }


        /// <summary>
        /// Общая сумма позиции
        /// </summary>
        [Precision(8, 2)]
        [Comment("Общая сумма позиции")]
        public decimal Dish_Summ { get; set; }


        /// <summary>
        /// Наименование товара в момент оформления
        /// </summary>
        [Comment("Наименование товара в момент оформления")]
        public string Dish_Name { get; set; } = null!;


        /// <summary>
        /// Код объёма
        /// </summary>
        [Comment("Код объёма")]
        public int? Dish_VolumeID { get; set; }


        /// <summary>
        /// Объём
        /// </summary>
        [Precision(8, 2)]
        [Comment("Объём")]
        public decimal? Dish_VolumeValue { get; set; }


        /// <summary>
        /// Цена
        /// </summary>
        [Precision(8, 2)]
        [Comment("Цена")]
        public decimal? Dish_VolumePrice { get; set; }


        /// <summary>
        /// Ингредиенты
        /// </summary>
        [Comment("Ингредиенты")]
        public List<Order_Data_Ingridient>? Order_Data_Ingridients { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable

    }
}