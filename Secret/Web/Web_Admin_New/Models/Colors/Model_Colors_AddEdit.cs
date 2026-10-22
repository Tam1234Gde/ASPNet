using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Colors
{
    /// <summary>
    /// Добавление\Изменение цвета
    /// </summary>
    public class Model_Colors_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование цвета
        /// </summary>
        [Required(ErrorMessage = "Поле 'Наименование цвета' обязательно к заполнению")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Наименование цвета - англ
        /// </summary>
        [Required(ErrorMessage = "Поле 'Наименование цвета - EN' обязательно к заполнению")]
        public string Name_Eng { get; set; } = null!;

        /// <summary>
        /// Цвет стакана
        /// </summary>
        [Required(ErrorMessage = "Поле 'Цвет стакана' обязательно к заполнению")]
        public string HexColor { get; set; } = null!;

    }
}
