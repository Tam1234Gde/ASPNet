using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Товар, блюдо (2 итерация)
    /// </summary>
    public class CDish_ForAdmin2 : CDish_Base_ForAdmin
    {
        /// <summary>
        /// Список ресторанов в которых есть это блюдо
        /// </summary>
        [DisplayName("Рестораны")]
        public List<Restaurant>? List_Restaurants { get; set; }
    }
}