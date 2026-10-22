using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Site_Menu.Models.Common;
using HIDE.RestAndRant.Web_Site_Menu.Models.Order;
using HIDE.Units_Core;
using static HIDE.RestAndRant.Web_Site_Menu.Helpers.Helper_Client;

namespace HIDE.RestAndRant.Web_Site_Menu.Controllers
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class OrderController(BL_Context _bl) : BaseControllerWithUser
    {
        /// <summary>
        /// Ввод кода заказа
        /// </summary>
        /// <returns></returns>
        public IActionResult Order_Index(int? OrderID)
        {
            Model_Order model = new();

            if (OrderID.HasValue && OrderID.Value > 0)
            {
                model.OrderID = OrderID.Value;
                model.Order = _bl.Orders.Get(OrderID.Value);
            }

            return View(model);
        }


        /// <summary>
        /// Частичное представление > Модальное окно с подробностями заказа
        /// </summary>
        /// <param name="OrderID">Код заказа</param>
        /// <returns></returns>
        public IActionResult Part_OrderMore(int? OrderID)
        {
            Model_Order model = new();

            if (OrderID.HasValue && OrderID.Value > 0)
            {
                model.OrderID = OrderID.Value;
                model.Order = _bl.Orders.Get(OrderID.Value);
                model.Sum = _bl.Orders.GetOrderSum(OrderID.Value, out decimal? _);
            }

            // Получаем выбранный столик
            string? restaurant_TableID = Request.Cookies[Consts.Cookies_RestaurantTable];
            if (!string.IsNullOrWhiteSpace(restaurant_TableID))
            {
                string? restaurant_tablename = _bl.Restaurant_Tables.Get(HIDE_Convert.ToInt(restaurant_TableID))?.Name;
                ViewData["Restaurant_Table"] = restaurant_tablename;
            }

            return View(model);
        }


        /// <summary>
        /// Проверка кода
        /// </summary>
        /// <param name="OrderID">Код заказа</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CheckOrder(int OrderID)
        {
            COrder? cOrder = _bl.Orders.Get(OrderID);
            string? restaurant_TableID = Request.Cookies[Consts.Cookies_RestaurantTable];

            if (cOrder != null && cOrder.Date_Act.HasValue && cOrder.Date_Act.Value.Date == DateTime.Now.Date && HIDE_Convert.ToInt(restaurant_TableID) == cOrder.Booking?.Restaurant_TableID)
            {
                return new JsonResult((int)ModelJsonResult.OK);
            }

            return new JsonResult((int)ModelJsonResult.Err);
        }


        /// <summary>
        /// Заказ оформлен
        /// </summary>
        /// <returns></returns>
        public IActionResult Order_Done(int OrderID)
        {
            Model_Order model = new()
            {
                OrderID = OrderID
            };

            // Получаем выбранный столик
            string? restaurant_table = Request.Cookies[Consts.Cookies_RestaurantTable];
            if (!string.IsNullOrWhiteSpace(restaurant_table))
            {
                string? restaurant_tablename = _bl.Restaurant_Tables.Get(HIDE_Convert.ToInt(restaurant_table))?.Name;
                ViewData["Restaurant_Table"] = restaurant_tablename;
            }

            return View(model);
        }

    }
}