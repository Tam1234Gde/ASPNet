using Microsoft.EntityFrameworkCore;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Web_Api.Models.v3.Orders
{
    /// <summary>
    /// Добавление заказов(оплата картой)
    /// </summary>
    public class Model_Orders_AddOrder_Card_v3
    {
        /// <summary>
        /// Код клиента
        /// </summary>
        public int? ClientID { get; set; }


        /// <summary>
        /// Имя, на кого заказ
        /// </summary>
        public string Name { get; set; } = null!;


        /// <summary>
        /// Телефон, на кого заказ
        /// </summary>
        public string Phone { get; set; } = null!;


        /// <summary>
        /// Вид оплаты - код
        /// </summary>
        public EPayType Pay_TypeID { get; set; }


        /// <summary>
        /// Тип заказа(С собой\Поесть в кафе\Доставка)
        /// </summary>
        public EOrder_Type Order_TypeID { get; set; }


        /// <summary>
        /// Ресторан, куда совершается заказ
        /// </summary>
        public int RestaurantID { get; set; }


        /// <summary>
        /// Дата и время к которому приготовить заказ
        /// </summary>
        public DateTime? Date_To { get; set; }


        /// <summary>
        /// Дата и время к которому приготовить заказ(long)
        /// </summary>
        public long? Date_To_Unix { get; set; }


        /// <summary>
        /// Флаг того, что клиент будет кушать в кафе, если false - то на вынос
        /// </summary>
        public bool Flag_Inhouse { get; set; }


        /// <summary>
        /// Идентификатор системы оплаты
        /// </summary>
        public string PaySystemIdent { get; set; } = null!;


        /// <summary>
        /// Сумма платежа
        /// </summary>
        public decimal Summ { get; set; }


        /// <summary>
        /// Комментарий к заказу
        /// </summary>
        public string? Comment { get; set; }


        /// <summary>
        /// Комментарий - Добавить к заказу
        /// </summary>
        [Comment("Комментарий - Добавить к заказу")]
        public string? Comment_AddToOrder { get; set; }


        /// <summary>
        /// Комментарий - Убрать из заказа
        /// </summary>
        [Comment("Комментарий - Убрать из заказа")]
        public string? Comment_RemoveFromOrder { get; set; }


        /// <summary>
        /// Марка машины
        /// </summary>
        public string? Car_Brand { get; set; }


        /// <summary>
        /// Госномера машины
        /// </summary>
        public string? Car_RegNumber { get; set; }


        /// <summary>
        /// Цвет машины
        /// </summary>
        public string? Car_Color { get; set; }


        /// <summary>
        /// Адрес доставки
        /// </summary>
        public string? Delivery_Address { get; set; }


        /// <summary>
        /// Квартира
        /// </summary>
        public string? Delivery_Apartment { get; set; }


        /// <summary>
        /// Код подъезда
        /// </summary>
        public string? Delivery_EntranceCode { get; set; }


        /// <summary>
        /// Номер подъезда
        /// </summary>
        public string? Delivery_EntranceNumber { get; set; }


        /// <summary>
        /// Номер этажа
        /// </summary>
        public string? Delivery_Floor { get; set; }


        /// <summary>
        /// Стоимость доставки
        /// </summary>
        public decimal? Delivery_Price { get; set; }


        /// <summary>
        /// Стоимость брони
        /// </summary>
        public decimal? Booking_Price { get; set; }


        /// <summary>
        /// Стол - Код
        /// </summary>
        [Comment("Стол - Код")]
        public int? Restaurant_TableID { get; set; }


        /// <summary>
        /// Дата начала брони
        /// </summary>
        [Comment("Дата начала брони")]
        public DateTime? Date_BookingStart { get; set; }


        /// <summary>
        /// Дата начала брони(long)
        /// </summary>
        public long? Date_BookingStart_Unix { get; set; }


        /// <summary>
        /// Дата окончания брони
        /// </summary>
        [Comment("Дата окончания брони")]
        public DateTime? Date_BookingEnd { get; set; }


        /// <summary>
        /// Дата окончания брони(long)
        /// </summary>
        public long? Date_BookingEnd_Unix { get; set; }


        /// <summary>
        /// Данные о товарах
        /// </summary>
        public List<COrder_Data> List_OrderData { get; set; } = null!;

    }
}