using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Класс операций с БД для уведомлений клиента
    /// </summary>
    public class Client_Notification_oper
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
        public Client_Notification_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");

            _db = new();
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        private IQueryable<Client_Notification> GetQuery()
        {
            IQueryable<Client_Notification> query = from p in _db.Client_Notifications.AsNoTracking()
                                                    select p;
            return query;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns>Данные Client</returns>
        private Client_Notification? Get_Edit(int ID)
        {
            Client_Notification? elem = (from p in _db.Client_Notifications where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns>Данные Client</returns>
        public Client_Notification? Get(int ID)
        {
            Client_Notification? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по ShablonID и OrderID
        /// </summary>
        /// <param name="ShablonID">Идентификатор шаблона</param>
        /// <param name="OrderID">Номер заказа</param>
        /// <returns></returns>
        public Client_Notification? Get(EClient_Notification_Shablon ShablonID, int OrderID)
        {
            Client_Notification? elem = (from p in GetQuery() where p.Client_Notification_ShablonID == ShablonID && p.OrderID.HasValue && p.OrderID.Value == OrderID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="ClientID">Идентификатор клиента</param>
        /// <param name="Flag_Del_val">Удален</param>
        /// <returns></returns>
        public IQueryable<Client_Notification> GetQuery(int ClientID, bool? Flag_Del_val = false)
        {
            IQueryable<Client_Notification> query = from p in GetQuery() where p.ClientID == ClientID select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса для редактирования по Клиенту
        /// </summary>
        /// <param name="ClientID">Идентификатор клиента</param>
        /// <param name="FlagDel_Val">Удален</param>
        /// <returns></returns>
        public IQueryable<Client_Notification> GetQuery_Edit(int ClientID, bool? FlagDel_Val = false)
        {
            IQueryable<Client_Notification> query = from p in _db.Client_Notifications where p.ClientID == ClientID select p;

            if (FlagDel_Val.HasValue)
            {
                query = from p in query where p.Flag_Del == FlagDel_Val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val">Удален</param>
        /// <returns></returns>
        public IQueryable<Client_Notification> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Client_Notification> query = from p in GetQuery() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="FlagDel_Val">Удален</param>
        /// <returns></returns>
        public IQueryable<Client_Notification> GetQuery_Edit(bool? FlagDel_Val = false)
        {
            IQueryable<Client_Notification> query = from p in _db.Client_Notifications select p;

            if (FlagDel_Val.HasValue)
            {
                query = from p in query where p.Flag_Del == FlagDel_Val select p;
            }

            return query;
        }


        /// <summary>
        /// Коммит
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            try
            {
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # ErrorCode='{Errors.Err205}' ## Error='{Errors.Err202_str}'");
                }
                return false;
            }
        }


        /// <summary>
        /// Добавление без сохранения
        /// </summary>
        /// <param name="ClientID">Идентификатор клиента</param>
        /// <param name="Name">Имя</param>
        /// <param name="Content">Контент</param>
        public void Add_NoCommit(int? ClientID, string Name, string Content, bool Push_Need)
        {
            string loginfo = $"ClientID='{ClientID}' Name='{Name}' Content='{Content}' Push_Need='{Push_Need}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification elem = new()
            {
                ClientID = ClientID,
                Date_Act = DateTime.Now,
                Name = Name,
                Content = Content,
                Push_Need = Push_Need,
                OrderID = null,
                Client_Notification_ShablonID = null,
            };

            _db.Client_Notifications.Add(elem);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public int Add(int ClientID, string Name, string Content, bool Push_Need, int? OrderID, EClient_Notification_Shablon? Client_Notification_ShablonID)
        {
            string loginfo = $"ClientID='{ClientID}' Name='{Name}' Content='{Content}' Push_Need='{Push_Need}' OrderID='{OrderID}' Client_Notification_ShablonID='{Client_Notification_ShablonID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification elem = new()
            {
                ClientID = ClientID,
                Date_Act = DateTime.Now,
                Name = Name,
                Content = Content,
                Push_Need = Push_Need,
                OrderID = OrderID,
                Client_Notification_ShablonID = Client_Notification_ShablonID,
            };

            _db.Client_Notifications.Add(elem);

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
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err202_str}' ## {loginfo}");
                return Errors.Err202;
            }
        }


        /// <summary>
        /// Установить на уведомлении текущую дату прочтения
        /// </summary>
        /// <param name="ID">Код записи, у которой нужно поменять дату прочтения</param>
        /// <returns></returns>
        public int Set_DateRead(int ID, DateTime? Date_Read)
        {
            string loginfo = $"ID='{ID}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }


            elem.Date_Read = Date_Read;

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
        /// Помечает все уведомления прочитанными
        /// </summary>
        /// <param name="ClientID">Идентификатор клиента</param>
        /// <returns></returns>
        public int Set_DateRead_All(int ClientID)
        {
            string loginfo = $"ClientId='{ClientID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            List<Client_Notification> clientNotifications = [.. GetQuery_Edit(ClientID: ClientID)];
            if (clientNotifications.Count == 0)
            {
                return ClientID;
            }

            clientNotifications.ForEach(x =>
            {
                x.Date_Read = DateTime.Now;
            });

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return ClientID;
            }
            catch (Exception e)
            {
                _log.LogError(exception: e, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Установить на уведомлении текущую дату прочтения
        /// </summary>
        /// <param name="ID">Код записи, у которой нужно поменять дату прочтения</param>
        /// <returns></returns>
        public int Set_PushDateSend(int ID, DateTime? Push_DateSend)
        {
            string loginfo = $"ID='{ID}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }


            elem.Push_DateSend = Push_DateSend;

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

            Client_Notification? elem = Get_Edit(ID: ID);

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


        /// <summary>
        /// Удаляет все уведомления
        /// </summary>
        /// <param name="ClientID">Идентификатор клиента</param>
        /// <returns></returns>
        public int Del_All(int ClientID)
        {
            string loginfo = $"ClientId='{ClientID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            List<Client_Notification> clientNotifications = [.. GetQuery_Edit(ClientID: ClientID)];
            if (clientNotifications.Count == 0)
            {
                return ClientID;
            }

            DateTime now = DateTime.Now;

            clientNotifications.ForEach(x =>
            {
                x.Flag_Del = true;
                x.Date_Del = now;
            });

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return ClientID;
            }
            catch (Exception e)
            {
                _log.LogError(exception: e, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Помечает уведомление как отправлено
        /// </summary>
        /// <param name="ID">Идентификатор уведомления</param>
        /// <returns></returns>
        public int MarkAs_Sent(int ID)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification? notification = Get_Edit(ID);
            if (notification == null)
            {
                return Errors.Err201;
            }

            notification.Push_DateSend = DateTime.Now;
            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return ID;
            }
            catch (Exception e)
            {
                _log.LogError(exception: e, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }
    }
}