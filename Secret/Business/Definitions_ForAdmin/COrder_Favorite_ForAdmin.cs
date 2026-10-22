using System.ComponentModel;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Избранный заказ
    /// </summary>
    public class COrder_Favorite_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код заказа")]
        public int ID { get; set; }


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
        [DisplayName("Дата и время выполнения")]
        public DateTime? Date_To { get; set; }


        /// <summary>
        /// Флаг того, что клиент будет кушать в кафе, если false - то на вынос
        /// </summary>
        [DisplayName("Кушает в кафе")]
        public bool Flag_Inhouse { get; set; }


        /// <summary>
        /// Флаг того, что заказ оплачен
        /// </summary>
        [DisplayName("Заказ оплачен")]
        public bool Flag_Payed { get; set; }


        /// <summary>
        /// Детали заказа
        /// </summary>
        [DisplayName("Детали заказа")]
        public List<COrder_Favorite_Data_ForAdmin>? List_OrderDatas { get; set; }
    }
}