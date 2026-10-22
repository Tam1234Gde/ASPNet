using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель страницы "Все блюда"
    /// </summary>
    public class Model_AllDishes
    {
        /// <summary>
        /// Выбранный фильтр по подкатегории
        /// </summary>
        public int? Menu_SubcategoryID { get; set; }

        /// <summary>
        /// Наименование выбранной подкатегории
        /// </summary>
        public string? SubcategoryName { get; set; }

        /// <summary>
        /// Показывать удалённые
        /// </summary>
        public bool ShowDeleted { get; set; }

        /// <summary>
        /// Список подкатегорий (для фильтра и кнопки "Добавить")
        /// </summary>
        public List<CMenu_Subcategory>? List_Subcategories { get; set; }

        /// <summary>
        /// Список блюд
        /// </summary>
        public List<CMenu_Dish>? List_Dishes { get; set; }
    }
}