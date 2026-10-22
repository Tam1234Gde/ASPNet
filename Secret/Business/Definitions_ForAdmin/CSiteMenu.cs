using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Меню сайта
    /// </summary>
    public class CSiteMenu
    {
        /// <summary>
        /// Код категории
        /// </summary>
        [DisplayName("Код категории")]
        public int CategoryID { get; set; }


        /// <summary>
        /// Категория
        /// </summary>
        [DisplayName("Категория")]
        public string? Category { get; set; }


        /// <summary>
        /// Сортировка
        /// </summary>
        [DisplayName("Сортировка")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Список блюд
        /// </summary>
        [DisplayName("Список блюд")]
        public List<CDish_ForAdmin>? ListDishes { get; set; }
    }
}