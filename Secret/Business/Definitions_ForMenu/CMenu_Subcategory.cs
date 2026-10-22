namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Подкатегория меню
    /// </summary>
    public class CMenu_Subcategory
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Код категории
        /// </summary>
        public int Menu_CategoryID { get; set; }

        /// <summary>
        /// Код родителя
        /// </summary>
        public int? ParentID { get; set; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public int OrderNum { get; set; }
    }
}