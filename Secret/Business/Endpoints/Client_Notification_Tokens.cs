using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Регистрация токенов уведомлений клиентов
    /// </summary>
    public class Client_Notification_Tokens
    {
        private readonly ILogger _log;
        private readonly Client_Notification_Token_oper _dl_Client_Notification_Token = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Client_Notification_Tokens()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CClient_Notification_Token? Get(Client_Notification_Token? Elem)
        {
            if (Elem != null)
            {
                CClient_Notification_Token item = new()
                {
                    ID = Elem.ID,
                    ClientID = Elem.ClientID,
                    Date_Del = Elem.Date_Del,
                    DeviceID = Elem.DeviceID,
                    Flag_Del = Elem.Flag_Del,
                    Token = Elem.Token,
                    TokenTypeID = Elem.TokenTypeID
                };
                return item;
            }
            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        public CClient_Notification_Token? Get(int ID)
        {
            return Get(Elem: _dl_Client_Notification_Token.Get(ID));
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        public CClient_Notification_Token? GetByDeviceID(string DeviceID)
        {
            return Get(Elem: _dl_Client_Notification_Token.GetByDeviceID(DeviceID));
        }


        /// <summary>
        /// Получает список токенов по идентификатору пользователя
        /// </summary>
        /// <param name="ClientID">Идентификатор пользователя</param>
        /// <returns></returns>
        public List<CClient_Notification_Token> GetList_ByClientId(int ClientID)
        {
            List<CClient_Notification_Token> result = [];

            List<Client_Notification_Token> query = [.. _dl_Client_Notification_Token.GetQuery(ClientID)];

            foreach (Client_Notification_Token token in query)
            {
                CClient_Notification_Token? tokenData = Get(token.ID);
                if (tokenData == null)
                {
                    continue;
                }

                result.Add(tokenData);
            }

            return result;
        }


        /// <summary>
        /// Добавление\Изменение
        /// </summary>
        /// <returns></returns>
        public int Add(int ClientID, int TokenTypeID, string DeviceID, string Token)
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                _log.LogWarning("Токен пустой");
                return Errors.Err202;
            }

            int result;
            Client_Notification_Token? query = _dl_Client_Notification_Token.CheckByDeviceID(DeviceID).ToList().FirstOrDefault();

           


            if (query != null)
            {
                result = _dl_Client_Notification_Token.Edit(ID: query.ID, Token: Token);
            }
            else
            {
                result = _dl_Client_Notification_Token.Add(ClientID: ClientID, TokenTypeID: TokenTypeID, DeviceID: DeviceID, Token: Token);
            }

            return result;
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        public int Del(string DeviceID)
        {
            CClient_Notification_Token? token = GetByDeviceID(DeviceID);

            if (token == null)
            {
                _log.LogWarning("Запись с данным идентификатором не найдена");
                return Errors.Err201;
            }

            return _dl_Client_Notification_Token.Edit_FlagDel(ID: token.ID, FlagDel_val: true);
        }

    }
}