using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Товар, блюдо
    /// </summary>
    public class CDish_ForAdmin : CDish_Base_ForAdmin
    {
        /// <summary>
        /// Список адресов ресторанов в которых есть блюдо
        /// </summary>
        [DisplayName("Адреса ресторанов")]
        public List<string>? List_Adress_Restoran { get; set; }


        /// <summary>
        /// Список адресов ресторанов в которых есть блюдо (для вывода в админке)
        /// </summary>
        [DisplayName("Адреса ресторанов (вывод)")]
        public string? Adress_Restoran_Output { get; set; }


        /// <summary>
        /// Дата начала действия новинки
        /// </summary>
        [DisplayName("Дата начала новинки")]
        public DateTime? New_DateFrom { get; set; }
    }
}