using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Класс операций с БД для шаблонов уведомлений
    /// </summary>
    public class Client_Notification_Shablon_oper
    {

        /// <summary>
        /// Логер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly DBContext _db;


        /// <summary>
        /// Конструктор
        /// </summary>
        public Client_Notification_Shablon_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
            _db = new();
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        private IQueryable<Client_Notification_Shablon> GetQuery()
        {
            IQueryable<Client_Notification_Shablon> query = from p in _db.Client_Notification_Shablons.AsNoTracking() select p;
            return query;
        }


        /// <summary>
        /// Получение запроса для редактирования
        /// </summary>
        /// <returns></returns>
        private IQueryable<Client_Notification_Shablon> GetQuery_Edit()
        {
            IQueryable<Client_Notification_Shablon> query = from p in _db.Client_Notification_Shablons select p;
            return query;
        }


        /// <summary>
        /// Получение по ID для редактирования
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns>Данные Client</returns>
        private Client_Notification_Shablon? Get_Edit(int ID)
        {
            Client_Notification_Shablon? elem = (from p in GetQuery_Edit() where (int)p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns>Данные Client</returns>
        public Client_Notification_Shablon? Get(int ID)
        {
            Client_Notification_Shablon? elem = (from p in GetQuery() where (int)p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        public IQueryable<Client_Notification_Shablon> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Client_Notification_Shablon> query = from p in GetQuery() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public int Add(int ID, string Name, string Content, bool Push_Need)
        {
            string loginfo = $"ID='{ID}' Name='{Name}' Content='{Content} Push_Need='{Push_Need}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification_Shablon elem = new()
            {
                ID = (EClient_Notification_Shablon)ID,
                Name = Name,
                Content = Content,
                Push_Need = Push_Need
            };

            _db.Client_Notification_Shablons.Add(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                
                return (int)elem.ID;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err202_str}' ## {loginfo}");
                return Errors.Err202;
            }
        }


        /// <summary>
        /// Установить на уведомлении текущую дату прочтения
        /// </summary>
        /// <param name="ID">Код записи, у которой нужно поменять дату прочтения</param>
        /// <returns></returns>
        public int Edit(int ID, string Name, string Content, bool Push_Need)
        {
            string loginfo = $"ID='{ID}' Name='{Name}' Content='{Content}' Push_Need='{Push_Need}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification_Shablon? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Name = Name;
            elem.Content = Content;
            elem.Push_Need = Push_Need;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return (int)elem.ID;
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
        /// <param name="UserID">Пользователь</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool FlagDel_val, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_val='{FlagDel_val}' UserID='{UserID}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification_Shablon? elem = Get_Edit(ID: ID);

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

                return (int)elem.ID;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }
    }
}