using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;

namespace HIDE.RestAndRant.Business.Helpers
{
    /// <summary>
    /// Помощник формирования почт
    /// </summary>
    public class Helper_Email
    {
        private readonly ILogger _log;
        private const string LOG_TEMPLATE_SEND_EMAIL = "EmailTo='{0}' Theme='{1}' Text='{2}' Flag_Html='{3}' ";
        private const string LOG_WARNING_SEND_FAILED = "Не удалось выполнить отправку почты ## {0}";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Helper_Email()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Отправка письма
        /// </summary>
        /// <param name="EmailTo">Кому отправить</param>
        /// <param name="Theme">Тема</param>
        /// <param name="Text">Текст</param>
        /// <param name="Flag_Html">True - Html</param>
        /// <returns>True - успешно, False - ошибка</returns>
        public bool? SendEmail(string EmailTo, string Theme, string Text, bool Flag_Html)
        {
            string loginfo = string.Format(LOG_TEMPLATE_SEND_EMAIL, EmailTo, Theme, Text, Flag_Html);

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug("{0}", loginfo);
            }

            bool result;

            if (AppConfig.Mail_Send)
            {
                result = Helper_MailSender.SendMail(MailTo: EmailTo, Message_Theme: Theme, Message_Text: Text, Message_IsHtml: Flag_Html);
            }
            else
            {
                result = true;
            }

            if (!result)
            {
                _log.LogWarning(LOG_WARNING_SEND_FAILED, loginfo);
            }

            return result;
        }
    }
}