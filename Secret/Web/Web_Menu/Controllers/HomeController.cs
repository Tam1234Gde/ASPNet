using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Site_Menu.Models.Cart;
using HIDE.RestAndRant.Web_Site_Menu.Models.Home;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Site_Menu.Controllers
{
    /// <summary>
    /// Основной контроллер
    /// </summary>
    /// <param name="_bl"></param>
    public class HomeController(BL_Context _bl) : Controller
    {
        /// <summary>
        /// Главная страница
        /// </summary>
        /// <param name="QRCode">Уникальный код стола для QR-кода</param>
        /// <returns></returns>
        public IActionResult Index(string? QRCode)
        {
            string cookie_method = "";

            // Кэшируем столик
            if (!string.IsNullOrWhiteSpace(QRCode))
            {
                ChooseRestaurantTable(QRCode, out cookie_method);
            }

            Model_Home_Index model = new();

            // Получаем время следующего бронирования для данного столика
            string? cookies = Request.Cookies[Consts.Cookies_RestaurantTable];
            if (!string.IsNullOrWhiteSpace(cookies) || !string.IsNullOrWhiteSpace(cookie_method))
            {
                cookies = !string.IsNullOrWhiteSpace(cookie_method) ? cookie_method : cookies;
                CBooking? date = _bl.Bookings.Get(Restaurant_TableID: HIDE_Convert.ToInt(cookies), Date_Booking: DateTime.Now);

                model.Date_NextBookingEnd = date?.Date_End;
                model.Date_NextBooking = date?.Date_Start;

                string? cookie = Request.Cookies["Order"];

                if (!string.IsNullOrWhiteSpace(cookie) || string.IsNullOrWhiteSpace(cookie_method))
                {
                    model.BookingEnd = HIDE_Convert.ToDateTime_Nullable(cookie);
                }
            }

            return View(model: model);
        }


        /// <summary>
        /// Нижняя панель
        /// </summary>
        /// <returns></returns>
        public IActionResult Part_BottomBar(string? Menu)
        {
            Model_BottomBar model = new();
            decimal dish_count = 0;
            List<Model_CartItem_Add>? list_result = [];
            string? Cookies_Cart = Request.Cookies.TryGetValue(Consts.Cookies_Cart, out string? strVal) ? strVal : null;

            if (!string.IsNullOrWhiteSpace(Cookies_Cart))
            {
                list_result = JsonConvert.DeserializeObject<List<Model_CartItem_Add>>(Cookies_Cart);

                if (list_result != null && list_result.Count != 0)
                {
                    dish_count = list_result.Select(p => p.Dish_Count).Sum();
                }
            }

            model.Dish_Count = dish_count;
            model.Menu = Menu;

            return View(model: model);
        }


        /// <summary>
        /// Добавить в куки Выбор столика
        /// </summary>
        /// <param name="QR_Code">Уникальный код стола для QR-кода</param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpGet]
        public int ChooseRestaurantTable(string QR_Code, out string cookie)
        {
            cookie = "";

            Response.Cookies.Delete(Consts.Cookies_Cart);
            Response.Cookies.Delete("Order");

            CRestaurant_Table? restaurant_table = _bl.Restaurant_Tables.Get(QR_Code: QR_Code);

            if (restaurant_table == null)
            {
                return Errors.Err200;
            }

            CookieOptions CookieOptions = new()
            {
                Expires = DateTime.Now.AddHours(2)
            };
            Response.Cookies.Append(Consts.Cookies_RestaurantTable, restaurant_table.ID.ToString(), options: CookieOptions);

            Response.Cookies.Append(Consts.Cookies_Restaurant, restaurant_table.RestaurantID.ToString(), options: CookieOptions);

            cookie = restaurant_table.ID.ToString();

            return 1;
        }

    }
}