namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Опция меню ресторана
    /// </summary>
    public class CMenu_Restaurant_Option
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Код привязки меню к ресторану
        /// </summary>
        public int? Menu_RestaurantID { get; set; }

        /// <summary>
        /// Код блюда
        /// </summary>
        public int? Menu_DishID { get; set; }

        /// <summary>
        /// Код ингредиента
        /// </summary>
        public int? Menu_IngridientID { get; set; }

        /// <summary>
        /// Код группы ингредиентов
        /// </summary>
        public int? Menu_Ingridient_GroupID { get; set; }

        /// <summary>
        /// Код меню
        /// </summary>
        public int MenuID { get; set; }
    }
}