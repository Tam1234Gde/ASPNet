using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель для добавления и редактирования привязки меню к ресторану.
    /// </summary>
    public class Model_Menu_Restaurant_AddEdit
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт дату начала показа.
        /// </summary>
        [Display(Name = "Дата начала")]
        public DateTime? DateFrom { get; set; }


        /// <summary>
        /// Получает или задаёт дату окончания показа.
        /// </summary>
        [Display(Name = "Дата окончания")]
        public DateTime? DateTo { get; set; }


        /// <summary>
        /// Получает или задаёт идентификатор записи.
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Получает или задаёт список состояний активности.
        /// </summary>
        public List<CMenu_OptionState>? List_Menu_OptionStates { get; set; }


        /// <summary>
        /// Получает или задаёт список ресторанов.
        /// </summary>
        public List<CRestaurant>? List_Restaurants { get; set; }


        /// <summary>
        /// Получает или задаёт идентификатор меню.
        /// </summary>
        public int MenuID { get; set; }


        /// <summary>
        /// Получает или задаёт наименование меню.
        /// </summary>
        public string MenuName { get; set; } = null!;


        /// <summary>
        /// Получает или задаёт идентификатор состояния активности.
        /// </summary>
        [Required(ErrorMessage = "Выберите состояние")]
        [Display(Name = "Состояние")]
        public int Menu_OptionStateID { get; set; }


        /// <summary>
        /// Получает или задаёт идентификатор ресторана.
        /// </summary>
        [Required(ErrorMessage = "Выберите ресторан")]
        [Display(Name = "Ресторан")]
        public int RestaurantID { get; set; }

        #endregion
    }
}