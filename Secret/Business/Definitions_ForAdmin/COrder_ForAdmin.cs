using System.ComponentModel;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class COrder_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код заказа")]
        public int ID { get; set; }


        /// <summary>
        /// Код избранного заказа
        /// </summary>
        [DisplayName("Код избранного заказа")]
        public int? Order_FavoriteID { get; set; }


        /// <summary>
        /// Дата заказа
        /// </summary>
        [DisplayName("Дата заказа")]
        public DateTime Date_Act { get; set; }


        /// <summary>
        /// Статус заказа - код
        /// </summary>
        [DisplayName("Код статуса")]
        public EOrder_Status Order_StatusID { get; set; }


        /// <summary>
        /// Статус заказа
        /// </summary>
        [DisplayName("Статус заказа")]
        public string Order_Status { get; set; } = null!;


        /// <summary>
        /// Вид заказа - код
        /// </summary>
        [DisplayName("Код вида заказа")]
        public EOrder_Type Order_TypeID { get; set; }


        /// <summary>
        /// Вид заказа
        /// </summary>
        [DisplayName("Вид заказа")]
        public string Order_Type { get; set; } = null!;


        /// <summary>
        /// Клиент - код
        /// </summary>
        [DisplayName("Код клиента")]
        public int ClientID { get; set; }


        /// <summary>
        /// Клиент
        /// </summary>
        [DisplayName("Клиент")]
        public string? Client { get; set; }


        /// <summary>
        /// Телефон клиента
        /// </summary>
        [DisplayName("Телефон клиента")]
        public string? Client_Phone { get; set; }


        /// <summary>
        /// Аватарка клиента
        /// </summary>
        [DisplayName("Аватарка")]
        public byte[]? Client_Photo { get; set; }


        /// <summary>
        /// Имя, на кого заказ
        /// </summary>
        [DisplayName("Имя")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Телефон, на кого заказ
        /// </summary>
        [DisplayName("Телефон")]
        public string Phone { get; set; } = null!;


        /// <summary>
        /// Вид оплаты - код
        /// </summary>
        [DisplayName("Код вида оплаты")]
        public EPayType Pay_TypeID { get; set; }


        /// <summary>
        /// Вид оплаты
        /// </summary>
        [DisplayName("Вид оплаты")]
        public string Pay_Type { get; set; } = null!;


        /// <summary>
        /// Ресторан, куда совершается заказ - код
        /// </summary>
        [DisplayName("Код ресторана")]
        public int RestaurantID { get; set; }


        /// <summary>
        /// Ресторан, куда совершается заказ
        /// </summary>
        [DisplayName("Ресторан")]
        public string? Restaurant { get; set; }


        /// <summary>
        /// Дата и время к которому приготовить заказ
        /// </summary>
        [DisplayName("Целевое время")]
        public DateTime? Date_To { get; set; }


        /// <summary>
        /// Сумма заказа
        /// </summary>
        [DisplayName("Сумма заказа")]
        public decimal? Sum { get; set; }


        /// <summary>
        /// Марка машины
        /// </summary>
        [DisplayName("Марка машины")]
        public string? Car_Brand { get; set; }


        /// <summary>
        /// Госномера машины
        /// </summary>
        [DisplayName("Госномера машины")]
        public string? Car_RegNumber { get; set; }


        /// <summary>
        /// Цвет машины
        /// </summary>
        [DisplayName("Цвет машины")]
        public string? Car_Color { get; set; }


        /// <summary>
        /// Комментарий
        /// </summary>
        [DisplayName("Комментарий")]
        public string? Comment { get; set; }


        /// <summary>
        /// Добавить к заказу
        /// </summary>
        [DisplayName("Добавить к заказу")]
        public string? Comment_AddToOrder { get; set; }


        /// <summary>
        /// Убрать из заказа
        /// </summary>
        [DisplayName("Убрать из заказа")]
        public string? Comment_RemoveFromOrder { get; set; }


        /// <summary>
        /// Стоимость доставки
        /// </summary>
        [DisplayName("Стоимость доставки")]
        public decimal? Delivery_Price { get; set; }


        /// <summary>
        /// Стоимость брони
        /// </summary>
        [DisplayName("Стоимость брони")]
        public decimal? Booking_Price { get; set; }


        /// <summary>
        /// Флаг того, что клиент будет кушать в кафе, если false - то на вынос
        /// </summary>
        [DisplayName("Кушает в кафе")]
        public bool Flag_Inhouse { get; set; }


        /// <summary>
        /// Флаг того, что заказ оплачен
        /// </summary>
        [DisplayName("Оплачен")]
        public bool Flag_Payed { get; set; }


        /// <summary>
        /// Бронирование - Код
        /// </summary>
        [DisplayName("Код бронирования")]
        public int? BookingID { get; set; }


        /// <summary>
        /// Дать сдачи с ...
        /// </summary>
        [DisplayName("Дать сдачи с")]
        public decimal? Handing { get; set; }


        /// <summary>
        /// Детали заказа
        /// </summary>
        [DisplayName("Детали заказа")]
        public List<COrder_Data_ForAdmin>? List_OrderDatas { get; set; }


        /// <summary>
        /// Детали доставки
        /// </summary>
        [DisplayName("Детали доставки")]
        public Order_Delivery? Order_Delivery { get; set; }
    }
}