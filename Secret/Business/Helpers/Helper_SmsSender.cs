using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Opers;
using HIDE.SmsInt;

namespace HIDE.RestAndRant.Business.Helpers
{
    /// <summary>
    /// Помощник отправки СМС
    /// </summary>
    public class Helper_SmsSender
    {
        private readonly ILogger _log;
        private readonly Sms_Log_oper _db_Sms_Log;

        private const string LOG_TEMPLATE_SEND_SMS = "ClientID='{0}' Phone='{1}' Text='{2}' ";
        private const string LOG_INFO_TESTER_SKIP = "Клиент стоит в группе тестировщиков, SMS ему отправлено не будет ## {0}";
        private const string LOG_ERROR_SMS_LOG_FAILED = "Не удалось добавить запись в лог СМС # Err={0} ## {1}";
        private const bool DEFAULT_DEBUG_MODE = true;
        private const bool DEFAULT_SMS_DEBUG_MODE = false;


        /// <summary>
        /// Конструктор
        /// </summary>
        public Helper_SmsSender()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");

            _db_Sms_Log = new Sms_Log_oper();
        }


        /// <summary>
        /// Отправить SMS
        /// </summary>
        /// <param name="ClientID">Код клиента</param>
        /// <param name="Phone">Номер телефона</param>
        /// <param name="Text">Текст сообщения</param>
        /// <returns>True - успешно, False - ошибка</returns>
        public bool SendSms(int ClientID, string Phone, string Text)
        {
            string loginfo = string.Format(LOG_TEMPLATE_SEND_SMS, ClientID, Phone, Text);

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{0}", args: loginfo);
            }

            SmsSender_v1 sms_sender;

            if (AppConfig.SMS_Send)
            {
                if (AppConfig.Testers_Code_ClientIDs != null && AppConfig.Testers_Code_ClientIDs.Count != 0 && AppConfig.Testers_Code_ClientIDs.Contains(ClientID))
                {
                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(message: "{0}", args: string.Format(LOG_INFO_TESTER_SKIP, loginfo));
                    }

                    return true;
                }

                sms_sender = new SmsSender_v1(Xtoken: AppConfig.SMS_Token, Log: _log, Debug: DEFAULT_SMS_DEBUG_MODE);

                int sms_log_id = _db_Sms_Log.Add(ClientID: ClientID, Phone: Phone, Text: Text);

                if (sms_log_id < 0)
                {
                    _log.LogError(message: "{0}", args: string.Format(LOG_ERROR_SMS_LOG_FAILED, sms_log_id, Errors.GetErrorString(sms_log_id)));
                }
            }
            else
            {
                sms_sender = new SmsSender_v1(Xtoken: AppConfig.SMS_Token, Log: _log, Debug: DEFAULT_DEBUG_MODE);
            }

            return sms_sender.Send(Phone: Phone, Message: Text);
        }
    }
}