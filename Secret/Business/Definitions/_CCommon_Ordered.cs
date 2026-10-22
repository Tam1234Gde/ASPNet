using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Общий класс для типов "наименование - код"
    /// </summary>
    public class CCommon_Ordered
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


        /// <summary>
        /// Порядок следования при отображении
        /// </summary>
        public int OrderNum { get; set; }


        /// <summary>
        /// Цвет
        /// </summary>
        [Comment("Цвет")]
        public string? ColorHex { get; set; }

    }
}