using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.Colors;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Цвета
    /// </summary>
    [Authorize]
    public class ColorsController : Controller
    {
        private readonly ILogger _log;

        readonly BL_Context _bl;

        private int user_id;

        /// <summary>
        /// Определение ID пользователя
        /// </summary>
        private int UserID
        {
            get
            {
                if (user_id <= 0 && HttpContext != null)
                {
                    user_id = Helper_User.GetCurrentUserID(HttpContext.User);
                }
                return user_id;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public ColorsController(ILogger<ColorsController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor ColorsController");
            _bl = BL;
        }

        /// <summary>
        /// Список цветов
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<CColor>? list_colors = _bl.Colors.GetList();
            return View(model: list_colors);
        }

        /// <summary>
        /// Таблица > Список цветов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Colors_Part_Table()
        {
            List<CColor>? list_colors = _bl.Colors.GetList();
            return View(model: list_colors);
        }

        #region Добавление

        /// <summary>
        /// Добавление цвета
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Добавление цвета
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Colors_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                int color_id = _bl.Colors.Add(Name: data.Name, Name_Eng: data.Name_Eng, HexColor: data.HexColor);

                if (color_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index");
                }

                return RedirectToAction(controllerName: "Colors", actionName: "Index");
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение цвета
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CColor? color = _bl.Colors.Get(ID: ID);
            if (color != null)
            {
                Model_Colors_AddEdit data = new()
                {
                    ID = color.ID,
                    Name = color.Name,
                    Name_Eng = color.Name_Eng,
                    HexColor = color.HexColor
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }
        }

        /// <summary>
        /// Изменение цвета
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Colors_AddEdit Data)
        {
            if (Data != null)
            {
                int color_id = _bl.Colors.Edit(ID: Data.ID, Name_New: Data.Name, Name_Eng_New: Data.Name_Eng, HexColor_New: Data.HexColor);

                if (color_id > 0)
                {
                    return RedirectToAction(controllerName: "Colors", actionName: "Index");
                }
                else
                {
                    return View(model: Data);
                }
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удаление цвета
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CColor? color = _bl.Colors.Get(ID: ID);

                if (color != null)
                {
                    Model_Colors_AddEdit data = new()
                    {
                        Name = color.Name
                    };

                    return View(model: data);
                }
                else
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index");
                }
            }
            else
            {
                return RedirectToAction(controllerName: "Admin", actionName: "Error");
            }
        }

        /// <summary>
        /// Удаление цвета
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Colors_AddEdit Data)
        {
            if (Data != null)
            {
                int color_id = _bl.Colors.Del(ID: Data.ID, UserID: UserID);

                if (color_id > 0)
                {
                    return RedirectToAction(controllerName: "Colors", actionName: "Index");
                }
                else
                {
                    return View(model: Data);
                }
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }
        }

        #endregion Удаление

        /// <summary>
        /// Выполняется для всех методов в контроллере
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
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