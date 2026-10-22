using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Тип заказа(С собой\Поесть в кафе\Доставка)
    /// </summary>
    public class Order_Type_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Order_Type_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Order_Type? Get(int ID)
        {
            Order_Type? elem = (from p in _db.Order_Types where p.ID == (EOrder_Type)ID select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        private IQueryable<Order_Type> GetQuery_NoTracking()
        {
            IQueryable<Order_Type> query = from p in _db.Order_Types.AsNoTracking() select p;
            return query;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        public IQueryable<Order_Type> GetQuery()
        {
            IQueryable<Order_Type> query = from p in GetQuery_NoTracking() select p;
            return query;
        }

    }
}