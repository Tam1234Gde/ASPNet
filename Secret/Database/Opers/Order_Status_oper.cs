using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    public class Order_Status_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Order_Status_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Order_Status? Get(int ID)
        {
            Order_Status? elem = (from p in _db.Order_Statuses where p.ID == (EOrder_Status)ID select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        private IQueryable<Order_Status> GetQuery_NoTracking()
        {
            IQueryable<Order_Status> query = from p in _db.Order_Statuses.AsNoTracking() select p;
            return query;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        public IQueryable<Order_Status> GetQuery()
        {
            IQueryable<Order_Status> query = from p in GetQuery_NoTracking() select p;
            return query;
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="ColorHex"></param>
        /// <returns></returns>
        public int Edit(int ID, string Name, string? ColorHex, int OrderNum)
        {
            string loginfo = $"ID='{ID}' Name='{Name}' ColorHex='{ColorHex}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order_Status? elem = Get(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Name = Name;
            elem.ColorHex = ColorHex;
            elem.OrderNum = OrderNum;
            

            try
            {
                _db.SaveChanges();

                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return (int)elem.ID;
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


    }
}
