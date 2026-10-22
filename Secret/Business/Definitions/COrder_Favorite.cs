using System.ComponentModel;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class COrder_Favorite
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Клиент - код
        /// </summary>
        public int ClientID { get; set; }


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
        /// Детали заказа
        /// </summary>
        public List<COrder_Favorite_Data>? List_OrderDatas { get; set; }

    }
}
