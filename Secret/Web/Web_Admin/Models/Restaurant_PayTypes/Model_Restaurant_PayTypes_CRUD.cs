using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Web_Admin.Models.Restaurant_PayTypes
{
    /// <summary>
    /// Доступные типы оплаты в ресторане > CRUD
    /// </summary>
    public class Model_Restaurant_PayTypes_CRUD
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Ресторан - код
        /// </summary>
        [DisplayName("Ресторан - код")]
        [Required(ErrorMessage = "Поле \"{0}\" обязательно к заполнению")]
        public int RestaurantID { get; set; }


        /// <summary>
        /// Ресторан
        /// </summary>
        [DisplayName("Ресторан")]
        public string? Restaurant { get; set; }


        /// <summary>
        /// Вид оплаты (картой, нал, ...) - Код
        /// </summary>
        [DisplayName("Вид оплаты (картой, нал, ...) - Код")]
        [Required(ErrorMessage = "Поле \"{0}\" обязательно к заполнению")]
        public EPayType Pay_TypeID { get; set; }


        /// <summary>
        /// Вид оплаты (картой, нал, ...)
        /// </summary>
        [DisplayName("Вид оплаты (картой, нал, ...)")]
        public string? Pay_Type { get; set; }


        /// <summary>
        /// Тип заказа(С собой\Поесть в кафе\Доставка\К машине) - Код
        /// </summary>
        [DisplayName("Тип заказа(С собой\\Поесть в кафе\\Доставка\\К машине) - Код")]
        [Required(ErrorMessage = "Поле \"{0}\" обязательно к заполнению")]
        public EOrder_Type Order_TypeID { get; set; }


        /// <summary>
        /// Тип заказа(С собой\Поесть в кафе\Доставка)
        /// </summary>
        [DisplayName("Тип заказа(С собой\\Поесть в кафе\\Доставка)")]
        public string? Order_Type { get; set; }


        /// <summary>
        /// Список ресторанов
        /// </summary>
        public List<CRestaurant>? List_Restaurants { get; set; }


        /// <summary>
        /// Список типов заказа(С собой\Поесть в кафе\Доставка)
        /// </summary>
        public List<CCommon>? List_Order_Types { get; set; }


        /// <summary>
        /// Список видов оплаты(картой, нал, ...)
        /// </summary>
        public List<CCommon>? List_Pay_Types { get; set; }

    }
}