namespace HIDE.RestAndRant.Web_Api.Models.v4.ZDevelopers
{
    /// <summary>
    /// Обновление версии и текста приложения
    /// </summary>
    public class Model_AppUpdateData_v4
    {
        /// <summary>
        /// Версия приложения
        /// </summary>
        public string Version { get; set; } = null!;


        /// <summary>
        /// Текст обновления
        /// </summary>
        public string Text { get; set; } = null!;

    }
}