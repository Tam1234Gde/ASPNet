using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace HIDE.RestAndRant.Web_Admin.Models.OrderStatuses
{
    /// <summary>
    /// CRUD - Статусы заказов
    /// </summary>
    public class Model_OrderStatuses_CRUD
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