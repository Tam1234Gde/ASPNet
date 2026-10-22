using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Cities
{
    /// <summary>
    /// Города
    /// </summary>
    public class Model_Cities_CRUD
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Название города
        /// </summary>
        [DisplayName("Название города")]
        [StringLength(40)]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Название города(склонение)
        /// </summary>
        [Comment("Название города(склонение)")]
        [StringLength(40)]
        public string? NameDeclension { get; set; }


        /// <summary>
        /// Порядок отображения
        /// </summary>
        [DisplayName("Порядок отображения")]
        public int OrderNum { get; set; }


        /// <summary>
        /// True - Отключен
        /// </summary>
        [DisplayName("Отключен")]
        public bool Flag_Hidden { get; set; }


        /// <summary>
        /// Заголовок сообщения при отключенном ресторане
        /// </summary>
        [DisplayName("Заголовок сообщения при отключенном ресторане")]
        public string? HiddenTitle { get; set; }


        /// <summary>
        /// Тело сообщения при отключенном ресторане
        /// </summary>
        [DisplayName("Тело сообщения при отключенном ресторане")]
        public string? HiddenText { get; set; }

    }
}