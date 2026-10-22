using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Site_Menu.Models.Cart;
using HIDE.RestAndRant.Web_Site_Menu.Models.Offer;
using HIDE.Units_Core;
using static HIDE.RestAndRant.Web_Site_Menu.Helpers.Helper_Client;

namespace HIDE.RestAndRant.Web_Site_Menu.Controllers
{
    /// <summary>
    /// Оформление заказа
    /// </summary>
    /// <param name="_bl"></param>
    /// <param name="_log"></param>
    [Authorize]
    public class OfferController(BL_Context _bl, ILogger<OfferController> _log) : BaseControllerWithUser
    {
        /// <summary>
        /// Оформление заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            string? RestaurantID = "";
            if (Request.Cookies[Consts.Cookies_Restaurant] != null)
            {
                RestaurantID = Request.Cookies[Consts.Cookies_Restaurant];
            }

            Model_Offer_Index model = new();
            List<Model_CartItem>? list_result = [];
            List<Model_CartItem>? list_result_main = [];
            string? Cookie = Request.Cookies.TryGetValue(Consts.Cookies_Cart, out string? strVal) ? strVal : null;
            if (!string.IsNullOrWhiteSpace(Cookie))
            {
                list_result = JsonConvert.DeserializeObject<List<Model_CartItem>>(Cookie);
                if (list_result != null && list_result.Count != 0)
                {
                    foreach (Model_CartItem elem_cookies in list_result)
                    {
                        decimal? ingredient_price_sum = 0;
                        if (elem_cookies.Dish_Ingredient_IDs != null && elem_cookies.Dish_Ingredient_IDs.Count != 0)
                        {
                            foreach (int ingredient in elem_cookies.Dish_Ingredient_IDs)
                            {
                                decimal? ingredient_price = _bl.Dish_Ingridients.Get(ingredient)?.Price;
                                ingredient_price_sum += ingredient_price * elem_cookies.Dish_Count;
                            }
                        }

                        CDish_ForAdmin? dish = _bl.Dishes.Get_ForAdmin(elem_cookies.DishID, Include_VolumesList: true);
                        Model_CartItem? dish_result = new();

                        if (dish == null)
                        {
                            return View(model: model);
                        }

                        decimal dish_price = 0;
                        int? dish_cooking_time = 0;

                        if (dish.Cooking_Minutes.HasValue)
                        {
                            dish_cooking_time = dish.Cooking_Minutes.Value;
                        }
                        else
                        {
                            CDish_Category? dish_category = _bl.Dish_Categories.Get(dish.Dish_CategoryID);
                            if (dish_category != null && dish_category.Cooking_Minutes.HasValue)
                            {
                                dish_cooking_time = dish_category?.Cooking_Minutes.Value;
                            }
                        }

                        if (elem_cookies.Dish_VolumeID > 0)
                        {
                            dish_price = _bl.Dish_Volumes.Get(HIDE_Convert.ToInt(elem_cookies.Dish_VolumeID))?.VolumePrice ?? 0;
                        }
                        else
                        {
                            dish_price = dish!.Price;
                        }

                        if (dish != null)
                        {
                            dish_result.ID = elem_cookies.ID;
                            dish_result.DishID = dish.ID;
                            dish_result.Dish_Name = dish.Name;
                            dish_result.Dish_Price = dish.Price;
                            dish_result.Dish_Count = elem_cookies.Dish_Count;
                            dish_result.Dish_VolumeID = elem_cookies.Dish_VolumeID;
                            dish_result.Dish_Image = dish.Image;
                            dish_result.Sum = ingredient_price_sum + dish_price * elem_cookies.Dish_Count;
                            dish_result.List_Dish_Volumes = dish.List_Dish_Volumes;
                            dish_result.Dish_Ingredient_IDs = elem_cookies.Dish_Ingredient_IDs;
                            dish_result.Dish_Cooking_Minutes = dish_cooking_time;
                        }

                        list_result_main.Add(dish_result);
                    }
                }
            }

