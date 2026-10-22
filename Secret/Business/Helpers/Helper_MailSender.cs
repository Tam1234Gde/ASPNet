using HIDE.RestAndRant.Common;
using HIDE.SendMails_Core;

namespace HIDE.RestAndRant.Business.Helpers
{
    /// <summary>
    /// Помощник отправки почты
    /// </summary>
    public static class Helper_MailSender
    {
        private const bool DEFAULT_DEBUG_MODE = true;


        /// <summary>
        /// Отправить почту
        /// </summary>
        /// <param name="MailTo">Email кому отправляем</param>
        /// <param name="Message_Theme">Тема письма</param>
        /// <param name="Message_Text">Тело письма</param>
        /// <param name="Message_IsHtml">Флаг того, что тело письма это HTML</param>
        /// <returns>True - успешно, False - ошибка</returns>
        public static bool SendMail(string MailTo, string Message_Theme, string Message_Text, bool Message_IsHtml)
        {
            SendMail sendmail;

#if DEBUG
            sendmail = new SendMail(
                Smtp_Host: AppConfig.Mail_SMTP_Host,
                Smtp_Port: AppConfig.Mail_SMTP_Port,
                Smtp_Mailfrom: AppConfig.Mail_MailFrom,
                Smtp_NameFrom: AppConfig.Mail_MailFrom,
                Smtp_User: AppConfig.Mail_SMTP_User,
                Smtp_Password: AppConfig.Mail_SMTP_Passwd,
                Smtp_EnableSSL: AppConfig.Mail_SMTP_FlagEnableSSL,
                Debug: DEFAULT_DEBUG_MODE);
#else
            sendmail = new SendMail(
                Smtp_Host: AppConfig.Mail_SMTP_Host,
                Smtp_Port: AppConfig.Mail_SMTP_Port,
                Smtp_Mailfrom: AppConfig.Mail_MailFrom,
                Smtp_NameFrom: AppConfig.Mail_MailFrom,
                Smtp_User: AppConfig.Mail_SMTP_User,
                Smtp_Password: AppConfig.Mail_SMTP_Passwd,
                Smtp_EnableSSL: AppConfig.Mail_SMTP_FlagEnableSSL);
#endif

            bool result = sendmail.Send(
                MailTo: MailTo,
                ReplyTo: AppConfig.Mail_MailReply,
                Message_Theme: Message_Theme,
                Message_Text: Message_Text);

            return result;
        }
    }
}