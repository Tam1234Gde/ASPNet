using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Admin.Models.Dish_Categories;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    public class DishParentCategoriesController : Controller
    {
        private readonly ILogger<DishParentCategoriesController> _logger;
        private readonly Dish_Categories _bl_DishCategory;

        public DishParentCategoriesController(ILogger<DishParentCategoriesController> logger)
        {
            _logger = logger;
            _bl_DishCategory = new Dish_Categories();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.Flag_ShowMenu = true;
            ViewBag.RestaurantID = null;
        }

        #region API Methods (JSON)

        /// <summary>
        /// Получение списка родительских категорий
        /// GET: /DishParentCategories
        /// </summary>
        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                List<CDish_Category> categories = _bl_DishCategory.GetParentList(Flag_Del_val: null);

                List<object> result = new List<object>();
                foreach (CDish_Category c in categories)
                {
                    result.Add(new
                    {
                        c.ID,
                        c.Name,
                        c.OrderNum,
                        c.Flag_IsParent,
                        c.Flag_AllowColors,
                        c.Cooking_Minutes,
                        c.ParentID,
                        c.Work_Hours,
                        c.Flag_UseDefaultWorkHours,
                        c.Flag_Del,

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
        /// Получение родительской категории по ID
        /// GET: /DishParentCategories/GetCategory?id={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetCategory(int id)
        {
            try
            {
                if (id <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CDish_Category category = _bl_DishCategory.Get(id);
                if (category == null)
                    return Json(new { success = false, message = "Категория не найдена" });

                object result = new
                {
                    category.ID,
                    category.Name,
                    category.OrderNum,
                    category.Flag_IsParent,
                    category.Flag_AllowColors,
                    category.Cooking_Minutes,
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
        /// Получение списка активных родительских категорий
        /// GET: /DishParentCategories/GetActiveCategories
        /// </summary>
        [HttpGet]
        public IActionResult GetActiveCategories()
        {
            try
            {
                List<CDish_Category> categories = _bl_DishCategory.GetParentList(Flag_Del_val: false);

                List<object> result = new List<object>();
                foreach (CDish_Category c in categories)
                {
                    result.Add(new
                    {
                        c.ID,
                        c.Name,
                        c.OrderNum,
                        c.Flag_IsParent,
                        c.Flag_AllowColors,
                        c.Cooking_Minutes,
                        c.ParentID,
                        c.Work_Hours,
                        c.Flag_UseDefaultWorkHours,
                        c.Flag_Del,
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
        /// Получение списка удаленных родительских категорий
        /// GET: /DishParentCategories/GetDeletedCategories
        /// </summary>
        [HttpGet]
        public IActionResult GetDeletedCategories()
        {
            try
            {
                List<CDish_Category> categories = _bl_DishCategory.GetParentList(Flag_Del_val: true);

                List<object> result = new List<object>();
                foreach (CDish_Category c in categories)
                {
                    result.Add(new
                    {
                        c.ID,
                        c.Name,
                        c.OrderNum,
                        c.Flag_IsParent,
                        c.Flag_AllowColors,
                        c.Cooking_Minutes,
                        c.ParentID,
                        c.Work_Hours,
                        c.Flag_UseDefaultWorkHours,
                        c.Flag_Del,

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

        #region Список

        public IActionResult Index()
        {
            List<CDish_Category> categories = _bl_DishCategory.GetParentList(Flag_Del_val: null);
            return View(categories);
        }

        #endregion

        #region Добавление

        [HttpGet]
        public IActionResult Add()
        {
            Model_Dish_Categories_AddEdit model = new Model_Dish_Categories_AddEdit();
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Model_Dish_Categories_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                // Получаем выбранные часы из формы
                string[] workHours = Request.Form["WorkHours"].ToArray();
                int[] workHoursArray = workHours.Select(int.Parse).ToArray();

                int id = _bl_DishCategory.Add(
                    Name: data.Name,
                    OrderNum: data.OrderNum,
                    Flag_AllowColors: false,
                    Cooking_Minutes: null,
                    Flag_IsParent: true,
                    ParentID: null,
                    Work_Hours: workHoursArray,
                    Flag_UseDefaultWorkHours: data.Flag_UseDefaultWorkHours
                );

                if (id > 0)
                {
                    TempData["Success"] = "Категория успешно добавлена.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(data);
        }

        #endregion

        #region Редактирование

        [HttpGet]
        public IActionResult Edit(int id)
        {
            CDish_Category? category = _bl_DishCategory.Get(id);
            if (category == null)
                return RedirectToAction("Index", "Error");

            Model_Dish_Categories_AddEdit model = new Model_Dish_Categories_AddEdit
            {
                ID = category.ID,
                Name = category.Name,
                OrderNum = category.OrderNum,
                Work_Hours = category.Work_Hours,
                Flag_UseDefaultWorkHours = category.Flag_UseDefaultWorkHours
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Model_Dish_Categories_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                // Получаем выбранные часы из формы
                string[] workHours = Request.Form["WorkHours"].ToArray();
                int[] workHoursArray = workHours.Select(int.Parse).ToArray();

                int result = _bl_DishCategory.Edit(
                    ID: data.ID,
                    Name_New: data.Name,
                    OrderNum_New: data.OrderNum,
                    Flag_AllowColors_New: false,
                    Cooking_Minutes_New: null,
                    Flag_IsParent_New: true,
                    ParentID_New: null,
                    Work_Hours_New: workHoursArray,
                    Flag_UseDefaultWorkHours: data.Flag_UseDefaultWorkHours
                );

                if (result > 0)
                {
                    TempData["Success"] = "Категория успешно обновлена.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(data);
        }

        #endregion

        #region Удаление

        [HttpGet]
        public IActionResult Del(int id)
        {
            CDish_Category? category = _bl_DishCategory.Get(id);
            if (category == null)
                return RedirectToAction("Index", "Error");

            Model_Dish_Categories_AddEdit model = new Model_Dish_Categories_AddEdit
            {
                ID = category.ID,
                Name = category.Name
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Del(Model_Dish_Categories_AddEdit data)
        {
            if (data == null || data.ID <= 0)
                return RedirectToAction("Index", "Error");

            int result = _bl_DishCategory.Del(data.ID, GetCurrentUserId());
            if (result > 0)
            {
                TempData["Success"] = "Категория удалена.";
                return RedirectToAction(nameof(Index));
            }

            return View(data);
        }

        #endregion

        #region Восстановление

        [HttpGet]
        public IActionResult Restore(int id)
        {
            if (id <= 0)
                return RedirectToAction("Index", "Error");

            int result = _bl_DishCategory.Restore(id);
            if (result > 0)
            {
                TempData["Success"] = "Категория восстановлена.";
            }
            else
            {
                TempData["Error"] = "Ошибка при восстановлении.";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Вспомогательные методы

        private int GetCurrentUserId()
        {
            return 1;
        }

        #endregion
    }
}