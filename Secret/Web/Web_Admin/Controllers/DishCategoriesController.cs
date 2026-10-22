using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.Dish_Categories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Категории блюд
    /// </summary>
    [Authorize]
    public class DishCategoriesController : Controller
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
        public DishCategoriesController(ILogger<DishCategoriesController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor DishCategoriesController");
            _bl = BL;
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех категорий блюд
        /// GET: /
        /// </summary>
        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                List<CDish_Category> categories = _bl.Dish_Categories.GetList(Flag_IsParent: false);

                List<object> result = new List<object>();
                foreach (CDish_Category item in categories)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.OrderNum,
                        item.Flag_AllowColors,
                        item.Cooking_Minutes,
                        item.Flag_IsParent,
                        item.ParentID,
                        item.Work_Hours,
                        item.Flag_UseDefaultWorkHours,
                        item.Flag_Del,
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
        /// Получить категорию по ID
        /// GET: /DishCategories/GetCategory?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetCategory(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CDish_Category category = _bl.Dish_Categories.Get(ID: ID);
                if (category == null)
                    return Json(new { success = false, message = "Категория не найдена" });

                object result = new
                {
                    category.ID,
                    category.Name,
                    category.OrderNum,
                    category.Flag_AllowColors,
                    category.Cooking_Minutes,
                    category.Flag_IsParent,
                    category.ParentID,
                    category.Work_Hours,
                    category.Flag_UseDefaultWorkHours,
                    category.Flag_Del,
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список родительских категорий
        /// GET: /DishCategories/GetParentCategories
        /// </summary>
        [HttpGet]
        public IActionResult GetParentCategories()
        {
            try
            {
                List<CDish_Category> categories = _bl.Dish_Categories.GetList(Flag_IsParent: true);

                List<object> result = new List<object>();
                foreach (CDish_Category item in categories)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.OrderNum,
                        item.Flag_AllowColors,
                        item.Cooking_Minutes,
                        item.Flag_IsParent,
                        item.ParentID,
                        item.Work_Hours,
                        item.Flag_UseDefaultWorkHours,
                        item.Flag_Del,

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
        /// Получить список активных категорий (не удаленных)
        /// GET: /DishCategories/GetActiveCategories
        /// </summary>
        [HttpGet]
        public IActionResult GetActiveCategories()
        {
            try
            {
                List<CDish_Category> allCategories = _bl.Dish_Categories.GetList(Flag_IsParent: false);

                List<CDish_Category> activeCategories = new List<CDish_Category>();
                foreach (CDish_Category item in allCategories)
                {
                    if (!item.Flag_Del)
                    {
                        activeCategories.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CDish_Category item in activeCategories)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.OrderNum,
                        item.Flag_AllowColors,
                        item.Cooking_Minutes,
                        item.Flag_IsParent,
                        item.ParentID,
                        item.Work_Hours,
                        item.Flag_UseDefaultWorkHours,
                        item.Flag_Del,
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
        /// Список категорий
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<CDish_Category> list_dish_categories = _bl.Dish_Categories.GetList(Flag_IsParent: false);
            return View(model: list_dish_categories);
        }

        /// <summary>
        /// Таблица > Список категорий
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Dish_Categories_Part_Table()
        {
            List<CDish_Category> list_dish_categories = _bl.Dish_Categories.GetList(Flag_IsParent: false);
            return View(model: list_dish_categories);
        }

        #region Добавление

        /// <summary>
        /// Добавление категории
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            Model_Dish_Categories_AddEdit model = new()
            {
                List_ParentCategories = _bl.Dish_Categories.GetList(Flag_IsParent: true)
            };

            return View(model);
        }

        /// <summary>
        /// Добавление категории
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Dish_Categories_AddEdit data)
        {

            // ВРЕМЕННО: Вывод всех ошибок валидации
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    // Логируем или смотрим в отладчике
                    var errorMessage = error.ErrorMessage;
                    var exception = error.Exception?.Message;
                }
            }

            if (ModelState.IsValid && data != null)
            {
                int dish_category_id = _bl.Dish_Categories.Add(
                    Name: data.Name,
                    OrderNum: data.OrderNum,
                    Flag_AllowColors: data.Flag_AllowColors,
                    Cooking_Minutes: data.Cooking_Minutes,
                    Flag_IsParent: data.Flag_IsParent,
                    ParentID: data.ParentID,
                    Work_Hours: data.Work_Hours,
                    Flag_UseDefaultWorkHours: data.Flag_UseDefaultWorkHours
                );

                if (dish_category_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index");
                }

                return RedirectToAction("Index"); //Возвращаем к списку категорий
            }
            else
            {
                data?.List_ParentCategories = _bl.Dish_Categories.GetList(Flag_IsParent: true);

                return View(data);
            }
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение категории (GET)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CDish_Category? dish_category = _bl.Dish_Categories.Get(ID: ID);
            if (dish_category != null)
            {
                Model_Dish_Categories_AddEdit data = new()
                {
                    ID = dish_category.ID,
                    Name = dish_category.Name,
                    OrderNum = dish_category.OrderNum,
                    Flag_AllowColors = dish_category.Flag_AllowColors,
                    Cooking_Minutes = dish_category.Cooking_Minutes,
                    Flag_IsParent = dish_category.Flag_IsParent,
                    ParentID = dish_category.ParentID,
                    Work_Hours = dish_category.Work_Hours,
                    // Загружаем список родительских категорий
                    List_ParentCategories = _bl.Dish_Categories.GetList(Flag_IsParent: true) ?? new List<CDish_Category>()
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }
        }

        /// <summary>
        /// Изменение категории (POST)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Dish_Categories_AddEdit Data)
        {
            // Всегда загружаем список родительских категорий для отображения в случае ошибки
            Data.List_ParentCategories = _bl.Dish_Categories.GetList(Flag_IsParent: true) ?? [];

            ModelState.Remove("Work_Hours"); 

            // Проверяем валидность модели
            if (ModelState.IsValid && Data != null)
            {
                int result = _bl.Dish_Categories.Edit(
                    ID: Data.ID,
                    Name_New: Data.Name,
                    OrderNum_New: Data.OrderNum,
                    Flag_AllowColors_New: Data.Flag_AllowColors,
                    Cooking_Minutes_New: Data.Cooking_Minutes,
                    Flag_IsParent_New: Data.Flag_IsParent,
                    ParentID_New: Data.ParentID,
                    Work_Hours_New: Data.Work_Hours,
                    Flag_UseDefaultWorkHours: Data.Flag_UseDefaultWorkHours
                );

                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Ошибка при сохранении
                    ModelState.AddModelError("", "Ошибка при сохранении категории");
                    return View(model: Data);
                }
            }
            else
            {
                // Если модель не валидна, возвращаем с ошибками
                return View(model: Data);
            }
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CDish_Category? dish_category = _bl.Dish_Categories.Get(ID: ID);

                if (dish_category != null)
                {
                    Model_Dish_Categories_AddEdit data = new()
                    {
                        Name = dish_category.Name
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
        /// Удаление категории
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Dish_Categories_AddEdit Data)
        {
            if (Data != null)
            {
                int dish_category_id = _bl.Dish_Categories.Del(ID: Data.ID, UserID: UserID);

                if (dish_category_id > 0)
                {
                    return RedirectToAction("Index");
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