using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель страницы "Все ингредиенты"
    /// </summary>
    public class Model_AllIngredients
    {
        /// <summary>
        /// Выбранный фильтр по коду группы ингредиентов
        /// </summary>
        public int? Menu_Ingridient_GroupID { get; set; }

        /// <summary>
        /// Наименование выбранной группы (для отображения в фильтре)
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// Список групп ингредиентов (для выпадающего фильтра)
        /// </summary>
        public List<CMenu_Ingridient_Group>? List_Groups { get; set; }

        /// <summary>
        /// Список ингредиентов
        /// </summary>
        public List<CMenu_Ingridient>? List_Ingredients { get; set; }
    }
}