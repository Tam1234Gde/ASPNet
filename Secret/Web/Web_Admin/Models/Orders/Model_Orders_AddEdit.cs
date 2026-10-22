using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Orders
{
    /// <summary>
    /// Добавление\Изменение заказа
    /// </summary>
    public class Model_Orders_AddEdit
    {
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
        /// Список типов оплаты
        /// </summary>
        public List<CCommon>? List_Pay_Types { get; set; }

    }
}
