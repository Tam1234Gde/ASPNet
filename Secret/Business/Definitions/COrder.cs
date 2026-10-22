using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json.Serialization;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class COrder
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Код избранного заказа
        /// </summary>
        public int? Order_FavoriteID { get; set; }


        /// <summary>
        /// Клиент - код
        /// </summary>
        public int ClientID { get; set; }


        /// <summary>
        /// Статус заказа - код
        /// </summary>
        public EOrder_Status Order_StatusID { get; set; }


        /// <summary>
        /// Тип заказа(С собой\Поесть в кафе\Доставка\Вынести к машине) - код
        /// </summary>
        [Comment("Тип заказа(С собой\\Поесть в кафе\\Доставка\\Вынести к машине) - код")]
        public EOrder_Type Order_TypeID { get; set; }


        /// <summary>
        /// Бронирование - Код
        /// </summary>
        [Comment("Бронирование - код")]
        public int? BookingID { get; set; }


        /// <summary>
        /// Бронирование
        /// </summary>
        [Comment("Бронирование")]
        public CBooking? Booking { get; set; }


        /// <summary>
        /// Дата и время заказа
        /// </summary>
        [DisplayName("Дата и время заказа")]
        public DateTime? Date_Act { get; set; }


        /// <summary>
        /// Дата последнего изменения статуса заказа
        /// </summary>
        [DisplayName("Дата последнего изменения статуса заказа")]
        [JsonIgnore]
        public DateTime Date_LastOrderStatusChange { get; set; }


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
        /// Ресторан, куда совершается заказ
        /// </summary>
        public int RestaurantID { get; set; }


        /// <summary>
        /// Дата и время к которому приготовить заказ
        /// </summary>
        [DisplayName("Дата и время выполнения заказа")]
        public DateTime? Date_To { get; set; }


        /// <summary>
        /// Флаг того, что клиент будет кушать в кафе, если false - то на вынос
        /// </summary>
        public bool Flag_Inhouse { get; set; }


        /// <summary>
        /// Флаг того, что заказ оплачен
        /// </summary>
        public bool Flag_Payed { get; set; }


        /// <summary>
        /// Дать сдачи с ...
        /// </summary>
        [Comment("Дать сдачи с ...")]
        public decimal? Handing { get; set; }


        /// <summary>
        /// Стоимость доставки
        /// </summary>
        [Comment("Стоимость доставки")]
        public decimal? Delivery_Price { get; set; }


        /// <summary>
        /// Адрес доставки
        /// </summary>
        [DisplayName("Адрес доставки")]
        public string? Delivery_Address { get; set; }


        /// <summary>
        /// Стоимость брони
        /// </summary>
        [Comment("Стоимость брони")]
        public decimal? Booking_Price { get; set; }


        /// <summary>
        /// Госномера машины
        /// </summary>
        [DisplayName("Госномера машины")]
        public string? Car_RegNumber { get; set; }


        /// <summary>
        /// Детали заказа
        /// </summary>
        public List<COrder_Data>? List_OrderDatas { get; set; }


        ///// <summary>
        ///// Детали доставки
        ///// </summary>
        //public COrder_Delivery? Order_Delivery { get; set; }

    }
}