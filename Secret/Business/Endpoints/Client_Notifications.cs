using Microsoft.Extensions.Logging;
using System.Text;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Нотификации клиентов
    /// </summary>
    public class Client_Notifications
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;
        private readonly Client_Notification_oper _dl_Client_Notification = new();
        private readonly Client_Notification_Shablons _bl_Client_Notification_Shablons = new();
        private readonly Order_oper _dl_Order = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Client_Notifications()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CClient_Notification? Get(Client_Notification? Elem)
        {
            if (Elem != null)
            {
                CClient_Notification item = new()
                {
                    ID = Elem.ID,
                    ClientID = Elem.ClientID,
                    Date_Act = Elem.Date_Act,
                    Name = Elem.Name,
                    Content = Elem.Content,
                    Flag_Read = Elem.Date_Read.HasValue,
                    OrderID = Elem.ID,
                    Client_Notification_ShablonID = Elem.Client_Notification_ShablonID,
                };
                return item;
            }
            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        public CClient_Notification? Get(int ID)
        {
            return Get(Elem: _dl_Client_Notification.Get(ID));
        }


        /// <summary>
        /// Получение даты последней нотификации клиенту. 
        /// </summary>
        /// <param name="ClientID">Код клиента</param>
        /// <returns>Возвращает дату последней нотификации или null если нотификаций не было</returns>
        public DateTime? GetLastEventDate(int ClientID)
        {
            IQueryable<Client_Notification> query = _dl_Client_Notification.GetQuery(ClientID: ClientID);

            DateTime? date_create_max = (from p in query orderby p.Date_Act descending select p).Max(p => (DateTime?)p.Date_Act);
            DateTime? date_read_max = (from p in query orderby p.Date_Act descending select p).Max(p => (DateTime?)p.Date_Read);

            List<DateTime> list = [];

            if (date_create_max.HasValue)
            {
                list.Add(date_create_max.Value);
            }

            if (date_read_max.HasValue)
            {
                list.Add(date_read_max.Value);
            }

            if (list.Count != 0)
            {
                return list.Max();
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public int GetUnreadedCount(int ClientID)
        {
            return _dl_Client_Notification.GetQuery(ClientID: ClientID).Where(p => p.Date_Read == null).Count();
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CClient_Notification> GetList(int ClientID, bool? Flag_Del_val = false)
        {
            List<CClient_Notification> list_result = [];

            foreach (Client_Notification elem in _dl_Client_Notification.GetQuery(ClientID: ClientID, Flag_Del_val: Flag_Del_val).ToList())
            {
                CClient_Notification? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Проверка наличия шаблона
        /// </summary>
        /// <param name="ShablonID"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public Client_Notification? CheckNotification(EClient_Notification_Shablon ShablonID, int OrderID)
        {
            return _dl_Client_Notification.Get(ShablonID: ShablonID, OrderID: OrderID);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public int Add(int ClientID, string Name, string Content, bool Push_Need, int? OrderID, EClient_Notification_Shablon? Client_Notification_ShablonID)
        {
            return _dl_Client_Notification.Add(ClientID: ClientID, Name: Name, Content: Content, Push_Need: Push_Need, OrderID: OrderID,
                                               Client_Notification_ShablonID: Client_Notification_ShablonID);
        }


        /// <summary>
        /// Добавление нотификации пользователю по шаблону
        /// </summary>
        /// <returns></returns>
        public int Add_ByShablon(int ClientID, int? OrderID, EClient_Notification_Shablon ShablonID, string? ReHIDEText1 = null, string? ReHIDEText2 = null, string? ReHIDEText3 = null)
        {
            string loginfo = $"ClientID='{ClientID}' OrderID='{OrderID}' ShablonID='{ShablonID}' ReHIDEText1='{ReHIDEText1}' ReHIDEText2='{ReHIDEText2}' ReHIDEText3='{ReHIDEText3}'";

            CClient_Notification_Shablon_ForAdmin? shablon = _bl_Client_Notification_Shablons.Get(ID: (int)ShablonID);

            if (shablon == null)
            {
                if (_log.IsEnabled(LogLevel.Information))
                {
                    _log.LogInformation(message: "{}", args: loginfo);
                }
                return Errors.Err200;
            }

            StringBuilder content = new();

            string? line = shablon.Content;
            string name = shablon.Name.ReHIDE(oldValue: Consts_Notification.Notification_ReHIDEKey_1, newValue: ReHIDEText1);

            if (!string.IsNullOrWhiteSpace(ReHIDEText1))
            {
                line = line.ReHIDE(oldValue: Consts_Notification.Notification_ReHIDEKey_1, newValue: ReHIDEText1);
            }


            if (!string.IsNullOrWhiteSpace(ReHIDEText2))
            {
                line = line.ReHIDE(oldValue: Consts_Notification.Notification_ReHIDEKey_2, newValue: ReHIDEText2);
            }


            if (!string.IsNullOrWhiteSpace(ReHIDEText3))
            {
                line = line.ReHIDE(oldValue: Consts_Notification.Notification_ReHIDEKey_3, newValue: ReHIDEText3);
            }

            content.Append(line);

            int result = Add(ClientID: ClientID, Name: name, Content: content.ToString(), Push_Need: shablon.Push_Need, OrderID: OrderID,
                                  Client_Notification_ShablonID: ShablonID);

            if (result < 0)
            {
                if (_log.IsEnabled(LogLevel.Information))
                {
                    _log.LogInformation(message: "{}", args: $"result='{result}' " + loginfo);
                }
            }

            return result;
        }


        /// <summary>
        /// Прочтение уведомления
        /// </summary>
        /// <param name="ID">Код уведомления</param>
        /// <returns>Код уведомления или код ошибки</returns>
        public int Read(int ID)
        {
            return _dl_Client_Notification.Set_DateRead(ID: ID, Date_Read: DateTime.Now);
        }


        /// <summary>
        /// Прочитать все уведомления
        /// </summary>
        /// <param name="ClientID">Идентификатор клиента</param>
        /// <returns></returns>
        public int Read_All(int ClientID)
        {
            return _dl_Client_Notification.Set_DateRead_All(ClientID);
        }


        /// <summary>
        /// Снятие прочтения с уведомления
        /// </summary>
        /// <param name="ID">Код уведомления</param>
        /// <returns>Код уведомления или код ошибки</returns>
        public int UnRead(int ID)
        {
            return _dl_Client_Notification.Set_DateRead(ID: ID, Date_Read: null);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Del(int ID)
        {
            return _dl_Client_Notification.Edit_FlagDel(ID: ID, FlagDel_val: true);
        }


        /// <summary>
        /// Удаляет все уведомления
        /// </summary>
        /// <param name="ClientID">Идентификатор клиента</param>
        /// <returns></returns>
        public int Del_All(int ClientID)
        {
            return _dl_Client_Notification.Del_All(ClientID);
        }


        /// <summary>
        /// Возвращает список
        /// <param name="MaxLifeHour">Максимальное время в часах которое должно жить уведомление</param>
        /// </summary>
        /// <returns></returns>
        public List<CClient_Notification> GetList_NeedPush(int MaxLifeHour)
        {


            List<CClient_Notification> list_result = [];

            IQueryable<Client_Notification> query = from p in _dl_Client_Notification.GetQuery()
                                                    where p.Push_Need &&
                                                          p.Push_DateSend == null &&
                                                          p.Date_Act.AddHours(MaxLifeHour) > DateTime.Now
                                                    select p;

            foreach (Client_Notification elem in query.ToList())
            {
                CClient_Notification? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Помечает уведомление как отправлено
        /// </summary>
        /// <param name="ID">Идентификатор уведомления</param>
        /// <returns></returns>
        public int MarkAs_Sent(int ID)
        {
            return _dl_Client_Notification.MarkAs_Sent(ID);
        }


        /// <summary>
        /// Создание уведомлений для отправки
        /// <param name="MaxLifeHour">Максимальное время в часах которое должно жить уведомление</param>
        /// </summary>
        /// <returns>Уведомления для отправки</returns>
        public List<CClient_Notification>? AddNotifications(int MaxLifeHour)
        {
            // Получаем все активные заказы
            IQueryable<Order> orders = from p in _dl_Order.GetQueryConsole() select p;

            if (orders != null && !orders.Any() || orders == null)
            {
                return null;
            }

            // Создаем уведомления
            foreach (Order order in orders.ToList())
            {
                // Через 1 час начнется ваша бронь столика №n.
                if (order.Booking != null && order.Booking.Date_Start.Subtract(DateTime.Now).TotalMinutes <= 60 && order.Booking.Date_Start.Subtract(DateTime.Now).TotalMinutes > 55)
                {
                    if (CheckNotification(EClient_Notification_Shablon.BookingHour, OrderID: order.ID) == null)
                    {
                        Add_ByShablon(ClientID: HIDE_Convert.ToInt(order.ClientID), ShablonID: EClient_Notification_Shablon.BookingHour,
                                           ReHIDEText1: order.Booking.Name, OrderID: order.ID);
                    }
                }

                // Ваш заказ №n начали готовить. Бронь столика начнется через 15 мин.
                if (order.Booking != null && order.Booking.Date_Start.Subtract(DateTime.Now).TotalMinutes <= 15 && order.Booking.Date_Start.Subtract(DateTime.Now).TotalMinutes > 13)
                {
                    if (CheckNotification(EClient_Notification_Shablon.BookingMinutes, OrderID: order.ID) == null)
                    {
                        Add_ByShablon(ClientID: HIDE_Convert.ToInt(order.ClientID), ShablonID: EClient_Notification_Shablon.BookingMinutes,
                                           ReHIDEText1: order.ID.ToString(), OrderID: order.ID);
                    }
                }

                // Завтра за вами забронирован столик №n на XX:XX.
                if (order.Booking != null && order.Booking.Date_Start.Subtract(DateTime.Now).TotalDays < 1 && order.Booking.Date_Start.Date != DateTime.Now.Date)
                {
                    if (CheckNotification(EClient_Notification_Shablon.BookingTomorrow, OrderID: order.ID) == null)
                    {
                        Add_ByShablon(ClientID: HIDE_Convert.ToInt(order.ClientID), ShablonID: EClient_Notification_Shablon.BookingTomorrow,
                                           ReHIDEText1: order.Booking.Name, ReHIDEText2: order.Booking.Date_Start.ToShortTimeString(), OrderID: order.ID);
                    }
                }

                // Сегодня за вами забронирован столик №n на XX:XX. - Отправлять в 09.00
                if (order.Booking != null && order.Booking.Date_Start.Date == DateTime.Now.Date && DateTime.Now >= DateTime.Now.Date.AddHours(9) && DateTime.Now < DateTime.Now.Date.AddHours(10))
                {
                    if (CheckNotification(EClient_Notification_Shablon.BookingToday, OrderID: order.ID) == null)
                    {
                        Add_ByShablon(ClientID: HIDE_Convert.ToInt(order.ClientID), ShablonID: EClient_Notification_Shablon.BookingToday,
                                           ReHIDEText1: order.Booking.Name, ReHIDEText2: order.Booking.Date_Start.ToShortTimeString(), OrderID: order.ID);
                    }
                }

                // Ваш заказ №n ожидает вас. Столик забронирован за вами до XX:XX.
                if (order.Booking != null && DateTime.Now >= order.Booking.Date_Start && order.Order_StatusID != EOrder_Status.Complete)
                {
                    if (CheckNotification(EClient_Notification_Shablon.BookingNotComplete, OrderID: order.ID) == null)
                    {
                        Add_ByShablon(ClientID: HIDE_Convert.ToInt(order.ClientID), ShablonID: EClient_Notification_Shablon.BookingNotComplete,
                                           ReHIDEText1: order.ID.ToString(), ReHIDEText2: order.Booking.Date_End.ToShortTimeString(), OrderID: order.ID);
                    }
                }
            }

            List<CClient_Notification> list_result = [];

            IQueryable<Client_Notification> query = from p in _dl_Client_Notification.GetQuery_Edit()
                                                    where p.Push_Need && p.Push_DateSend == null &&
                                                    p.Date_Act.AddHours(MaxLifeHour) > DateTime.Now &&
                                                    p.Push_DateSend == null
                                                    select p;

            foreach (Client_Notification elem in query.ToList())
            {
                CClient_Notification? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

    }
}