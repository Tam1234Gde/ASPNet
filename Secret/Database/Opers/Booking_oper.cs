using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Бронирование
    /// </summary>
    public class Booking_oper
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
        public Booking_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Booking> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Booking> query = from p in _db.Bookings.Include(x => x.Client).Include(x => x.Restaurant_Table).AsNoTracking() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса для редактирования
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Booking> GetQuery_Edit(bool? Flag_Del_val = false)
        {
            IQueryable<Booking> query = from p in _db.Bookings.Include(x => x.Client).Include(x => x.Restaurant_Table) select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Booking? Get(int ID, bool? Flag_Del_val = false)
        {
            Booking? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по ID для редактирования
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Booking? GetEdit(int ID, bool? Flag_Del_val = false)
        {
            Booking? elem = (from p in GetQuery_Edit(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Booking> GetQueryByRestaurant_TableID(int Restaurant_TableID, bool? Flag_Del_val = false)
        {
            IQueryable<Booking> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.Restaurant_TableID == Restaurant_TableID select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<string> GetQuery(int Restaurant_TableID, DateTime Date_Booking, bool? Flag_Del_val = false)
        {
            IQueryable<Booking> query = from p in GetQuery(Flag_Del_val: Flag_Del_val)
                                        where p.Restaurant_TableID == Restaurant_TableID &&
                                        p.Date_Start.Date == Date_Booking.Date &&
                                        (p.Date_Start > DateTime.Now || (DateTime.Now >= p.Date_Start && DateTime.Now < p.Date_End))
                                        orderby p.Date_End
                                        select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            IQueryable<string> query_result = from p in query select $"{p.Date_Start:HH:mm} - {p.Date_End:HH:mm}";

            return query_result;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Booking> GetQueryBooking(int Restaurant_TableTypeID, DateTime Date_Booking, bool? Flag_Del_val = false)
        {
            IQueryable<Booking> query = from p in GetQuery(Flag_Del_val: Flag_Del_val)
                                        where p.Restaurant_Table!.Restaurant_Table_TypeID == (ERestaurant_Table_Type)Restaurant_TableTypeID &&
                                        p.Date_Start.Date == Date_Booking.Date &&
                                        (p.Date_Start > DateTime.Now || (DateTime.Now >= p.Date_Start && DateTime.Now < p.Date_End))
                                        orderby p.Date_End
                                        select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public Booking? Get(int Restaurant_TableID, DateTime Date_Booking, bool? Flag_Del_val = false)
        {
            Booking? query = (from p in GetQuery(Flag_Del_val: Flag_Del_val)
                              where p.Restaurant_TableID == Restaurant_TableID && p.Date_Start.Date == Date_Booking.Date && p.Date_Start > Date_Booking 
                              && p.Flag_Del == Flag_Del_val orderby p.Date_Start select p).FirstOrDefault();

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public Booking? GetCheck(int Restaurant_TableID, DateTime Date_Booking, bool? Flag_Del_val = false)
        {
            Booking? query = (from p in GetQuery(Flag_Del_val: Flag_Del_val)
                              where p.Restaurant_TableID == Restaurant_TableID &&
                                        Date_Booking >= p.Date_Start && Date_Booking < p.Date_End && p.Flag_Del == Flag_Del_val
                              orderby p.Date_Start
                              select p).FirstOrDefault();

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// && (p.Date_Start.Date - Date_Booking).Value.TotalMinutes > 30 
        /// <returns></returns>
        public IQueryable<Booking> GetQueryByRestaurant_TablesID(List<int> List_Restaurant_Tables, bool? Flag_Del_val = false)
        {
            IQueryable<Booking> query = from p in GetQuery(Flag_Del_val: Flag_Del_val)
                                        where List_Restaurant_Tables.Contains(p.Restaurant_TableID) select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Booking> GetQuery(List<int> List_Restaurant_Tables, DateTime? Date_Booking, bool? Flag_Del_val = false)
        {
            IQueryable<Booking> query = from p in GetQuery(Flag_Del_val: Flag_Del_val)
                                        where List_Restaurant_Tables.Contains(p.Restaurant_TableID) select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            if (Date_Booking.HasValue && Date_Booking > DateTime.MinValue)
            {
                query = from p in query where p.Date_Start.Date == Date_Booking select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Date_Start"></param>
        /// <param name="Date_End"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public int AddInApp(int Restaurant_TableID, DateTime Date_Start, DateTime Date_End, int ClientID)
        {
            string loginfo = $"ClientID='{ClientID}' Date_Start='{Date_Start}' Date_End='{Date_End}' Restaurant_TableID='{Restaurant_TableID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Booking elem = new()
            {
                ClientID = ClientID,
                Date_Start = Date_Start,
                Date_End = Date_End,
                Restaurant_TableID = Restaurant_TableID,
                Date_Action = DateTime.Now,
            };

            _db.Bookings.Add(elem);

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
        /// Добавление
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Date_Start"></param>
        /// <param name="Date_End"></param>
        /// <param name="ClientID"></param>
        /// <param name="NumberQuests"></param>
        /// <returns></returns>
        public int AddInSite(int Restaurant_TableID, DateTime Date_Start, DateTime Date_End, int ClientID, int? NumberQuests)
        {
            string loginfo = $"ClientID='{ClientID}' Date_Start='{Date_Start}' Date_End='{Date_End}' Restaurant_TableID='{Restaurant_TableID}' NumberQuests='{NumberQuests}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Booking elem = new()
            {
                ClientID = ClientID,
                Date_Start = Date_Start,
                Date_End = Date_End,
                Restaurant_TableID = Restaurant_TableID,
                NumberQuests = NumberQuests,
                Date_Action = DateTime.Now,
            };

            _db.Bookings.Add(elem);

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
        /// Добавление
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Date_Start"></param>
        /// <param name="Date_End"></param>
        /// <param name="ClientID"></param>
        /// <param name="Phone"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int Add(int Restaurant_TableID, DateTime Date_Start, DateTime Date_End, int? ClientID, string? Phone, string? Name)
        {
            string loginfo = $"ClientID='{ClientID}' Date_Start='{Date_Start}' Date_End='{Date_End}' Restaurant_TableID='{Restaurant_TableID}'" +
                             $"Phone='{Phone}' Name='{Name}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Booking elem = new()
            {
                ClientID = ClientID,
                Date_Start = Date_Start,
                Date_End = Date_End,
                Restaurant_TableID = Restaurant_TableID,
                Date_Action = DateTime.Now,
                Phone = Phone,
                Name = Name
            };

            _db.Bookings.Add(elem);

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
        /// Добавление
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Date_Start"></param>
        /// <param name="Date_End"></param>
        /// <returns></returns>
        public int AddReserve(int Restaurant_TableID, DateTime Date_Start, DateTime Date_End)
        {
            string loginfo = $"Date_Start='{Date_Start}' Date_End='{Date_End}' Restaurant_TableID='{Restaurant_TableID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Booking elem = new()
            {
                Date_Start = Date_Start,
                Date_End = Date_End,
                Restaurant_TableID = Restaurant_TableID,
                Date_Action = DateTime.Now,
            };

            _db.Bookings.Add(elem);

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
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ClientID"></param>
        /// <param name="Date_Start"></param>
        /// <param name="Date_End"></param>
        /// <param name="Restaurant_TableID"></param>
        /// <returns></returns>
        public int Edit(int ID, int Restaurant_TableID, DateTime Date_Start, DateTime Date_End, int? ClientID, string? Phone, string? Name)
        {
            string loginfo = $"ID='{ID}' ClientID='{ClientID}' Date_Start='{Date_Start}' Date_End='{Date_End}' Restaurant_TableID='{Restaurant_TableID}'" +
                             $"Phone='{Phone}' Name='{Name}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Booking? elem = GetEdit(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "Err # ErrorCode={} ~ Error={} ### {}", args: [Errors.Err200, Errors.Err200_str, loginfo]);

                return Errors.Err200;
            }

            elem.ClientID = ClientID;
            elem.Date_Start = Date_Start;
            elem.Date_End = Date_End;
            elem.Restaurant_TableID = Restaurant_TableID;
            elem.Phone = Phone;
            elem.Name = Name;

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

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "Err # {} ## {}", args: [Errors.Err203_str, loginfo]);
                }

                return Errors.Err203;
            }
        }

        /// <summary>
        /// Изменение пометки на удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Flag_Del_New"></param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del_New, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' UserID='{UserID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Booking? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={} ~ Error={} ### {}", args: [Errors.Err200, Errors.Err200_str, loginfo]);
                }

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
                    _log.LogDebug(message: "OK # {}", args: loginfo);
                }

                return elem.ID;
            }
            catch (Exception ex)
            {

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "Err # {} ## {}", args: [Errors.Err203_str, loginfo], exception: ex);
                }

                return Errors.Err203;
            }
        }


        #region DEL By MAE - 14.05.2026 - Не используется
        ///// <summary>
        ///// Получение по Restaurant_TableID
        ///// </summary>
        ///// <param name="Restaurant_TableID"></param>
        ///// <returns></returns>
        //public Booking? GetByRestaurant_TableID(int Restaurant_TableID)
        //{
        //    Booking? elem = (from p in _db.Bookings.Include(x => x.Client).Include(x => x.Restaurant_Table) where p.Restaurant_TableID == Restaurant_TableID select p).FirstOrDefault();
        //    return elem;
        //} 
        #endregion DEL By MAE - 14.05.2026 - Не используется
    }
}