using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Oper.Models.Dishes
{
    /// <summary>
    /// Добавление\Изменение блюда
    /// </summary>
    public class Model_Dishes_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Категория блюда - код
        /// </summary>
        [Required(ErrorMessage = "Поле 'Категория блюда' обязательно к заполнению")]
        public int Dish_CategoryID { get; set; }

        /// <summary>
        /// Категория блюда
        /// </summary>
        public string? Dish_Category { get; set; }

        /// <summary>
        /// Наименование товара
        /// </summary>
        [Required(ErrorMessage = "Поле 'Наименование категории' обязательно к заполнению")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Стоимость
        /// </summary>
        [Required(ErrorMessage = "Поле 'Стоимость' обязательно к заполнению")]
        public decimal Price { get; set; }

        /// <summary>
        /// Флаг того, что этот товар кофе и для него нужно подгрузить шапочку напитка и менять цвет стакана
        /// </summary>
        public bool Flag_AllowColor { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// Список категорий блюд
        /// </summary>
        public List<CDish_Category>? List_Categories { get; set; }

    }
}