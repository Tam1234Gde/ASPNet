namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Меню
    /// </summary>
    public class CMenu
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
        /// Код состояния активности
        /// </summary>
        public int Menu_OptionStateID { get; set; }
    }
}