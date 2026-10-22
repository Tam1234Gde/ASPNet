using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Web_Oper.Helpers;
using HIDE.RestAndRant.Web_Oper.Models.Orders;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Oper.Controllers
{
    /// <summary>
    /// Заказы
    /// </summary>
    [Authorize]
    public class OrdersController : Controller
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
                    user_id = Helper_Client.GetCurrentClientID(HttpContext.User);
                }
                return user_id;
            }
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public OrdersController(ILogger<OrdersController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor OrdersController");
            _bl = BL;
        }

        /// <summary>
        /// Модальное окно > Подробнее
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Part_Modal_More(int ID)
        {
            Model_Orders_Common data = new();

            COrder_ForAdmin? order = _bl.Orders.Get_ForAdmin(ID);
            if (order == null)
            {
                return View(model: data);
            }

            Model_Details order_more = new()
            {
                Order = _bl.Orders.Get(ID),
                Client = _bl.Clients.Get(order.ClientID)?.Name,
                Comment = order.Comment,
                Client_Phone = _bl.Clients.Get(order.ClientID)?.Phone
            };

            if (ID > 0)
            {
                data.Order_More = order_more;
            }

            return View(model: data);
        }

        /// <summary>
        /// Модальное окно > Комментарий
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Part_Modal_Comment(int ID)
        {
            Model_Orders_Common data = new();

            COrder_ForAdmin? order = _bl.Orders.Get_ForAdmin(ID);
            if (order == null)
            {
                return View(model: data);
            }

            Model_Details order_more = new()
            {
                Order = _bl.Orders.Get(ID),
                Comment = order.Comment,
                Comment_AddToOrder = order.Comment_AddToOrder,
                Comment_RemoveFromOrder = order.Comment_RemoveFromOrder,
            };

            if (ID > 0)
            {
                data.Order_More = order_more;
            }

            return View(model: data);
        }

        /// <summary>
        /// Модальное окно > Подтверждение
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Part_Modal_Confirm(int ID, EOrder_Status Order_StatusID, bool Flag_Payed)
        {
            Model_Orders_Common data = new();

            COrder_ForAdmin? order = _bl.Orders.Get_ForAdmin(ID);
            if (order == null)
            {
                return View(model: data);
            }

            Model_Details order_more = new()
            {
                Order = _bl.Orders.Get(ID),
            };

            if (ID > 0)
            {
                data.Order_More = order_more;
            }

            if (Order_StatusID > 0)
            {
                data.Order_StatusID = (int)Order_StatusID;
                data.Order_Status = _bl.OrderStatuses.Get((int)Order_StatusID)?.Name;
                data.Flag_Payed = Flag_Payed;
            }

            return View(model: data);
        }

        /// <summary>
        /// Проверка даты
        /// </summary>
        /// <returns></returns>
        public bool Check_Date()
        {
            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Orders_DateChange);

            string? Cookie = Request.Cookies.TryGetValue("Check_Date", out string? strVal) ? strVal : null;

            if (val != null && Cookie != val.Value.ToString())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Заказы
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int? OrderID)
        {
            int RestaurantID = HIDE_Convert.ToInt(GetRestaurantIDByCookies());

            DateTime? val = _bl.Setups.Get_Date(Key: Consts.SetupKey_Orders_DateChange);
            if (val != null && val.Value > DateTime.MinValue)
            {
                CookieOptions option = new()
                {
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Append("Check_Date", val.Value.ToString(), option);
            }

            Model_Orders_Common model = new();

            if (RestaurantID == -1)
            {
                model.List_Orders = _bl.Orders.GetListByUserID(UserID: Helper_Client.GetCurrentClientID(HttpContext.User));
            }
            else
            {
                model.List_Orders = [.. _bl.Orders.GetList_ForOperator(RestaurantID: RestaurantID)
                                       .Where(p =>
                                       p.Order_StatusID != EOrder_Status.Cancelled &&
                                       p.Order_StatusID != EOrder_Status.Complete &&
                                       p.List_OrderDatas != null && p.List_OrderDatas.Count != 0)];
            }

            if (OrderID > 0)
            {
                model.OrderID = OrderID;
            }

            ViewBag.RestaurantID = RestaurantID;

            return View(model: model);
        }

        /// <summary>
        /// Таблица > Заказы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Orders_Part_Table(int? OrderID)
        {
            return Index(OrderID);
        }

        /// <summary>
        /// Модальное окно > Бронирование
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Part_Modal_Booking(int BookingID, string? Booking_Price)
        {
            if (BookingID <= 0)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }

            CBooking? order_booking = _bl.Bookings.Get(BookingID);

            if (order_booking == null)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }

            Model_Orders_Common data = new();

            string? type = _bl.Restaurant_Tables.Get(ID: order_booking.Restaurant_TableID)?.Restaurant_Table_Type;
            string? restaurant = _bl.Restaurant_Tables.Get(ID: order_booking.Restaurant_TableID)?.Restaurant;

            Model_Order_Booking model = new()
            {
                Booking = order_booking,
                Restaurant = restaurant,
                Restaurant_Table_Type = type,
                Booking_Price = HIDE_Convert.ToDecimal_Nullable(Booking_Price)
            };

            data.Order_Booking = model;

            return View(model: data);
        }

        /// <summary>
        /// Модальное окно > Доставка
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Part_Modal_Delivery(int ID)
        {
            Model_Orders_Common data = new();

            COrder_Delivery? order_delivery = _bl.Order_Deliveries.GetByOrderID(ID);

            if (order_delivery == null)
            {
                return View(model: data);
            }

            COrder_ForAdmin? order = _bl.Orders.Get_ForAdmin(ID: ID);

            decimal? order_summ = 0;

            if(order == null)
            {
                return View(model: data);
            }

            if (order.Delivery_Price != null)
            {
                order_summ += order.Delivery_Price;
            }

            if (order != null && order.List_OrderDatas != null && order.List_OrderDatas.Count != 0)
            {
                foreach (var elem in order.List_OrderDatas)
                {
                    if (elem.OrderID == ID)
                    {
                        order_summ += elem.Dish_Summ;
                    }
                }
            }

            Model_Order_Delivery order_delivery_more = new()
            {
                OrderID = ID,
                Delivery_Address = order_delivery.Delivery_Address,
                Delivery_EntranceCode = order_delivery.Delivery_EntranceCode,
                Delivery_EntranceNumber = order_delivery.Delivery_EntranceNumber,
                Delivery_Floor = order_delivery.Delivery_Floor,
                Delivery_Comment = order!.Comment_AddToOrder,
                Delivery_Comment_Remove = order.Comment_RemoveFromOrder,
                Delivery_Handing = order_delivery.Delivery_Handing,
                Delivery_To = order_delivery.Delivery_To,
                Delivery_Price = order_summ,
            };

            if (ID > 0)
            {
                data.Order_Delivery = order_delivery_more;
            }

            return View(model: data);
        }

        /// <summary>
        /// Новые заказы
        /// </summary>
        /// <returns></returns>
        public IActionResult Orders_New()
        {
            Model_Orders_Common model = new()
            {
                List_Orders = [.. _bl.Orders.GetList_ForAdmin().Where(p => p.Order_StatusID == EOrder_Status.New)]
            };
            return View(model: model);
        }

        /// <summary>
        /// Таблица > Новые заказы
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        public IActionResult Orders_New_Part_Table()
        {
            return Index(null);
        }

        /// <summary>
        /// Заказ готовится
        /// </summary>
        /// <returns></returns>
        public IActionResult Orders_Cooking()
        {
            Model_Orders_Common model = new()
            {
                List_Orders = [.. _bl.Orders.GetList_ForAdmin().Where(p => p.Order_StatusID == EOrder_Status.Cooking)]
            };
            return View(model: model);
        }

        /// <summary>
        /// Таблица > Заказ готовится
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Orders_Cooking_Part_Table()
        {
            return Index(null);
        }

        /// <summary>
        /// Готов к выдаче
        /// </summary>
        /// <returns></returns>
        public IActionResult Orders_ReadyToClient()
        {
            Model_Orders_Common model = new()
            {
                List_Orders = [.. _bl.Orders.GetList_ForAdmin().Where(p => p.Order_StatusID == EOrder_Status.ReadyToClient)]
            };
            return View(model: model);
        }

        /// <summary>
        /// Таблица > Готов к выдаче
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Orders_ReadyToClient_Part_Table()
        {
            return Index(null);
        }

        /// <summary>
        /// Передан в доставку
        /// </summary>
        /// <returns></returns>
        public IActionResult Orders_Delivery()
        {
            Model_Orders_Common model = new()
            {
                List_Orders = [.. _bl.Orders.GetList_ForAdmin().Where(p => p.Order_StatusID == EOrder_Status.Delivery)]
            };
            return View(model: model);
        }

        /// <summary>
        /// Таблица > Передан в доставку
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Orders_Delivery_Part_Table()
        {
            return Index(null);
        }

        /// <summary>
        /// Отмененные заказы
        /// </summary>
        /// <returns></returns>
        public IActionResult Orders_Cancelled()
        {
            Model_Orders_Common model = new()
            {
                List_Orders = [.. _bl.Orders.GetList_ForAdmin().Where(p => p.Order_StatusID == EOrder_Status.Cancelled)]
            };
            return View(model: model);
        }

        /// <summary>
        /// Таблица > Отмененные заказы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Orders_Cancelled_Part_Table()
        {
            return Index(null);
        }

        /// <summary>
        /// Выполненные заказы
        /// </summary>
        /// <returns></returns>
        public IActionResult Orders_Complete()
        {
            Model_Orders_Common model = new()
            {
                List_Orders = [.. _bl.Orders.GetList_ForAdmin().Where(p => p.Order_StatusID == EOrder_Status.Complete)]
            };
            return View(model: model);
        }

        /// <summary>
        /// Таблица > Выполненные заказы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Orders_Complete_Part_Table()
        {
            return Index(null);
        }

        /// <summary>
        /// Подробности заказа
        /// </summary>
        /// <returns></returns>
        public IActionResult Details(int ID)
        {
            Model_Details data = new();

            COrder? order = _bl.Orders.Get(ID);
            if (order == null)
            {
                return View(model: data);
            }

            if (ID > 0)
            {
                data.Order = _bl.Orders.Get(ID);
                data.Client = _bl.Clients.Get(order.ClientID)?.Name;
                data.Client_Phone = _bl.Clients.Get(order.ClientID)?.Phone;
            }

            return View(model: data);
        }



        /// <summary>
        /// Заказ выполнен
        /// </summary>
        /// <returns></returns>
        public IActionResult Details_Complete(int ID)
        {
            Model_Details data = new();

            if (ID > 0)
            {
                //data.Order = _bl.Orders.Get(ID);
            }

            return View(model: data);
        }

        /// <summary>
        /// Отмена заказа
        /// </summary>
        /// <returns></returns>
        public IActionResult Details_Cancel(int ID)
        {
            Model_Details data = new();

            if (ID > 0)
            {
                //data.Order = _bl.Orders.Get(ID);
            }

            return View(model: data);
        }

        #region Добавление

        /// <summary>
        /// Добавление заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            List<COrder_Data>? list_result = [];
            string? Cookie = Request.Cookies.TryGetValue("Add_Dish", out string? strVal) ? strVal : null;

            if (!string.IsNullOrWhiteSpace(Cookie))
            {
                list_result = JsonConvert.DeserializeObject<List<COrder_Data>>(Cookie);
            }

            IEnumerable<COrder_Data_Ingridient>? allIngredients = list_result?.SelectMany(p => p.List_Dish_Ingridients);
            decimal ingredient_price = allIngredients.Sum(x => x.Ingridient_Summ);   

            Model_Orders_AddEdit data = new()
            {
                List_Categories = _bl.Dish_Categories.GetList(Flag_IsParent: false),
                List_Clients = _bl.Clients.GetList(),
                List_Dishes2 = list_result,
                Sum_Itog = list_result.Sum(p => p.Dish_Summ) + ingredient_price
            };

            return View(model: data);
        }

        /// <summary>
        /// Добавление заказа
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(int? ClientID, string? Name, string? Phone)
        {
            Model_Orders_AddEdit data = new();
            decimal ingredient_price = 0;
            List<COrder_Data>? list_result = [];
            string? Cookie = Request.Cookies.TryGetValue("Add_Dish", out string? strVal) ? strVal : null;

            if (!string.IsNullOrWhiteSpace(Cookie))
            {
                list_result = JsonConvert.DeserializeObject<List<COrder_Data>>(Cookie);
            }

            if (ClientID < 0 && string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Phone))
            {
                ModelState.AddModelError("ClientID", "Выберите клиента или заполните поля Имя и Телефон");

                if(list_result != null)
                {
                    ingredient_price = list_result.Sum(p => p.List_Dish_Ingridients?.Sum(x => x.Ingridient_Summ) ?? 0);           //TODO: MAE - NULLABLE
                }

                data = new()
                {
                    List_Categories = _bl.Dish_Categories.GetList(Flag_IsParent: false),
                    List_Clients = _bl.Clients.GetList(),
                    List_Dishes2 = list_result,
                    Sum_Itog = list_result.Sum(p => p.Dish_Summ) + ingredient_price
                };

                return View(model: data);
            }

            if (list_result != null && list_result.Count == 0)           //TODO: MAE - NULLABLE
            {
                ModelState.AddModelError("ClientID", "Выберите блюда");
                ingredient_price = list_result.Sum(p => p.List_Dish_Ingridients?.Sum(x => x.Ingridient_Summ) ?? 0);            //TODO: MAE - NULLABLE

                data = new()
                {
                    List_Categories = _bl.Dish_Categories.GetList(Flag_IsParent: false),
                    List_Clients = _bl.Clients.GetList(),
                    List_Dishes2 = list_result,
                    Sum_Itog = list_result.Sum(p => p.Dish_Summ) + ingredient_price
                };

                return View(model: data);
            }

            int client_id = AppConfig.SystemClient;
            if (ClientID > 0)
            {
                client_id = HIDE_Convert.ToInt(ClientID);
            }

            int order_id = _bl.Orders.AddByOperator(ClientID: client_id, Name: Name ?? "", Phone: Phone ?? "", Pay_TypeID: EPayType.Money,
                                                   RestaurantID: AppConfig.Restaurant_Default, Date_To: null, Flag_Inhouse: true,
                                                   List_OrderData: list_result, Comment: null);

            if (order_id > 0)
            {
                Response.Cookies.Delete("Add_Dish");
                return RedirectToAction("Index"); //Возвращаем к списку заказов
            }

            ingredient_price = list_result.Sum(p => p.List_Dish_Ingridients.Sum(x => x.Ingridient_Summ));            //TODO: MAE - NULLABLE

            data = new()
            {
                List_Categories = _bl.Dish_Categories.GetList(Flag_IsParent: false),
                List_Clients = _bl.Clients.GetList(),
                List_Dishes2 = list_result,
                Sum_Itog = list_result.Sum(p => p.Dish_Summ) + ingredient_price
            };

            return View(model: data);
        }

        /// <summary>
        /// Частичное представление > Список блюд > Отображается после выбора категории
        /// </summary>
        /// <returns></returns>
        public IActionResult Add_Part_Table(int CategoryID)
        {
            Model_Orders_AddEdit data = new();
            if (CategoryID > 0)
            {
                data.CategoryID = CategoryID;
                data.Category = _bl.Dish_Categories.Get(CategoryID)?.Name;
                data.List_Dishes = _bl.Dishes.GetList_ForAdmin(DishCategoryID: CategoryID, Include_VolumesList: true, Include_Ingridients: true);
            }

            return View(model: data);
        }

        /// <summary>
        /// Частичное представление > Модальное окно > Список добавленных блюд с общей суммой
        /// </summary>
        /// <returns></returns>
        public IActionResult ResultOrder_Part_Table(int DishID, decimal Dish_Count, int? VolumeID, string? Dish_Ingredients, bool Flag_Delete)
        {
            Model_Orders_AddEdit data = new();

            List<COrder_Data>? list_result = [];
            string? Cookie = Request.Cookies.TryGetValue("Add_Dish", out string? strVal) ? strVal : null;

            if (Flag_Delete)
            {
                if (!string.IsNullOrWhiteSpace(Cookie))
                {
                    list_result = JsonConvert.DeserializeObject<List<COrder_Data>>(Cookie);
                }

                if (list_result != null && list_result.Count != 0)
                {
                    var item_delete = list_result.Single(p => p.DishID == DishID);
                    list_result.Remove(item_delete);
                }

                string json = JsonConvert.SerializeObject(list_result);

                CookieOptions option = new()
                {
                    Expires = DateTime.Now.AddMinutes(10)
                };
                Response.Cookies.Append("Add_Dish", json, option);


                decimal ingredient_price = list_result.Sum(p => p.List_Dish_Ingridients.Sum(x => x.Ingridient_Summ));            //TODO: MAE - NULLABLE

                data.List_Dishes2 = list_result;
                data.Sum_Itog = list_result.Sum(p => p.Dish_Summ) + ingredient_price;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Cookie))
                {
                    list_result = JsonConvert.DeserializeObject<List<COrder_Data>>(Cookie);
                }

                decimal ingredient_price = 0;
                List<COrder_Data_Ingridient> list_ingredients = [];
                if (!string.IsNullOrWhiteSpace(Dish_Ingredients))
                {
                    List<int>? list = Helper_ToArray.StringArr2_ToList(Dish_Ingredients);
                    foreach (var elem in list)
                    {
                        CDish_Ingridient? dish_ingredient = _bl.Dish_Ingridients.Get(elem);
                        if (dish_ingredient != null)
                        {
                            COrder_Data_Ingridient ingr = new()
                            {
                                IngridientID = dish_ingredient.IngridientID,
                                Ingridient_Count = Dish_Count,
                                Ingridient_Name = dish_ingredient.Ingridient ?? "",
                                Ingridient_Price = dish_ingredient.Price,
                                Ingridient_Summ = dish_ingredient.Price * Dish_Count,
                            };

                            list_ingredients.Add(ingr);
                            ingredient_price += dish_ingredient.Price * Dish_Count;
                        }
                    }
                }

                int return_ID = 0;
                int ID_Max = 0;
                if (list_result != null && list_result.Count != 0) { ID_Max = (from p in list_result select p.ID).Max(); }
                decimal sum_itog = 0;

                // Получение товара
                CDish? elem_product = _bl.Dishes.Get(DishID);
                if (elem_product != null)
                {
                    // Далее идет добавление товара в корзину
                    //COrder_Data? elem_check = list_result.Where(p => p.DishID == DishID).FirstOrDefault();

                    //if (elem_check == null)
                    //{
                    decimal dish_price = 0;

                    if (VolumeID > 0)
                    {
                        dish_price = _bl.Dish_Volumes.Get(HIDE_Convert.ToInt(VolumeID))?.VolumePrice ?? 0;
                        sum_itog = ((_bl.Dish_Volumes.Get(HIDE_Convert.ToInt(VolumeID))?.VolumePrice ?? 0) * Dish_Count);
                    }
                    else
                    {
                        dish_price = elem_product.Price;
                        sum_itog = elem_product.Price * Dish_Count;
                    }

                    COrder_Data cookie_model = new()
                    {
                        ID = ID_Max + 1,
                        DishID = DishID,
                        Bonuses_Minus = null,
                        Bonuses_Plus = null,
                        Dish_ColorID = null,
                        Dish_Name = elem_product.Name,
                        Dish_Price = dish_price,
                        Dish_Summ = sum_itog,
                        Dish_Count = Dish_Count,
                        Dish_VolumeID = VolumeID,
                        Dish_VolumeValue = VolumeID > 0 ? _bl.Dish_Volumes.Get(HIDE_Convert.ToInt(VolumeID))?.VolumeValue : null,
                        Dish_VolumePrice = VolumeID > 0 ? _bl.Dish_Volumes.Get(HIDE_Convert.ToInt(VolumeID))?.VolumePrice : null,
                        List_Dish_Ingridients = list_ingredients,
                    };

                    list_result?.Add(cookie_model);     

                    return_ID = ID_Max + 1;
                    //}
                    //else
                    //{
                    //    list_result[list_result.IndexOf(elem_check)].Dish_Count += Dish_Count;
                    //    return_ID = list_result[list_result.IndexOf(elem_check)].ID;
                    //}
                }

                string json = JsonConvert.SerializeObject(list_result);

                CookieOptions option = new()
                {
                    Expires = DateTime.Now.AddMinutes(10)
                };
                Response.Cookies.Append("Add_Dish", json, option);

                if(list_result != null)
                {
                    ingredient_price = list_result.Where(p => p.List_Dish_Ingridients != null).Sum(p => p.List_Dish_Ingridients!.Sum(x => x.Ingridient_Summ));            //TODO: MAE - NULLABLE

                    data.List_Dishes2 = list_result;
                    data.Sum_Itog = list_result.Sum(p => p.Dish_Summ) + ingredient_price;
                }

                
            }

            return View(model: data);
        }

        /// <summary>
        /// Частичное представление > Модальное окно > Выбор ингредиентов для блюда
        /// </summary>
        /// <returns></returns>
        public IActionResult Modal_Part_Table(int DishID)
        {
            Model_Orders_AddEdit data = new();
            CDish? dish = _bl.Dishes.Get(DishID, Include_Ingridients: true, Include_VolumesList: true);
            data.Dish = dish;
            return View(model: data);
        }

        #endregion

        /// <summary>
        /// Редирект на страницу с описанием ошибки
        /// </summary>
        /// <param name="ErrCode"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        private RedirectToActionResult RedirectToError(int ErrCode, string Url)
        {
            return RedirectToAction(actionName: Contst_Admin.ErrorActionName,
                                    controllerName: Contst_Admin.ErrorControllerName,
                                    routeValues: new { ErrCode, Url });

        }

        #region Изменение статуса заказа

        /// <summary>
        /// Изменение статуса заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit_Status(int ID)
        {
            COrder? order = _bl.Orders.Get(ID: ID);
            if (order != null)
            {
                Model_Orders_EditStatus data = new()
                {
                    ID = order.ID,
                    Flag_Payed = order.Flag_Payed,
                    Order_StatusID = order.Order_StatusID,
                    List_Order_Status = _bl.OrderStatuses.GetList()
                };

                return View(model: data);
            }

            return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
        }

        /// <summary>
        /// Изменение статуса заказа
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit_Status(Model_Orders_EditStatus data)
        {
            if (data != null)
            {
                int? client_id = _bl.Orders.Get(ID: data.ID)?.ClientID;
                COrder? order = _bl.Orders.Get(ID: data.ID);
                EOrder_Status? order_statusID = order?.Order_StatusID;
                int? order_id = _bl.Orders.Edit_Status(ID: data.ID, Order_StatusID: data.Order_StatusID, Flag_Payed: data.Flag_Payed);
                string? restaurant = _bl.Restaurants.GetList()?.FirstOrDefault()?.Address;

                if (order_id > 0)
                {
                    if (order_statusID != data.Order_StatusID)
                    {
                        switch (data.Order_StatusID)
                        {
                            //case EOrder_Status.New:
                            //    _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.New,
                            //                                           ReHIDEText1: order_id.ToString());
                            //    break;
                            case EOrder_Status.Cancelled:
                                _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.Cancelled,
                                                                       ReHIDEText1: order_id.ToString(), OrderID: order_id.Value);

                                if (order != null && order.BookingID.HasValue)
                                {
                                    _bl.Bookings.Del(ID: order.BookingID.Value, UserID: UserID);
                                }

                                break;
                            case EOrder_Status.Cooking:
                                _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.Cooking,
                                                                       ReHIDEText1: order_id.ToString(), OrderID: order_id.Value);
                                break;
                            case EOrder_Status.ReadyToClient:
                                _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.ReadyToClient,
                                                                       ReHIDEText1: order_id.ToString(), ReHIDEText2: restaurant, OrderID: order_id.Value);
                                break;
                            case EOrder_Status.Delivery:
                                _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.Delivery,
                                                                       ReHIDEText1: order_id.ToString(), OrderID: order_id.Value);
                                break;
                            case EOrder_Status.Complete:
                                _bl.Client_Notifications.Add_ByShablon(ClientID: HIDE_Convert.ToInt(client_id), ShablonID: EClient_Notification_Shablon.Complete,
                                                                       ReHIDEText1: order_id.ToString(), OrderID: order_id.Value);
                                break;
                            default:

                                break;
                        }
                    }

                    //return RedirectToAction("Orders_" + data.Order_StatusID.ToString());
                    return RedirectToAction(controllerName: "Orders", actionName: "Index", routeValues: new { OrderID = data.ID });
                }

                return View(model: data);
            }

            return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
        }

        #endregion


        /// <summary>
        /// Считываем куки > Получаем RestaurantID
        /// </summary>
        /// <returns></returns>
        private string? GetRestaurantIDByCookies()
        {
            CUser? user = _bl.Users.Get(ID: Helper_Client.GetCurrentClientID(HttpContext.User));
            int? restaurantID = user?.RestaurantID ?? null;

            if (user != null && user.Flag_Root)
            {
                ViewBag.Restaurants = _bl.Restaurants.GetList();
            }
            else
            {
                ViewBag.Restaurants = _bl.Restaurants.GetList()?.Where(p => p.ID == restaurantID).ToList();
            }

            string? RestaurantID;
            if (Request.Cookies["RestaurantCookies"] != null)
            {
                RestaurantID = Request.Cookies["RestaurantCookies"];
            }
            else
            {
                RestaurantID = ((List<CRestaurant>)ViewBag.Restaurants).FirstOrDefault()?.ID.ToString();
            }

            return RestaurantID;
        }


        /// <summary>
        /// Добавить в куки для сохранения ресторана
        /// </summary>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        public string AddRestaurantCookies(string page, string value)
        {
            var CookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(60)
            };
            Response.Cookies.Append(page, value, options: CookieOptions);

            return value;
        }

    }
}