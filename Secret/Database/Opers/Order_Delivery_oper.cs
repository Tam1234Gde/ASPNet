using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Доставки
    /// </summary>
    public class Order_Delivery_oper
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
        public Order_Delivery_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Order_Delivery? Get(int ID, bool? Flag_Del_val = false)
        {
            Order_Delivery? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }

        /// <summary>
        /// Получение по OrderID
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public Order_Delivery? GetByOrderID(int OrderID, bool? Flag_Del_val = false)
        {
            Order_Delivery? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.OrderID == OrderID select p).FirstOrDefault();

            return elem;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Order_Delivery> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Order_Delivery> query = from p in _db.Order_Deliveries.Include(x => x.Order) select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="Delivery_EntranceNumber"></param>
        /// <param name="Delivery_EntranceCode"></param>
        /// <param name="Delivery_Floor"></param>
        /// <param name="Delivery_Handing"></param>
        /// <param name="Delivery_Address"></param>
        /// <param name="Delivery_Comment"></param>
        /// <param name="Delivery_To"></param>
        /// <returns></returns>
        public int Add(int OrderID, string? Delivery_Apartment, string? Delivery_EntranceNumber, string? Delivery_EntranceCode, string? Delivery_Floor, decimal? Delivery_Handing,
                       string? Delivery_Address, string? Delivery_Comment, DateTime? Delivery_To)
        {
            string loginfo = $"OrderID='{OrderID}' Delivery_EntranceNumber='{Delivery_EntranceNumber}' Delivery_EntranceCode='{Delivery_EntranceCode}' " +
                             $"Delivery_Floor='{Delivery_Floor}' Delivery_Handing='{Delivery_Handing}' Delivery_Address='{Delivery_Address}' " +
                             $"Delivery_Comment='{Delivery_Comment}' Delivery_To='{Delivery_To}' Delivery_Apartment='{Delivery_Apartment}'";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order_Delivery elem = new()
            {
                OrderID = OrderID,
                Delivery_EntranceNumber = Delivery_EntranceNumber,
                Delivery_EntranceCode = Delivery_EntranceCode,
                Delivery_Floor = Delivery_Floor,
                Delivery_Handing = Delivery_Handing,
                Delivery_Address = Delivery_Address,
                Delivery_Comment = Delivery_Comment,
                Delivery_To = Delivery_To,
                Delivery_Apartment = Delivery_Apartment,
            };

            _db.Order_Deliveries.Add(elem);

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

    }
}