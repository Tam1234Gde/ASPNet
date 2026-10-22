using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель страницы "Все категории"
    /// </summary>
    public class Model_AllCategories
    {
        /// <summary>
        /// Выбранный фильтр по меню
        /// </summary>
        public int? MenuID { get; set; }

        /// <summary>
        /// Наименование выбранного меню
        /// </summary>
        public string? MenuName { get; set; }

        /// <summary>
        /// Показывать удалённые
        /// </summary>
        public bool ShowDeleted { get; set; }

        /// <summary>
        /// Список меню (для фильтра и кнопки "Добавить")
        /// </summary>
        public List<CMenu>? List_Menus { get; set; }

        /// <summary>
        /// Список категорий
        /// </summary>
        public List<CMenu_Category>? List_Categories { get; set; }
    }
}