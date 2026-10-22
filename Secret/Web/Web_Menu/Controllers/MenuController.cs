using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Site_Menu.Models.Cart;
using HIDE.RestAndRant.Web_Site_Menu.Models.Home;
using HIDE.RestAndRant.Web_Site_Menu.Models.Menu;
using static HIDE.RestAndRant.Web_Site_Menu.Helpers.Helper_Client;

namespace HIDE.RestAndRant.Web_Site_Menu.Controllers
{
    /// <summary>
    /// Меню
    /// </summary>
    public class MenuController : BaseControllerWithUser
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;
        private readonly IWebHostEnvironment _env;
        readonly Images Images_BL;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public MenuController(ILogger<MenuController> Logger, BL_Context BL, IWebHostEnvironment Env)
        {
            _log = Logger;
            _log.LogDebug("Constructor DishesController");
            _bl = BL;
            _env = Env;
            Images_BL = new Images();
        }

        /// <summary>
        /// Меню
        /// </summary>
        /// <returns></returns>
        public IActionResult Menu_Index()
        {
            // Получаем код ресторана
            string? cookieRestaurantID = Request.Cookies[Consts.Cookies_Restaurant];

            Model_Menu model = new();

            // Получаем список блюд по коду ресторана
            List<CSiteMenu> list_dishes = _bl.Dishes.GetListForSite_v2(RestaurantID: Convert.ToInt32(cookieRestaurantID), out List<CDish_Category> _,
                                                                            Include_VolumesList: false,
                                                                            Include_Ingridients: false);

            string? Cookie = Request.Cookies.TryGetValue(Consts.Cookies_Cart, out string? strVal) ? strVal : null;
            if (!string.IsNullOrWhiteSpace(Cookie))
            {
                List<Model_CartItem>? list_result = JsonConvert.DeserializeObject<List<Model_CartItem>>(Cookie);
                model.List_Dishes_Cart = list_result;
            }

            if (list_dishes != null && list_dishes.Count != 0)
            {
                foreach (CSiteMenu category in list_dishes)
                {
                    if (category.ListDishes != null && category.ListDishes.Count != 0)
                    {
                        foreach (CDish_ForAdmin dish in category.ListDishes)
                        {
                            string? image_url = Images_BL.Get_Dish(DishID: dish.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
                            if (!string.IsNullOrWhiteSpace(image_url))
                            {
                                dish.Image_Url = image_url;
                            }
                        }
                    }
                }
            }

            model.List_Dishes = list_dishes;

            return View(model: model);
        }


        /// <summary>
        /// Блюдо
        /// </summary>
        /// <param name="Dish_ID">Код блюда</param>
        /// <returns></returns>
        public IActionResult Dish_More(int Dish_ID)
        {
            // Блюда в корзине
            string? Cookie = Request.Cookies[Consts.Cookies_Cart];
            List<Model_CartItem>? list_result = [];

            // Получаем код ресторана
            string? cookieRestaurantID = Request.Cookies[Consts.Cookies_Restaurant];

            if (!string.IsNullOrWhiteSpace(Cookie))
            {
                list_result = JsonConvert.DeserializeObject<List<Model_CartItem>>(Cookie);
            }

            Model_Home_Dish model = new()
            {
                Dish = _bl.Dishes.Get_ForAdmin_v2(Dish_ID, Convert.ToInt32(cookieRestaurantID), Include_Ingridients: true, Include_VolumesList: true),
                List_Dishes = list_result,
            };

            // Рекомендованные товары
            if (model.Dish != null && model.Dish.List_RecommendDishIDs != null && model.Dish.List_RecommendDishIDs.Count != 0)
            {
                model.List_DishRecommended = _bl.Dishes.GetListByIDs(model.Dish.List_RecommendDishIDs);
            }

            // Изображение блюда
            if (model.Dish != null)
            {
                string? image_url = _bl.Images.Get_Dish(DishID: model.Dish.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
                if (!string.IsNullOrWhiteSpace(image_url))
                {
                    model.Dish.Image_Url = image_url;
                }
            }

            return View(model: model);
        }


        /// <summary>
        /// Частичное представление > Рекомендуемые товары
        /// </summary>
        /// <param name="Dish_ID">Код блюда</param>
        /// <returns></returns>
        public IActionResult PartDish_Recommend(int Dish_ID)
        {
            string? Cookie = Request.Cookies[Consts.Cookies_Cart];
            List<Model_CartItem>? list_result = [];

            if (!string.IsNullOrWhiteSpace(Cookie))
            {
                list_result = JsonConvert.DeserializeObject<List<Model_CartItem>>(Cookie);
            }

            Model_Home_Dish model = new()
            {
                Dish = _bl.Dishes.Get_ForAdmin(Dish_ID, Include_Ingridients: true, Include_VolumesList: true),
                List_Dishes = list_result,
            };

            if (model.Dish != null && model.Dish.List_RecommendDishIDs != null && model.Dish.List_RecommendDishIDs.Count != 0)
            {
                model.List_DishRecommended = _bl.Dishes.GetListByIDs(model.Dish.List_RecommendDishIDs);
            }

            return View(model: model);
        }


        /// <summary>
        /// Получить изображение товара по DishID
        /// </summary>
        /// <param name="DishID">Код продукта</param>
        /// <returns></returns>
        public IActionResult GetImageDish(int DishID)
        {
            CDish_ForAdmin? dish = _bl.Dishes.Get_ForAdmin(DishID);

            if (dish != null && dish.Image != null)
            {
                return File(fileContents: dish.Image, contentType: "image/png", fileDownloadName: dish.Name);
            }
            else
            {
                return NotFound();
            }
        }

    }
}