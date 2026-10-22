using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Класс работы с БД оплат карт
    /// </summary>
    public class ClientPay_Card_Pay_oper
    {
        /// <summary>
        /// Логер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст БД
        /// </summary>
        private readonly DBContext _db;
        

        /// <summary>
        /// Конструктор
        /// </summary>
        public ClientPay_Card_Pay_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
            _db = new();
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns>Данные Client</returns>
        private ClientPay_Card_Pay? Get_Edit(int ID)
        {
            ClientPay_Card_Pay? elem = (from p in _db.ClientPay_Card_Pays.Include(x => x.Client).Include(x => x.Order).Include(x => x.ClientPay_Card_Pay_Type).Include(x => x.ClientPay_Card_Data) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public int Add(int ClientID, int? OrderID, EClientPay_Card_Pay_Type ClientPay_Card_Pay_TypeID, int ClientPay_Card_DataID, string? PaymentIdent, decimal? Summ)
        {
            string loginfo = $"ClientID='{ClientID}' OrderID='{OrderID}' ClientPay_Card_Pay_TypeID='{ClientPay_Card_Pay_TypeID}' " +
                             $"ClientPay_Card_DataID='{ClientPay_Card_DataID}' PaymentIdent='{PaymentIdent}' Summ='{Summ}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            ClientPay_Card_Pay elem = new()
            {
                Date_Create = DateTime.Now,
                ClientID = ClientID,
                OrderID = OrderID,
                ClientPay_Card_Pay_TypeID = ClientPay_Card_Pay_TypeID,
                ClientPay_Card_DataID = ClientPay_Card_DataID,
                PaymentIdent = PaymentIdent,
                Summ = Summ,
            };

            _db.ClientPay_Card_Pays.Add(elem);

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
        /// <param name="FlagComplete_val">Новое значение флага</param>
        /// <returns></returns>
        public int Edit_FlagComplete(int ID, bool FlagComplete_val)
        {
            string loginfo = $"ID='{ID}' FlagComplete_val='{FlagComplete_val}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            ClientPay_Card_Pay? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }
            elem.Flag_Complete = FlagComplete_val;
            if (FlagComplete_val)
            {
                elem.Date_Complete = DateTime.Now;
            }
            else
            {
                elem.Date_Complete = null;
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


        #region DEL By MAE - Не используется - 23.04.2026
        ///// <summary>
        ///// Получение запроса
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<ClientPay_Card_Pay> GetQuery()
        //{
        //    IQueryable<ClientPay_Card_Pay> query = from p in _db.ClientPay_Card_Pays
        //                                           .Include(x => x.Client).Include(x => x.ClientPay_Card_Pay_Type)
        //                                           .Include(x => x.ClientPay_Card_Data).AsNoTracking()
        //                                           select p;
        //    return query;
        //}

        ///// <summary>
        ///// Получение запроса
        ///// </summary>
        ///// <param name="ClientID">Идентификатор клиента</param>
        ///// <returns></returns>
        //public IQueryable<ClientPay_Card_Pay> GetQuery(int ClientID)
        //{
        //    IQueryable<ClientPay_Card_Pay> query = from p in GetQuery()
        //                                           where p.ClientID == ClientID
        //                                           select p;
        //    return query;
        //}


        ///// <summary>
        ///// Получение запроса
        ///// </summary>
        ///// <param name="ClientID">Идентификатор клиента</param>
        ///// <param name="OrderID">Идентификатор заказа</param>
        ///// <returns></returns>
        //public IQueryable<ClientPay_Card_Pay> GetQuery(int ClientID, int OrderID)
        //{
        //    IQueryable<ClientPay_Card_Pay> query = from p in GetQuery()
        //                                           where p.ClientID == ClientID && p.OrderID == OrderID
        //                                           select p;
        //    return query;
        //}


        ///// <summary>
        ///// Получение по ID
        ///// </summary>
        ///// <param name="ID">Код записи, которую нужно получить</param>
        ///// <returns>Данные Client</returns>
        //public ClientPay_Card_Pay? Get(int ID)
        //{
        //    ClientPay_Card_Pay? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();
        //    return elem;
        //} 
        #endregion DEL By MAE - Не используется - 23.04.2026
    }
}