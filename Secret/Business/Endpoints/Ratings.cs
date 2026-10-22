using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Оценка приложения
    /// </summary>
    public class Ratings
    {
        private readonly ILogger _log;
        private readonly Rating_oper _dl_Rating = new();
        private readonly Order_oper _dl_Order = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Ratings()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CRating? Get(Rating? Elem)
        {
            if (Elem != null)
            {
                CRating item = new()
                {
                    ID = Elem.ID,
                    ClientID = Elem.ClientID,
                    Client = Elem.Client?.Phone,
                    Grade = Elem.Grade,
                    Text = Elem.Text,
                    Phone = Elem.Phone,
                    Restaurant_TableID = Elem.Restaurant_TableID,
                    Date_Action = Elem.Date_Action,
                };
                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CRating? Get(int ID)
        {
            return Get(Elem: _dl_Rating.Get(ID));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRating>? GetList(bool? Flag_Del_val = false)
        {
            List<CRating> list_result = [];

            foreach (Rating elem in _dl_Rating.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CRating? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Проверка требуется ли предложить оценку пользователю
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public bool CheckRating(int ClientID)
        {
            IQueryable<Order> query_orders = from p in _dl_Order.GetQuery(ClientID: ClientID) select p;
            Rating? elem = _dl_Rating.GetQueryLast(ClientID: ClientID);

            if ((query_orders.ToList().Count > 3 && elem == null) || (elem != null && elem.Date_Action < DateTime.Now.AddDays(-90)))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="ClientID">Код клиента</param>
        /// <param name="Grade">Оценка(от 1 до 5)</param>
        /// <param name="Text">Отзыв</param>
        /// <returns></returns>
        public int Add(int ClientID, int Grade, string? Text)
        {
            return _dl_Rating.Add(ClientID: ClientID, Grade: Grade, Text: Text);
        }


        /// <summary>
        /// Добавление(для сайта меню)
        /// </summary>
        /// <param name="ClientID">Код клиента</param>
        /// <param name="Grade">Оценка(от 1 до 5)</param>
        /// <param name="Text">Отзыв</param>
        /// <param name="Phone">Телефон</param>
        /// <param name="Restaurant_TableID">Код столика</param>
        /// <returns></returns>
        public int AddSite(int ClientID, int Grade, string? Text, string? Phone, int? Restaurant_TableID)
        {
            return _dl_Rating.AddSite(ClientID: ClientID, Grade: Grade, Text: Text, Phone: Phone, Restaurant_TableID: Restaurant_TableID);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="ClientID">Код клиента</param>
        /// <param name="Grade">Оценка(от 1 до 5)</param>
        /// <param name="Text">Отзыв</param>
        /// <returns></returns>
        public int Edit(int ID, int ClientID, int Grade, string? Text)
        {
            return _dl_Rating.Edit(ID: ID, ClientID: ClientID, Grade: Grade, Text: Text);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            return _dl_Rating.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }

    }
}