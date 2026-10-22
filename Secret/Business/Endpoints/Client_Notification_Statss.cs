//TODO: FUTURE - Сделать статистику отправленных пушей.

//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using HIDE.RestAndRant.Common;
//using HIDE.RestAndRant.Database;
//using HIDE.RestAndRant.Database.Definitions;

//namespace HIDE.RestAndRant.Business
//{
//    /// <summary>
//    /// Класс работы с логикой статистики уведолмлений
//    /// </summary>
//    public class Client_Notification_Statss
//    {
//        /// <summary>
//        /// Логгер
//        /// </summary>
//        private readonly ILogger _log;

//        /// <summary>
//        /// Класс работы с БД
//        /// </summary>
//        private readonly Client_Notification_Stats_oper _dl_Client_Notification_Stats = new(); 


//        /// <summary>
//        /// Конструктор
//        /// </summary>
//        public Client_Notification_Statss()
//        {
//            _log = AppLog.CreateLogger(GetType());
//            _log.LogTrace("Constructor");
//        }


//        /// <summary>
//        /// Добавление записи
//        /// </summary>
//        /// <param name="TokenID"></param>
//        /// <param name="None"></param>
//        /// <param name="PermissionDenied"></param>
//        /// <param name="InvalidArgument"></param>
//        /// <param name="Unauthenticated"></param>
//        /// <param name="InvalidError"></param>
//        /// <param name="NotFound"></param>
//        /// <param name="AlreadyExists"></param>
//        /// <returns></returns>
//        public int Add(int TokenID, bool None = false, bool PermissionDenied = false, bool InvalidArgument = false, bool Unauthenticated = false,
//                    bool InvalidError = false, bool NotFound = false, bool AlreadyExists = false)
//        {
//            return _dl_Client_Notification_Stats.Add(TokenID: TokenID, None: None, PermissionDenied: PermissionDenied, InvalidArgument: InvalidArgument, Unauthenticated: Unauthenticated,
//                    InvalidError: InvalidError, NotFound: NotFound, AlreadyExists: AlreadyExists);
//        }


//        /// <summary>
//        /// Изменение
//        /// </summary>
//        /// <param name="TokenID"></param>
//        /// <param name="None"></param>
//        /// <param name="PermissionDenied"></param>
//        /// <param name="InvalidArgument"></param>
//        /// <param name="Unauthenticated"></param>
//        /// <param name="InvalidError"></param>
//        /// <param name="NotFound"></param>
//        /// <param name="AlreadyExists"></param>
//        /// <returns></returns>
//        public int Edit_AddCounts(int TokenID, bool None = false, bool PermissionDenied = false, bool InvalidArgument = false, bool Unauthenticated = false,
//                    bool InvalidError = false, bool NotFound = false, bool AlreadyExists = false)
//        {
//            return _dl_Client_Notification_Stats.Edit_AddCounts(TokenID: TokenID, None: None, PermissionDenied: PermissionDenied, InvalidArgument: InvalidArgument, Unauthenticated: Unauthenticated,
//                    InvalidError: InvalidError, NotFound: NotFound, AlreadyExists: AlreadyExists);
//        }


//        /// <summary>
//        /// Коммит в БД
//        /// </summary>
//        /// <returns></returns>
//        public bool Commit()
//        {
//            return _dl_Client_Notification_Stats.Commit();
//        }


//        /// <summary>
//        /// Получение по ID
//        /// </summary>
//        /// <param name="TokenID"></param>
//        /// <returns></returns>
//        public int? GetID_ByToken(int TokenID)
//        {
//            // return _dl_Client_Notification_Stats.Get_ByToken(TokenID: TokenID).ID;

//            return null;
//        }
//    }
//}
