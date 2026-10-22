using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Класс работы с запланированными пуш-уведомлениями
    /// </summary>
    public class Client_Notifications_Planned
    {

        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Класс работы с уведомлениями
        /// </summary>
        private readonly Client_Notification_Planned_oper _dl_Client_Notification_Planned = new();


        /// <summary>
        /// Класс работы со списками пользователей
        /// </summary>
        private readonly Client_Notification_Planned_Data_oper _dl_Client_Notification_Planned_Data = new();


        /// <summary>
        /// Класс работы с уведомлениями
        /// </summary>
        private readonly Client_Notification_oper _dl_Client_Notification = new();


        /// <summary>
        /// Класс работы с клиентами
        /// </summary>
        private readonly Clients _bl_Clients = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Client_Notifications_Planned()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }



        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CClient_Notification_Planned? Get(Client_Notification_Planned? Elem)
        {
            if (Elem != null)
            {
                CClient_Notification_Planned item = new()
                {
                    ID = Elem.ID,
                    Name = Elem.Name,
                    Content = Elem.Content,
                    Date_Created = Elem.Date_Created,
                    Date_Send = Elem.Date_Send,
                    Flag_Selected = Elem.Flag_Selected,
                    Client_Notification_ShablonID = Elem.Client_Notification_ShablonID,
                    Flag_Sended = Elem.Flag_Sended,
                    Flag_Del = Elem.Flag_Del,
                    Date_Del = Elem.Date_Del,
                };
                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CClientNotification_History_ForAdmin? Get_History(Client_Notification_Planned? Elem)
        {
            if (Elem != null)
            {
                CClientNotification_History_ForAdmin item = new()
                {
                    ID = Elem.ID,
                    Name = Elem.Name,
                    Content = Elem.Content,
                    Date_Created = Elem.Date_Created,
                    Date_Send = Elem.Date_Send,
                    Flag_Selected = Elem.Flag_Selected
                };
                return item;
            }

            return null;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns>Consts.Status_Success - Успешно.</returns>
        public int Add(string Name, string Content, DateTime Date_Send, EClient_Notification_Shablon? Client_Notification_ShablonID = null)
        {
            int result = _dl_Client_Notification_Planned.Add(Name: Name, Content: Content, Date_Send: Date_Send, Flag_Selected: false, Client_Notification_ShablonID: Client_Notification_ShablonID);

            return result >= 0 ? Consts.Status_Success : result;
        }


        /// <summary>
        /// Добавление со списком пользователей
        /// </summary>
        /// <returns>Consts.Status_Success - Успешно.</returns>
        public int? Add_ForUsers(string Name, string Content, DateTime Date_Send, List<int> List_ClientsIDs, EClient_Notification_Shablon? Client_Notification_ShablonID = null)
        {

            //Добавляем запись в таблицу Planned
            int client_Notification_Planned_ID = _dl_Client_Notification_Planned.Add(Name: Name, Content: Content, Date_Send: Date_Send, Flag_Selected: true, Client_Notification_ShablonID: Client_Notification_ShablonID);

            //Фильтруем список существующих клиентов с БД
            List<int> List_ClientsIDs_DB = [.. _bl_Clients.GetList_ByIDs(List_IDs: List_ClientsIDs).Select(x => x.ID)];


            //Проверка на наличие записей
            if (List_ClientsIDs_DB != null && List_ClientsIDs_DB.Count > 0)
            {

                //Перебираем пользователей и добавляем в таблицу Client_Notification_Planned_Data
                foreach (int client_ID in List_ClientsIDs_DB)
                {
                    //Добавляем записи в таблицу Planned_Data
                    _dl_Client_Notification_Planned_Data.Add_NoCommit(Client_Notification_Planned_ID: client_Notification_Planned_ID, Client_ID: client_ID);
                }

                //Сохранение в БД 
                if (_dl_Client_Notification_Planned_Data.Commit())
                {
                    return Consts.Status_Success;
                }
                else
                {
                    _log.LogError(message: "Err # {}", args: "Ошибка группового commit");
                    return Errors.Err202;
                }
            }
            else
            {
                _log.LogError(message: "Err # {}", args: "Список существующих клиентов пуст");
                return Errors.Err202;
            }
        }


        /// <summary>
        /// Отправка
        /// </summary>
        /// <returns>Consts.Status_Success - Успешно.</returns>
        public int? Send()
        {
            //Заполняемый листик уведомлений
            List<CClient_Notification_Planned> list_CClient_Notification_Planned = [];

            //Полученный листик уведомлений с БД
            List<Client_Notification_Planned>? list_Client_Notification_Planned = _dl_Client_Notification_Planned.Get_ListActual()?.ToList();

            if (list_Client_Notification_Planned == null || list_Client_Notification_Planned.Count <= 0)
            {
                return null;
            }

            //Заполняем
            foreach (Client_Notification_Planned сlient_Notification_Planned in list_Client_Notification_Planned)
            {
                CClient_Notification_Planned? cClient_Notification_Planned = Get(сlient_Notification_Planned);

                if (cClient_Notification_Planned == null)
                {
                    continue;
                }

                list_CClient_Notification_Planned.Add(cClient_Notification_Planned);
            }
            List<int>? list_ResultSuccess = [];

            //Отправка
            foreach (CClient_Notification_Planned actual_Notifications in list_CClient_Notification_Planned)
            {

                List<int>? list_ClientIDs = [];

                if (actual_Notifications.Flag_Selected) //Определенным пользователям
                {
                    list_ClientIDs = _dl_Client_Notification_Planned_Data.GetList_Planned(Client_Notification_Planned_ID: actual_Notifications.ID)?.Select(p => p.ClientID).ToList();
                }
                else //Всем пользователям
                {
                    list_ClientIDs = [.. _bl_Clients.GetList_Active(ActiveDays: AppConfig.Allowed_Activity_Days_Notification).Select(p => p.ID)];
                }


                if (list_ClientIDs != null && list_ClientIDs.Count > 0)
                {
                    //Заполнение Уведомлений в отправку сейчас
                    foreach (int client_ID in list_ClientIDs)
                    {
                        _dl_Client_Notification.Add_NoCommit(ClientID: client_ID, Name: actual_Notifications.Name, Content: actual_Notifications.Content, Push_Need: true);
                        list_ResultSuccess.Add(client_ID);
                    }

                    //Сохранение в БД 
                    if (!_dl_Client_Notification.Commit())
                    {
                        _log.LogError(message: "Err # {}", args: "Ошибка группового commit");
                        continue;
                    }

                }

                _dl_Client_Notification_Planned.MarkAs_Sent(ID: actual_Notifications.ID);
            }

            return list_ResultSuccess.Count;
        }


        /// <summary>
        /// Возвращает список запланированных уведомлений
        /// </summary>
        /// <returns></returns>
        public CClient_Notification_Planned? Get(int ID)
        {
            Client_Notification_Planned? elem = _dl_Client_Notification_Planned.GetQuery().Where(x => x.ID == ID).FirstOrDefault();
            
            return Get(Elem: elem);
        }



        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CClientNotification_History_ForAdmin> GetList()
        {
            List<CClientNotification_History_ForAdmin> list_result = [];
            foreach (Client_Notification_Planned elem in _dl_Client_Notification_Planned.GetQuery().ToList())
            {


                CClientNotification_History_ForAdmin? item = Get_History(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Удалить планируемое уведомление
        /// </summary>
        /// <returns></returns>
        public int Delete(int ID, bool Flag_Del)
        {
            return _dl_Client_Notification_Planned.Edit_FlagDel(ID: ID, Flag_Del: Flag_Del);
        }
    }
    
}
