using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Site_Menu.Helpers;
using HIDE.RestAndRant.Web_Site_Menu.Models.Cart;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Site_Menu.Controllers
{
    /// <summary>
    /// Корзина
    /// </summary>
    /// <param name="_bl"></param>
    /// <param name="_log"></param>
    public class CartController(BL_Context _bl, ILogger<CartController> _log) : Controller
    {
        /// <summary>
        /// Корзина - Главная
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            Model_Cart_Index model = new();
            List<Model_CartItem>? list_result = [];
            List<Model_CartItem>? list_result_main = [];
            string? Cookie = Request.Cookies[Consts.Cookies_Cart];

            // Получаем код ресторана
            string? cookieRestaurantID = Request.Cookies[Consts.Cookies_Restaurant];

            // Получаем выбранный столик
            string? restaurant_table = Request.Cookies[Consts.Cookies_RestaurantTable];
            if (!string.IsNullOrWhiteSpace(restaurant_table))
            {
                string? restaurant_tablename = _bl.Restaurant_Tables.Get(HIDE_Convert.ToInt(restaurant_table))?.Name;
                model.RestaurantTableID = HIDE_Convert.ToInt(restaurant_tablename);
            }

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
                                decimal? ingredient_price = _bl.Restaurant_Menu_Dish_Ingredients.Get(ingredient)?.Price;
                                ingredient_price_sum += ingredient_price * elem_cookies.Dish_Count;
                            }
                        }

                        CDish_ForAdmin? dish = _bl.Dishes.Get_ForAdmin_v2(elem_cookies.DishID, Convert.ToInt32(cookieRestaurantID), Include_Ingridients: true, Include_VolumesList: true);
                        Model_CartItem? dish_result = new();

                        decimal dish_price = 0;

                        if (elem_cookies.Dish_VolumeID > 0)
                        {
                            dish_price = _bl.Restaurant_Menu_Dish_Volumes.Get(HIDE_Convert.ToInt(elem_cookies.Dish_VolumeID))?.VolumePrice ?? 0;
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
                        }

                        list_result_main.Add(dish_result);
                    }
                }
            }

            model.List_Dishes = list_result_main;
            model.Sum_Itog = list_result_main?.Select(x => x.Sum).Sum();

            return View(model: model);
        }


        /// <summary>
        /// Корзина - Подробности
        /// </summary>
        /// <param name="CartItemID">Код позиции в корзине</param>
        /// <param name="DishID">Код блюда</param>
        /// <returns></returns>
        public IActionResult Cart_More(int CartItemID, int DishID)
        {
            Model_Cart_More model = new();
            Model_CartItem? dish_cart = new();
            List<int>? list_ingredients = [];
            string? Cookie = Request.Cookies.TryGetValue(Consts.Cookies_Cart, out string? strVal) ? strVal : null;

            // Получаем код ресторана
            string? cookieRestaurantID = Request.Cookies[Consts.Cookies_Restaurant];

            if (!string.IsNullOrWhiteSpace(Cookie))
            {
                dish_cart = JsonConvert.DeserializeObject<List<Model_CartItem>>(Cookie)?.Where(p => p.ID == CartItemID && p.DishID == DishID).FirstOrDefault();

                CDish_ForAdmin? dish = _bl.Dishes.Get_ForAdmin_v2(dish_cart!.DishID, Convert.ToInt32(cookieRestaurantID), Include_Ingridients: true, Include_VolumesList: true);
                Model_CartItem? dish_result = new();

                decimal? ingredient_price = 0;
                decimal dish_price = 0;

                if (dish_cart.Dish_VolumeID > 0)
                {
                    dish_price = _bl.Dish_Volumes.Get(HIDE_Convert.ToInt(dish_cart.Dish_VolumeID))?.VolumePrice ?? 0;
                }
                else
                {
                    dish_price = dish!.Price;
                }

                List<int>? list = dish_cart.Dish_Ingredient_IDs;
                if (list != null && list.Count != 0)
                {
                    foreach (int elem in list)
                    {
                        CDish_Ingridient? dish_ingredient = _bl.Dish_Ingridients.Get(elem);
                        if (dish_ingredient != null)
                        {
                            ingredient_price += dish_ingredient.Price * dish_cart.Dish_Count;
                        }
                    }
                }

                if (dish != null)
                {
                    dish_cart.Sum = ingredient_price + dish_price * dish_cart.Dish_Count;
                }
            }

            model.Dish = _bl.Dishes.Get_ForAdmin_v2(dish_cart!.DishID, Convert.ToInt32(cookieRestaurantID), Include_Ingridients: true, Include_VolumesList: true);
            model.Dish_Cart = dish_cart;
            model.List_Ingredients = list_ingredients;

            return View(model: model);
        }


        /// <summary>
        /// Добавить в корзину
        /// </summary>
        /// <param name="DishID">Код блюда</param>
        /// <param name="Dish_Count">Количество блюд</param>
        /// <param name="VolumeID">Выбранный объём</param>
        /// <param name="Dish_Ingredient_IDs">Список выбранных ингредиентов</param>
        /// <returns></returns>
        [HttpPost]
        public string Cart_Add(int DishID, decimal Dish_Count, int? VolumeID, string? Dish_Ingredient_IDs)
        {
            List<Model_CartItem_Add>? list_result = [];
            string? Cookies_Cart = Request.Cookies.TryGetValue(Consts.Cookies_Cart, out string? strVal) ? strVal : null;

            if (!string.IsNullOrWhiteSpace(Cookies_Cart))
            {
                list_result = JsonConvert.DeserializeObject<List<Model_CartItem_Add>>(Cookies_Cart);
            }

            int return_ID = 0;
            int ID_Max = 0;
            if (list_result != null && list_result.Count != 0) { ID_Max = (from p in list_result select p.ID).Max(); }

            // Получаем код ресторана
            string? cookieRestaurantID = Request.Cookies[Consts.Cookies_Restaurant];

            // Получение товара
            CDish_ForAdmin? elem_product = _bl.Dishes.Get_ForAdmin_v2(DishID, Convert.ToInt32(cookieRestaurantID), Include_Ingridients: false, Include_VolumesList: false);
            if (elem_product != null)
            {
                List<int>? list_ingredients = !string.IsNullOrWhiteSpace(Dish_Ingredient_IDs) ? Helper_ToArray.StringArr2_ToList(Dish_Ingredient_IDs) : null;

                Model_CartItem_Add? elem_plus = (from p in list_result where p.DishID == DishID && p.Dish_VolumeID == VolumeID && p.Dish_Ingredient_IDs == list_ingredients select p).FirstOrDefault();
                if (elem_plus != null)
                {
                    list_result![list_result.IndexOf(elem_plus)].Dish_Count += 1;
                }
                else
                {
                    Model_CartItem_Add cookie_model = new()
                    {
                        ID = ID_Max + 1,
                        DishID = DishID,
                        Dish_Count = Dish_Count,
                        Dish_VolumeID = VolumeID,
                        Dish_Ingredient_IDs = !string.IsNullOrWhiteSpace(Dish_Ingredient_IDs) ? Helper_ToArray.StringArr2_ToList(Dish_Ingredient_IDs) : null,
                    };

                    list_result!.Add(cookie_model);
                    return_ID = ID_Max + 1;
                }
            }

            string json = JsonConvert.SerializeObject(list_result);

            CookieOptions option = new()
            {
                Expires = DateTime.Now.AddHours(2),
            };
            Response.Cookies.Append(Consts.Cookies_Cart, json, option);

            return "+";
        }


        /// <summary>
        /// Изменить в корзине
        /// </summary>
        /// <param name="CartItemID">Код элемента корзины'</param>
        /// <param name="DishID">Код блюда</param>
        /// <param name="Dish_Count">Количество блюд</param>
        /// <param name="VolumeID">Выбранный объём</param>
        /// <param name="Dish_Ingredient_IDs">Список выбранных ингредиентов</param>
        /// <returns></returns>
        [HttpPost]
        public string Cart_Edit(int CartItemID, int DishID, decimal Dish_Count, int? VolumeID, string? Dish_Ingredient_IDs)
        {
            List<Model_CartItem_Add>? list_result = [];
            string? Cookies_Cart = Request.Cookies.TryGetValue(Consts.Cookies_Cart, out string? strVal) ? strVal : null;

            if (!string.IsNullOrWhiteSpace(Cookies_Cart))
            {
                list_result = JsonConvert.DeserializeObject<List<Model_CartItem_Add>>(Cookies_Cart);
            }

            if (list_result != null && list_result.Count != 0)
            {
                Model_CartItem_Add item_delete = list_result.Single(p => p.DishID == DishID && p.ID == CartItemID);
                list_result.Remove(item_delete);

                if (list_result != null && list_result.Count != 0)
                {
                    // Получение товара
                    CDish? elem_product = _bl.Dishes.Get(DishID);
                    if (elem_product != null)
                    {
                        Model_CartItem_Add cookie_model = new()
                        {
                            ID = CartItemID,
                            DishID = DishID,
                            Dish_Count = Dish_Count,
                            Dish_VolumeID = VolumeID,
                            Dish_Ingredient_IDs = !string.IsNullOrWhiteSpace(Dish_Ingredient_IDs) ? Helper_ToArray.StringArr2_ToList(Dish_Ingredient_IDs) : null,
                        };

                        list_result!.Add(cookie_model);
                    }

                    string json = JsonConvert.SerializeObject(list_result);

                    CookieOptions option = new()
                    {
                        Expires = DateTime.Now.AddHours(2),
                    };
                    Response.Cookies.Append(Consts.Cookies_Cart, json, option);
                }
                else
                {
                    int return_ID = 0;
                    int ID_Max = 0;
                    if (list_result != null && list_result.Count != 0) { ID_Max = (from p in list_result select p.ID).Max(); }

                    // Получение товара
                    CDish? elem_product = _bl.Dishes.Get(DishID);
                    if (elem_product != null)
                    {
                        Model_CartItem_Add cookie_model = new()
                        {
                            ID = ID_Max + 1,
                            DishID = DishID,
                            Dish_Count = Dish_Count,
                            Dish_VolumeID = VolumeID,
                            Dish_Ingredient_IDs = !string.IsNullOrWhiteSpace(Dish_Ingredient_IDs) ? Helper_ToArray.StringArr2_ToList(Dish_Ingredient_IDs) : null,
                        };

                        list_result!.Add(cookie_model);
                        return_ID = ID_Max + 1;
                    }

                    string json = JsonConvert.SerializeObject(list_result);

                    CookieOptions option = new()
                    {
                        Expires = DateTime.Now.AddHours(2),
                    };
                    Response.Cookies.Append(Consts.Cookies_Cart, json, option);
                }
            }

            return "+";
        }


        /// <summary>
        /// Увеличить количество
        /// </summary>
        /// <param name="CartItemID">Код элемента корзины'</param>
        /// <param name="DishID">Код блюда</param>
        /// <returns></returns>
        [HttpPost]
        public string Cart_Plus(int CartItemID)
        {
            List<Model_CartItem_Add>? list_result = [];
            string? Cookies_Cart = Request.Cookies.TryGetValue(Consts.Cookies_Cart, out string? strVal) ? strVal : null;

            if (!string.IsNullOrWhiteSpace(Cookies_Cart))
            {
                list_result = JsonConvert.DeserializeObject<List<Model_CartItem_Add>>(Cookies_Cart);
            }

            if (list_result != null && list_result.Count != 0)
            {
                Model_CartItem_Add? elem_plus = (from p in list_result where p.ID == CartItemID select p).FirstOrDefault();
                if (elem_plus == null || list_result == null)
                {
                    return "-";
                }
                list_result[list_result.IndexOf(elem_plus)].Dish_Count += 1;


                if (list_result[list_result.IndexOf(elem_plus)].Dish_Count <= 0)
                {
                    return "-";
                }

                string json = JsonConvert.SerializeObject(list_result);

                CookieOptions option = new()
                {
                    Expires = DateTime.Now.AddHours(2),
                };
                Response.Cookies.Append(Consts.Cookies_Cart, json, option);
            }

            return "+";
        }


        /// <summary>
        /// Уменьшить количество
        /// </summary>
        /// <param name="CartItemID">Код элемента корзины'</param>
        /// <param name="DishID">Код блюда</param>
        /// <returns></returns>
        [HttpPost]
        public string Cart_Minus(int CartItemID)
        {
            List<Model_CartItem_Add>? list_result = [];
            string? Cookies_Cart = Request.Cookies.TryGetValue(Consts.Cookies_Cart, out string? strVal) ? strVal : null;

            if (!string.IsNullOrWhiteSpace(Cookies_Cart))
            {
                list_result = JsonConvert.DeserializeObject<List<Model_CartItem_Add>>(Cookies_Cart);
            }

            if (list_result != null && list_result.Count != 0)
            {
                Model_CartItem_Add? elem_plus = (from p in list_result where p.ID == CartItemID select p).FirstOrDefault();
                if (elem_plus == null || list_result == null)
                {
                    return "-";
                }
                list_result[list_result.IndexOf(elem_plus)].Dish_Count -= 1;


                if (list_result[list_result.IndexOf(elem_plus)].Dish_Count <= 0)
                {
                    return "-";
                }

                string json = JsonConvert.SerializeObject(list_result);

                CookieOptions option = new()
                {
                    Expires = DateTime.Now.AddHours(2),
                };
                Response.Cookies.Append(Consts.Cookies_Cart, json, option);
            }

            return "+";
        }


        /// <summary>
        /// Удалить с корзины
        /// </summary>
        /// <param name="DishID">Код блюда</param>
        /// <returns></returns>
        [HttpPost]
        public string Cart_Delete(int CartItemID, int DishID)
        {
            List<Model_CartItem_Add>? list_result = [];
            string? Cookies_Cart = Request.Cookies.TryGetValue(Consts.Cookies_Cart, out string? strVal) ? strVal : null;

            if (!string.IsNullOrWhiteSpace(Cookies_Cart))
            {
                list_result = JsonConvert.DeserializeObject<List<Model_CartItem_Add>>(Cookies_Cart);
            }

            if (list_result != null && list_result.Count != 0)
            {
                Model_CartItem_Add item_delete = list_result.Single(p => p.DishID == DishID && p.ID == CartItemID);
                list_result.Remove(item_delete);
            }

            string json = JsonConvert.SerializeObject(list_result);

            CookieOptions option = new()
            {
                Expires = DateTime.Now.AddHours(2),
            };
            Response.Cookies.Append(Consts.Cookies_Cart, json, option);

            return "+";
        }


        /// <summary>
        /// Оформить заказ
        /// </summary>
        /// <param name="NumberQuests">Количество гостей за столиком</param>
        /// <param name="AddToOrder">Добавить к заказу</param>
        /// <param name="RemoveToOrder">Убрать с заказа</param>
        /// <returns></returns>
        [HttpPost]
        public int Offer(int NumberQuests, string? AddToOrder, string? RemoveToOrder)
        {
            string? restaurant_TableID = Request.Cookies[Consts.Cookies_RestaurantTable];
            if (string.IsNullOrWhiteSpace(restaurant_TableID))
            {
                return Errors.Err200;
            }

            List<Model_CartItem>? list_result;
            List<COrder_Data>? list_result_main = [];
            int? dish_cooking_time = 0;
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
                                CRestaurant_Menu_Dish_Ingredient? dish_ingredient = _bl.Restaurant_Menu_Dish_Ingredients.Get(elem);
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

                        // Получаем код ресторана
                        string? cookieRestaurantID = Request.Cookies[Consts.Cookies_Restaurant];

                        CDish_ForAdmin? dish = _bl.Dishes.Get_ForAdmin_v2(elem_cookies.DishID, Convert.ToInt32(cookieRestaurantID), Include_Ingridients: true, Include_VolumesList: true);
                        Model_CartItem? dish_result = new();

                        decimal dish_price = 0;

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
                            dish_price = _bl.Restaurant_Menu_Dish_Volumes.Get(HIDE_Convert.ToInt(elem_cookies.Dish_VolumeID))?.VolumePrice ?? 0;
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

            DateTime date_to = DateTime.Now.AddMinutes(HIDE_Convert.ToInt(dish_cooking_time));

            DateTime date_booking_start = DateTime.Now;
            DateTime date_new = DateTime.Now.Date;
            if (date_to.Minute < 30)
            {
                date_booking_start = date_to.AddMinutes(30 - date_to.Minute).AddSeconds(-date_booking_start.Second);
            }
            else if (date_to.Minute > 30 && date_to.Minute < 60)
            {
                date_booking_start = date_to.AddMinutes(60 - date_to.Minute).AddSeconds(-date_booking_start.Second);
            }
            int? restaurantid = _bl.Restaurant_Tables.Get(HIDE_Convert.ToInt(restaurant_TableID))?.RestaurantID;

            date_new = date_new.AddHours(date_booking_start.Hour).AddMinutes(date_booking_start.Minute);

            CRestaurant? restaurant = _bl.Restaurants.Get(HIDE_Convert.ToInt(restaurantid));

            if (restaurant == null)
            {
                return Errors.Err200;
            }

            int order_id = _bl.Orders.AddSiteMenu(ClientID: restaurant.ClientID ?? 149, Name: "", Phone: "", Pay_TypeID: Common.ENums.EPayType.Money,
                                                  RestaurantID: HIDE_Convert.ToInt(restaurantid), Comment_AddToOrder: AddToOrder, Comment_RemoveFromOrder: RemoveToOrder,
                                                  Date_To: date_to, Order_TypeID: Common.ENums.EOrder_Type.Incafe,
                                                  Restaurant_TableID: HIDE_Convert.ToInt_Nullable(restaurant_TableID),
                                                  Date_BookingStart: date_new, Date_BookingEnd: date_new.AddMinutes(30), Handing: null, NumberQuests: NumberQuests,
                                                  List_OrderData: list_result_main);

            if (order_id < 0)
            {
                _log.LogError(message: "{}", args: $"Ошибка добавления заказа для клиента {restaurant.ClientID ?? 149}");

                return Errors.Err202;
            }

            Response.Cookies.Delete(Consts.Cookies_Cart);
            Response.Cookies.Delete("Order");

            CookieOptions CookieOptions = new()
            {
                Expires = DateTime.Now.AddMinutes((date_booking_start.AddMinutes(30) - DateTime.Now).TotalMinutes)
            };
            Response.Cookies.Append("Order", date_booking_start.AddMinutes(30).ToString(), options: CookieOptions);

            return order_id;
        }



        /// <summary>
        /// Удалить все товары с корзины
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public void CartClear()
        {
            Response.Cookies.Delete(Consts.Cookies_Cart);
        }

    }
}