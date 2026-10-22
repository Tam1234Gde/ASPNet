using Microsoft.Extensions.Logging;
using Telegram.Bot;
using HIDE.RestAndRant.Common;

namespace HIDE.RestAndRant.Business.Helpers
{
    /// <summary>
    /// Класс работы с Telegram
    /// </summary>
    public class Helper_Telegram
    {
        private readonly ILogger _log;

        private const string LOG_TEMPLATE_SEND_TELEGRAM = "Попытка отправки сообщения в Telegram: \nChatID: {0} \nMessageText: \n{1}";
        private const string LOG_ERROR_TELEGRAM_SEND = "ERR # Ошибка отправки заказа в тг-канал: {0}";
        private const string ERROR_TELEGRAM_DISABLED = "Telegram отключен на уровне настроек приложения";
        private const string ERROR_TELEGRAM_EXCEPTION = "Метод отправки сообщения в Telegram вызвал исключение - {0}";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Helper_Telegram()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Отправка оповещения о новом заказе в Telegram бот
        /// </summary>
        /// <param name="ChatID">ID чата</param>
        /// <param name="MessageText">Текст сообщения</param>
        /// <param name="ErrMessage">Сообщение об ошибке</param>
        /// <returns>True - успешно, False - ошибка</returns>
        public bool SendMessageTelegram(string ChatID, string MessageText, out string? ErrMessage)
        {
            ErrMessage = null;

            if (AppConfig.TG_Active)
            {
                TelegramBotClientOptions options = new TelegramBotClientOptions(
                    token: AppConfig.TG_Token,
                    baseUrl: AppConfig.TG_ProxyUrl
                );

                ITelegramBotClient bot = new TelegramBotClient(options);

                try
                {
                    if (_log.IsEnabled(LogLevel.Information))
                    {
                        _log.LogInformation(
                            message: LOG_TEMPLATE_SEND_TELEGRAM,
                            args: [ChatID, MessageText]);
                    }

                    bot.SendMessage(
                        chatId: ChatID,
                        text: MessageText,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

                    return true;
                }
                catch (Exception ex)
                {
                    _log.LogError(LOG_ERROR_TELEGRAM_SEND, ex);
                    ErrMessage = string.Format(ERROR_TELEGRAM_EXCEPTION, ex);
                    return false;
                }
            }
            else
            {
                ErrMessage = ERROR_TELEGRAM_DISABLED;
                return false;
            }
        }
    }
}