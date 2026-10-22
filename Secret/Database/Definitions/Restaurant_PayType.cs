using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Доступные типы оплаты в ресторане
    /// </summary>
    [Table("Restaurant_PayTypes")]
    [Comment("Доступные типы оплаты в ресторане")]
    public class Restaurant_PayType
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Ресторан - код
        /// </summary>
        [Comment("Ресторан - код")]
        public int RestaurantID { get; set; }


        /// <summary>
        /// Ресторан
        /// </summary>
        [Comment("Ресторан")]
        public Restaurant Restaurant { get; set; } = null!;


        /// <summary>
        /// Вид оплаты (картой, нал, ...) - Код
        /// </summary>
        [Comment("Вид оплаты (картой, нал, ...) - Код")]
        public EPayType Pay_TypeID { get; set; }


        /// <summary>
        /// Вид оплаты (картой, нал, ...)
        /// </summary>
        [Comment("Вид оплаты (картой, нал, ...)")]
        public Pay_Type Pay_Type { get; set; } = null!;


        /// <summary>
        /// Тип заказа(С собой\Поесть в кафе\Доставка\К машине) - Код
        /// </summary>
        [Comment("Тип заказа(С собой\\Поесть в кафе\\Доставка\\К машине) - Код")]
        public EOrder_Type Order_TypeID { get; set; }


        /// <summary>
        /// Тип заказа(С собой\Поесть в кафе\Доставка)
        /// </summary>
        [Comment("Тип заказа(С собой\\Поесть в кафе\\Доставка)")]
        public Order_Type Order_Type { get; set; } = null!;


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        [Comment("Флаг пометки на удаление")]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        [Comment("Дата пометки на удаление")]
        public DateTime? Date_Del { get; set; }


        /// <summary>
        /// Пользователь, который поставил запись на удаление
        /// </summary>
        [Comment("Пользователь, который поставил запись на удаление")]
        public int? UserID_Del { get; set; }

    }
}