using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Dish_Ingridients
{
    /// <summary>
    /// Добавление\Изменение ингридиента для блюда
    /// </summary>
    public class Model_Dish_Ingridients_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Блюдо
        /// </summary>
        public int DishID { get; set; }

        /// <summary>
        /// Блюдо
        /// </summary>
        public string? Dish { get; set; }

        /// <summary>
        /// Ингридиент
        /// </summary>
        public int IngridientID { get; set; }

        /// <summary>
        /// Ингридиент
        /// </summary>
        public string? Ingridient { get; set; }

        /// <summary>
        /// Количество минут для приготовления
        /// </summary>
        public int? Cooking_Minutes { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public int? IngridientGroupID { get; set; }

        /// <summary>
        /// Флаг - Группа
        /// </summary>
        public bool Flag_IsGroup { get; set; }

        /// <summary>
        /// Изменить цену для всех ресторанов
        /// </summary>
        public bool FlagForAll { get; set; }

        /// <summary>
        /// Список ингридиентов
        /// </summary>
        public List<CIngridient>? List_Ingridients { get; set; }

    }
}
