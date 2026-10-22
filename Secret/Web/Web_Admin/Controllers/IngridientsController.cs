using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Web_Admin.Models.Ingridients;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    public class IngridientsController : Controller
    {
        private readonly ILogger<IngridientsController> _logger;
        private readonly Ingridients _bl_Ingridients;
        private readonly Dish_IngridientCategory _categoryService;
        private readonly Restaurants _bl_Restaurants;

        public IngridientsController(ILogger<IngridientsController> logger)
        {
            _logger = logger;
            _bl_Ingridients = new Ingridients();
            _categoryService = new Dish_IngridientCategory();
            _bl_Restaurants = new Restaurants();
        }

        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех ингредиентов
        /// GET: /Ingridients
        /// </summary>
        [HttpGet]
        public IActionResult GetIngredients()
        {
            try
            {
                List<CIngridient_ForAdmin> ingredients = _bl_Ingridients.GetAll_ForAdmin_WithCategories();
                List<object> result = new List<object>();

                foreach (CIngridient_ForAdmin item in ingredients)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Category_ID,
                        CategoryName = item.Category_Name ?? string.Empty,
                        Category_IsDeleted = item.Category_IsDeleted,
                        item.Image_Url
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
        /// Получить ингредиент по ID
        /// GET: /Ingridients/GetIngredient?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetIngredient(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CIngridient_ForAdmin ingredient = _bl_Ingridients.Get_ForAdmin(ID);
                if (ingredient == null)
                    return Json(new { success = false, message = "Ингредиент не найден" });

                object result = new
                {
                    ingredient.ID,
                    ingredient.Name,
                    ingredient.Category_ID,
                    CategoryName = ingredient.Category_Name ?? string.Empty,
                    Category_IsDeleted = ingredient.Category_IsDeleted,
                    ingredient.Image_Url
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список ингредиентов по категории
        /// GET: /Ingridients/GetIngredientsByCategory?Category_ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetIngredientsByCategory(int Category_ID)
        {
            try
            {
                if (Category_ID <= 0)
                    return Json(new { success = false, message = "Неверный ID категории" });

                List<CIngridient_ForAdmin> allIngredients = _bl_Ingridients.GetAll_ForAdmin_WithCategories();
                List<CIngridient_ForAdmin> filteredIngredients = allIngredients.Where(x => x.Category_ID == Category_ID).ToList();
                List<object> result = new List<object>();

                foreach (CIngridient_ForAdmin item in filteredIngredients)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Category_ID,
                        CategoryName = item.Category_Name ?? string.Empty,
                        Category_IsDeleted = item.Category_IsDeleted,
                        item.Image_Url
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
        /// Получить список активных ингредиентов (не удаленных)
        /// GET: /Ingridients/GetActiveIngredients
        /// </summary>
        [HttpGet]
        public IActionResult GetActiveIngredients()
        {
            try
            {
                List<CIngridient_ForAdmin> allIngredients = _bl_Ingridients.GetAll_ForAdmin_WithCategories();
                List<CIngridient_ForAdmin> activeIngredients = allIngredients.Where(x => !x.Category_IsDeleted).ToList();
                List<object> result = new List<object>();

                foreach (CIngridient_ForAdmin item in activeIngredients)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Category_ID,
                        CategoryName = item.Category_Name ?? string.Empty,
                        item.Image_Url
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

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.Flag_ShowMenu = true;
            ViewBag.RestaurantID = null;
        }

        #region Список

        /// <summary>
        /// Список ингредиентов
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ViewBag.Flag_ShowMenu = true;
            ViewBag.RestaurantID = null;

            List<CIngridient_ForAdmin> ingredients = _bl_Ingridients.GetAll_ForAdmin_WithCategories();
            return View(ingredients);
        }

        #endregion

        #region Добавление

        /// <summary>
        /// Добавление ингредиента - GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            Model_Ingridients_AddEdit model = new Model_Ingridients_AddEdit
            {
                Categories = GetCategoriesSelectList()
            };
            return View(model);
        }

        /// <summary>
        /// Добавление ингредиента - POST
        /// </summary>
        /// <param name="data">Данные ингредиента</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Ingridients_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                int ingridient_id = _bl_Ingridients.Add(
                    Name: data.Name,
                    Image: null,
                    Category_ID: data.Category_ID
                );

                if (ingridient_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index");
                }

                if (data.UploadImge != null)
                {
                    // Здесь ваша логика сохранения изображения
                }

                TempData["Success"] = "Ингредиент успешно добавлен.";
                return RedirectToAction(nameof(Index));
            }

            data.Categories = GetCategoriesSelectList();
            return View(data);
        }

        #endregion

        #region Редактирование

        /// <summary>
        /// Редактирование ингредиента - GET
        /// </summary>
        /// <param name="ID">ID ингредиента</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CIngridient_ForAdmin ingredient = _bl_Ingridients.Get_ForAdmin(ID);
            if (ingredient == null)
                return RedirectToAction("Index", "Error");

            Model_Ingridients_AddEdit model = new Model_Ingridients_AddEdit
            {
                ID = ingredient.ID,
                Name = ingredient.Name,
                Image = ingredient.Image,
                Image_Url = ingredient.Image_Url,
                Category_ID = ingredient.Category_ID,
                Categories = GetCategoriesSelectList()
            };

            return View(model);
        }

        /// <summary>
        /// Редактирование ингредиента - POST
        /// </summary>
        /// <param name="data">Данные ингредиента</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Ingridients_AddEdit data)
        {
            if (data != null && ModelState.IsValid)
            {
                byte[]? img_Data = null;
                if (data.UploadImge != null)
                {
                    using (BinaryReader binaryReader = new BinaryReader(data.UploadImge.OpenReadStream()))
                    {
                        img_Data = binaryReader.ReadBytes((int)data.UploadImge.Length);
                    }
                }

                int result = _bl_Ingridients.Edit(
                    ID: data.ID,
                    Name_New: data.Name,
                    Image_New: img_Data,
                    CategoryIDNew: data.Category_ID
                );

                if (result > 0)
                {
                    if (data.UploadImge != null)
                    {
                        // Здесь ваша логика обновления изображения
                    }

                    TempData["Success"] = "Ингредиент успешно обновлен.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Ошибка при обновлении.");
            }

            data.Categories = GetCategoriesSelectList();
            return View(data);
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удаление ингредиента - GET (страница подтверждения)
        /// </summary>
        /// <param name="ID">ID ингредиента</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            CIngridient_ForAdmin ingredient = _bl_Ingridients.Get_ForAdmin(ID);
            if (ingredient == null)
                return RedirectToAction("Index", "Error");

            Model_Ingridients_AddEdit model = new Model_Ingridients_AddEdit
            {
                ID = ingredient.ID,
                Name = ingredient.Name
            };

            return View(model);
        }

        /// <summary>
        /// Удаление ингредиента - POST
        /// </summary>
        /// <param name="data">Данные ингредиента</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Ingridients_AddEdit data)
        {
            if (data == null || data.ID <= 0)
                return RedirectToAction("Index", "Error");

            int result = _bl_Ingridients.Del(data.ID, GetCurrentUserId());
            if (result > 0)
            {
                TempData["Success"] = "Ингредиент удален.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Ошибка при удалении.");
            return View(data);
        }

        #endregion

        #region Редактирование стоимости

        /// <summary>
        /// Редактирование стоимости ингредиента - GET
        /// </summary>
        /// <param name="ID">ID ингредиента</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditPrice(int ID)
        {
            if (ID <= 0)
                return RedirectToAction("Index", "Error");

            CIngridient_ForAdmin ingredient = _bl_Ingridients.Get_ForAdmin(ID);
            if (ingredient == null)
                return RedirectToAction("Index", "Error");

            List<CRestaurant> restaurants = GetRestaurantsList();
            int defaultRestaurantId = 0;
            decimal currentPrice = 0;

            if (restaurants != null && restaurants.Count > 0)
            {
                defaultRestaurantId = restaurants[0].ID;
                currentPrice = GetIngredientPrice(ID, defaultRestaurantId);
            }

            Model_Ingredients_PriceChange model = new Model_Ingredients_PriceChange
            {
                ID = ingredient.ID,
                Name = ingredient.Name,
                RestaurantID = defaultRestaurantId,
                Price = currentPrice,
                ListRestaurants = restaurants
            };

            return View(model);
        }

        /// <summary>
        /// Редактирование стоимости ингредиента - POST
        /// </summary>
        /// <param name="data">Данные стоимости</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditPrice(Model_Ingredients_PriceChange data)
        {
            if (data == null || data.ID <= 0)
            {
                ModelState.AddModelError("", "Неверные данные");
                data.ListRestaurants = GetRestaurantsList();
                return View(data);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int result = UpdateIngredientPrice(
                        ingredientId: data.ID,
                        restaurantId: data.RestaurantID,
                        price: data.Price
                    );

                    if (result > 0)
                    {
                        TempData["Success"] = "Стоимость ингредиента успешно обновлена.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ошибка при обновлении стоимости.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при обновлении стоимости ингредиента ID: {IngredientId}", data.ID);
                    ModelState.AddModelError("", string.Format("Ошибка: {0}", ex.Message));
                }
            }

            data.ListRestaurants = GetRestaurantsList();
            return View(data);
        }

        #endregion

        #region Вспомогательные методы

        /// <summary>
        /// Получить список категорий для выпадающего списка
        /// </summary>
        /// <returns>Список категорий</returns>
        private List<SelectListItem> GetCategoriesSelectList()
        {
            List<CDish_Ingridient_Category?> categories = _categoryService.GetAllCategories();
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (CDish_Ingridient_Category? cat in categories)
            {
                if (cat != null && cat.Flag_Del != true)
                {
                    SelectListItem item = new SelectListItem
                    {
                        Value = cat.ID.ToString(),
                        Text = cat.Name
                    };
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// Получить ID текущего пользователя
        /// </summary>
        /// <returns>ID пользователя</returns>
        private int GetCurrentUserId()
        {
            return 1; // Заглушка
        }

        /// <summary>
        /// Получить список ресторанов
        /// </summary>
        /// <returns>Список ресторанов</returns>
        private List<CRestaurant> GetRestaurantsList()
        {
            // TODO: Заменить на реальный сервис получения ресторанов
            // return _bl_Restaurants.GetAll();

            // Временная заглушка
            List<CRestaurant> restaurants = new List<CRestaurant>();

            CRestaurant restaurant1 = new CRestaurant();
            restaurant1.ID = 1;
            restaurant1.Address = "Ресторан 1, ул. Примерная, д. 1";
            restaurants.Add(restaurant1);

            CRestaurant restaurant2 = new CRestaurant();
            restaurant2.ID = 2;
            restaurant2.Address = "Ресторан 2, ул. Тестовая, д. 2";
            restaurants.Add(restaurant2);

            CRestaurant restaurant3 = new CRestaurant();
            restaurant3.ID = 3;
            restaurant3.Address = "Ресторан 3, пр. Главный, д. 3";
            restaurants.Add(restaurant3);

            return restaurants;
        }

        /// <summary>
        /// Получить цену ингредиента для ресторана
        /// </summary>
        /// <param name="ingredientId">ID ингредиента</param>
        /// <param name="restaurantId">ID ресторана</param>
        /// <returns>Цена</returns>
        private decimal GetIngredientPrice(int ingredientId, int restaurantId)
        {
            // TODO: Заменить на реальную логику получения цены
            Random random = new Random();
            return Math.Round((decimal)(random.NextDouble() * 100 + 10), 2);
        }

        /// <summary>
        /// Обновить цену ингредиента для ресторана
        /// </summary>
        /// <param name="ingredientId">ID ингредиента</param>
        /// <param name="restaurantId">ID ресторана</param>
        /// <param name="price">Новая цена</param>
        /// <returns>Количество обновленных записей</returns>
        private int UpdateIngredientPrice(int ingredientId, int restaurantId, decimal price)
        {
            // TODO: Заменить на реальную логику обновления цены
            _logger.LogInformation("Обновление цены: Ингредиент ID={IngredientId}, Ресторан ID={RestaurantId}, Цена={Price}",
                ingredientId, restaurantId, price);

            return 1; // Успешно
        }

        #endregion
    }
}