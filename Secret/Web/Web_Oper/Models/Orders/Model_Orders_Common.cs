using HIDE.RestAndRant.Business.Definitions_ForAdmin;

namespace HIDE.RestAndRant.Web_Oper.Models.Orders
{
    /// <summary>
    /// Общая модель для заказов
    /// </summary>
    public class Model_Orders_Common
    {
        /// <summary>
        /// Список заказов
        /// </summary>
        public List<COrder_ForAdmin>? List_Orders { get; set; }

        /// <summary>
        /// Подробности заказа
        /// </summary>
        public Model_Details? Order_More { get; set; }

        /// <summary>
        /// Подробности доставки
        /// </summary>
        public Model_Order_Delivery? Order_Delivery { get; set; }

        /// <summary>
        /// Подробности бронирования
        /// </summary>
        public Model_Order_Booking? Order_Booking { get; set; }

        /// <summary>
        /// Статус заказа(на который нужно изменить) - Код
        /// </summary>
        public int? Order_StatusID { get; set; }

        /// <summary>
        /// Статус заказа(на который нужно изменить)
        /// </summary>
        public string? Order_Status { get; set; }

        /// <summary>
        /// Оплачено
        /// </summary>
        public bool Flag_Payed { get; set; }

        /// <summary>
        /// Код заказа для якоря
        /// </summary>
        public int? OrderID { get; set; }

    }
}