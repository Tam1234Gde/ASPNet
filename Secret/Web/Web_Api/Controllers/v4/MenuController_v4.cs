using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.RestAndRant.Web_Api.Models.v4._Enum;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Api.Controllers.v4
{
    /// <summary>
    /// Контроллер - Новое меню (Menu)
    /// </summary>
    [Tags("Menu_v4 - Меню (новая система)")]
    [ApiVersion("4.0")]
    [Route("v{version:apiVersion}/Menu/[action]")]
    [ApiController]
    public class MenuController_v4(ILogger<MenuController_v4> _log, BL_Context _bl) : ControllerBase
    {
        #region Меню

        /// <summary>
        /// Возвращает дату последнего изменения меню
        /// </summary>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int))]
        public JsonResult GetMenuChangeDate()
        {
            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Menu_DateChange);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: val);
        }

        /// <summary>
        /// Получает список всех меню
        /// </summary>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CMenu>))]
        public JsonResult GetMenusList()
        {
            List<CMenu> list_result = _bl.Menu.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }

        /// <summary>
        /// Получает меню по ID
        /// </summary>
        [HttpGet("{MenuID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CMenu))]
        public JsonResult GetMenuById([Required] int MenuID)
        {
            CMenu? menu = _bl.Menu.Get(MenuID);
            if (menu == null)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: null);
            }
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: menu);
        }

        #endregion

        #region Категории

        /// <summary>
        /// Получает список категорий по ID меню
        /// </summary>
        [HttpGet("{MenuID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CMenu_Category>))]
        public JsonResult GetCategoriesListByMenuId([Required] int MenuID)
        {
            List<CMenu_Category> list_result = _bl.Menu_Category.GetList_ByMenuID(MenuID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }

        /// <summary>
        /// Получает категорию по ID
        /// </summary>
        [HttpGet("{CategoryID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CMenu_Category))]
        public JsonResult GetCategoryById([Required] int CategoryID)
        {
            CMenu_Category? category = _bl.Menu_Category.Get(CategoryID);
            if (category == null)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: null);
            }
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: category);
        }

        #endregion

        #region Подкатегории

        /// <summary>
        /// Получает список подкатегорий по ID категории
        /// </summary>
        [HttpGet("{CategoryID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CMenu_Subcategory>))]
        public JsonResult GetSubcategoriesListByCategoryId([Required] int CategoryID)
        {
            List<CMenu_Subcategory> list_result = _bl.Menu_Subcategory.GetList_ByMenu_CategoryID(CategoryID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }

        /// <summary>
        /// Получает подкатегорию по ID
        /// </summary>
        [HttpGet("{SubcategoryID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CMenu_Subcategory))]
        public JsonResult GetSubcategoryById([Required] int SubcategoryID)
        {
            CMenu_Subcategory? subcategory = _bl.Menu_Subcategory.Get(SubcategoryID);
            if (subcategory == null)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: null);
            }
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: subcategory);
        }

        #endregion

        #region Блюда

        /// <summary>
        /// Получает список блюд по ID подкатегории (БЕЗ ИЗОБРАЖЕНИЙ)
        /// </summary>
        [HttpGet("{SubcategoryID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CMenu_Dish>))]
        public JsonResult GetDishesListBySubcategoryId([Required] int SubcategoryID)
        {
            List<CMenu_Dish> list_result = _bl.Menu_Dish.GetList_ByMenu_SubcategoryID(SubcategoryID);

            // Очищаем изображения, чтобы не передавать их в основном запросе
            foreach (var dish in list_result)
            {
                dish.Image = null;

                // Загружаем ингредиенты
                List<CMenu_Dish_Ingridient> dishIngredients = _bl.Menu_Dish_Ingredient.GetList_ByMenu_DishID(dish.ID);
                dish.List_Dish_Ingridients = dishIngredients;
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }

        /// <summary>
        /// Получает полную структуру меню по ID ресторана (БЕЗ ИЗОБРАЖЕНИЙ)
        /// </summary>
        [HttpGet("{RestaurantID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CMenu_Dish>))]
        public JsonResult GetMenuByRestaurantId([Required] int RestaurantID)
        {
            // Получаем все привязки меню к ресторану
            List<CMenu_Restaurant> menuRestaurants = _bl.Menu_Restaurant.GetList_ByRestaurantID(RestaurantID);

            if (menuRestaurants == null || menuRestaurants.Count == 0)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: new List<CMenu_Dish>());
            }

            List<CMenu_Dish> result = new List<CMenu_Dish>();

            foreach (var menuRestaurant in menuRestaurants)
            {
                // Получаем все категории меню
                List<CMenu_Category> categories = _bl.Menu_Category.GetList_ByMenuID(menuRestaurant.MenuID);

                foreach (var category in categories)
                {
                    // Получаем подкатегории
                    List<CMenu_Subcategory> subcategories = _bl.Menu_Subcategory.GetList_ByMenu_CategoryID(category.ID);

                    foreach (var subcategory in subcategories)
                    {
                        // Получаем блюда
                        List<CMenu_Dish> dishes = _bl.Menu_Dish.GetList_ByMenu_SubcategoryID(subcategory.ID);

                        foreach (var dish in dishes)
                        {
                            // Очищаем изображение, чтобы не передавать в основном запросе
                            dish.Image = null;

                            // Загружаем ингредиенты
                            List<CMenu_Dish_Ingridient> dishIngredients = _bl.Menu_Dish_Ingredient.GetList_ByMenu_DishID(dish.ID);
                            dish.List_Dish_Ingridients = dishIngredients;
                            result.Add(dish);
                        }
                    }
                }
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: result);
        }

        /// <summary>
        /// Получает блюдо по ID (БЕЗ ИЗОБРАЖЕНИЯ)
        /// </summary>
        [HttpGet("{DishID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CMenu_Dish))]
        public JsonResult GetDishById([Required] int DishID)
        {
            CMenu_Dish? dish = _bl.Menu_Dish.Get(DishID);
            if (dish == null)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: null);
            }

            // Очищаем изображение
            dish.Image = null;

            // Загружаем ингредиенты
            List<CMenu_Dish_Ingridient> dishIngredients = _bl.Menu_Dish_Ingredient.GetList_ByMenu_DishID(DishID);
            dish.List_Dish_Ingridients = dishIngredients;

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: dish);
        }

        /// <summary>
        /// Получает изображение блюда (ОТДЕЛЬНЫЙ ЗАПРОС)
        /// </summary>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        public IActionResult GetDishImage([Required] int DishID)
        {
            if (DishID <= 0)
            {
                _log.LogWarning("Не удалось идентифицировать блюдо. DishID: {DishID}", DishID);
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: null);
            }

            CMenu_Dish? dish = _bl.Menu_Dish.Get(DishID);
            if (dish == null || dish.Image == null || dish.Image.Length == 0)
            {
                return NotFound();
            }

            // Возвращаем изображение как файл
            return File(dish.Image, "image/jpeg");
        }

        #endregion

        #region Ингредиенты

        /// <summary>
        /// Получает список групп ингредиентов
        /// </summary>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CMenu_Ingridient_Group>))]
        public JsonResult GetIngredientGroupsList()
        {
            List<CMenu_Ingridient_Group> list_result = _bl.Menu_Ingredient_Group.GetList();
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }

        /// <summary>
        /// Получает список ингредиентов по ID группы (БЕЗ ИЗОБРАЖЕНИЙ)
        /// </summary>
        [HttpGet("{GroupID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CMenu_Ingridient>))]
        public JsonResult GetIngredientsListByGroupId([Required] int GroupID)
        {
            List<CMenu_Ingridient> list_result = _bl.Menu_Ingredient.GetList_ByMenu_Ingridient_GroupID(GroupID);

            // Очищаем изображения
            foreach (var ingredient in list_result)
            {
                ingredient.Image = null;
            }

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }

        /// <summary>
        /// Получает ингредиент по ID (БЕЗ ИЗОБРАЖЕНИЯ)
        /// </summary>
        [HttpGet("{IngredientID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CMenu_Ingridient))]
        public JsonResult GetIngredientById([Required] int IngredientID)
        {
            CMenu_Ingridient? ingredient = _bl.Menu_Ingredient.Get(IngredientID);
            if (ingredient == null)
            {
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: null);
            }

            // Очищаем изображение
            ingredient.Image = null;

            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: ingredient);
        }

        /// <summary>
        /// Получает изображение ингредиента (ОТДЕЛЬНЫЙ ЗАПРОС)
        /// </summary>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(byte[]))]
        public IActionResult GetIngredientImage([Required] int IngredientID)
        {
            if (IngredientID <= 0)
            {
                _log.LogWarning("Не удалось идентифицировать ингредиент. IngredientID: {IngredientID}", IngredientID);
                return Helper_Result.GetJsonResult(Status: Consts.Status_Err, Code: (int)EResult_v4.Err, Data: null);
            }

            CMenu_Ingridient? ingredient = _bl.Menu_Ingredient.Get(IngredientID);
            if (ingredient == null || ingredient.Image == null || ingredient.Image.Length == 0)
            {
                return NotFound();
            }

            // Возвращаем изображение как файл
            return File(ingredient.Image, "image/jpeg");
        }

        #endregion

        #region Привязка к ресторанам

        /// <summary>
        /// Получает список привязок меню к ресторанам
        /// </summary>
        [HttpGet("{MenuID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CMenu_Restaurant>))]
        public JsonResult GetMenuRestaurantsListByMenuId([Required] int MenuID)
        {
            List<CMenu_Restaurant> list_result = _bl.Menu_Restaurant.GetList_ByMenuID(MenuID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }

        /// <summary>
        /// Получает список меню по ID ресторана
        /// </summary>
        [HttpGet("{RestaurantID}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CMenu_Restaurant>))]
        public JsonResult GetMenusListByRestaurantId([Required] int RestaurantID)
        {
            List<CMenu_Restaurant> list_result = _bl.Menu_Restaurant.GetList_ByRestaurantID(RestaurantID);
            return Helper_Result.GetJsonResult(Status: Consts.Status_OK, Code: (int)EResult_v4.OK, Data: list_result);
        }

        #endregion
    }
}