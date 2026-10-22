using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель для списка ингредиентов.
    /// </summary>
    public class Model_Menu_Ingredient_List
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт список ингредиентов.
        /// </summary>
        public List<CMenu_Ingridient>? List_Ingredients { get; set; }


        /// <summary>
        /// Получает или задаёт идентификатор меню.
        /// </summary>
        public int MenuID { get; set; }


        /// <summary>
        /// Получает или задаёт наименование меню.
        /// </summary>
        public string MenuName { get; set; } = null!;

        #endregion
    }
}