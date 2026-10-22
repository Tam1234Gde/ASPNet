using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Класс работя с БД даныых клиентских карт
    /// </summary>
    public class ClientPay_Card_Data_oper
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контенкст базы данных
        /// </summary>
        private readonly DBContext _db;


        /// <summary>
        /// Конструктор
        /// </summary>
        public ClientPay_Card_Data_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
            _db = new();
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        private IQueryable<ClientPay_Card_Data> GetQuery()
        {
            IQueryable<ClientPay_Card_Data> query = from p in _db.ClientPay_Card_Datas.Include(p => p.ClientPay_Card_Type).Include(p => p.Client).AsNoTracking() select p;
            return query;
        }


        /// <summary>
        /// Получение запроса для редактирования
        /// </summary>
        /// <returns></returns>
        private IQueryable<ClientPay_Card_Data> GetQuery_Edit()
        {
            IQueryable<ClientPay_Card_Data> query = from p in _db.ClientPay_Card_Datas.Include(p => p.ClientPay_Card_Type).Include(p => p.Client) select p;
            return query;
        }


        /// <summary>
        /// Получение по ID для редактирования
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns>Данные Client</returns>
        private ClientPay_Card_Data? Get_Edit(int ID)
        {
            ClientPay_Card_Data? elem = (from p in GetQuery_Edit() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса - для редактирования
        /// </summary>
        /// <returns></returns>
        public IQueryable<ClientPay_Card_Data> GetQuery_Edit_ByClientID(int ClientID)
        {
            IQueryable<ClientPay_Card_Data> query = from p in GetQuery_Edit() where p.ClientID == ClientID select p;
            return query;
        }
       

        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns>Данные Client</returns>
        public ClientPay_Card_Data? Get(int ID)
        {
            ClientPay_Card_Data? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        public IQueryable<ClientPay_Card_Data> GetQuery(int ClientID, bool? Flag_Del_val = false)
        {
            IQueryable<ClientPay_Card_Data> query = from p in GetQuery() where p.ClientID == ClientID select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Проверка существования идентификатора карты, полученного от платежного сервиса
        /// </summary>
        /// <param name="CardIdent"></param>
        /// <returns></returns>
        public bool Check_CardIdentPresent(int ClientID, string CardIdent)
        {
            int count = (from p in GetQuery(ClientID: ClientID).Where(p => p.CardIdent == CardIdent) select p).Count();
            return count > 0;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public int Add(int ClientID, EClientPay_Card_Type ClientPay_Card_TypeID, string CardIdent, string CardLastNum)
        {
            string loginfo = $"ClientID='{ClientID}' CardIdent='{CardIdent}' CardLastNum='{CardLastNum}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            ClientPay_Card_Data elem = new()
            {
                ClientID = ClientID,
                ClientPay_Card_TypeID = ClientPay_Card_TypeID,
                CardIdent = CardIdent,
                CardLastNum = CardLastNum,
                Date_Update = DateTime.Now,
            };

            _db.ClientPay_Card_Datas.Add(elem);

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
        /// Изменение флага пометки на удаление
        /// </summary>
        /// <param name="ID">Код записи, у которой нужно поменять флаг</param>
        /// <param name="FlagDel_val">Новое значение флага</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool FlagDel_val)
        {
            string loginfo = $"ID='{ID}' FlagDel_val='{FlagDel_val}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            ClientPay_Card_Data? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Date_Update = DateTime.Now;
            elem.Flag_Del = FlagDel_val;
            if (FlagDel_val)
            {
                elem.CardIdent = $"DEL: {Guid.NewGuid()}";
                elem.Date_Del = DateTime.Now;
            }
            else
            {
                elem.Date_Del = null;
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
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Помечает на удаление всех карт пользователя
        /// </summary>
        /// <param name="ClientID">Идентификатор клиента</param>
        /// <returns>Код клиента если все прошло успешно или код ошибки</returns>
        public int SetAll_FlagDel(int ClientID)
        {
            string loginfo = $"ClientID='{ClientID}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            List<ClientPay_Card_Data> list = [.. GetQuery_Edit_ByClientID(ClientID: ClientID)];
            list.ForEach(a => { a.Flag_Del = true; a.Date_Update = DateTime.Now; a.CardIdent = $"DEL: {Guid.NewGuid()}"; });

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return ClientID;
            }
            catch (Exception ex)
            {
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");
                return Errors.Err203;
            }
        }
    }
}