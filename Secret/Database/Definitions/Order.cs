using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Заказ
    /// TODO: 
    /// KA - добавить тут виртуальное поле Order_Favorite
    /// KA - Сделать таблицу с журналом изменения статусов заказа
    /// </summary>
    [Table("Orders")]
    [Comment("Заказы")]
    public class Order
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Код избранного заказа
        /// </summary>
        [Comment("Код избранного заказа")]
        public int? Order_FavoriteID { get; set; }


        /// <summary>
        /// Дата заказа
        /// </summary>
        [Comment("Дата заказа")]
        public DateTime Date_Act { get; set; }


        /// <summary>
        /// Статус заказа - код
        /// </summary>
        [Comment("Статус заказа - код")]
        public EOrder_Status Order_StatusID { get; set; }


        /// <summary>
        /// Статус заказа
        /// </summary>
        [Comment("Статус заказа")]
        public Order_Status Order_Status { get; set; } = null!;


        /// <summary>
        /// Бронирование - Код
        /// </summary>
        [Comment("Бронирование - код")]
        public int? BookingID { get; set; }


        /// <summary>
        /// Бронирование
        /// </summary>
        [Comment("Бронирование")]
        public Booking? Booking { get; set; }


        /// <summary>
        /// Дата последнего изменения статуса заказа
        /// </summary>
        [Comment("Дата последнего изменения статуса заказа")]
        public DateTime Date_LastOrderStatusChange { get; set; }


        /// <summary>
        /// Клиент - Код
        /// </summary>
        [Comment("Клиент - Код")]
        public int ClientID { get; set; }


        /// <summary>
        /// Клиент
        /// </summary>
        [Comment("Клиент")]
        public Client Client { get; set; } = null!;


        /// <summary>
        /// Имя, на кого заказ
        /// </summary>
        [Comment("Имя, на кого заказ")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Телефон, на кого заказ
        /// </summary>
        [Comment("Телефон, на кого заказ")]
        public string Phone { get; set; } = null!;


        /// <summary>
        /// Вид оплаты - Код
        /// </summary>
        [Comment("Вид оплаты - Код")]
        public EPayType Pay_TypeID { get; set; }


        /// <summary>
        /// Вид оплаты
        /// </summary>
        [Comment("Вид оплаты")]
        public Pay_Type Pay_Type { get; set; } = null!;


        /// <summary>
        /// Тип заказа(С собой\Поесть в кафе\Доставка) - код
        /// </summary>
        [Comment("Тип заказа(С собой\\Поесть в кафе\\Доставка) - код")]
        public EOrder_Type Order_TypeID { get; set; }


        ///// <summary>
        ///// Тип заказа(С собой\Поесть в кафе\Доставка)
        ///// </summary>
        //[Comment("Тип заказа(С собой\\Поесть в кафе\\Доставка)")]
        //public virtual Order_Type Order_Type { get; set; } = null!;


        /// <summary>
        /// Ресторан, куда совершается заказ - Код
        /// </summary>
        [Comment("Ресторан, куда совершается заказ - Код")]
        public int RestaurantID { get; set; }


        /// <summary>
        /// Ресторан, куда совершается заказ
        /// </summary>
        [Comment("Ресторан, куда совершается заказ")]
        public Restaurant Restaurant { get; set; } = null!;


        /// <summary>
        /// Дата и время к которому приготовить заказ
        /// </summary>
        [Comment("Дата и время к которому приготовить заказ")]
        public DateTime? Date_To { get; set; }


        /// <summary>
        /// Флаг того, что клиент будет кушать в кафе, если false - то на вынос
        /// </summary>
        [Comment("Флаг того, что клиент будет кушать в кафе, если false - то на вынос")]
        public bool Flag_Inhouse { get; set; }


        /// <summary>
        /// Флаг того, что заказ оплачен
        /// </summary>
        [Comment("Флаг того, что заказ оплачен")]
        public bool Flag_Payed { get; set; }


        /// <summary>
        /// Дать сдачи с ...
        /// </summary>
        [Comment("Дать сдачи с ...")]
        [Precision(8, 2)]
        public decimal? Handing { get; set; }


        /// <summary>
        /// Операция по карте, прошедшая по этой подписке последней - код
        /// </summary>
        [Comment("Операция по карте, прошедшая по этой подписке последней - код")]
        public int? ClientPay_Card_PayID { get; set; }


        /// <summary>
        /// Операция по карте, прошедшая по этой подписке последней
        /// </summary>
        [Comment("Операция по карте, прошедшая по этой подписке последней")]
        [ForeignKey(nameof(ClientPay_Card_PayID))]
        public ClientPay_Card_Pay? ClientPay_Card_Pay { get; set; }


        /// <summary>
        /// Комментарий к заказу
        /// </summary>
        [Comment("Комментарий к заказу")]
        public string? Comment { get; set; }


        /// <summary>
        /// Добавить к заказу
        /// </summary>
        [Comment("Добавить к заказу")]
        public string? Comment_AddToOrder { get; set; }


        /// <summary>
        /// Убрать из заказа
        /// </summary>
        [Comment("Убрать из заказа")]
        public string? Comment_RemoveFromOrder { get; set; }


        /// <summary>
        /// Стоимость доставки
        /// </summary>
        [Comment("Стоимость доставки")]
        [Precision(8, 2)]
        public decimal? Delivery_Price { get; set; }


        /// <summary>
        /// Стоимость брони
        /// </summary>
        [Comment("Стоимость брони")]
        [Precision(8, 2)]
        public decimal? Booking_Price { get; set; }


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


        /// <summary>
        /// Адрес доставки
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Comment("Адрес доставки")]
        public string? Delivery_Address
        {
            get
            {
                return Order_Deliveries?.FirstOrDefault(p => !p.Flag_Del)?.Delivery_Address;
            }
        }


        /// <summary>
        /// Общая сумма товаров для заказа
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // Вычисляемое поле
        [Comment("Общая сумма товаров для заказа")]
        public decimal Order_Sum
        {
            get
            {
                return (Order_Datas?.Sum(p => p.Dish_Summ) + (Delivery_Price ?? 0)) ?? 0;
            }
        }


        /// <summary>
        /// Доставка
        /// </summary>
        [Comment("Доставка")]
        public List<Order_Delivery>? Order_Deliveries { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable


        /// <summary>
        /// Детали заказа
        /// </summary>
        [Comment("Детали заказа")]
        public List<Order_Data>? Order_Datas { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable

    }
}