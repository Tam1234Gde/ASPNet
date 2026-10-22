using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Содержимое избранного заказа
    /// </summary>
    [Table("Order_Favorite_Datas")]
    [Comment("Содержимое избранного заказа")]
    public class Order_Favorite_Data
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Избранный заказ - Код
        /// </summary>
        [Comment("Избранный заказ - Код")]
        public int OrderFavoriteID { get; set; }


        /// <summary>
        /// Избранный заказ
        /// </summary>
        [Comment("Избранный заказ")]
        public Order_Favorite? OrderFavorite { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable


        /// <summary>
        /// Блюдо - Код
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
        public Dish_Color? Dish_Color { get; set; }


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
        /// Объем - Код
        /// </summary>
        [Comment("Объем - Код")]
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
        /// Флаг пометки на удаление
        /// </summary>
        [Comment("Флаг пометки на удаление")]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Пользователь, установивший пометку на удаление - код
        /// </summary>
        [Comment("Пользователь, установивший пометку на удаление")]
        public int? UserID_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        [Comment("Дата пометки на удаление")]
        public DateTime? Date_Del { get; set; }


        /// <summary>
        /// Ингредиенты
        /// </summary>
        [Comment("Ингредиенты")]
        public List<Order_Favorite_Data_Ingridient>? Order_Data_Ingridients { get; set; }


    }
}