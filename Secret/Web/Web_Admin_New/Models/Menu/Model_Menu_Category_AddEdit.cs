using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель добавления/редактирования категории меню
    /// </summary>
    public class Model_Menu_Category_AddEdit
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
        /// Код меню (родитель)
        /// </summary>
        public int MenuID { get; set; }

        /// <summary>
        /// Наименование меню (только для отображения в заголовке формы)
        /// </summary>
        public string? MenuName { get; set; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        [Display(Name = "Порядок сортировки")]
        public int OrderNum { get; set; }
    }
}