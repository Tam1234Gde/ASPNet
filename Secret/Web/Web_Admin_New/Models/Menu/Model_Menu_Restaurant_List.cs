using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель для списка привязок меню к ресторанам.
    /// </summary>
    public class Model_Menu_Restaurant_List
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт список привязок.
        /// </summary>
        public List<CMenu_Restaurant>? List_Menu_Restaurants { get; set; }


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