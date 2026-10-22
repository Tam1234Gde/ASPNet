using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Web_Site_Menu.Models.Cabinet;
using static HIDE.RestAndRant.Web_Site_Menu.Helpers.Helper_Client;

namespace HIDE.RestAndRant.Web_Site_Menu.Controllers
{
    /// <summary>
    /// Личный кабинет
    /// </summary>
    /// <param name="_bl"></param>
    /// <param name="_log"></param>
    [Authorize]
    public class CabinetController(BL_Context _bl, ILogger<CartController> _log) : BaseControllerWithUser
    {
        /// <summary>
        /// Главная страница
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Главная страница");
            }

            Model_Cabinet_Index model = new()
            {
                List_Orders = _bl.Orders.GetList_ForAdmin_Active(ClientID: Client_ID, Minuts_AddCompleted: 20)
            };

            return View(model: model);
        }

    }
}