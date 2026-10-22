using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Oper.Models.Dish_Categories
{
    /// <summary>
    /// Добавление\Изменение категории блюд
    /// </summary>
    public class Model_Dish_Categories_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование категории
        /// </summary>
        [Required(ErrorMessage = "Поле 'Наименование категории' обязательно к заполнению")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int OrderNum { get; set; }

    }
}