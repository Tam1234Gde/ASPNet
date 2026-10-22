using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.Dish_Colors;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Цвета блюда
    /// </summary>
    [Authorize]
    public class DishColorsController : Controller
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
        public DishColorsController(ILogger<DishColorsController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor DishColorsController");
            _bl = BL;
        }

        /// <summary>
        /// Список цветов
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int? DishID)
        {
            Model_Dish_Colors_List data = new()
            {
                List_Colors = Dish_Colors.GetList(List_from: null),
            };

            if (DishID > 0)
            {
                data.DishID = DishID;
                data.DishName = _bl.Dishes.Get(ID: (int)DishID)?.Name;
                data.Dish_CategoryID = _bl.Dishes.Get(ID: (int)DishID)?.Dish_CategoryID;
                data.List_Colors = _bl.Dish_Colors.GetList(DishID: (int)DishID);
            }

            return View(model: data);
        }


        /// <summary>
        /// Таблица > Список цветов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Dish_Colors_Part_Table(int? DishID)
        {
            Model_Dish_Colors_List data = new()
            {
                List_Colors = Dish_Colors.GetList(List_from: null),
            };

            if (DishID > 0)
            {
                data.DishID = DishID;
                data.DishName = _bl.Dishes.Get(ID: (int)DishID)?.Name;
                data.Dish_CategoryID = _bl.Dishes.Get(ID: (int)DishID)?.Dish_CategoryID;
                data.List_Colors = _bl.Dish_Colors.GetList(DishID: (int)DishID);
            }

            return View(model: data);
        }

        #region Добавление

        /// <summary>
        /// Добавление цвета
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add(int DishID)
        {
            Model_Dish_Colors_AddEdit model = new()
            {
                List_Colors = _bl.Colors.GetList(),
                DishID = DishID
            };

            return View(model);
        }

        /// <summary>
        /// Добавление цвета
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Dish_Colors_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                int dish_color_id = _bl.Dish_Colors.Add(DishID: data.DishID, ColorID: data.ColorID);

                if (dish_color_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index");
                }

                return RedirectToAction(controllerName: "DishColors", actionName: "Index", routeValues: new { data.DishID });
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
            CDish_Color? dish_color = _bl.Dish_Colors.Get(ID: ID);
            if (dish_color != null)
            {
                Model_Dish_Colors_AddEdit data = new()
                {
                    ID = dish_color.ID,
                    ColorID = dish_color.ColorID,
                    Color = dish_color.Color,
                    DishID = dish_color.DishID,
                    List_Colors = _bl.Colors.GetList()
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
        public IActionResult Edit(Model_Dish_Colors_AddEdit Data)
        {
            if (Data != null)
            {
                int dish_color_id = _bl.Dish_Colors.Edit(ID: Data.ID, DishID_New: Data.DishID, ColorID_New: Data.ColorID);

                if (dish_color_id > 0)
                {
                    return RedirectToAction(controllerName: "DishColors", actionName: "Index", routeValues: new { Data.DishID });
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
                CDish_Color? dish_color = _bl.Dish_Colors.Get(ID: ID);

                if (dish_color != null)
                {
                    Model_Dish_Colors_AddEdit data = new()
                    {
                        Color = dish_color.Color,
                        DishID = dish_color.DishID,
                        Dish = dish_color.Dish,
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
        public IActionResult Del(Model_Dish_Colors_AddEdit Data)
        {
            if (Data != null)
            {
                int dish_color_id = _bl.Dish_Colors.Del(ID: Data.ID, UserID: UserID);

                if (dish_color_id > 0)
                {
                    return RedirectToAction(controllerName: "DishColors", actionName: "Index", routeValues: new { Data.DishID });
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