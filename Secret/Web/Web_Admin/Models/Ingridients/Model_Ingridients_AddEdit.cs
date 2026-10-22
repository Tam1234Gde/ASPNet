using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Admin.Models.Ingridients
{
    /// <summary>
    /// Добавление\Изменение ингридиента
    /// </summary>
    public class Model_Ingridients_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование ингридиента
        /// </summary>
        [Required(ErrorMessage = "Поле 'Наименование' обязательно к заполнению")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Изображение
        /// </summary>
        public IFormFile? UploadImge { get; set; }

        /// <summary>
        /// Изображение товара
        /// </summary>
        public byte[]? Image { get; set; }

        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string? Image_Url { get; set; }

        /// <summary>
        /// ID категории
        /// </summary>
        public int? Category_ID { get; set; }

        /// <summary>
        /// Список категорий для выпадающего списка
        /// </summary>
        public List<SelectListItem>? Categories { get; set; }


    }
}
