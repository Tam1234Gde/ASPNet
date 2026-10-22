using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель страницы "Все подкатегории"
    /// </summary>
    public class Model_AllSubcategories
    {
        /// <summary>
        /// Выбранный фильтр по категории
        /// </summary>
        public int? Menu_CategoryID { get; set; }

        /// <summary>
        /// Наименование выбранной категории
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Показывать удалённые
        /// </summary>
        public bool ShowDeleted { get; set; }

        /// <summary>
        /// Список категорий (для фильтра и кнопки "Добавить")
        /// </summary>
        public List<CMenu_Category>? List_Categories { get; set; }

        /// <summary>
        /// Список подкатегорий
        /// </summary>
        public List<CMenu_Subcategory>? List_Subcategories { get; set; }
    }
}