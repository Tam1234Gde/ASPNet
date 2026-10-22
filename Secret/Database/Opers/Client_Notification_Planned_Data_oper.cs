using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{

    /// <summary>
    /// Класс работы с БД списка уведомлений для запланированной отправки.
    /// </summary>
    public class Client_Notification_Planned_Data_oper
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
        public Client_Notification_Planned_Data_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение списка запросов
        /// </summary>
        /// <returns></returns>
        public IQueryable<Client_Notification_Planned_Data> GetQuery()
        {
            IQueryable<Client_Notification_Planned_Data> query = from p in _db.Client_Notification_Planned_Data.Include(x => x.Client_Notification_Planned).Include(x => x.Client) 
                                                                 select p;

            return query;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Client_Notification_Planned_Data? Get(int ID)
        {
            Client_Notification_Planned_Data? elem = (from p in GetQuery().Include(x => x.Client).AsNoTracking() 
                                                      where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="Client_Notification_Planned_ID">Идентификатор запланированной записи</param>
        /// <returns></returns>
        public IQueryable<Client_Notification_Planned_Data>? GetList_Planned(int Client_Notification_Planned_ID)
        {
            IQueryable<Client_Notification_Planned_Data>? elem = (from p in GetQuery().AsNoTracking() where p.Client_Notification_Planned_ID == Client_Notification_Planned_ID select p);
            return elem;
        }


        /// <summary>
        /// Получение по ID для редактирования
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Client_Notification_Planned_Data? Get_Edit(int ID)
        {
            Client_Notification_Planned_Data? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Сохранение в БД
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "Commit OK");
                }
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, "Err # ErrorCode='{}'", Errors.Err202_str);
                return false;
            }
        }


        /// <summary>
        /// Добавление без сохранения в БД
        /// </summary>
        /// <param name="Client_Notification_Planned_ID">Идентификатор запланированного уведомления</param>
        /// <param name="Client_ID">Идентификатор клиента</param>
        /// <returns></returns>
        public int Add_NoCommit(int Client_Notification_Planned_ID, int Client_ID)
        {
            string loginfo = $"Client_Notification_Planned_ID='{Client_Notification_Planned_ID}' Client_ID='{Client_ID}'";
            
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Client_Notification_Planned_Data elem = new()
            {
                Client_Notification_Planned_ID = Client_Notification_Planned_ID,
                ClientID = Client_ID,
            };

            _db.Client_Notification_Planned_Data.Add(elem);

            return elem.ID;
        }
    }
}
