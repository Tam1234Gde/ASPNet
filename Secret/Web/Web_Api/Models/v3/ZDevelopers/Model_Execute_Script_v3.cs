namespace HIDE.RestAndRant.Web_Api.Models.v3.ZDevelopers
{
    /// <summary>
    /// Выполнить скрипт
    /// </summary>
    public class Model_Execute_Script_v3
    {
        /// <summary>
        /// Guid
        /// </summary>
        public string? Guid { get; set; }


        /// <summary>
        /// Запрос
        /// </summary>
        public string? SqlText { get; set; }


        /// <summary>
        /// Файл
        /// </summary>
        public IFormFile? File { get; set; }

    }
}
