using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    public class Model_Menu_Ingredient_AddEdit
    {
        public int ID { get; set; }

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Укажите наименование")]
        public string Name { get; set; } = null!;

        public byte[]? Image { get; set; }

        [Display(Name = "Изображение")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Цена")]
        public int? Price { get; set; }

        [Display(Name = "Группа")]
        [Required(ErrorMessage = "Укажите группу")]
        public int Menu_Ingridient_GroupID { get; set; }

        [Display(Name = "Порядок сортировки")]
        public int OrderNum { get; set; }

        [Display(Name = "Состояние")]
        [Required(ErrorMessage = "Укажите состояние")]
        public int Menu_OptionStateID { get; set; }

        public List<CMenu_Ingridient_Group>? List_Groups { get; set; }
        public List<CMenu_OptionState>? List_Menu_OptionStates { get; set; }
    }
}