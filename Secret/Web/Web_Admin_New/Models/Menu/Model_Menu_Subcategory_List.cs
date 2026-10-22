using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель для списка подкатегорий конкретной категории.
    /// </summary>
    public class Model_Menu_Subcategory_List
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт наименование категории.
        /// </summary>
        public string CategoryName { get; set; } = null!;


        /// <summary>
        /// Получает или задаёт список подкатегорий.
        /// </summary>
        public List<CMenu_Subcategory>? List_Menu_Subcategories { get; set; }


        /// <summary>
        /// Получает или задаёт идентификатор категории.
        /// </summary>
        public int Menu_CategoryID { get; set; }

        #endregion
    }
}