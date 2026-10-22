using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Helpers;
using HIDE.RestAndRant.Common;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Отправка смс
    /// </summary>
    public class Smses
    {
        private readonly ILogger _log;
        private readonly Helper_SmsSender _bl_Helper_SmsSender = new();
        private readonly Clients _bl_Clients = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Smses()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Отправка СМС-чека
        /// </summary>
        /// <returns></returns>
        public bool Send_Chek(int ClientID, string Phone, string Url)
        {
            CClient? client = _bl_Clients.Get(ID: ClientID);
            if (client == null)
            {
                _log.LogError(message: "Клиент для отправки SMS не найден");
                return false;
            }

            if (client.Phone != Phone)
            {
                _log.LogError(message: "Номер телефона не совпадает с указанным в базе");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Url))
            {
                _log.LogError(message: "Ссылка на чек пустая");
                return false;
            }

            string text = $"Ссылка на чек: {Url}";
            bool result_sms = _bl_Helper_SmsSender.SendSms(ClientID: ClientID, Phone: Phone, Text: text);
            if (!result_sms)
            {
                _log.LogWarning(message: "Не удалось отправить СМС");
            }
            return result_sms;
        }

    }
}