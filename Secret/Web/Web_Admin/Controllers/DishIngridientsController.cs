using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.Dish_Ingridients;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Ингридиенты для блюда
    /// </summary>
    [Authorize]
    public class DishIngridientsController : Controller
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
        public DishIngridientsController(ILogger<DishIngridientsController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor DishIngridientsController");
            _bl = BL;
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список ингредиентов для блюда
        /// GET: /DishIngridients?DishID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetDishIngredients(int? DishID)
        {
            try
            {
                List<CDish_Ingridient> ingredients = new List<CDish_Ingridient>();

                if (DishID > 0)
                {
                    ingredients = _bl.Dish_Ingridients.GetList(DishID: (int)DishID);
                }
                else
                {
                    ingredients = _bl.Dish_Ingridients.GetList(DishID: 0);
                }

                List<object> result = new List<object>();
                foreach (CDish_Ingridient item in ingredients)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.DishID,
                        item.IngridientID,
                        IngridientName = item.Ingridient ?? string.Empty,
                        item.Price,
                        item.IngridientGroupID,
                        item.Cooking_Minutes,
                        item.Category_ID
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
        /// Получить ингредиент блюда по ID
        /// GET: /DishIngridients/GetDishIngredient?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetDishIngredient(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CDish_Ingridient ingredient = _bl.Dish_Ingridients.Get(ID: ID);
                if (ingredient == null)
                    return Json(new { success = false, message = "Ингредиент не найден" });

                object result = new
                {
                    ingredient.ID,
                    ingredient.DishID,
                    ingredient.IngridientID,
                    IngridientName = ingredient.Ingridient ?? string.Empty,
                    ingredient.Price,
                    ingredient.IngridientGroupID,
                    ingredient.Cooking_Minutes,
                    ingredient.Category_ID
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список ингредиентов по группе
        /// GET: /DishIngridients/GetDishIngredientsByGroup?IngridientGroupID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetDishIngredientsByGroup(int IngridientGroupID)
        {
            try
            {
                if (IngridientGroupID <= 0)
                    return Json(new { success = false, message = "Неверный ID группы" });

                List<CDish_Ingridient> allIngredients = _bl.Dish_Ingridients.GetList(DishID: 0);

                List<CDish_Ingridient> filteredIngredients = new List<CDish_Ingridient>();
                foreach (CDish_Ingridient item in allIngredients)
                {
                    if (item.IngridientGroupID == IngridientGroupID)
                    {
                        filteredIngredients.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CDish_Ingridient item in filteredIngredients)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.DishID,
                        item.IngridientID,
                        IngridientName = item.Ingridient ?? string.Empty,
                        item.Price,
                        item.Cooking_Minutes,
                        item.Category_ID
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
        /// Получить список ингредиентов по категории
        /// GET: /DishIngridients/GetDishIngredientsByCategory?Category_ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetDishIngredientsByCategory(int Category_ID)
        {
            try
            {
                if (Category_ID <= 0)
                    return Json(new { success = false, message = "Неверный ID категории" });

                List<CDish_Ingridient> allIngredients = _bl.Dish_Ingridients.GetList(DishID: 0);

                List<CDish_Ingridient> filteredIngredients = new List<CDish_Ingridient>();
                foreach (CDish_Ingridient item in allIngredients)
                {
                    if (item.Category_ID == Category_ID)
                    {
                        filteredIngredients.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CDish_Ingridient item in filteredIngredients)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.DishID,
                        item.IngridientID,
                        IngridientName = item.Ingridient ?? string.Empty,
                        item.Price,
                        item.IngridientGroupID,
                        item.Cooking_Minutes
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
        /// Список ингридиентов блюда
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int? DishID)
        {
            Model_Dish_Ingridients_List data = new()
            {
                List_Ingridients = Dish_Ingridients.GetList(List_from: null),
            };

            if (DishID > 0)
            {
                data.DishID = DishID;
                data.DishName = _bl.Dishes.Get(ID: (int)DishID)?.Name;
                data.Dish_CategoryID = _bl.Dishes.Get(ID: (int)DishID)?.Dish_CategoryID;
                data.List_Ingridients = _bl.Dish_Ingridients.GetList(DishID: (int)DishID);
            }

            return View(model: data);
        }

        /// <summary>
        /// Таблица > Список ингридиентов блюда
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Dish_Ingridients_Part_Table(int? DishID)
        {
            Model_Dish_Ingridients_List data = new()
            {
                List_Ingridients = Dish_Ingridients.GetList(List_from: null),
            };

            if (DishID > 0)
            {
                data.DishID = DishID;
                data.DishName = _bl.Dishes.Get(ID: (int)DishID)?.Name;
                data.Dish_CategoryID = _bl.Dishes.Get(ID: (int)DishID)?.Dish_CategoryID;
                data.List_Ingridients = _bl.Dish_Ingridients.GetList(DishID: (int)DishID);
            }

            return View(model: data);
        }

        #region Добавление

        /// <summary>
        /// Добавление ингридиента для блюда
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add(int DishID)
        {
            Model_Dish_Ingridients_AddEdit model = new()
            {
                List_Ingridients = _bl.Ingridients.GetList(),
                DishID = DishID,
            };

            return View(model);
        }

        /// <summary>
        /// Добавление ингридиента для блюда
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Dish_Ingridients_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                if (data.Flag_IsGroup && data.IngridientGroupID < 0)
                {
                    ModelState.AddModelError("Flag_IsGroup", "Группа не указана");
                }

                int dish_ingridient_id = _bl.Dish_Ingridients.Add(DishID: data.DishID, IngridientID: data.IngridientID, Price: data.Price,
                                                                  IngridientGroupID: data.IngridientGroupID, Cooking_Minutes: data.Cooking_Minutes);

                if (dish_ingridient_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index");
                }

                // Добавить ингредиент в другие рестораны
                if (data.FlagForAll)
                {
                    _bl.Restaurant_Menu_Dish_Ingredients.MassAddDishIngredients(DishID: data.DishID, Dish_IngridientID: dish_ingridient_id);
                }

                return RedirectToAction(controllerName: "DishIngridients", actionName: "Index", routeValues: new { data.DishID });
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение ингридиента для блюда
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CDish_Ingridient? dish_ingridient = _bl.Dish_Ingridients.Get(ID: ID);
            if (dish_ingridient != null)
            {
                Model_Dish_Ingridients_AddEdit data = new()
                {
                    ID = dish_ingridient.ID,
                    IngridientID = dish_ingridient.IngridientID,
                    Ingridient = dish_ingridient.Ingridient,
                    Price = dish_ingridient.Price,
                    DishID = dish_ingridient.DishID,
                    IngridientGroupID = dish_ingridient.IngridientGroupID,
                    Cooking_Minutes = dish_ingridient.Cooking_Minutes,
                    List_Ingridients = _bl.Ingridients.GetList()
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }
        }

        /// <summary>
        /// Изменение ингридиента для блюда
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Dish_Ingridients_AddEdit Data)
        {
            if (Data != null)
            {
                if (Data.Flag_IsGroup && Data.IngridientGroupID < 0)
                {
                    ModelState.AddModelError("Flag_IsGroup", "Группа не указана");
                }

                int dish_ingridient_id = _bl.Dish_Ingridients.Edit(ID: Data.ID, DishID_New: Data.DishID, IngridientID_New: Data.IngridientID,
                                                                   Price_New: Data.Price, IngridientGroupID_New: Data.IngridientGroupID, Cooking_Minutes_New: Data.Cooking_Minutes);

                if (dish_ingridient_id > 0)
                {
                    // Редактирование цены для ингредиента в других ресторанах
                    if (Data.FlagForAll)
                    {
                        _bl.Restaurant_Menu_Dish_Ingredients.EditIngredientPrices(DishID: Data.DishID, IngredientID: Data.IngridientID, Price: Data.Price);
                    }

                    return RedirectToAction(controllerName: "DishIngridients", actionName: "Index", routeValues: new { Data.DishID });
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
        /// Удаление ингридиента для блюда
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CDish_Ingridient? dish_ingridient = _bl.Dish_Ingridients.Get(ID: ID);

                if (dish_ingridient != null)
                {
                    Model_Dish_Ingridients_AddEdit data = new()
                    {
                        Ingridient = dish_ingridient.Ingridient,
                        IngridientID = dish_ingridient.IngridientID,
                        DishID = dish_ingridient.DishID,
                        Dish = dish_ingridient.Dish,
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
        /// Удаление ингридиента для блюда
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Dish_Ingridients_AddEdit Data)
        {
            if (Data != null)
            {
                int dish_ingridient_id = _bl.Dish_Ingridients.Del(ID: Data.ID, UserID: UserID);

                if (dish_ingridient_id > 0)
                {
                    // Удалить в других ресторанах
                    if (Data.FlagForAll)
                    {
                        _bl.Restaurant_Menu_Dish_Ingredients.DelIngredients(DishID: Data.DishID, IngredientID: Data.IngridientID);
                    }

                    return RedirectToAction(controllerName: "DishIngridients", actionName: "Index", routeValues: new { Data.DishID });
                }

                return View(model: Data);
            }
            return RedirectToAction(controllerName: "Error", actionName: "Index");
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