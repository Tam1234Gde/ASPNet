using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель для добавления и редактирования меню.
    /// </summary>
    public class Model_Menu_AddEdit
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт идентификатор записи.
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Получает или задаёт список доступных состояний активности.
        /// </summary>
        public List<CMenu_OptionState>? List_Menu_OptionStates { get; set; }


        /// <summary>
        /// Получает или задаёт идентификатор состояния активности.
        /// </summary>
        [Required(ErrorMessage = "Выберите состояние")]
        [Display(Name = "Состояние")]
        public int Menu_OptionStateID { get; set; }


        /// <summary>
        /// Получает или задаёт наименование меню.
        /// </summary>
        [Required(ErrorMessage = "Введите наименование")]
        [Display(Name = "Наименование")]
        public string Name { get; set; } = null!;

        #endregion
    }
}