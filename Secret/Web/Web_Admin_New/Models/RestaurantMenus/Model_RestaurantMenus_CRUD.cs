using System.ComponentModel;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.RestaurantMenus
{
    /// <summary>
    /// Меню ресторана
    /// </summary>
    public class Model_RestaurantMenus_CRUD
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Код ресторана
        /// </summary>
        [DisplayName("Код ресторана")]
        public int RestaurantID { get; set; }


        /// <summary>
        /// Ресторан
        /// </summary>
        [DisplayName("Ресторан")]
        public virtual Restaurant? Restaurant { get; set; }


        /// <summary>
        /// Название меню
        /// </summary>
        [DisplayName("Название меню")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Активен
        /// </summary>
        [DisplayName("Активен")]
        public bool Flag_Active { get; set; }

    }
}