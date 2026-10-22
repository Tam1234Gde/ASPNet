using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Бронирование
    /// </summary>
    public class Bookings
    {
        private readonly ILogger _log;
        private readonly Booking_oper _dl_Booking = new();
        private readonly Restaurant_Table_oper _dl_Restaurant_Table = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Bookings()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CBooking? Get(Booking? Elem)
        {
            if (Elem != null)
            {
                CBooking item = new()
                {
                    ID = Elem.ID,
                    ClientID = Elem.ClientID,
                    Client = Elem.Client?.Phone,
                    Date_Action = Elem.Date_Action,
                    Date_Start = Elem.Date_Start,
                    Date_End = Elem.Date_End,
                    Phone = Elem.Phone,
                    Name = Elem.Name,
                    Restaurant_Table = Elem.Restaurant_Table?.Name,
                    Restaurant_TableID = Elem.Restaurant_TableID,
                    Restaurant_Table_Type = Elem.Restaurant_Table?.Restaurant_Table_Type?.Name,
                    NumberQuests = Elem.NumberQuests
                };
                return item;
            }

            return null;
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CBooking? Get(int ID)
        {
            return Get(Elem: _dl_Booking.Get(ID));
        }


      

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CBooking>? GetListByRestaurant_TableID(int Restaurant_TableID, bool? Flag_Del_val = false)
        {
            List<CBooking> list_result = [];

            foreach (Booking elem in _dl_Booking.GetQueryByRestaurant_TableID(Restaurant_TableID: Restaurant_TableID, Flag_Del_val: Flag_Del_val).ToList())
            {
                CBooking? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Date_End)];
        }


        /// <summary>
        /// Возвращает список броней для стола
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Date_Booking"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<string>? GetList(int Restaurant_TableID, DateTime Date_Booking, bool? Flag_Del_val = false)
        {
            return [.. _dl_Booking.GetQuery(Restaurant_TableID: Restaurant_TableID, Date_Booking: Date_Booking, Flag_Del_val: Flag_Del_val)];
        }


        /// <summary>
        /// Возвращает список броней для стола
        /// </summary>
        /// <param name="Restaurant_TableTypeID"></param>
        /// <param name="Date_Booking"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CBooking>? GetListBooking(int Restaurant_TableTypeID, DateTime Date_Booking, bool? Flag_Del_val = false)
        {
            List<CBooking> list_result = [];

            foreach (Booking elem in _dl_Booking.GetQueryBooking(Restaurant_TableTypeID: Restaurant_TableTypeID, Date_Booking: Date_Booking, Flag_Del_val: Flag_Del_val).ToList())
            {
                CBooking? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CBooking>? GetListByRestaurantID(int RestaurantID, bool? Flag_Del_val = false)
        {
            List<CBooking> list_result = [];

            List<int> List_restaurant_tables = [.. _dl_Restaurant_Table.GetQuery_ByRestarantID(RestaurantID: RestaurantID, Flag_Del_val: Flag_Del_val).Select(p => p.ID)];

            foreach (Booking elem in _dl_Booking.GetQueryByRestaurant_TablesID(List_Restaurant_Tables: List_restaurant_tables, Flag_Del_val: Flag_Del_val).ToList())
            {
                CBooking? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Date_End)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CBooking>? GetListByRestaurantID_ForAdmin(int RestaurantID, bool? Flag_Del_val = false)
        {
            List<CBooking> list_result = [];

            List<int> List_restaurant_tables = [.. _dl_Restaurant_Table.GetQuery_ByRestarantID(RestaurantID: RestaurantID, Flag_Del_val: Flag_Del_val).Select(p => p.ID)];

            foreach (Booking elem in _dl_Booking.GetQuery(List_Restaurant_Tables: List_restaurant_tables, Date_Booking: DateTime.Now.Date, Flag_Del_val: Flag_Del_val).ToList())
            {
                CBooking? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Date_End)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Restaurant_HallID"></param>
        /// <param name="Restaurant_TableTypeID"></param>
        /// <param name="Date_Booking"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CBooking>? GetListByRestaurant_TableTypeID(int RestaurantID, int? Restaurant_HallID, int Restaurant_TableTypeID, DateTime Date_Booking, bool? Flag_Del_val = false)
        {
            List<CBooking> list_result = [];

            List<int> List_restaurant_tables = [.. _dl_Restaurant_Table.GetQueryByRestaurant_TableTypeID(RestaurantID: RestaurantID,
                                                                                                         Restaurant_Table_TypeID: Restaurant_TableTypeID,
                                                                                                         Restaurant_HallID: Restaurant_HallID,
                                                                                                         Flag_Del_val: Flag_Del_val).Select(p => p.ID)];

            List<Booking> list = [.. _dl_Booking.GetQueryByRestaurant_TablesID(List_Restaurant_Tables: List_restaurant_tables,
                                                                           Flag_Del_val: Flag_Del_val).ToList()
                                                                           .Where(p => ((Date_Booking - p.Date_Start).TotalMinutes > -30 && (Date_Booking - p.Date_Start).TotalMinutes < 0) ||
                                                                           Date_Booking >= p.Date_Start && Date_Booking < p.Date_End)];

            foreach (Booking elem in list)
            {
                CBooking? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Date_End)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Date_Booking"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public CBooking? Get(int Restaurant_TableID, DateTime Date_Booking, bool? Flag_Del_val = false)
        {
            return Get(Elem: _dl_Booking.Get(Restaurant_TableID, Date_Booking, Flag_Del_val: Flag_Del_val));
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Date_Booking"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public CBooking? CheckBooking(int Restaurant_TableID, DateTime Date_Booking, bool? Flag_Del_val = false)
        {
            return Get(Elem: _dl_Booking.GetCheck(Restaurant_TableID, Date_Booking, Flag_Del_val: Flag_Del_val));
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="Date_Start"></param>
        /// <param name="Date_End"></param>
        /// <param name="Restaurant_TableID"></param>
        /// <returns></returns>
        public int AddInApp(int ClientID, DateTime Date_Start, DateTime Date_End, int Restaurant_TableID)
        {
            _bl_Setup.Set(Key: Consts.SetupBonuses_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Booking.AddInApp(ClientID: ClientID, Date_Start: Date_Start, Date_End: Date_End, Restaurant_TableID: Restaurant_TableID);
        }


        /// <summary>
        /// Добавление брони с сайта меню
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="Date_Start"></param>
        /// <param name="Date_End"></param>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="NumberQuests"></param>
        /// <returns></returns>
        public int AddInSite(int ClientID, DateTime Date_Start, DateTime Date_End, int Restaurant_TableID, int? NumberQuests)
        {
            _bl_Setup.Set(Key: Consts.SetupBonuses_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Booking.AddInSite(ClientID: ClientID, Date_Start: Date_Start, Date_End: Date_End, Restaurant_TableID: Restaurant_TableID, NumberQuests: NumberQuests);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="Date_Start"></param>
        /// <param name="Date_End"></param>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Phone"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int Add(int? ClientID, DateTime Date_Start, DateTime Date_End, int Restaurant_TableID, string? Phone, string? Name)
        {
            _bl_Setup.Set(Key: Consts.SetupBookings_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Booking.Add(ClientID: ClientID, Date_Start: Date_Start, Date_End: Date_End, Restaurant_TableID: Restaurant_TableID,
                                   Phone: Phone, Name: Name);
        }


        /// <summary>
        /// Добавление резерва
        /// </summary>
        /// <param name="Date_Start"></param>
        /// <param name="Date_End"></param>
        /// <param name="Restaurant_TableID"></param>
        /// <returns></returns>
        public int AddReserve(DateTime Date_Start, DateTime Date_End, int Restaurant_TableID)
        {

            //TODO: MAE закоментировано, мне кажется он не нужен, понять как это работало
            //// Перед основной бронью 1 час
            //int result = _dl_Booking.AddReserve(Date_Start: Date_Start.AddHours(-1), Date_End: Date_Start, Restaurant_TableID: Restaurant_TableID);

            // После основной брони 1 час
            int result = _dl_Booking.AddReserve(Date_Start: Date_End, Date_End: Date_End.AddHours(1), Restaurant_TableID: Restaurant_TableID);

            _bl_Setup.Set(Key: Consts.SetupBookings_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return result;
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ClientID"></param>
        /// <param name="Date_Start"></param>
        /// <param name="Date_End"></param>
        /// <param name="Restaurant_TableID"></param>
        /// <param name="Phone"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int Edit(int ID, int? ClientID, DateTime Date_Start, DateTime Date_End, int Restaurant_TableID, string? Phone, string? Name)
        {
            _bl_Setup.Set(Key: Consts.SetupBookings_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Booking.Edit(ID: ID, ClientID: ClientID, Date_Start: Date_Start, Date_End: Date_End, Restaurant_TableID: Restaurant_TableID,
                                    Phone: Phone, Name: Name);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupBookings_DateChange, Val_New: DateTime.Now.ToString()); // Обновление даты последнего обновления

            return _dl_Booking.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }



        #region DEL By MAE - Не используется - 22.04.2026 - То что не используется ставим в конец файла
        ///// <summary>
        ///// Получение данных 
        ///// </summary>
        ///// <param name="Restaurant_TableID"></param>
        ///// <returns></returns>
        //public CBooking? GetByRestaurant_TableID(int Restaurant_TableID)
        //{
        //    return Get(Elem: _dl_Booking.GetByRestaurant_TableID(Restaurant_TableID));
        //}


        ///// <summary>
        ///// Возвращает список
        ///// </summary>
        ///// <param name="Flag_Del_val"></param>
        ///// <returns></returns>
        //public List<CBooking>? GetList(bool? Flag_Del_val = false)
        //{
        //    List<CBooking> list_result = [];

        //    foreach (Booking elem in _dl_Booking.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
        //    {
        //        CBooking? item = Get(Elem: elem);
        //        if (item != null)
        //        {
        //            list_result.Add(item);
        //        }
        //    }
        //    return [.. list_result.OrderBy(x => x.Date_End)];
        //} 
        #endregion DEL By MAE - Не используется - 22.04.2026

    }
}