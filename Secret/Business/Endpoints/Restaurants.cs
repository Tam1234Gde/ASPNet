using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Рестораны
    /// </summary>
    public class Restaurants
    {
        private readonly ILogger _log;
        private readonly Restaurant_oper _dl_Restaurant = new();
        private readonly Restaurant_Menus _bl_Restaurant_Menus = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurants()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CRestaurant? Get(Restaurant? Elem)
        {
            if (Elem != null)
            {
                CRestaurant item = new()
                {
                    ID = Elem.ID,
                    Address = Elem.Adress,
                    Latitude = Elem.Latitude,
                    Longitude = Elem.Longitude,
                    Hours_Work = Elem.Hours_Work,
                    Hours_WorkApp = Elem.Hours_WorkApp,
                    Hours_HardWork = Elem.Hours_HardWork,
                    Delivery_Price = Elem.Delivery_Price,
                    Flag_Car = Elem.Flag_Car,
                    Flag_Delivery = Elem.Flag_Delivery,
                    Flag_Incafe = Elem.Flag_Incafe,
                    Flag_Myself = Elem.Flag_Myself,
                    Express_Delivery_Time = Elem.Express_Delivery_Time,
                    Delivery_Time = Elem.Delivery_Time,
                    TgChatID = Elem.TgChatID,
                    TgChatBookingID = Elem.TgChatBookingID,
                    Client = Elem.Client?.Name,
                    ClientID = Elem.ClientID,
                    Hours_BookingWork = Elem.Hours_BookingWork,
                    BookingHour = Elem.BookingHour,
                    BookingPhone = Elem.BookingPhone,
                    CityID = Elem.CityID,
                    City = Elem.City?.Name,
                    Flag_Hidden = Elem.Flag_Hidden,
                    Flag_Hide = Elem.Flag_Hide,
                    HiddenTitle = Elem.HiddenTitle,
                    HiddenText = Elem.HiddenText,
                    YouKassaLogin = Elem.YouKassaLogin,
                    YouKassaPassword = Elem.YouKassaPassword,
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
        public CRestaurant? Get(int ID)
        {
            return Get(Elem: _dl_Restaurant.Get_Edit(ID));
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant>? GetListAdmin(bool? Flag_Del_val = false)
        {
            List<CRestaurant> list_result = [];

            foreach (Restaurant elem in _dl_Restaurant.GetQueryWithHide(Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Address)];
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant>? GetList(bool? Flag_Del_val = false)
        {
            List<CRestaurant> list_result = [];

            foreach (Restaurant elem in _dl_Restaurant.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Address)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="CityID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant>? GetListByCityID(int CityID, bool? Flag_Del_val = false)
        {
            List<CRestaurant> list_result = [];

            foreach (Restaurant elem in _dl_Restaurant.GetQueryByCityID(CityID: CityID, Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Address)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CRestaurant>? GetListActive(bool? Flag_Del_val = false)
        {
            List<CRestaurant> list_result = [];

            foreach (Restaurant elem in _dl_Restaurant.GetQueryActive(Flag_Del_val: Flag_Del_val).ToList())
            {
                CRestaurant? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Address)];
        }


        /// <summary>
        /// Получить словарь с соответствием Ресторан/блюдо
        /// </summary>
        /// <param name="Dict"></param>
        /// <returns></returns>
        public Dictionary<int, List<CRestaurant>?>? Get_IDRestPair(Dictionary<int, List<CRestaurant_Menu_Dish>> Dict)
        {
            Dictionary<int, List<CRestaurant>?> result = [];

            List<int> IDs_Restarant_Menu = [.. Dict.Values.SelectMany(p => p.Select(x => x.Restaurant_MenuID))];

            List<CRestaurant_Menu>? cRestaurant_Menus = _bl_Restaurant_Menus.GetList(IDs: IDs_Restarant_Menu);

            List<CRestaurant>? list_CRestaurant = GetList();

            if (list_CRestaurant == null)
            {
                return null;
            }

            foreach (KeyValuePair<int, List<CRestaurant_Menu_Dish>> elem in Dict)
            {

                List<int> cRestaurant_Menus_IDs_Now = [.. elem.Value.Select(x => x.Restaurant_MenuID)];

                List<CRestaurant_Menu>? cRestaurant_Menus_Now = cRestaurant_Menus?.Where(x => cRestaurant_Menus_IDs_Now.Contains(x.ID)).ToList();

                if (cRestaurant_Menus_Now == null)
                {
                    continue;
                }

                result.Add(key: elem.Key, value: [.. list_CRestaurant.Where(p => cRestaurant_Menus_Now.Select(x => x.RestaurantID).ToList().Contains(p.ID))]);
            }

            return result;
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
        /// <param name="Express_Delivery_Time"></param>
        /// <param name="Delivery_Time"></param>
        /// <param name="TgChatID"></param>
        /// <param name="ClientID"></param>
        /// <param name="Hours_BookingWork"></param>
        /// <param name="BookingHour"></param>
        /// <param name="BookingPhone"></param>
        /// <param name="CityID"></param>
        /// <param name="Flag_Hidden"></param>
        /// <param name="Flag_Hide"></param>
        /// <param name="HiddenTitle"></param>
        /// <param name="HiddenText"></param>
        /// <param name="YouKassaLogin"></param>
        /// <param name="YouKassaPassword"></param>
        /// <param name="TgChatBookingID"></param>
        /// <param name="TgChatQrID"></param>
        /// <returns></returns>
        public int Add(string Address, string Latitude, string Longitude, string? Hours_Work, string? Hours_WorkApp, string? Hours_HardWork, decimal? Delivery_Price,
                       bool Flag_Car, bool Flag_Delivery, bool Flag_Incafe, bool Flag_Myself, int? Express_Delivery_Time, int? Delivery_Time, string? TgChatID,
                       int? ClientID, string? Hours_BookingWork, int? BookingHour, string? BookingPhone, int? CityID, string? TgChatBookingID, string? TgChatQrID,
                       bool Flag_Hidden, bool Flag_Hide, string? HiddenTitle, string? HiddenText, string? YouKassaLogin, string? YouKassaPassword)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Restaurants_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Restaurant.Add(Address: Address, Latitude: Latitude, Longitude: Longitude, Hours_Work: Hours_Work, Hours_WorkApp: Hours_WorkApp,
                                      Hours_HardWork: Hours_HardWork, Delivery_Price: Delivery_Price, Flag_Car: Flag_Car, Flag_Delivery: Flag_Delivery,
                                      Flag_Incafe: Flag_Incafe, Flag_Myself: Flag_Myself, Express_Delivery_Time: Express_Delivery_Time, Delivery_Time: Delivery_Time,
                                      TgChatID: TgChatID, ClientID: ClientID, Hours_BookingWork: Hours_BookingWork, BookingHour: BookingHour, BookingPhone: BookingPhone,
                                      CityID: CityID, TgChatBookingID: TgChatBookingID, TgChatQrID: TgChatQrID, Flag_Hidden: Flag_Hidden, Flag_Hide: Flag_Hide, HiddenTitle: HiddenTitle,
                                      HiddenText: HiddenText, YouKassaLogin: YouKassaLogin, YouKassaPassword: YouKassaPassword);
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
        /// <param name="Delivery_Time_New"></param>
        /// <param name="Express_Delivery_Time_New"></param>
        /// <param name="TgChatID_New"></param>
        /// <param name="ClientID_New"></param>
        /// <param name="Hours_BookingWork_New"></param>
        /// <param name="BookingHour_New"></param>
        /// <param name="BookingPhone_New"></param>
        /// <param name="CityID_New"></param>
        /// <param name="TgChatBookingID_New"></param>
        /// <param name="TgChatQrID_New"></param>
        /// <param name="Flag_Hidden_New"></param>
        /// <param name="Flag_Hide_New"></param>
        /// <param name="HiddenTitle_New"></param>
        /// <param name="HiddenText_New"></param>
        /// <param name="YouKassaLogin_New"></param>
        /// <param name="YouKassaPassword_New"></param>
        /// <returns></returns>
        public int Edit(int ID, string Address_New, string Latitude_New, string Longitude_New, string? Hours_Work_New, string? Hours_WorkApp_New, string? Hours_HardWork_New,
                        decimal? Delivery_Price_New, bool Flag_Car_New, bool Flag_Delivery_New, bool Flag_Incafe_New, bool Flag_Myself_New, int? Express_Delivery_Time_New,
                        int? Delivery_Time_New, string? TgChatID_New, int? ClientID_New, string? Hours_BookingWork_New, int? BookingHour_New, string? BookingPhone_New,
                        int? CityID_New, string? TgChatBookingID_New, string? TgChatQrID_New, bool Flag_Hidden_New, bool Flag_Hide_New, string? HiddenTitle_New, string? HiddenText_New,
                        string? YouKassaLogin_New, string? YouKassaPassword_New)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Restaurants_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            int result = _dl_Restaurant.Edit(ID: ID, Address_New: Address_New, Latitude_New: Latitude_New, Longitude_New: Longitude_New,
                                       Hours_Work_New: Hours_Work_New, Hours_WorkApp_New: Hours_WorkApp_New, Hours_HardWork_New: Hours_HardWork_New,
                                       Delivery_Price_New: Delivery_Price_New, Flag_Car_New: Flag_Car_New, Flag_Delivery_New: Flag_Delivery_New,
                                       Flag_Incafe_New: Flag_Incafe_New, Flag_Myself_New: Flag_Myself_New, Express_Delivery_Time_New: Express_Delivery_Time_New,
                                       Delivery_Time_New: Delivery_Time_New, TgChatID_New: TgChatID_New, ClientID_New: ClientID_New,
                                       Hours_BookingWork_New: Hours_BookingWork_New, BookingHour_New: BookingHour_New, BookingPhone_New: BookingPhone_New,
                                       CityID_New: CityID_New, TgChatBookingID_New: TgChatBookingID_New, TgChatQrID_New: TgChatQrID_New, Flag_Hidden_New: Flag_Hidden_New, Flag_Hide_New: Flag_Hide_New,
                                       HiddenTitle_New: HiddenTitle_New, HiddenText_New: HiddenText_New, YouKassaLogin_New: YouKassaLogin_New,
                                       YouKassaPassword_New: YouKassaPassword_New);

            if (result > 0)
            {
                CRestaurant? restaurant = Get(ID);

                if (restaurant == null)
                {
                    return Errors.Err200;
                }

                bool? check = GetListByCityID(HIDE_Convert.ToInt(restaurant.CityID))?.TrueForAll(p => p.Flag_Hidden);

                if (check.HasValue)
                {
                    new Cities().EditFlag_Hidden(ID: HIDE_Convert.ToInt(restaurant.CityID), Flag_Hidden: check.Value);
                }

            }

            return result;
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Restaurants_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления

            return _dl_Restaurant.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }




        #region DEL By MAE - Не используется - 22.04.2026
        ///// <summary>
        ///// Получение данных 
        ///// </summary>
        ///// <param name="ListElems"></param>
        ///// <returns></returns>
        //private static List<CRestaurant>? GetList(List<Restaurant>? ListElems)
        //{

        //    List<CRestaurant>? result = [];

        //    if (ListElems == null)
        //    {
        //        return null;
        //    }


        //    foreach (Restaurant elem in ListElems)
        //    {
        //        if (elem != null)
        //        {
        //            CRestaurant item = new()
        //            {
        //                ID = elem.ID,
        //                Adress = elem.Adress,
        //                Latitude = elem.Latitude,
        //                Longitude = elem.Longitude,
        //                Hours_Work = elem.Hours_Work,
        //                Hours_WorkApp = elem.Hours_WorkApp,
        //                Hours_HardWork = elem.Hours_HardWork,
        //                Delivery_Price = elem.Delivery_Price,
        //                Flag_Car = elem.Flag_Car,
        //                Flag_Delivery = elem.Flag_Delivery,
        //                Flag_Incafe = elem.Flag_Incafe,
        //                Flag_Myself = elem.Flag_Myself,
        //                Express_Delivery_Time = elem.Express_Delivery_Time,
        //                Delivery_Time = elem.Delivery_Time,
        //                TgChatID = elem.TgChatID,
        //                Client = elem.Client?.Name,
        //                ClientID = elem.ClientID,
        //                Hours_BookingWork = elem.Hours_BookingWork,
        //                BookingHour = elem.BookingHour,
        //                BookingPhone = elem.BookingPhone,
        //                CityID = elem.CityID,
        //                City = elem.City?.Name,
        //                Flag_Hidden = elem.Flag_Hidden,
        //                Flag_Hide = elem.Flag_Hide,
        //                HiddenTitle = elem.HiddenTitle,
        //                HiddenText = elem.HiddenText,
        //                YouKassaLogin = elem.YouKassaLogin,
        //                YouKassaPassword = elem.YouKassaPassword,
        //            };
        //            result.Add(item);
        //        }


        //    }
        //    return result;
        //} 
        #endregion DEL By MAE - Не используется - 22.04.2026
    }
}