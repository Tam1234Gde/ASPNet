using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Регистрация токенов уведомлений клиентов
    /// </summary>
    public class Client_Notification_Token_oper
    {
        /// <summary>
        /// Логер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Конткест базы данных
        /// </summary>
        private readonly DBContext _db;


        /// <summary>
        /// Конструктор
        /// </summary>
        public Client_Notification_Token_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
            _db = new();
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        private IQueryable<Client_Notification_Token> GetQuery()
        {
            IQueryable<Client_Notification_Token> query = from p in _db.Client_Notification_Tokens.AsNoTracking()
                                                          select p;
            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        private IQueryable<Client_Notification_Token> GetQuery_Edit()
        {
            IQueryable<Client_Notification_Token> query = from p in _db.Client_Notification_Tokens.AsNoTracking()
                                                          select p;
            return query;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns>Данные Client</returns>
        private Client_Notification_Token? Get_Edit(int ID)
        {
            Client_Notification_Token? elem = (from p in GetQuery_Edit() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns>Данные Client</returns>
        public Client_Notification_Token? Get(int ID)
        {
            Client_Notification_Token? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по DeviceID
        /// </summary>
        /// <param name="DeviceID">Код записи, которую нужно получить</param>
        /// <returns>Данные Client</returns>
        public Client_Notification_Token? GetByDeviceID(string DeviceID)
        {
            Client_Notification_Token? elem = (from p in GetQuery() where p.DeviceID == DeviceID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="ClientID">Идентификато клиента</param>
        /// <param name="Flag_Del_val">Удален</param>
        /// <returns></returns>
        public IQueryable<Client_Notification_Token> GetQuery(int ClientID, bool? Flag_Del_val = false)
        {
            IQueryable<Client_Notification_Token> query = from p in GetQuery() where p.ClientID == ClientID select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Проверка по DeviceID
        /// </summary>
        /// <param name="DeviceID">Идентификатор устройства</param>
        /// <param name="Flag_Del_val">Удален</param>
        /// <returns></returns>
        public IQueryable<Client_Notification_Token> CheckByDeviceID(string DeviceID, bool? Flag_Del_val = false)
        {
            IQueryable<Client_Notification_Token> query = from p in GetQuery() where p.DeviceID == DeviceID select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


   

        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="ClientID">Идентификатор клиента</param>
        /// <param name="TokenTypeID">Идентификаитор типа токена</param>
        /// <param name="DeviceID">Идентификатор устройства</param>
        /// <param name="Token">Токен</param>
        /// <returns></returns>
        public int Add(int ClientID, int TokenTypeID, string DeviceID, string Token)
        {
            string loginfo = $"ClientID='{ClientID}' TokenTypeID='{TokenTypeID}' DeviceID='{DeviceID}' Token='{Token}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification_Token elem = new()
            {
                ClientID = ClientID,
                DeviceID = DeviceID,
                TokenTypeID = TokenTypeID,
                Token = Token,
            };

            _db.Client_Notification_Tokens.Add(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args:   $"Err # Error='{Errors.Err202_str}' ## {loginfo}");
                return Errors.Err202;
            }
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Код записи, у которой нужно поменять флаг</param>
        /// <param name="Token">Токен</param>
        /// <returns></returns>
        public int Edit(int ID, string Token)
        {
            string loginfo = $"ID='{ID}' Token='{Token}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification_Token? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Token = Token;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return elem.ID;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение флага пометки на удаление
        /// </summary>
        /// <param name="ID">Код записи, у которой нужно поменять флаг</param>
        /// <param name="FlagDel_val">Новое значение флага</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool FlagDel_val)
        {
            string loginfo = $"ID='{ID}' FlagDel_val='{FlagDel_val}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification_Token? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Flag_Del = FlagDel_val;
            if (FlagDel_val)
            {
                elem.Date_Del = DateTime.Now;
            }
            else
            {
                elem.Date_Del = null;
            }

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return elem.ID;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }
    }
}