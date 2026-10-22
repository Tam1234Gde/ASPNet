using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Нотификации клиентов
    /// </summary>
    public class Client_Notification_Shablons
    {
        private readonly ILogger _log;
        private readonly Client_Notification_Shablon_oper _dl_Client_Notification_Shablon = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Client_Notification_Shablons()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CClient_Notification_Shablon_ForAdmin? Get(Client_Notification_Shablon? Elem)
        {
            if (Elem != null)
            {
                CClient_Notification_Shablon_ForAdmin item = new()
                {
                    ID = (int)Elem.ID,
                    Name = Elem.Name,
                    Content = Elem.Content,
                    Push_Need = Elem.Push_Need,
                };
                return item;
            }
            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <returns></returns>
        public CClient_Notification_Shablon_ForAdmin? Get(int ID)
        {
            return Get(Elem: _dl_Client_Notification_Shablon.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CClient_Notification_Shablon_ForAdmin> GetList(bool? Flag_Del_val = false)
        {
            List<CClient_Notification_Shablon_ForAdmin> list_result = [];

            foreach (Client_Notification_Shablon elem in _dl_Client_Notification_Shablon.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CClient_Notification_Shablon_ForAdmin? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public int Add(int ID, string Name, string Content, bool Push_Need)
        {
            return _dl_Client_Notification_Shablon.Add(ID: ID, Name: Name, Content: Content, Push_Need: Push_Need);
        }


        /// <summary>
        /// Установить на уведомлении текущую дату прочтения
        /// </summary>
        /// <returns></returns>
        public int Edit(int ID, string Name, string Content, bool Push_Need)
        {
            return _dl_Client_Notification_Shablon.Edit(ID: ID, Name: Name, Content: Content, Push_Need: Push_Need);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            return _dl_Client_Notification_Shablon.Edit_FlagDel(ID: ID, FlagDel_val: true, UserID: UserID);
        }

    }
}