using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель для списка всех меню.
    /// </summary>
    public class Model_Menu_List
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт список меню.
        /// </summary>
        public List<CMenu>? List_Menu { get; set; }

        #endregion
    }
}