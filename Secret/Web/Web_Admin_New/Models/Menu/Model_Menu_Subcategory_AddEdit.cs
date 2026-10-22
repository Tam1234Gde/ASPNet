using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель добавления/редактирования подкатегории меню
    /// </summary>
    public class Model_Menu_Subcategory_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Укажите наименование")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Код категории (родитель)
        /// </summary>
        public int Menu_CategoryID { get; set; }

        /// <summary>
        /// Наименование категории (только для отображения)
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Код родительской подкатегории (если есть)
        /// </summary>
        [Display(Name = "Родитель")]
        public int? ParentID { get; set; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        [Display(Name = "Порядок сортировки")]
        public int OrderNum { get; set; }

        /// <summary>
        /// Список доступных родителей
        /// </summary>
        public List<CMenu_Subcategory>? List_Parents { get; set; }
    }
}