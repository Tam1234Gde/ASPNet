using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель для списка блюд конкретной подкатегории.
    /// </summary>
    public class Model_Menu_Dish_List
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт список блюд.
        /// </summary>
        public List<CMenu_Dish>? List_Menu_Dishes { get; set; }


        /// <summary>
        /// Получает или задаёт идентификатор подкатегории.
        /// </summary>
        public int Menu_SubcategoryID { get; set; }


        /// <summary>
        /// Получает или задаёт наименование подкатегории.
        /// </summary>
        public string SubcategoryName { get; set; } = null!;

        #endregion
    }
}