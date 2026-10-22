using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Site_Menu.Models.Offer
{
    /// <summary>
    /// Оформление заказа
    /// </summary>
    public class Model_Offer_Index
    {
        /// <summary>
        /// Ресторан
        /// </summary>
        public CRestaurant? Restaurant { get; set; }


        /// <summary>
        /// Заказ ожидает вас через ...
        /// </summary>
        public int Cooking_Ready { get; set; }


        /// <summary>
        /// Количество блюд
        /// </summary>
        public int Dish_Count { get; set; }


        /// <summary>
        /// Итоговая цена
        /// </summary>
        public decimal? Sum { get; set; }


        /// <summary>
        /// Список типов оплаты
        /// </summary>
        public List<CCommon>? List_PayTypes { get; set; }


        /// <summary>
        /// Клиент
        /// </summary>
        public CClient? Client { get; set; }

    }
}