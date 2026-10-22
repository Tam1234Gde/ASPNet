using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Избранный заказ
    /// </summary>
    [Table("Order_Favorites")]
    [Comment("Избранные заказы")]
    public class Order_Favorite
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Дата добавления заказа в избранное
        /// </summary>
        [Comment("Дата добавления заказа в избранное")]
        public DateTime Date_Act { get; set; }


        /// <summary>
        /// Дата последнего изменения избранного заказа
        /// </summary>
        [Comment("Дата последнего изменения избранного заказа")]
        public DateTime Date_LastEdit { get; set; }


        /// <summary>
        /// Статус заказа - Код
        /// </summary>
        [Comment("Статус заказа - Код")]
        public EOrder_Status Order_StatusID { get; set; }


        /// <summary>
        /// Статус заказа
        /// </summary>
        [Comment("Статус заказа")]
        public Order_Status Order_Status { get; set; } = null!;


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
        /// Детали заказа
        /// </summary>
        [Comment("Детали заказа")]
        public List<Order_Favorite_Data>? Order_Datas { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable

    }
}