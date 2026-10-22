using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель для отображения списка всех групп ингредиентов.
    /// </summary>
    public class Model_AllIngredientGroups
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт список групп ингредиентов.
        /// </summary>
        public List<CMenu_Ingridient_Group>? List_Groups { get; set; }

        #endregion
    }
}