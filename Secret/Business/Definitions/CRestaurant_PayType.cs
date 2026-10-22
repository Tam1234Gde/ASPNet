using System.ComponentModel;
using System.Text.Json.Serialization;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Доступные типы оплаты в ресторане
    /// </summary>
    public class CRestaurant_PayType
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        [JsonIgnore]
        public int ID { get; set; }


        /// <summary>
        /// Ресторан - код
        /// </summary>
        [DisplayName("Ресторан - код")]
        [JsonIgnore]
        public int RestaurantID { get; set; }


        /// <summary>
        /// Ресторан
        /// </summary>
        [DisplayName("Ресторан")]
        [JsonIgnore]
        public string? Restaurant { get; set; }


        /// <summary>
        /// Вид оплаты (картой, нал, ...) - Код
        /// </summary>
        [DisplayName("Вид оплаты (картой, нал, ...) - Код")]
        public EPayType Pay_TypeID { get; set; }


        /// <summary>
        /// Вид оплаты (картой, нал, ...)
        /// </summary>
        [DisplayName("Вид оплаты (картой, нал, ...)")]
        [JsonIgnore]
        public string? Pay_Type { get; set; }


        /// <summary>
        /// Тип заказа(С собой\Поесть в кафе\Доставка\К машине) - Код
        /// </summary>
        [DisplayName("Тип заказа(С собой\\Поесть в кафе\\Доставка\\К машине) - Код")]
        [JsonIgnore]
        public EOrder_Type Order_TypeID { get; set; }


        /// <summary>
        /// Тип заказа(С собой\Поесть в кафе\Доставка)
        /// </summary>
        [DisplayName("Тип заказа(С собой\\Поесть в кафе\\Доставка)")]
        [JsonIgnore]
        public string? Order_Type { get; set; }


    }
}