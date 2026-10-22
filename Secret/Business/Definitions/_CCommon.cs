using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Общий класс для типов "наименование - код"
    /// </summary>
    public class CCommon
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DisplayName("Наименование")]
        public string Name { get; set; } = "";

    }
}