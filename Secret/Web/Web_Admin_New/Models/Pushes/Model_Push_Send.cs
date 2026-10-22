using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Pushes
{
    /// <summary>
    /// Отправка пушей с файла
    /// </summary>
    public class Model_Push_Send
    {
        /// <summary>
        /// Заголовок пуша
        /// </summary>
        [Required(ErrorMessage = "Поле 'Заголовок' обязательно к заполнению")]
        public string Title { get; set; } = null!;


        /// <summary>
        /// Текст пуша
        /// </summary>
        [Required(ErrorMessage = "Поле 'Текст' обязательно к заполнению")]
        public string Text { get; set; } = null!;


        /// <summary>
        /// Дата пуша
        /// </summary>
        public DateTime? Date { get; set; }


        /// <summary>
        /// Файл с клиентами
        /// </summary>
        //[Required(ErrorMessage = "Поле 'Файл' обязательно к заполнению")]
        public IFormFile? File { get; set; } = null!;


        /// <summary>
        /// Информационное сообщение
        /// </summary>
        public string? Info { get; set; }

    }
}