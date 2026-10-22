using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Helpers;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Web_Site_Menu.Models.Common;
using HIDE.Units_Core;
using static HIDE.RestAndRant.Web_Site_Menu.Helpers.Helper_Client;

namespace HIDE.RestAndRant.Web_Site_Menu.Controllers
{
    /// <summary>
    /// Общий контроллер
    /// </summary>
    /// <param name="_bl"></param>
    public class CommonController(BL_Context _bl) : BaseControllerWithUser
    {
        /// <summary>
        /// Комментарий официанту
        /// </summary>
        /// <returns></returns>
        public IActionResult Comment(string? Comment)
        {
            Model_Comment model = new();

            if (!string.IsNullOrWhiteSpace(Comment))
            {
                model.Comment = Comment;
            }

            return View(model);
        }


        /// <summary>
        /// Отправить сообщение в телеграм
        /// </summary>
        /// <param name="Type">Тип сообщения</param>
        /// <param name="Text">Сообщение</param>
        /// <param name="OrderID">Код заказа</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TgSend(ETgTypeMessage Type, string? Text, int? OrderID)
        {
            // Проверка указан ли тип
            if (Type == ETgTypeMessage._NotSet)
            {
                return new JsonResult((int)ModelJsonResult.Err);
            }

            // Получаем выбранный столик
            string? restaurant_table = Request.Cookies[Consts.Cookies_RestaurantTable];
            if (string.IsNullOrWhiteSpace(restaurant_table))
            {
                return new JsonResult((int)ModelJsonResult.Err);
            }

            // Получаем название столика
            string? restaurant_tablename = _bl.Restaurant_Tables.Get(HIDE_Convert.ToInt(restaurant_table))?.Name;
            ViewData["Restaurant_Table"] = restaurant_tablename;

            // Текст сообщения
            string text = string.Empty;

            // Формирование сообщения в зависимости от типа
            switch (Type)
            {
                case ETgTypeMessage.Menu:
                    text = $"Столик №{restaurant_tablename} - Просят меню";
                    break;

                case ETgTypeMessage.Waiter:
                    text = $"Столик №{restaurant_tablename} - Позвать официанта";
                    break;

                case ETgTypeMessage.Comment:
                    // Проверка указан ли комментарий
                    if (string.IsNullOrEmpty(Text))
                    {
                        return new JsonResult((int)ModelJsonResult.Err);
                    }

                    text = $"Столик №{restaurant_tablename} - Комментарий официанту: \n {Text}";
                    break;

                case ETgTypeMessage.Pay:
                    // Проверка передан ли номер заказа
                    if (!OrderID.HasValue)
                    {
                        text = $"Оплатить счет для столика №{restaurant_tablename}";
                    }
                    else
                    {
                        text = $"Оплатить счет для столика №{restaurant_tablename}. Код заказа - №{OrderID.Value}";
                    }
                    break;
            }

            int? restaurantid = _bl.Restaurant_Tables.Get(HIDE_Convert.ToInt(restaurant_table))?.RestaurantID;

            CRestaurant? restaurant = _bl.Restaurants.Get(HIDE_Convert.ToInt(restaurantid));

            if (restaurant == null || string.IsNullOrWhiteSpace(restaurant.TgChatID))
            {
                return new JsonResult((int)ModelJsonResult.Err);
            }

#if !DEBUG
            Helper_Telegram _h_Telegram = new();
            // Отправка сообщения в телеграм
            _h_Telegram.SendMessageTelegram(ChatID: restaurant.TgChatID, MessageText: text, out string? _);
#endif

            return new JsonResult((int)ModelJsonResult.OK);
        }


        /// <summary>
        /// Проверка столика
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CheckRestaurantTable()
        {
            string? restaurant_TableID = Request.Query["Restaurant_TableID"];

            // Если в кэше столик не сохранен и в URL нет столика, то выводим модалку "Отсканируйте QR-код"
            if (Request.Cookies[Consts.Cookies_RestaurantTable] == null && string.IsNullOrEmpty(restaurant_TableID))
            {
                return new JsonResult((int)ModelJsonResult.Err);
            }

            return new JsonResult((int)ModelJsonResult.OK);
        }

    }
}