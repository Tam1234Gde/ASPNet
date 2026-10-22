using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    public class Model_Menu_Ingredient_Group_AddEdit
    {
        public int ID { get; set; }

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Укажите наименование")]
        public string Name { get; set; } = null!;

        [Display(Name = "На выбор одна")]
        public bool IsSelectable { get; set; }

        [Display(Name = "Цена по умолчанию")]
        public int? Default_Price { get; set; }

        [Display(Name = "Порядок сортировки")]
        public int OrderNum { get; set; }

        [Display(Name = "Состояние")]
        [Required(ErrorMessage = "Укажите состояние")]
        public int Menu_OptionStateID { get; set; }

        public List<CMenu_OptionState>? List_Menu_OptionStates { get; set; }
    }
}