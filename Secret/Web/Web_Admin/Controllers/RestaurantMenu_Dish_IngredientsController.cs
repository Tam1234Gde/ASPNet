using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.RestaurantMenu_Dish_Ingredients;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Ингредиенты блюда в ресторане
    /// </summary>
    /// <param name="_log"></param>
    /// <param name="_bl"></param>
    [Authorize]
    public class RestaurantMenu_Dish_IngredientsController(ILogger<OrdersController> _log, BL_Context _bl) : Controller //TODO: Logs
    {
        readonly Images Images_BL = new();
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


        /// <summary>
        /// Список
        /// </summary>
        /// <param name="Restaurant_Menu_DishID">Код блюда ресторана</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Ingredient_Index(int Restaurant_Menu_DishID, int? Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Список");
            }

            var dish = _bl.Restaurant_Menu_Dishes.Get(Restaurant_Menu_DishID);

            Model_RestaurantMenu_Dish_Ingredients_Index model = new()
            {
                List_Restaurant_Menu_Dish_Ingredients = _bl.Restaurant_Menu_Dish_Ingredients.GetList(Restaurant_Menu_DishID),
                Restaurant_Menu_DishID = Restaurant_Menu_DishID,
                Restaurant_Menu_Dish = dish?.Dish ?? String.Empty,
                RestaurantMenuID = dish!.Restaurant_MenuID
            };

            if (Dish_CategoryID.HasValue && Dish_CategoryID.Value > 0)
            {
                model.Dish_CategoryID = Dish_CategoryID.Value;
            }

            return View(model: model);
        }


        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Ingredient_Part_Table(int Restaurant_Menu_DishID, int? Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Таблица > Список");
            }

            return RestaurantMenu_Dish_Ingredient_Index(Restaurant_Menu_DishID, Dish_CategoryID);
        }


        #region Добавление


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Ingredient_Add(int Restaurant_Menu_DishID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление1");
            }

            Model_RestaurantMenu_Dish_Ingredients_CRUD model = new()
            {
                List_Ingridients = _bl.Ingridients.GetList(),
                Restaurant_Menu_DishID = Restaurant_Menu_DishID
            };

            return View(model: model);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RestaurantMenu_Dish_Ingredient_Add(Model_RestaurantMenu_Dish_Ingredients_CRUD data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Добавление2");
            }

            if (ModelState.IsValid && data != null)
            {
                if (data.Flag_IsGroup && data.IngridientGroupID < 0)
                {
                    ModelState.AddModelError("Flag_IsGroup", "Группа не указана");
                }

                int dish_ingridient_id = _bl.Restaurant_Menu_Dish_Ingredients.Add(Restaurant_Menu_DishID: data.Restaurant_Menu_DishID, IngridientID: data.IngridientID, Price: data.Price,
                                                                                  IngridientGroupID: data.IngridientGroupID, Cooking_Minutes: data.Cooking_Minutes);

                if (dish_ingridient_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index");
                }

                return RedirectToAction(controllerName: "RestaurantMenu_Dish_Ingredients", actionName: "RestaurantMenu_Dish_Ingredient_Index", routeValues: new { data.Restaurant_Menu_DishID });
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
        public IActionResult RestaurantMenu_Dish_Ingredient_Edit(int ID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение1");
            }

            CRestaurant_Menu_Dish_Ingredient? dish_ingridient = _bl.Restaurant_Menu_Dish_Ingredients.Get(ID: ID);
            if (dish_ingridient != null)
            {
                Model_RestaurantMenu_Dish_Ingredients_CRUD data = new()
                {
                    ID = dish_ingridient.ID,
                    IngridientID = dish_ingridient.IngridientID,
                    Ingridient = dish_ingridient.Ingridient,
                    Price = dish_ingridient.Price,
                    Restaurant_Menu_DishID = dish_ingridient.Restaurant_Menu_DishID,
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
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RestaurantMenu_Dish_Ingredient_Edit(Model_RestaurantMenu_Dish_Ingredients_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Изменение2");
            }

            if (Data != null)
            {
                if (Data.Flag_IsGroup && Data.IngridientGroupID < 0)
                {
                    ModelState.AddModelError("Flag_IsGroup", "Группа не указана");
                }

                int dish_ingridient_id = _bl.Restaurant_Menu_Dish_Ingredients.Edit(ID: Data.ID, Restaurant_Menu_DishID: Data.Restaurant_Menu_DishID, IngridientID: Data.IngridientID,
                                                                                   Price: Data.Price, IngridientGroupID: Data.IngridientGroupID, Cooking_Minutes: Data.Cooking_Minutes);

                if (dish_ingridient_id > 0)
                {
                    return RedirectToAction(controllerName: "RestaurantMenu_Dish_Ingredients", actionName: "RestaurantMenu_Dish_Ingredient_Index", routeValues: new { Data.Restaurant_Menu_DishID, Data.Dish_CategoryID });
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
        /// Удаление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantMenu_Dish_Ingredient_Del(int ID, int Dish_CategoryID)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление1");
            }

            if (ID > 0)
            {
                CRestaurant_Menu_Dish_Ingredient? elem = _bl.Restaurant_Menu_Dish_Ingredients.Get(ID: ID);

                if (elem != null)
                {
                    Model_RestaurantMenu_Dish_Ingredients_CRUD data = new()
                    {
                        Ingridient = elem.Ingridient,
                        Restaurant_Menu_DishID = elem.Restaurant_Menu_DishID,
                        Dish_CategoryID = Dish_CategoryID
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
        public IActionResult RestaurantMenu_Dish_Ingredient_Del(Model_RestaurantMenu_Dish_Ingredients_CRUD Data)
        {
            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: "Выполнение метода Удаление2");
            }

            if (Data != null)
            {
                int result = _bl.Restaurant_Menu_Dish_Ingredients.Del(ID: Data.ID, UserID: UserID);

                if (result > 0)
                {
                    return RedirectToAction(actionName: "RestaurantMenu_Dish_Ingredient_Index", routeValues: new { Data.Restaurant_Menu_DishID, Data.Dish_CategoryID });
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