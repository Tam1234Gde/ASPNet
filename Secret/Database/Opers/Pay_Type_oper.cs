using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Способы оплаты
    /// </summary>
    public class Pay_Type_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Pay_Type_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Pay_Type? Get(int ID)
        {
            Pay_Type? elem = (from p in _db.Pay_Types where p.ID == (EPayType)ID select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        public IQueryable<Pay_Type> GetQuery()
        {
            IQueryable<Pay_Type> query = from p in _db.Pay_Types select p;
            return query;
        }

    }
}