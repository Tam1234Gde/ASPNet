namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Ингредиент блюда меню
    /// </summary>
    public class CMenu_Dish_Ingridient
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Код блюда
        /// </summary>
        public int Menu_DishID { get; set; }

        /// <summary>
        /// Код ингредиента
        /// </summary>
        public int Menu_IngridientID { get; set; }
    }
}