namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Категория меню
    /// </summary>
    public class CMenu_Category
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
        /// Код меню
        /// </summary>
        public int MenuID { get; set; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public int OrderNum { get; set; }
    }
}