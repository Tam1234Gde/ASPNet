namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Группа ингредиентов меню
    /// </summary>
    public class CMenu_Ingridient_Group
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
        /// На выбор одна
        /// </summary>
        public bool IsSelectable { get; set; }

        /// <summary>
        /// Цена по умолчанию для новых ингредиентов группы
        /// </summary>
        public int? Default_Price { get; set; }

        /// <summary>
        /// Код меню
        /// </summary>
        public int MenuID { get; set; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// Код состояния активности
        /// </summary>
        public int Menu_OptionStateID { get; set; }
    }
}