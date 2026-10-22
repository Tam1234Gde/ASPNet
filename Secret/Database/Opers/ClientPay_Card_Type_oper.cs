using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Типы карты
    /// </summary>
    public class ClientPay_Card_Type_oper
    {
        /// <summary>
        /// Логер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly DBContext _db;


        /// <summary>
        /// Конструктор
        /// </summary>
        public ClientPay_Card_Type_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
            _db = new();
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        private IQueryable<ClientPay_Card_Type> GetQuery()
        {
            IQueryable<ClientPay_Card_Type> query = from p in _db.ClientPay_Card_Types.AsNoTracking()
                                                    select p;
            return query;
        }


        /// <summary>
        /// Получить по Системному имени
        /// </summary>
        /// <param name="SName">Системное имя карты</param>
        /// <returns></returns>
        public EClientPay_Card_Type Get_ByName(string SName)
        {
            EClientPay_Card_Type? id = (from p in GetQuery() where p.SName == SName select p.ID).FirstOrDefault();

            if (id == null)
            {
                return EClientPay_Card_Type._NotSet;
            }

            return id.Value;
        }

    }
}