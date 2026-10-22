using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v3._Enum;
using HIDE.RestAndRant.Web_Api.Models.v3.Bookings;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Api.Controllers.v3
{
    /// <summary>
    /// Контроллер - Бронирование столиков\залов
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Tags("Bookings_v3 - Бронирование столиков")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/Bookings/[action]")]
    [ApiController]
    public class BookingsController_v3(ILogger<BookingsController_v3> _log, BL_Context _bl) : ControllerBase
    {
        /// <summary>
        /// Возвращает дату последнего изменения бронирования столиков\залов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        [Obsolete]
        public JsonResult GetBookingsChangeDate()
        {
            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupBookings_DateChange);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Получает список столиков в выбранное время
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CRestaurant_Table>))]
        public JsonResult GetBookingsList(DateTime Date_Booking, long? Date_Booking_Unix, [Required] int Restaurant_Table_TypeID, [Required] int RestaurantID)
        {
            DateTime date_Booking;
            if (Date_Booking_Unix.HasValue)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(Date_Booking_Unix.Value);
                date_Booking = dateTimeOffset.DateTime;
            }
            else
            {
                date_Booking = Date_Booking;
            }

            // Получаем коды занятых столов
            List<int>? list_tables = _bl.Bookings.GetListByRestaurant_TableTypeID(RestaurantID: RestaurantID,
                                                                                  Restaurant_TableTypeID: Restaurant_Table_TypeID,
                                                                                  Date_Booking: date_Booking, Restaurant_HallID: null)?.Select(p => p.Restaurant_TableID).ToList();

            // Получаем список столов по типу столика и ресторану
            List<CRestaurant_Table>? list_rest_tables = _bl.Restaurant_Tables.GetListByRestaurant_Table_TypeID(RestaurantID: RestaurantID,
                                                                                                               Restaurant_Table_TypeID: Restaurant_Table_TypeID,
                                                                                                               Restaurant_HallID: null);

            if (list_rest_tables == null || list_rest_tables.Count == 0)
            {
                _log.LogWarning(message: "Не удалось найти столики");
                return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: null);
            }

            List<CRestaurant_Table>? list_result = [];
            foreach (CRestaurant_Table elem in list_rest_tables)
            {
                if (list_tables != null && list_tables.Count != 0)
                {
                    foreach (var item in list_tables)
                    {
                        if (elem.ID == item)
                        {
                            elem.Restaurant_Table_StatusID = ERestaurant_Table_Status.Disabled;
                        }
                    }
                }
                list_result.Add(elem);
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Получает список столиков в выбранное время
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Model_Bookings_Times_New_v3))]
        public JsonResult GetBookingsListNew(DateTime? Date_Booking, [Required] int Restaurant_Table_TypeID, [Required] int RestaurantID, int? Restaurant_HallID)
        {
            DateTime? date = DateTime.Now;
            if (Date_Booking.HasValue && Date_Booking.Value.Date != date.Value.Date)
            {
                date = Date_Booking.Value;
            }

            // Получаем интервалы времен когда можно сделать бронь 
            CRestaurant? restaurant = _bl.Restaurants.Get(RestaurantID);
            List<string> list_times_restaurant = Helper_ToArray.StringArr_ToListString(restaurant?.Hours_BookingWork ?? "");

            List<DateTime> list_dates = [];
            DateTime start = HIDE_Convert.ToDateTime(list_times_restaurant.FirstOrDefault());
            DateTime end = HIDE_Convert.ToDateTime(list_times_restaurant.LastOrDefault());

            end = Restaurant_Table_TypeID switch
            {
                (int)ERestaurant_Table_Type.Ordinary => end.AddMinutes(-AppConfig.RangeOrder),
                (int)ERestaurant_Table_Type.Veranda => end.AddMinutes(-AppConfig.RangeOrder),
                (int)ERestaurant_Table_Type.VIP => end.AddMinutes(-120),
                _ => end.AddMinutes(-AppConfig.RangeOrder),
            };

            if (Date_Booking.HasValue)
            {
                start = Date_Booking.Value.Date.Add(start.TimeOfDay);
                end = Date_Booking.Value.Date.Add(end.TimeOfDay);
            }

            int index = 0;

            while (start < end)
            {
                index++;

                if (index > 1)
                {
                    start = Restaurant_Table_TypeID switch
                    {
                        (int)ERestaurant_Table_Type.Ordinary => start.AddMinutes(AppConfig.RangeOrder),
                        (int)ERestaurant_Table_Type.Veranda => start.AddMinutes(AppConfig.RangeOrder),
                        (int)ERestaurant_Table_Type.VIP => start.AddMinutes(AppConfig.RangeBookingVIP),
                        _ => start.AddMinutes(AppConfig.RangeOrder),
                    };
                }

                if (!Date_Booking.HasValue)
                {
                    if (start >= date)
                    {
                        list_dates.Add(start);
                    }
                }
                else if (start >= DateTime.Now)
                {
                    list_dates.Add(start);
                }
            }

            if (Restaurant_Table_TypeID != (int)ERestaurant_Table_Type.VIP)
            {
                list_dates = [.. list_dates.Where(p => (p - date.Value).TotalMinutes > AppConfig.RangeOrder)];
            }
            else
            {

                list_dates = [.. list_dates.Where(p => (p - date.Value).TotalMinutes > AppConfig.RangeBookingVIP)];
            }

            Model_Bookings_Times_New_v3 model = new();
            List<Model_Booking_Time_v3>? list_result = [];


            if (restaurant != null && restaurant.Flag_Incafe)
            {
                foreach (var item in list_dates)
                {
                    Model_Booking_Time_v3 elem_main = new();
                    List<CRestaurant_Table>? list_tables_filter = [];

                    // Получаем коды занятых столов
                    List<int>? list_table_disabled = _bl.Bookings.GetListByRestaurant_TableTypeID(RestaurantID: RestaurantID,
                                                                                                  Restaurant_TableTypeID: Restaurant_Table_TypeID,
                                                                                                  Restaurant_HallID: Restaurant_HallID,
                                                                                                  Date_Booking: item)?.Select(p => p.Restaurant_TableID).ToList();
                    // Получаем список столов по типу столика и ресторану
                    List<CRestaurant_Table>? list_tables = _bl.Restaurant_Tables.GetListByRestaurant_Table_TypeID(RestaurantID: RestaurantID, Restaurant_HallID: Restaurant_HallID,
                                                                                                                  Restaurant_Table_TypeID: Restaurant_Table_TypeID);

                    if (list_tables != null && list_tables.Count != 0)
                    {
                        foreach (CRestaurant_Table table in list_tables)
                        {
                            if (list_table_disabled != null && list_table_disabled.Count != 0)
                            {
                                foreach (int table_disabled in list_table_disabled)
                                {
                                    if (table.ID == table_disabled)
                                    {
                                        table.Restaurant_Table_StatusID = ERestaurant_Table_Status.Disabled;
                                    }
                                }
                            }
                            list_tables_filter.Add(table);
                        }
                    }

                    elem_main.List_Restaurant_Tables = list_tables_filter;
                    elem_main.Date_Booking = item;

                    list_result.Add(elem_main);
                }
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_result);
        }


        /// <summary>
        /// Получает время когда можно забронировать данный тип столика
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<DateTime>))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public JsonResult GetBookingsTimeList([FromQuery] Model_Bookings_Times_v3 data)
        {
            CRestaurant? restaurant = _bl.Restaurants.Get(data.RestaurantID);
            List<string> list_times_restaurant = Helper_ToArray.StringArr_ToListString(restaurant?.Hours_Work ?? "");

            List<DateTime> list_dates = [];
            DateTime start = HIDE_Convert.ToDateTime(list_times_restaurant.FirstOrDefault());
            while (start < HIDE_Convert.ToDateTime(list_times_restaurant.LastOrDefault()))
            {
                start = data.Restaurant_Table_TypeID switch
                {
                    (int)ERestaurant_Table_Type.Ordinary => start.AddMinutes(AppConfig.RangeOrder),
                    (int)ERestaurant_Table_Type.Veranda => start.AddMinutes(AppConfig.RangeOrder),
                    (int)ERestaurant_Table_Type.VIP => start.AddMinutes(AppConfig.RangeBookingVIP),
                    _ => start.AddMinutes(AppConfig.RangeOrder),
                };

                if (start >= DateTime.Now)
                {
                    list_dates.Add(start);
                }
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: list_dates);
        }


        /// <summary>
        /// Получает время следующей брони
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DateTime))]
        [Obsolete]
        public JsonResult GetBookingsNext(DateTime Date_Booking, int Restaurant_TableID)
        {
            CBooking? result = _bl.Bookings.Get(Restaurant_TableID: Restaurant_TableID, Date_Booking: Date_Booking);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: result?.Date_Start);
        }


        /// <summary>
        /// Получить все брони для указанного стола
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<string>))]
        public JsonResult GetBookingsForTable([Required] int Restaurant_TableID, long? Date_Booking_Unix)
        {
            DateTime date_Booking = DateTime.Now;
            if (Date_Booking_Unix.HasValue)
            {
                DateTimeOffset dateTimeOffsetBookingEnd = DateTimeOffset.FromUnixTimeMilliseconds(Date_Booking_Unix.Value);
                date_Booking = dateTimeOffsetBookingEnd.DateTime;
            }

            List<string>? result = _bl.Bookings.GetList(Restaurant_TableID: Restaurant_TableID, Date_Booking: date_Booking);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: result);
        }


        /// <summary>
        /// Возвращает сумму предоплаты брони
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(decimal))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public JsonResult GetBooking_Price()
        {
            decimal? val = HIDE_Convert.ToDecimal(_bl.Setups.Get(Key: Consts.SetupKey_Dop_BookingPrice)?.Val);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }


        /// <summary>
        /// Возвращает текст при бронировании столиков(Бронировать можно на ближайшие N месяцев)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(decimal))]
        [Obsolete]
        public JsonResult GetBooking_Text()
        {
            string? val = _bl.Setups.Get(Key: Consts.SetupKey_Dop_BookingText)?.Val;
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v3.OK, Data: val);
        }

    }
}