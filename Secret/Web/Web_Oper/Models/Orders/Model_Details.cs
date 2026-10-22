using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Web_Oper.Models.Orders
{
    /// <summary>
    /// Детали заказа
    /// </summary>
    public class Model_Details
    {
        public COrder? Order { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Дата заказа
        /// </summary>
        public DateTime Date_Act { get; set; }

        /// <summary>
        /// Клиент - код
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        public string? Client { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string? Client_Phone { get; set; }

        /// <summary>
        /// Токен клиента для отправки ему уведомлений
        /// </summary>
        public string? ClientToken { get; set; }

        /// <summary>
        /// Вид оплаты - код
        /// </summary>
        public int Pay_TypeID { get; set; }

        /// <summary>
        /// Вид оплаты
        /// </summary>
        public string? Pay_Type { get; set; }

        /// <summary>
        /// Ресторан, куда совершается заказ
        /// </summary>
        public int RestaurantID { get; set; }

        /// <summary>
        /// Ресторан, куда совершается заказ
        /// </summary>
        public string? Restaurant { get; set; }

        /// <summary>
        /// Дата и время к которому приготовить заказ
        /// </summary>
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
        /// Комментарий
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Добавить к заказу
        /// </summary>
        public string? Comment_AddToOrder { get; set; }

        /// <summary>
        /// Убрать из заказа
        /// </summary>
        public string? Comment_RemoveFromOrder { get; set; }

        /// <summary>
        /// Детали заказа
        /// </summary>
        public virtual List<Order_Data>? Order_Datas { get; set; }

    }
}
