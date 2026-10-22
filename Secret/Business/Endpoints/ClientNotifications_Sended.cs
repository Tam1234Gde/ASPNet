#region DEL By MAE - 05.05.2026 - Удалено для того, чтобы правильно реализовать данную систему с журналом отправленых
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using HIDE.RestAndRant.Common;
//using HIDE.RestAndRant.Common.ENums;
//using HIDE.RestAndRant.Database;

//namespace HIDE.RestAndRant.Business
//{

//    /// <summary>
//    /// Бизнес для таблицы отправленных уведомлений
//    /// </summary>
//    public class ClientNotifications_Sended
//    {

//        /// <summary>
//        /// Логгер
//        /// </summary>
//        private readonly ILogger _log;


//        /// <summary>
//        /// Контекст БД
//        /// </summary>
//        private readonly Client_Notification_History_oper _dl_ClientNotification_Sended;


//        /// <summary>
//        /// Конструктор
//        /// </summary>
//        public ClientNotifications_Sended()
//        {
//            _log = AppLog.CreateLogger(this.GetType());
//            _dl_ClientNotification_Sended = new();
//        }


//        /// <summary>
//        /// Сохранение в БД
//        /// </summary>
//        /// <returns></returns>
//        public bool Commit()
//        {
//            return _dl_ClientNotification_Sended.Commit();
//        }


//        /// <summary>
//        /// Добавление записи в историю
//        /// </summary>
//        /// <returns></returns>
//        public int Add(int ClientID, string Name, string Content, DateTime Date_Send, EClient_Notification_Shablon? Client_Notification_ShablonID, bool Flag_IsSuccess)
//        {
//            return _dl_ClientNotification_Sended.Add(ClientID: ClientID, Name: Name, Content: Content, Date_Send: Date_Send, Client_Notification_ShablonID: Client_Notification_ShablonID, Flag_IsSuccess: Flag_IsSuccess).ID;
//        }



//    }
//} 
#endregion DEL By MAE - 05.05.2026 - Удалено для того, чтобы правильно реализовать данную систему с журналом отправленых