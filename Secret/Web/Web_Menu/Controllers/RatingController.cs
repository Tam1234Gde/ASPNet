using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Helpers;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Site_Menu.Models.Common;
using HIDE.RestAndRant.Web_Site_Menu.Models.Rating;
using HIDE.Units_Core;
using static HIDE.RestAndRant.Web_Site_Menu.Helpers.Helper_Client;

namespace HIDE.RestAndRant.Web_Site_Menu.Controllers
{
    /// <summary>
    /// Оценка
    /// </summary>
    /// <param name="_bl"></param>
    /// <param name="_log"></param>
    public class RatingController(BL_Context _bl, ILogger<CartController> _log) : BaseControllerWithUser
    {
        /// <summary>
        /// Оценка
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public IActionResult Rating_Index(int? OrderID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Оценка");
            }

            Model_Rating model = new();
            if (OrderID.HasValue && OrderID.Value > 0)
            {
                model.OrderID = OrderID.Value;
            }

            return View(model);
        }


        /// <summary>
        /// Отправка отзыва
        /// </summary>
        /// RestAndRant@mail.ru
        /// support@HIDEerv.ru
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RatingSend(Model_Rating Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Отправка отзыва");
            }

            // Получаем номер столика
            string? restaurant_table = Request.Cookies[Consts.Cookies_RestaurantTable];

            if (Data.Grade == 0 || string.IsNullOrWhiteSpace(restaurant_table))
            {
                return new JsonResult((int)ModelJsonResult.Err);
            }

            int? restaurantid = _bl.Restaurant_Tables.Get(HIDE_Convert.ToInt(restaurant_table))?.RestaurantID;

            CRestaurant? restaurant = _bl.Restaurants.Get(HIDE_Convert.ToInt(restaurantid));

            if (restaurant == null)
            {
                return new JsonResult((int)ModelJsonResult.Err);
            }

            // Добавление отзыва
            int result = _bl.Ratings.AddSite(ClientID: HIDE_Convert.ToInt(restaurant.ClientID), Grade: Data.Grade, Text: Data.Text, Phone: Data.Phone,
                                             Restaurant_TableID: HIDE_Convert.ToInt_Nullable(restaurant_table));

            if (result < 0)
            {
                return new JsonResult((int)ModelJsonResult.Err);
            }

            // Отправка отзыва на почту при 3 и меньше звездах
#if !DEBUG
            if (Data.Grade <= 3)
            {
                List<string> list_emails = ["RestAndRant@mail.ru", "support@HIDEerv.ru"];
                foreach (string email in list_emails)
                {
                    string text = $"Количество звезд: {Data.Grade} \n" +
                                  $"Номер телефона: {Data.Phone ?? "Не указан"} \n" +
                                  $"Отзыв: {Data.Text ?? "Не указан"} \n" +
                                  $"Столик №{restaurant_table} \n" +
                                  $"Заказ: {string.Format(Data.OrderID.HasValue ? Data.OrderID.Value.ToString() : "Не указан")}";

                    // Отправка письма
                    bool? result_email = new Helper_Email().SendEmail(EmailTo: email, Theme: "RestAndRant - Отзыв", Text: text, Flag_Html: false);

                    if (result_email.HasValue && !result_email.Value)
                    {
                        _log.LogError("Ошибка отправки письма с отзывом на Email= {}", email);
                    }

                }
            }
#endif

            return new JsonResult((int)ModelJsonResult.OK);
        }


        /// <summary>
        /// Спасибо за отзыв
        /// </summary>
        /// <returns></returns>
        public IActionResult Rating_Thanks()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Спасибо за отзыв");
            }

            // Получаем выбранный столик
            string? restaurant_table = Request.Cookies[Consts.Cookies_RestaurantTable];
            if (!string.IsNullOrWhiteSpace(restaurant_table))
            {
                string? restaurant_tablename = _bl.Restaurant_Tables.Get(HIDE_Convert.ToInt(restaurant_table))?.Name;
                ViewData["Restaurant_Table"] = restaurant_tablename;
            }

            return View();
        }

    }
}