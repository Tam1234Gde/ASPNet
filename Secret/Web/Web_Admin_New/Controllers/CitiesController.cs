using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin_ForMenu.Helpers;
using HIDE.RestAndRant.Web_Admin_ForMenu.Models.Cities;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    /// <summary>
    /// Города
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Authorize]
    public class CitiesController(ILogger<OrdersController> _log, BL_Context _bl) : Controller
    {
        private int user_id;


        /// <summary>
        /// Определение ID пользователя
        /// </summary>
        private int UserID
        {
            get
            {
                if (_log.IsEnabled(LogLevel.Information))
                {
                    _log.LogInformation(message: "{}", args: "Выполнение метода Определение ID пользователя");
                }

                if (user_id <= 0 && HttpContext != null)
                {
                    user_id = Helper_User.GetCurrentUserID(HttpContext.User);
                }
                return user_id;
            }
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех городов
        /// GET: /Cities/City_Index
        /// </summary>
        [HttpGet]
        [Route("City_Index")]
        public IActionResult GetCities()
        {
            try
            {
                List<CCity> cities = _bl.Cities.GetList();

                List<object> result = new List<object>();
                foreach (CCity item in cities)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.NameDeclension,
                        item.OrderNum,
                        item.Flag_Hidden,
                        item.HiddenTitle,
                        item.HiddenText
                    });
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить город по ID
        /// GET: /Cities/GetCity?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetCity(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CCity city = _bl.Cities.Get(ID: ID);
                if (city == null)
                    return Json(new { success = false, message = "Город не найден" });

                object result = new
                {
                    city.ID,
                    city.Name,
                    city.NameDeclension,
                    city.OrderNum,
                    city.Flag_Hidden,
                    city.HiddenTitle,
                    city.HiddenText
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список активных городов (не скрытых)
        /// GET: /Cities/GetActiveCities
        /// </summary>
        [HttpGet]
        public IActionResult GetActiveCities()
        {
            try
            {
                List<CCity> allCities = _bl.Cities.GetList();

                List<CCity> activeCities = new List<CCity>();
                foreach (CCity item in allCities)
                {
                    if (!item.Flag_Hidden)
                    {
                        activeCities.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CCity item in activeCities)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.NameDeclension,
                        item.OrderNum,
                        item.HiddenTitle,
                        item.HiddenText
                    });
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion


        /// <summary>
        /// Список
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult City_Index()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Список");
            }

            List<CCity> list = _bl.Cities.GetList();
            return View(model: list);
        }


        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult City_Part_Table()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Таблица > Список");
            }

            return City_Index();
        }


        #region Добавление


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult City_Add()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление1");
            }

            return View();
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult City_Add(Model_Cities_CRUD data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление2");
            }

            if (ModelState.IsValid && data != null)
            {
                int result = _bl.Cities.Add(Name: data.Name, NameDeclension: data.NameDeclension, OrderNum: data.OrderNum, Flag_Hidden: data.Flag_Hidden,
                                            HiddenTitle: data.HiddenTitle, HiddenText: data.HiddenText);

                if (result < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = result });
                }

                return RedirectToAction("City_Index");
            }
            else
            {
                return View();
            }
        }


        #endregion


        #region Изменение

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult City_Edit(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение1");
            }

            CCity? elem = _bl.Cities.Get(ID: ID);
            if (elem != null)
            {
                Model_Cities_CRUD data = new()
                {
                    ID = elem.ID,
                    Name = elem.Name,
                    NameDeclension = elem.NameDeclension,
                    OrderNum = elem.OrderNum,
                    Flag_Hidden = elem.Flag_Hidden,
                    HiddenTitle = elem.HiddenTitle,
                    HiddenText = elem.HiddenText,
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult City_Edit(Model_Cities_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение2");
            }

            if (Data != null)
            {
                int result = _bl.Cities.Edit(ID: Data.ID, Name: Data.Name, NameDeclension: Data.NameDeclension, OrderNum: Data.OrderNum, Flag_Hidden: Data.Flag_Hidden,
                                             HiddenTitle: Data.HiddenTitle, HiddenText: Data.HiddenText);

                if (result > 0)
                {
                    return RedirectToAction("City_Index");
                }
                else
                {
                    return View(model: Data);
                }
            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }

        #endregion


        #region Удаление

        /// <summary>
        /// Удаление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult City_Del(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление1");
            }

            if (ID > 0)
            {
                CCity? elem = _bl.Cities.Get(ID: ID);

                if (elem != null)
                {
                    Model_Cities_CRUD data = new()
                    {
                        Name = elem.Name
                    };

                    return View(model: data);
                }
                else
                {
                    return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
                }
            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult City_Del(Model_Cities_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление2");
            }

            if (Data != null)
            {
                int result = _bl.Cities.Del(ID: Data.ID, UserID: UserID);

                if (result > 0)
                {
                    return RedirectToAction("City_Index");
                }
                else
                {
                    return View(model: Data);
                }
            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }

        #endregion Удаление

        /// <summary>
        /// Редирект на страницу с описанием ошибки
        /// </summary>
        /// <param name="ErrCode"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        private RedirectToActionResult RedirectToError(int ErrCode, string Url)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Редирект на страницу с описанием ошибки");
            }

            return RedirectToAction(actionName: Contst_Admin.ErrorActionName,
                                 controllerName: Contst_Admin.ErrorControllerName,
                                 routeValues: new { ErrCode, Url });

        }

        #region Получение имени пользователя

        /// <summary>
        /// Получение имени пользователя
        /// </summary>
        /// <returns></returns>
        public string? Get_UserName()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Получение имени пользователя");
            }

            CUser? user = _bl.Users.Get(ID: Helper_User.GetCurrentUserID(HttpContext.User));
            if (user != null)
            {
                return user.Name;
            }
            return "";
        }

        #endregion Получение имени пользователя

        /// <summary>
        /// Выполняется для всех методов в контроллере
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Выполняется для всех методов в контроллере");
            }

            ViewBag.Flag_ShowMenu = Get_FlagAdmin();
            base.OnActionExecuting(filterContext);
        }

        #region Получение Флага > Админ

        /// <summary>
        /// Получение Флага > Админ
        /// </summary>
        /// <returns></returns>
        public bool Get_FlagAdmin()
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Получение Флага > Админ");
            }

            CUser? user = _bl.Users.Get(ID: Helper_User.GetCurrentUserID(HttpContext.User));
            if (user != null && user.Flag_Root)
            {
                ViewBag.Flag_ShowMenu = true;
                return true;
            }

            ViewBag.Flag_ShowMenu = false;
            return false;
        }

        #endregion Получение Флага > Админ

    }
}