namespace HIDE.RestAndRant.Web_Api.Models.v3.ZDevelopers
{
    /// <summary>
    /// Модель данных для записи
    /// </summary>
    public class Model_Logs_Write_v3
    {
        /// <summary>
        /// Сообщение для записи
        /// </summary>
        public string Message { get; set; } = null!;


        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int? UserId { get; set; }


        /// <summary>
        /// <para>Уровень лога</para>
        /// <para>1 - Debug</para>
        /// <para>2 - Information</para>
        /// <para>3 - Warning</para>
        /// <para>4 - Error</para>
        /// <para>5 - Critical</para>
        /// </summary>
        public int LogLevel { get; set; }

    }
}