//TODO: FUTURE - Сделать статистику отправленных пушей.

//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using HIDE.RestAndRant.Common;
//using HIDE.RestAndRant.Common.ENums;
//using HIDE.RestAndRant.Database.Definitions;

//namespace HIDE.RestAndRant.Database
//{
//    /// <summary>
//    /// Класс взаимодействия с БД
//    /// </summary>
//    public class Client_Notification_Stats_oper
//    {
//        /// <summary>
//        /// Логер
//        /// </summary>
//        private readonly ILogger _log;


//        /// <summary>
//        /// Контекст базы данных
//        /// </summary>
//        private readonly DBContext _db = new();


//        /// <summary>
//        /// Конструктор
//        /// </summary>
//        public Client_Notification_Stats_oper()
//        {
//            _log = AppLog.CreateLogger(GetType());
//            _log.LogTrace("Constructor");
//        }


//        /// <summary>
//        /// Коммит
//        /// </summary>
//        /// <returns></returns>
//        public bool Commit()
//        {
//            try
//            {
//                _db.SaveChanges();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                if (_log.IsEnabled(LogLevel.Critical))
//                {
//                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # ErrorCode='{Errors.Err205}' ## Error='{Errors.Err202_str}'");
//                }
//                return false;
//            }
//        }


//        /// <summary>
//        /// Получение запроса
//        /// </summary>
//        /// <returns></returns>
//        private IQueryable<Client_Notification_Stats> GetQuery()
//        {
//            IQueryable<Client_Notification_Stats> query = from p in _db.Client_Notification_Stats.AsNoTracking() select p;
//            return query;
//        }


//        /// <summary>
//        /// Получение запроса для редактирования
//        /// </summary>
//        /// <returns></returns>
//        private IQueryable<Client_Notification_Stats> GetQuery_Edit()
//        {
//            IQueryable<Client_Notification_Stats> query = from p in _db.Client_Notification_Stats select p;
//            return query;
//        }


//        /// <summary>
//        /// Получение по ID
//        /// </summary>
//        /// <param name="ID">Код записи, которую нужно получить</param>
//        /// <returns>Данные Client</returns>
//        public Client_Notification_Stats? Get(int ID)
//        {
//            Client_Notification_Stats? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();
//            return elem;
//        }


//        /// <summary>
//        /// Получение по ID
//        /// </summary>
//        /// <param name="ID">Код записи, которую нужно получить</param>
//        /// <returns>Данные Client</returns>
//        public Client_Notification_Stats? Get_ByToken(int TokenID)
//        {
//            Client_Notification_Stats? elem = (from p in GetQuery() where p.TokenID == TokenID select p).FirstOrDefault();
//            return elem;
//        }


//        /// <summary>
//        /// Получение по ID для редактирования
//        /// </summary>
//        /// <param name="ID">Код записи, которую нужно получить</param>
//        /// <returns>Данные Client</returns>
//        public Client_Notification_Stats? Get_Edit(int ID)
//        {
//            Client_Notification_Stats? elem = (from p in GetQuery_Edit() where p.ID == ID select p).FirstOrDefault();
//            return elem;
//        }


//        /// <summary>
//        /// Получение по ID
//        /// </summary>
//        /// <param name="TokenID">Код токена, которую нужно получить</param>
//        /// <returns>Данные Client</returns>
//        public Client_Notification_Stats? Get_Edit_ByTokenID(int TokenID)
//        {
//            Client_Notification_Stats? elem = (from p in GetQuery_Edit() where p.TokenID == TokenID select p).FirstOrDefault();
//            return elem;
//        }


//        /// <summary>
//        /// Добавление
//        /// </summary>
//        /// <returns></returns>
//        public int Add(int TokenID, bool None = false, bool PermissionDenied = false, bool InvalidArgument = false, bool Unauthenticated = false, 
//                       bool InvalidError = false, bool NotFound = false, bool AlreadyExists = false)
//        {
//            string loginfo = $"Token='{TokenID}' None='{None}' PermissionDenied='{PermissionDenied} InvalidArgument='{InvalidArgument}' Unauthenticated='{Unauthenticated}'" +
//                $"InvalidError='{InvalidError}' NotFound='{NotFound}' AlreadyExists='{AlreadyExists}'";

//            if (_log.IsEnabled(LogLevel.Debug))
//            {
//                _log.LogDebug(message: "{}", args: loginfo);
//            }

//            Client_Notification_Stats elem = new()
//            {
//                TokenID = TokenID,
//                None_Counts = None ? 1 : 0,
//                PermissionDenied_Counts = PermissionDenied ? 1 : 0,
//                InvalidArgument_Counts = InvalidArgument ? 1 : 0,
//                Unauthenticated_Counts = Unauthenticated ? 1 : 0,
//                InvalidError_Counts = InvalidError ? 1 : 0,
//                NotFound_Counts = NotFound ? 1 : 0,
//                AlreadyExists_Counts = AlreadyExists ? 1 : 0,
//            };

//            _db.Client_Notification_Stats.Add(elem);

//            try
//            {
//                _db.SaveChanges();
//                if (_log.IsEnabled(LogLevel.Debug))
//                {
//                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
//                }

//                return elem.ID;
//            }
//            catch (Exception ex)
//            {
//                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err202_str}' ## {loginfo}");
//                return Errors.Err202;
//            }
//        }


//        /// <summary>
//        /// Изменение
//        /// </summary>
//        /// <param name="ID">Код записи, у которой нужно поменять флаг</param>
//        /// <param name="Token">Токен</param>
//        /// <returns></returns>
//        public int Edit_AddCounts(int TokenID, bool None = false, bool PermissionDenied = false, bool InvalidArgument = false, bool Unauthenticated = false,
//                       bool InvalidError = false, bool NotFound = false, bool AlreadyExists = false)
//        {
//            string loginfo = $"ID='{TokenID}' None='{None}' PermissionDenied='{PermissionDenied}' InvalidArgument='{InvalidArgument}' Unauthenticated='{Unauthenticated}' " +
//                $"InvalidError='{InvalidError}' NotFound='{NotFound}' AlreadyExists='{AlreadyExists}'";

//            if (_log.IsEnabled(LogLevel.Debug))
//            {
//                _log.LogDebug(message: "{}", args: loginfo);
//            }

//            Client_Notification_Stats? elem = Get_Edit_ByTokenID(TokenID: TokenID);

//            if (elem == null)
//            {
//                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
//                return Errors.Err200;
//            }

//            elem.None_Counts += (None ? 1 : 0);
//            elem.PermissionDenied_Counts += (PermissionDenied ? 1 : 0);
//            elem.InvalidArgument_Counts += (InvalidArgument ? 1 : 0);
//            elem.Unauthenticated_Counts += (Unauthenticated ? 1 : 0);
//            elem.InvalidError_Counts += (InvalidError ? 1 : 0);
//            elem.NotFound_Counts += (NotFound ? 1 : 0);
//            elem.AlreadyExists_Counts += (AlreadyExists ? 1 : 0);


//            try
//            {
//                _db.SaveChanges();
//                if (_log.IsEnabled(LogLevel.Debug))
//                {
//                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
//                }

//                return elem.ID;
//            }
//            catch (Exception ex)
//            {
//                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");
//                return Errors.Err203;
//            }
//        }
//    }
//}
