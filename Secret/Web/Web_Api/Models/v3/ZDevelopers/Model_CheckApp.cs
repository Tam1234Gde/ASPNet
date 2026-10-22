namespace HIDE.RestAndRant.Web_Api.Models.v3.ZDevelopers
{
    /// <summary>
    /// Проверка приложения
    /// </summary>
    public class Model_CheckApp
    {
        /// <summary>
        /// Отобразить модальное окно
        /// </summary>
        public bool ModalShow { get; set; }


        /// <summary>
        /// Заголовок в модальном окне
        /// </summary>
        public string? ModalTitle { get; set; }


        /// <summary>
        /// Текст в модальном окне
        /// </summary>
        public string? ModalText { get; set; }


        /// <summary>
        /// Текст кнопки в модальном окне
        /// </summary>
        public string? ModalButtonText { get; set; }


        /// <summary>
        /// Город
        /// </summary>
        public int? CityID { get; set; }

    }
}