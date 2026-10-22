using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Admin.Models.Volumes
{
    /// <summary>
    /// Добавление\Редактирование
    /// </summary>
    public class Model_Volumes_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Объём
        /// </summary>
        [DisplayName("Объём")]
        [Required(ErrorMessage = "Поле 'Объём' обязательно к заполнению")]
        public decimal Value { get; set; }


        /// <summary>
        /// Объём
        /// </summary>
        [DisplayName("Объём")]
        public string? Name { get; set; }

    }
}