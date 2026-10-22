using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Ingridients
{
    /// <summary>
    /// Ингредиент > Изменить стоимость для ресторана
    /// </summary>
    public class Model_Ingredients_PriceChange
    {
        /// <summary>
        /// Код ингредиента
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Название ингредиента
        /// </summary>
        public string? Name { get; set; }


        /// <summary>
        /// Стоимость блюда
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// Выбранный ресторан
        /// </summary>
        public int RestaurantID { get; set; }


        /// <summary>
        /// Список ресторанов
        /// </summary>
        public List<CRestaurant>? ListRestaurants { get; set; }

    }
}