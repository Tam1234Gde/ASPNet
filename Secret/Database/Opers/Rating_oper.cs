using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Оценка приложения
    /// </summary>
    public class Rating_oper
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст БД
        /// </summary>
        private readonly DBContext _db = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Rating_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Rating? Get(int ID, bool? Flag_Del_val = false)
        {
            Rating? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Rating> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Rating> query = from p in _db.Ratings.Include(x => x.Client).AsNoTracking() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public Rating? GetQueryLast(int ClientID, bool? Flag_Del_val = false)
        {
            Rating? query = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ClientID == ClientID && p.Flag_Del == false orderby p.Date_Action descending select p).FirstOrDefault();
            
            return query;
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
            string loginfo = $"ClientID='{ClientID}' Grade='{Grade}' Text='{Text}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            Rating elem = new()
            {
                ClientID = ClientID,
                Grade = Grade,
                Text = Text,
                Phone = Phone,
                Restaurant_TableID = Restaurant_TableID,
                Date_Action = DateTime.Now,
            };

            _db.Ratings.Add(elem);
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
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err202_str}' ## {loginfo}");
                return Errors.Err202;
            }
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
            string loginfo = $"ClientID='{ClientID}' Grade='{Grade}' Text='{Text}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            

            Rating elem = new()
            {
                ClientID = ClientID,
                Grade = Grade,
                Text = Text,
                Date_Action = DateTime.Now,
            };

            _db.Ratings.Add(elem);
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
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err202_str}' ## {loginfo}");
                return Errors.Err202;
            }
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
            string loginfo = $"ID='{ID}' ClientID='{ClientID}' Grade='{Grade}' Text='{Text}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Rating? elem = Get(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.ClientID = ClientID;
            elem.Grade = Grade;
            elem.Text = Text;

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
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение пометки на удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Flag_Del_New"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del_New, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            Rating? elem = Get(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Flag_Del = Flag_Del_New;
            if (Flag_Del_New)
            {
                elem.Date_Del = DateTime.Now;
                elem.UserID_Del = UserID;
            }
            else
            {
                elem.Date_Del = null;
                elem.UserID_Del = null;
            }

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
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}", exception: ex);
                }
                return Errors.Err203;
            }
        }

    }
}