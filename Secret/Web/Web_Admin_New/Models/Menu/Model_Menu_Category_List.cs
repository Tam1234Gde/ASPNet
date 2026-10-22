using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель для списка категорий конкретного меню.
    /// </summary>
    public class Model_Menu_Category_List
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт список категорий меню.
        /// </summary>
        public List<CMenu_Category>? List_Menu_Categories { get; set; }


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