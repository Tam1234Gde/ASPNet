using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Детали содержимого заказа - ингредиенты
    /// </summary>
    [Table("Order_Data_Ingridients")]
    [Comment("Детали содержимого заказа - ингредиенты")]
    public class Order_Data_Ingridient
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Ссылка на позицию заказа - Код
        /// </summary>
        [Comment("Ссылка на позицию заказа - Код")]
        public int Order_DataID { get; set; }


        /// <summary>
        /// Ссылка на позицию заказа
        /// </summary>
        [Comment("Ссылка на позицию заказа")]
        public Order_Data Order_Data { get; set; } = null!;


        /// <summary>
        /// Ингредиент - Код
        /// </summary>
        [Comment("Ингредиент - Код")]
        public int IngridientID { get; set; }


        /// <summary>
        /// Ингредиент
        /// </summary>
        [Comment("Ингредиент")]
        public Ingridient Ingridient { get; set; } = null!;


        /// <summary>
        /// Цена ингредиента
        /// </summary>
        [Precision(8, 2)]
        [Comment("Цена ингредиента")]
        public decimal Ingridient_Price { get; set; }


        /// <summary>
        /// Количество игредиента
        /// </summary>
        [Precision(8, 2)]
        [Comment("Количество игредиента")]
        public decimal Ingridient_Count { get; set; }


        /// <summary>
        /// Общая сумма ингредиента
        /// </summary>
        [Precision(8, 2)]
        [Comment("Общая сумма ингредиента")]
        public decimal Ingridient_Summ { get; set; }


        /// <summary>
        /// Наименование ингредиента в момент оформления
        /// </summary>
        [Comment("Наименование ингредиента в момент оформления")]
        public string Ingridient_Name { get; set; } = null!;

    }
}