            model.Sum = list_result_main?.Select(x => x.Sum).Sum();
            model.Restaurant = _bl.Restaurants.Get(HIDE_Convert.ToInt(RestaurantID));
            model.Dish_Count = HIDE_Convert.ToInt(list_result_main?.Select(x => x.Dish_Count).Sum());
            model.Cooking_Ready = HIDE_Convert.ToInt(list_result_main?.Select(x => x.Dish_Cooking_Minutes).Sum()) > 30 ? 30 : HIDE_Convert.ToInt(list_result_main?.Select(x => x.Dish_Cooking_Minutes).Sum());
            model.List_PayTypes = _bl.Pay_Types.GetList();
            model.Client = _bl.Clients.Get(Client_ID);

            return View(model: model);
        }


        /// <summary>
        /// Оформить заказ
        /// </summary>
        /// <param name="Pay_TypeID">Способ оплаты</param>
        /// <param name="Handing">Требуется сдача с ...</param>
        /// <returns></returns>
        [HttpPost]
        public string Offer()
        {
            List<Model_CartItem>? list_result;
            List<COrder_Data>? list_result_main = [];
            string? Cookie = Request.Cookies.TryGetValue(Consts.Cookies_Cart, out string? strVal) ? strVal : null;
            if (!string.IsNullOrWhiteSpace(Cookie))
            {
                list_result = JsonConvert.DeserializeObject<List<Model_CartItem>>(Cookie);
                if (list_result != null && list_result.Count != 0)
                {
                    foreach (Model_CartItem elem_cookies in list_result)
                    {
                        decimal? ingredient_price = 0;
                        List<COrder_Data_Ingridient> list_ingredients = [];
                        if (elem_cookies.Dish_Ingredient_IDs != null && elem_cookies.Dish_Ingredient_IDs.Count != 0)
                        {
                            foreach (int elem in elem_cookies.Dish_Ingredient_IDs)
                            {
                                CDish_Ingridient? dish_ingredient = _bl.Dish_Ingridients.Get(elem);
                                if (dish_ingredient != null)
                                {
                                    COrder_Data_Ingridient ingr = new()
                                    {
                                        IngridientID = dish_ingredient.IngridientID,
                                        Ingridient_Count = elem_cookies.Dish_Count,
                                        Ingridient_Name = dish_ingredient.Ingridient ?? "",
                                        Ingridient_Price = dish_ingredient.Price,
                                        Ingridient_Summ = dish_ingredient.Price * elem_cookies.Dish_Count,
                                    };

                                    list_ingredients.Add(ingr);
                                    ingredient_price += dish_ingredient.Price * elem_cookies.Dish_Count;
                                }
                            }
                        }

                        CDish_ForAdmin? dish = _bl.Dishes.Get_ForAdmin(elem_cookies.DishID, Include_VolumesList: true);

                        decimal dish_price;

                        if (elem_cookies.Dish_VolumeID > 0)
                        {
                            dish_price = _bl.Dish_Volumes.Get(HIDE_Convert.ToInt(elem_cookies.Dish_VolumeID))?.VolumePrice ?? 0;
                        }
                        else
                        {
                            dish_price = dish!.Price;
                        }

                        COrder_Data cookie_model = new()
                        {
                            ID = elem_cookies.ID,
                            DishID = elem_cookies.DishID,
                            Bonuses_Minus = null,
                            Bonuses_Plus = null,
                            Dish_ColorID = null,
                            Dish_Name = dish.Name,
                            Dish_Price = dish_price,
                            Dish_Summ = dish_price * elem_cookies.Dish_Count,
                            Dish_Count = elem_cookies.Dish_Count,
                            Dish_VolumeID = elem_cookies.Dish_VolumeID,
                            Dish_VolumeValue = elem_cookies.Dish_VolumeID > 0 ? _bl.Dish_Volumes.Get(HIDE_Convert.ToInt(elem_cookies.Dish_VolumeID))?.VolumeValue : null,
                            Dish_VolumePrice = elem_cookies.Dish_VolumeID > 0 ? _bl.Dish_Volumes.Get(HIDE_Convert.ToInt(elem_cookies.Dish_VolumeID))?.VolumePrice : null,
                            List_Dish_Ingridients = list_ingredients,
                        };

                        list_result_main.Add(cookie_model);
                    }
                }
            }

            int order_id = 1;

            if (order_id < 0)
            {
                _log.LogError(message: "{}", args: $"Ошибка добавления заказа для клиента {Client_ID}");

                return "-";
            }

            Response.Cookies.Delete(Consts.Cookies_Cart);

            return "+";
        }

    }
}