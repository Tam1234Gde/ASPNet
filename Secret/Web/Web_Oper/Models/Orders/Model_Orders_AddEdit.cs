using System.ComponentModel;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Web_Oper.Models.Orders
{
    /// <summary>
    /// Добавление\Изменение заказа
    /// </summary>
    public class Model_Orders_AddEdit
    {
        /// <summary>
        /// Блюдо
        /// </summary>
        public CDish? Dish { get; set; }

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
        public int? ClientID { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        public string? Client { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        [DisplayName("Телефон")]
        public string? Phone { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [DisplayName("Телефон")]
        public string? Name { get; set; }

        /// <summary>
        /// Клиент - код
        /// </summary>
        public int? CategoryID { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        public string? Category { get; set; }

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
        public virtual Pay_Type? Pay_Type { get; set; }

        /// <summary>
        /// Ресторан, куда совершается заказ
        /// </summary>
        public int RestaurantID { get; set; }

        /// <summary>
        /// Ресторан, куда совершается заказ
        /// </summary>
        public virtual Restaurant? Restaurant { get; set; }

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
        /// Итоговая цена
        /// </summary>
        public decimal Sum_Itog { get; set; }

        /// <summary>
        /// Список категорий блюд
        /// </summary>
        public List<CDish_Category>? List_Categories { get; set; }

        /// <summary>
        /// Список блюд
        /// </summary>
        public List<CDish_ForAdmin>? List_Dishes { get; set; }

        /// <summary>
        /// Список блюд 2
        /// </summary>
        public List<COrder_Data>? List_Dishes2 { get; set; }

        /// <summary>
        /// Список клиентов
        /// </summary>
        public List<CClient>? List_Clients { get; set; }

    }
}