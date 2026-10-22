using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Opers;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Работа с Excel
    /// </summary>
    public class Client_Notifications_Excel
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст работы с БД Client_Notification
        /// </summary>
        private readonly Client_Notification_oper _dl_Client_Notification = new();


        /// <summary>
        /// Контекст работы с БД Client_Notification
        /// </summary>
        private readonly Client_Notifications_Planned _bl_Client_Notification_Planned = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Client_Notifications_Excel()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Считывание данных
        /// </summary>
        /// <param name="ExcelFileName"></param>
        /// <returns></returns>
        public static List<int> GetFirstColumnData(string ExcelFileName)
        {
            List<int> list_result = [];
            ExcelPackage.License.SetNonCommercialPersonal(AppConfig.Excel_CommercialPersonal);
            using (ExcelPackage package = new(ExcelFileName))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
                int rowCount = worksheet.Dimension.End.Row;

                for (int row = 3; row <= rowCount; row++)
                {
                    string? val = worksheet.Cells[row, 1]?.Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        list_result.Add(HIDE_Convert.ToInt(val));
                    }
                }
            }

            return list_result;
        }


        /// <summary>
        /// Добавление уведомлений
        /// </summary>
        /// <param name="Name">Заголовок уведомления</param>
        /// <param name="Content">Описание уведомления</param>
        /// <param name="List_Clients">Список клиентов</param>
        /// <param name="Date_Send">Дата нотификации</param>
        /// <returns></returns>
        public int Add_ClientNotifications(string Name, string Content, List<int> List_Clients, DateTime? Date_Send)
        {
            if(Date_Send == null) //Немедленная отправка пуш-уведомлений
            {
                if (List_Clients != null && List_Clients.Count != 0)
                {
                    int? result = _bl_Client_Notification_Planned.Add_ForUsers(Name: Name, Content: Content, Date_Send: DateTime.Now, List_ClientsIDs: List_Clients);

                    return result ?? Errors.Err205;
                }
                else
                {
                    return _bl_Client_Notification_Planned.Add(Name: Name, Content: Content, Date_Send: DateTime.Now);
                }
            }
            else //Отправка запланированных пуш-уведомлений
            {
                if (List_Clients != null && List_Clients.Count != 0)
                {
                    int? result = _bl_Client_Notification_Planned.Add_ForUsers(Name: Name, Content: Content, Date_Send: Date_Send.Value, List_ClientsIDs: List_Clients);

                    return result ?? Errors.Err205;
                }
                else
                {
                    return _bl_Client_Notification_Planned.Add(Name: Name, Content: Content, Date_Send: Date_Send.Value);
                }                    
            }
        }


        /// <summary>
        /// Добавление уведомлений всем клиентам
        /// </summary>
        /// <param name="Name">Заголовок уведомления</param>
        /// <param name="Content">Описание уведомления</param>
        /// <returns></returns>
        public int Add_AllClientNotifications(string Name, string Content)
        {
            _dl_Client_Notification.Add_NoCommit(ClientID: null, Name: Name, Content: Content, Push_Need: true);

            if (_dl_Client_Notification.Commit())
            {
                return Consts.Status_Success;
            }

            return Errors.Err205;
        }

    }
}