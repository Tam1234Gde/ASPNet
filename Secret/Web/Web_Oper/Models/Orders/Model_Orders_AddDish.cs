using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Oper.Models.Orders
{
    /// <summary>
    /// Модель - Добавление блюда в заказ
    /// </summary>
    public class Model_Orders_AddDish
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Код блюда
        /// </summary>
        public int DishID { get; set; }

        /// <summary>
        /// Список ингредиентов
        /// </summary>
        public List<CDish_Ingridient>? List_Ingredients { get; set; }

        /// <summary>
        /// Объём блюда
        /// </summary>
        public int? VolumeID { get; set; }

        /// <summary>
        /// Количество блюд
        /// </summary>
        public int Dish_Count { get; set; }

        /// <summary>
        /// Список блюд
        /// </summary>
        public List<CDish>? List_Dishes { get; set; }

    }
}