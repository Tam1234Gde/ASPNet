using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// История отправки кастомных уведомлений
    /// </summary>
    public class Client_Notification_Planned_oper
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
        public Client_Notification_Planned_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <returns></returns>
        public IQueryable<Client_Notification_Planned> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Client_Notification_Planned> query = from p in _db.Client_Notification_Planned select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение актуальных (С истекщей датой, не отправленные и не удаленные)
        /// </summary>
        /// <returns></returns>
        public IQueryable<Client_Notification_Planned>? Get_ListActual()
        {
            IQueryable<Client_Notification_Planned>? elem = (from p in GetQuery().AsNoTracking() where p.Date_Send < DateTime.Now && p.Flag_Del == false && p.Flag_Sended == false select p);
            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Client_Notification_Planned? Get_Edit(int ID)
        {
            Client_Notification_Planned? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }




        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Name">Заголовок уведомления</param>
        /// <param name="Content">Текст уведомления</param>
        /// <param name="File">Файл</param>
        /// <returns></returns>
        public int Add(string Name, string Content, DateTime Date_Send, bool Flag_Selected, EClient_Notification_Shablon? Client_Notification_ShablonID)
        {
            string loginfo = $"Name='{Name}' Content='{Content}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Client_Notification_Planned elem = new()
            {
                Name = Name, //Заголовок уведомления
                Content = Content, //Тело уведомления
                Date_Created = DateTime.Now, //Дата создания
                Date_Send = Date_Send, //Дата запланированной отправки
                Flag_Selected = Flag_Selected, //Выбранные получатели или все
                Client_Notification_ShablonID = Client_Notification_ShablonID //Шаблон уведомления
            };

            _db.Client_Notification_Planned.Add(elem);

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
                if(_log.IsEnabled(logLevel: LogLevel.Error))
                {
                    _log.LogError(exception: ex, "Err # ErrorCode='{}' ## {}", Errors.Err202_str, loginfo); //TODO: MAE - Проверить чтобы соответствовало требованиям.
                }
                return Errors.Err202;
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

            Client_Notification_Planned? notification = Get_Edit(ID);
            if (notification == null)
            {
                return Errors.Err201;
            }

            notification.Flag_Sended = true;
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
                _log.LogError(exception: e, message: "Err # Error={} ## {}", args: [Errors.Err203_str, loginfo]);
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Помечает уведомление как удалено
        /// </summary>
        /// <param name="ID">Идентификатор уведомления</param>
        /// <param name="Flag_Del">Пометка об удалении</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del = true)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification_Planned? notification = Get_Edit(ID);
            if (notification == null)
            {
                return Errors.Err201;
            }

            notification.Flag_Del = Flag_Del;
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
                _log.LogError(exception: e, message: "Err # Error={} ## {}", args: [Errors.Err203_str, loginfo]);
                return Errors.Err203;
            }
        }

    }
}