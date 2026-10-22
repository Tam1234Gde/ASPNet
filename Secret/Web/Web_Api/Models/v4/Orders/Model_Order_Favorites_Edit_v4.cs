using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.ENums;
namespace HIDE.RestAndRant.Web_Api.Models.v4.Orders
{
    /// <summary>
    /// Редактирование избранного заказа
    /// </summary>
    public class Model_Order_Favorites_Edit_v4
    {
        /// <summary>
        /// Код 
        /// </summary>
        public int ID { get; set; }


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
        /// Ресторан, куда совершается заказ
        /// </summary>
        public int RestaurantID { get; set; }


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
        /// Данные о товарах
        /// </summary>
        public List<COrder_Favorite_Data> List_Order_Favorite_Datas { get; set; } = null!;

    }
}