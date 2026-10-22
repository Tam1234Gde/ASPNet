using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.Dishes;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Блюда
    /// </summary>
    [Authorize]
    public class DishesController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;
        private readonly IWebHostEnvironment _env;
        private int user_id;
        readonly Images Images_BL;

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
        public DishesController(ILogger<DishesController> Logger, BL_Context BL, IWebHostEnvironment Env)
        {
            _log = Logger;
            _log.LogDebug("Constructor DishesController");
            _bl = BL;
            _env = Env;
            Images_BL = new Images();
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список блюд (если указан DishCategoryID - фильтрует по категории)
        /// GET: /Dishes?Dish_CategoryID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetDishes(int? DishCategoryID)
        {
            try
            {
                List<CDish_ForAdmin> dishes = _bl.Dishes.GetList_ForAdmin(DishCategoryID: DishCategoryID);

                List<object> result = new List<object>();
                foreach (CDish_ForAdmin item in dishes)
                {
                    string imageUrl = Images_BL.Get_Dish(DishID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Dish_CategoryID,
                        item.Price,
                        item.Flag_AllowColors,
                        item.OrderNum,
                        item.Teaser,
                        item.Text,
                        item.Bonuses,
                        item.Flag_Bonus,
                        item.Cooking_Minutes,
                        item.RecommendDishIDs,
                        item.FlagNew,
                        item.FlagNoActive,
                        Image_Url = imageUrl ?? string.Empty
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
        /// Получить блюдо по ID
        /// GET: /Dishes/GetDish?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetDish(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CDish_ForAdmin dish = _bl.Dishes.Get_ForAdmin(ID: ID);
                if (dish == null)
                    return Json(new { success = false, message = "Блюдо не найдено" });

                string imageUrl = Images_BL.Get_Dish(DishID: dish.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                object result = new
                {
                    dish.ID,
                    dish.Name,
                    dish.Dish_CategoryID,
                    dish.Price,
                    dish.Flag_AllowColors,
                    dish.OrderNum,
                    dish.Teaser,
                    dish.Text,
                    dish.Bonuses,
                    dish.Flag_Bonus,
                    dish.Cooking_Minutes,
                    dish.RecommendDishIDs,
                    dish.FlagNew,
                    dish.FlagNoActive,
                    Image_Url = imageUrl ?? string.Empty
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список блюд по категории
        /// GET: /Dishes/GetDishesByCategory?DishCategoryID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetDishesByCategory(int DishCategoryID)
        {
            try
            {
                if (DishCategoryID <= 0)
                    return Json(new { success = false, message = "Неверный ID категории" });

                List<CDish_ForAdmin> dishes = _bl.Dishes.GetList_ForAdmin(DishCategoryID: DishCategoryID);

                List<object> result = new List<object>();
                foreach (CDish_ForAdmin item in dishes)
                {
                    string imageUrl = Images_BL.Get_Dish(DishID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Price,
                        item.OrderNum,
                        item.FlagNoActive,
                        Image_Url = imageUrl ?? string.Empty
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
        /// Получить список активных блюд (не удаленных)
        /// GET: /Dishes/GetActiveDishes?DishCategoryID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetActiveDishes(int? DishCategoryID)
        {
            try
            {
                List<CDish_ForAdmin> allDishes = _bl.Dishes.GetList_ForAdmin(DishCategoryID: DishCategoryID);

                List<CDish_ForAdmin> activeDishes = new List<CDish_ForAdmin>();
                foreach (CDish_ForAdmin item in allDishes)
                {
                    if (!item.FlagNoActive)
                    {
                        activeDishes.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CDish_ForAdmin item in activeDishes)
                {
                    string imageUrl = Images_BL.Get_Dish(DishID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Price,
                        item.OrderNum,
                        Image_Url = imageUrl ?? string.Empty
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
        /// Список блюд
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int? Dish_CategoryID)
        {
            Model_Dishes_List data = new()
            {
                List_Categories = _bl.Dish_Categories.GetList_Short(),
                List_Categories_ForSelect = Helper_SelectListItems.GetList(_bl.Dish_Categories.GetList_Short())
            };

            if (Dish_CategoryID > 0)
            {
                List<CDish_ForAdmin> list_dishes_result = [];

                List<CDish_ForAdmin> list_dishes = _bl.Dishes.GetList_ForAdmin(DishCategoryID: Dish_CategoryID);

                Dictionary<int, List<CRestaurant>?>? keyValuePairs = _bl.Restaurants.Get_IDRestPair(Dict: _bl.Restaurant_Menu_Dishes.Get_ByListIDs([.. _bl.Dishes.GetList().Select(x => x.ID)]) ?? []);

                foreach (CDish_ForAdmin item in list_dishes)
                {
                    string? image_url = Images_BL.Get_Dish(DishID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
                    if (!string.IsNullOrWhiteSpace(image_url))
                    {
                        item.Image_Url = image_url;
                    }

                    item.List_Dish_Ingridients = _bl.Dish_Ingridients.GetList_ForAdmin(DishID: item.ID);
                    item.List_Dish_Volumes = _bl.Dish_Volumes.GetList(DishID: item.ID);
                    item.List_Dish_Colors = _bl.Dish_Colors.GetList(DishID: item.ID);


                    if (keyValuePairs != null)
                    {
                        List<CRestaurant>? restaurants = keyValuePairs.GetValueOrDefault(item.ID);
                        List<string>? addresses = [.. restaurants?.Where(r => r != null).Select(r => r.Address) ?? []];

                        string output_addresses = string.Join(Environment.NewLine, addresses);

                        item.List_Adress_Restoran = addresses;
                        item.Adress_Restoran_Output = output_addresses;
                    }

                    list_dishes_result.Add(item);
                }
                data.List_Dishes = list_dishes_result;
                data.Dish_CategoryID = Dish_CategoryID;

            }

            return View(model: data);
        }


        /// <summary>
        /// Список блюд
        /// </summary>
        /// <returns></returns>
        public IActionResult Index_v2(int? Dish_CategoryID)
        {
            Model_Dishes_List data = new()
            {
                List_Categories = _bl.Dish_Categories.GetList_Short(),
                List_Categories_ForSelect = Helper_SelectListItems.GetList(_bl.Dish_Categories.GetList_Short())
            };

            if (Dish_CategoryID > 0)
            {
                List<CDish_ForAdmin> list_dishes_result = [];

                List<CDish_ForAdmin> list_dishes = _bl.Dishes.GetList_ForAdmin(DishCategoryID: Dish_CategoryID);

                foreach (CDish_ForAdmin item in list_dishes)
                {
                    string? image_url = Images_BL.Get_Dish(DishID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
                    if (!string.IsNullOrWhiteSpace(image_url))
                    {
                        item.Image_Url = image_url;
                    }

                    item.List_Dish_Ingridients = _bl.Dish_Ingridients.GetList_ForAdmin(DishID: item.ID);
                    item.List_Dish_Volumes = _bl.Dish_Volumes.GetList(DishID: item.ID);
                    item.List_Dish_Colors = _bl.Dish_Colors.GetList(DishID: item.ID);

                    list_dishes_result.Add(item);
                }
                data.List_Dishes = list_dishes_result;
                data.Dish_CategoryID = Dish_CategoryID;
            }

            return View(model: data);
        }


        /// <summary>
        /// Таблица > Список блюд
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Dishes_Part_Table(int? Dish_CategoryID)
        {
            Model_Dishes_List data = new()
            {
                List_Categories = _bl.Dish_Categories.GetList_Short(),
                List_Categories_ForSelect = Helper_SelectListItems.GetList(_bl.Dish_Categories.GetList_Short())
            };

            if (Dish_CategoryID > 0)
            {
                List<CDish_ForAdmin> list_dishes_result = [];
                List<CDish_ForAdmin> list_dishes = _bl.Dishes.GetList_ForAdmin(DishCategoryID: Dish_CategoryID);

                foreach (CDish_ForAdmin item in list_dishes)
                {
                    string? image_url = Images_BL.Get_Dish(DishID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
                    if (!string.IsNullOrWhiteSpace(image_url))
                    {
                        item.Image_Url = image_url;
                    }
                    list_dishes_result.Add(item);
                }
                data.List_Dishes = list_dishes_result;
                data.Dish_CategoryID = Dish_CategoryID;
            }

            return View(model: data);
        }


        #region Добавление

        /// <summary>
        /// Добавление блюда
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add(int Dish_CategoryID)
        {
            Model_Dishes_AddEdit model = new()
            {
                List_Categories = _bl.Dish_Categories.GetList(Flag_IsParent: false),
                Dish_CategoryID = Dish_CategoryID
            };

            return View(model);
        }

        /// <summary>
        /// Добавление блюда
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Dishes_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                int dish_id = _bl.Dishes.Add(Name: data.Name, Dish_CategoryID: data.Dish_CategoryID, Price: data.Price,
                                             Flag_AllowColors: data.Flag_AllowColors, OrderNum: data.OrderNum, Image: null, Teaser: data.Teaser,
                                             Text: data.Text, Bonuses: data.Bonuses, Flag_Bonus: data.Flag_Bonus, Cooking_Minutes: data.Cooking_Minutes,
                                             RecommendDishIDs: data.RecommendDishIDs, FlagNew: data.FlagNew, FlagNoActive: data.FlagNoActive, New_DateFrom: data.New_DateFrom);

                if (dish_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = dish_id });
                }

                if (data.UploadImge != null)
                {
                    byte[] image_data = Helper_Image.Add_Dish(DishID: dish_id, CurrentFile: data.UploadImge, ContentRootPath: _env.ContentRootPath);
                    _bl.Dishes.Edit_Image(ID: dish_id, Dish_Image: image_data);
                }

                // Добавить товар в другие ресторанах
                if (data.FlagForAll)
                {
                    _bl.Restaurant_Menu_Dishes.MassAddDishes(DishID: dish_id);
                }

                return RedirectToAction(controllerName: "Dishes", actionName: "Index", routeValues: new { data.Dish_CategoryID });
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение блюда
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CDish_ForAdmin? dish = _bl.Dishes.Get_ForAdmin(ID: ID);
            if (dish != null)
            {
                Model_Dishes_AddEdit data = new()
                {
                    ID = dish.ID,
                    Name = dish.Name,
                    Dish_CategoryID = dish.Dish_CategoryID,
                    Price = dish.Price,
                    Flag_AllowColors = dish.Flag_AllowColors,
                    List_Categories = _bl.Dish_Categories.GetList(Flag_IsParent: false),
                    Image = dish.Image,
                    Teaser = dish.Teaser,
                    Text = dish.Text,
                    OrderNum = dish.OrderNum,
                    Bonuses = dish.Bonuses,
                    Flag_Bonus = dish.Flag_Bonus,
                    Cooking_Minutes = dish.Cooking_Minutes,
                    RecommendDishIDs = dish.RecommendDishIDs,
                    FlagNew = dish.FlagNew,
                    FlagNoActive = dish.FlagNoActive,
                };

                string? image_url = Images_BL.Get_Dish(DishID: data.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
                if (!string.IsNullOrWhiteSpace(image_url))
                {
                    data.Image_Url = image_url;
                }

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Dishes_AddEdit data)
        {
            if (data != null)
            {
                // Преобразование из IFormFile в byte[]
                byte[]? img_Data = null;
                if (data.UploadImge != null)
                {
                    using var binaryReader = new BinaryReader(data.UploadImge.OpenReadStream());
                    img_Data = binaryReader.ReadBytes((int)data.UploadImge.Length);
                }
                else
                {
                    img_Data = data.Image;
                }

                // Получаем текущее изображение блюда в базе
                //byte[]? Dish_Image = _bl.Dishes.Get_ForAdmin(ID: data.ID)?.Image;

                int dish_id = _bl.Dishes.Edit(ID: data.ID, Name_New: data.Name, Dish_CategoryID_New: data.Dish_CategoryID, Price_New: data.Price,
                                              Flag_AllowColors_New: data.Flag_AllowColors, OrderNum_New: data.OrderNum, Image_New: img_Data,
                                              Teaser_New: data.Teaser, Text_New: data.Text, Bonuses_New: data.Bonuses, Flag_Bonus_New: data.Flag_Bonus,
                                              Cooking_Minutes_New: data.Cooking_Minutes, RecommendDishIDs_New: data.RecommendDishIDs, FlagNew_New: data.FlagNew,
                                              FlagNoActive_New: data.FlagNoActive, New_DateFrom_New: data.New_DateFrom);


                if (dish_id > 0)
                {
                    //// Изменение даты изменения изображения
                    //if (img_Data != null && Dish_Image != img_Data)
                    //{
                    //    _bl.Dishes.Edit_Image_ChangeData(ID: dish_id);
                    //}

                    if (data.UploadImge != null)
                    {
                        Helper_Image.Del_Dish(DishID: dish_id, ContentRootPath: _env.ContentRootPath);
                        byte[] image_data = Helper_Image.Add_Dish(DishID: dish_id, CurrentFile: data.UploadImge, ContentRootPath: _env.ContentRootPath);
                        _bl.Dishes.Edit_Image(ID: dish_id, Dish_Image: image_data);
                    }

                    // Редактирование цены для товарах в других ресторанах
                    if (data.FlagForAll)
                    {
                        _bl.Restaurant_Menu_Dishes.EditPrices(DishID: dish_id, Price: data.Price);
                    }

                    return RedirectToAction(controllerName: "Dishes", actionName: "Index", routeValues: new { data.Dish_CategoryID });
                }
                else
                {
                    return View(model: data);
                }

            }
            else
            {
                return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
            }
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удаление блюда
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID, int? Dish_CategoryID)
        {
            if (ID > 0)
            {
                CDish_ForAdmin? dish = _bl.Dishes.Get_ForAdmin(ID: ID);
                int dish_CategoryID = 0;

                if (dish != null)
                {
                    if (Dish_CategoryID > 0)
                    {
                        dish_CategoryID = dish.Dish_CategoryID;
                    }

                    Model_Dishes_AddEdit data = new()
                    {
                        Name = dish.Name,
                        Dish_CategoryID = dish_CategoryID
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
        /// Удаление блюда
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Dishes_AddEdit Data)
        {
            if (Data != null)
            {
                int dish_id = _bl.Dishes.Del(ID: Data.ID, UserID: UserID);

                if (dish_id > 0)
                {
                    // Редактирование цены для товарах в других ресторанах
                    if (Data.FlagForAll)
                    {
                        _bl.Restaurant_Menu_Dishes.DelDishes(DishID: dish_id);
                    }

                    return RedirectToAction(controllerName: "Dishes", actionName: "Index", routeValues: new { Data.Dish_CategoryID });
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
            return RedirectToAction(actionName: Contst_Admin.ErrorActionName,
                                 controllerName: Contst_Admin.ErrorControllerName,
                                 routeValues: new { ErrCode, Url });

        }

        /// <summary>
        /// Удаление изображения
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Dish_DelImage(int ID)
        {
            if (ID <= 0)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }

            _bl.Dishes.Del_Image(ID: ID);

            Helper_Image.Del_Dish(DishID: ID, ContentRootPath: _env.ContentRootPath);
            return RedirectToAction(controllerName: "Dishes", actionName: "Edit", routeValues: new { ID });
        }

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