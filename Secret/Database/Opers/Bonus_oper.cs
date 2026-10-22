using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Бонусы клиента
    /// </summary>
    public class Bonus_oper
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly DBContext _db;


        /// <summary>
        /// Конструктор
        /// </summary>
        public Bonus_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        public IQueryable<Bonus> GetQuery()
        {
            IQueryable<Bonus> query = from p in _db.Bonuses.Include(x => x.Client).AsNoTracking() select p;
            return query;
        }


        /// <summary>
        /// Получение запроса для редактирования
        /// </summary>
        /// <returns></returns>
        public IQueryable<Bonus> GetQuery_Edit()
        {
            IQueryable<Bonus> query = from p in _db.Bonuses.Include(x => x.Client) select p;
            return query;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Bonus? Get(int ID)
        {
            Bonus? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение по ID для редактирования
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Bonus? Get_Edit(int ID)
        {
            Bonus? elem = (from p in GetQuery_Edit() where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="ClientID">Идентификатор клиента</param>
        /// <returns></returns>
        public IQueryable<Bonus> GetQuery_ByClientID(int ClientID) //TODO: MAE - проверить корректность работы
        {
            IQueryable<Bonus> query = from p in GetQuery()
                                      where p.ClientID == ClientID
                                      select p;

            return query;
        }


        /// <summary>
        /// Получение запроса для редактирования
        /// </summary>
        /// <returns></returns>
        public IQueryable<Bonus> GetQuery_Edit(int ClientID)
        {
            IQueryable<Bonus> query = from p in _db.Bonuses.Include(x => x.Client)
                                      where p.ClientID == ClientID
                                      select p;

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="ClientID">Код клиента, по которому идет движение бонусов</param>
        /// <param name="Bonuses"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public int Add(int ClientID, decimal Bonuses, int OrderID)
        {
            string loginfo = $"ClientID='{ClientID}' Bonuses='{Bonuses}' OrderID='{OrderID}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Bonus elem = new()
            {
                ClientID = ClientID,
                Bonuses = Bonuses,
                OrderID = OrderID,
                Date_Action = DateTime.Now,
            };

            _db.Bonuses.Add(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "OK # {}", args: loginfo);
                }
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(exception: ex, message: "Err # Error='{}' ## {}", args: [Errors.Err202_str, loginfo]);
                }
                return Errors.Err202;
            }
        }


        /// <summary>
        /// Помечает на удаление всех бонусов пользователя
        /// </summary>
        /// <returns>Код клиента если все прошло успешно или код ошибки</returns>
        public int SetAll_FlagDel(int ClientID)
        {
            string loginfo = $"ClientID='{ClientID}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            List<Bonus> list = [.. GetQuery_Edit(ClientID: ClientID)];
            list.ForEach(a => { a.Bonuses = 0; });

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "OK # {}", args: loginfo);
                }
                return ClientID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(exception: ex, message: "Err # Error='{}' ## {}", args: [Errors.Err203_str, loginfo]);
                }
                return Errors.Err203;
            }
        }

    }
}