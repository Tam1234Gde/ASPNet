using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Рестораны
    /// </summary>
    public class Restaurant_oper
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст Базы данных
        /// </summary>
        private readonly DBContext _db = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_oper()
        {
            _log = AppLog.CreateLogger(GetType());
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Restaurant? Get(int ID, bool? Flag_Del_val = false)
        {
            Restaurant? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Restaurant? Get_Edit(int ID, bool? Flag_Del_val = false)
        {
            Restaurant? elem = (from p in GetQuery_Edit(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant> query = from p in _db.Restaurants.Include(x => x.City).AsNoTracking() 
                                           select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val && p.Flag_Hide == false select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant> GetQuery_Edit(bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant> query = from p in _db.Restaurants.Include(x => x.City) select p;

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
        public IQueryable<Restaurant> GetQueryWithHide(bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="CityID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant> GetQueryByCityID(int CityID, bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.CityID == CityID && p.Flag_Hide == false select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса с активными ресторанами
        /// </summary>
        /// <param name="Flag_Del_val">Флаг - удаленные</param>
        /// <returns></returns>
        public IQueryable<Restaurant> GetQueryActive(bool? Flag_Del_val = false)
        {
            IQueryable<int> query = from p in _db.Restaurant_Menus.Include(x => x.Restaurant).AsNoTracking() where p.Flag_Active == true select p.RestaurantID; //TODO: MAE - убрать в бизнес

            IQueryable<Restaurant> queryRestaurant = from p in GetQuery(Flag_Del_val: Flag_Del_val) where query.ToList().Contains(p.ID) select p;

            if (Flag_Del_val.HasValue)
            {
                queryRestaurant = from p in queryRestaurant where p.Flag_Del == Flag_Del_val select p;
            }

            return queryRestaurant;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <param name="Hours_Work"></param>
        /// <param name="Hours_WorkApp"></param>
        /// <param name="Hours_HardWork"></param>
        /// <param name="Delivery_Price"></param>
        /// <param name="Flag_Car"></param>
        /// <param name="Flag_Delivery"></param>
        /// <param name="Flag_Incafe"></param>
        /// <param name="Flag_Myself"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public int Add(string Address, string Latitude, string Longitude, string? Hours_Work, string? Hours_WorkApp, string? Hours_HardWork, decimal? Delivery_Price,
                       bool Flag_Car, bool Flag_Delivery, bool Flag_Incafe, bool Flag_Myself, int? Express_Delivery_Time, int? Delivery_Time, string? TgChatID,
                       int? ClientID, string? Hours_BookingWork, int? BookingHour, string? BookingPhone, int? CityID, string? TgChatBookingID, string? TgChatQrID,
                       bool Flag_Hidden, bool Flag_Hide, string? HiddenTitle, string? HiddenText, string? YouKassaLogin, string? YouKassaPassword)
        {
            string loginfo = $"Address='{Address}' Latitude='{Latitude}' Longitude='{Longitude}' TgChatID='{TgChatID}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            Restaurant elem = new()
            {
                Adress = Address,
                Latitude = Latitude,
                Longitude = Longitude,
                Hours_Work = Hours_Work,
                Hours_HardWork = Hours_HardWork,
                Hours_WorkApp = Hours_WorkApp,
                Delivery_Price = Delivery_Price,
                Flag_Car = Flag_Car,
                Flag_Delivery = Flag_Delivery,
                Flag_Incafe = Flag_Incafe,
                Flag_Myself = Flag_Myself,
                Express_Delivery_Time = Express_Delivery_Time,
                Delivery_Time = Delivery_Time,
                TgChatID = TgChatID,
                ClientID = ClientID,
                Hours_BookingWork = Hours_BookingWork,
                BookingHour = BookingHour,
                BookingPhone = BookingPhone,
                CityID = CityID,
                TgChatBookingID = TgChatBookingID,
                TgChatQrID = TgChatQrID,
                Flag_Hidden = Flag_Hidden,
                Flag_Hide = Flag_Hide,
                HiddenTitle = HiddenTitle,
                HiddenText = HiddenText,
                YouKassaLogin = YouKassaLogin,
                YouKassaPassword = YouKassaPassword
                //Flag_Active = Flag_Active,
            };

            _db.Restaurants.Add(elem);

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
        /// <param name="ID"></param>
        /// <param name="Address_New"></param>
        /// <param name="Latitude_New"></param>
        /// <param name="Longitude_New"></param>
        /// <param name="Hours_Work_New"></param>
        /// <param name="Hours_WorkApp_New"></param>
        /// <param name="Hours_HardWork_New"></param>
        /// <param name="Delivery_Price_New"></param>
        /// <param name="Flag_Car_New"></param>
        /// <param name="Flag_Delivery_New"></param>
        /// <param name="Flag_Incafe_New"></param>
        /// <param name="Flag_Myself_New"></param>
        /// <param name="Flag_Active_New"></param>
        /// <returns></returns>
        public int Edit(int ID, string Address_New, string Latitude_New, string Longitude_New, string? Hours_Work_New, string? Hours_HardWork_New, string? Hours_WorkApp_New,
                        decimal? Delivery_Price_New, bool Flag_Car_New, bool Flag_Delivery_New, bool Flag_Incafe_New, bool Flag_Myself_New, int? Express_Delivery_Time_New,
                        int? Delivery_Time_New, string? TgChatID_New, int? ClientID_New, string? Hours_BookingWork_New, int? BookingHour_New, string? BookingPhone_New,
                        int? CityID_New, string? TgChatBookingID_New, string? TgChatQrID_New, bool Flag_Hidden_New, bool Flag_Hide_New, string? HiddenTitle_New, string? HiddenText_New,
                        string? YouKassaLogin_New, string? YouKassaPassword_New)
        {
            string loginfo = $"ID='{ID}' Address_New='{Address_New}' Latitude_New='{Latitude_New}' Longitude_New='{Longitude_New}'" +
                             $"Hours_Work_New='{Hours_Work_New}' Hours_WorkApp_New='{Hours_WorkApp_New}' Express_Delivery_Time_New='{Express_Delivery_Time_New}'" +
                             $"TgChatID_New='{TgChatID_New}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            Restaurant? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Adress = Address_New;
            elem.Latitude = Latitude_New;
            elem.Longitude = Longitude_New;
            elem.Hours_Work = Hours_Work_New;
            elem.Hours_WorkApp = Hours_WorkApp_New;
            elem.Hours_HardWork = Hours_HardWork_New;
            elem.Delivery_Price = Delivery_Price_New;
            elem.Flag_Car = Flag_Car_New;
            elem.Flag_Delivery = Flag_Delivery_New;
            elem.Flag_Incafe = Flag_Incafe_New;
            elem.Flag_Myself = Flag_Myself_New;
            elem.Express_Delivery_Time = Express_Delivery_Time_New;
            elem.Delivery_Time = Delivery_Time_New;
            elem.TgChatID = TgChatID_New;
            elem.ClientID = ClientID_New;
            elem.Hours_BookingWork = Hours_BookingWork_New;
            elem.BookingHour = BookingHour_New;
            elem.BookingPhone = BookingPhone_New;
            elem.CityID = CityID_New;
            elem.TgChatBookingID = TgChatBookingID_New;
            elem.TgChatQrID = TgChatQrID_New;
            elem.Flag_Hidden = Flag_Hidden_New;
            elem.Flag_Hide = Flag_Hide_New;
            elem.HiddenTitle = HiddenTitle_New;
            elem.HiddenText = HiddenText_New;
            elem.YouKassaLogin = YouKassaLogin_New;
            elem.YouKassaPassword = YouKassaPassword_New;

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
            Restaurant? elem = Get_Edit(ID: ID);

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