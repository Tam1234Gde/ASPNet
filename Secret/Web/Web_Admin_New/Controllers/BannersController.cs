using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin_ForMenu.Helpers;
using HIDE.RestAndRant.Web_Admin_ForMenu.Models.Banners;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    /// <summary>
    /// Баннеры
    /// </summary>
    /// <remarks>
    /// Конструктор
    /// </remarks>
    /// <param name="BL"></param>
    [Authorize]
    public class BannersController(BL_Context BL, IWebHostEnvironment Env) : Controller
    {

        readonly BL_Context _bl = BL;
        private int user_id;
        private readonly IWebHostEnvironment _env = Env;
        readonly Images Images_BL = new();


        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех баннеров
        /// GET: /Banners
        /// </summary>
        [HttpGet]
        public IActionResult GetBanners()
        {
            try
            {
                List<CBanner> banners = _bl.Banners.GetList_Full();

                List<object> result = new List<object>();
                foreach (CBanner item in banners)
                {
                    string imageBigUrl = Images_BL.Get_Banner_Big(BannerID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
                    string imageSmallUrl = Images_BL.Get_Banner_Small(BannerID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.CityID,
                        CityName = item.City != null ? item.City.Name : string.Empty,
                        item.Category_ID,
                        item.Product_ID,
                        item.Date_Start,
                        item.Date_End,
                        item.Date_Create,
                        item.OrderNum,
                        Image_Big_Url = imageBigUrl ?? string.Empty,
                        Image_Small_Url = imageSmallUrl ?? string.Empty
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
        /// Получить баннер по ID
        /// GET: /Banners/GetBanner?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetBanner(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CBanner banner = _bl.Banners.Get(ID: ID);
                if (banner == null)
                    return Json(new { success = false, message = "Баннер не найден" });

                string imageBigUrl = Images_BL.Get_Banner_Big(BannerID: banner.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
                string imageSmallUrl = Images_BL.Get_Banner_Small(BannerID: banner.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                object result = new
                {
                    banner.ID,
                    banner.Name,
                    banner.CityID,
                    CityName = banner.City != null ? banner.City.Name : string.Empty,
                    banner.Category_ID,
                    banner.Product_ID,
                    banner.Date_Start,
                    banner.Date_End,
                    banner.Date_Create,
                    banner.OrderNum,
                    Image_Big_Url = imageBigUrl ?? string.Empty,
                    Image_Small_Url = imageSmallUrl ?? string.Empty
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список баннеров по городу
        /// GET: /Banners/GetBannersByCity?CityID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetBannersByCity(int CityID)
        {
            try
            {
                if (CityID <= 0)
                    return Json(new { success = false, message = "Неверный ID города" });

                List<CBanner> allBanners = _bl.Banners.GetList_Full();

                List<CBanner> filteredBanners = new List<CBanner>();
                foreach (CBanner item in allBanners)
                {
                    if (item.CityID == CityID)
                    {
                        filteredBanners.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CBanner item in filteredBanners)
                {
                    string imageBigUrl = Images_BL.Get_Banner_Big(BannerID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
                    string imageSmallUrl = Images_BL.Get_Banner_Small(BannerID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.CityID,
                        CityName = item.City != null ? item.City.Name : string.Empty,
                        item.Category_ID,
                        item.Product_ID,
                        item.Date_Start,
                        item.Date_End,
                        item.OrderNum,
                        Image_Big_Url = imageBigUrl ?? string.Empty,
                        Image_Small_Url = imageSmallUrl ?? string.Empty
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
        /// Получить список активных баннеров (текущие)
        /// GET: /Banners/GetActiveBanners
        /// </summary>
        [HttpGet]
        public IActionResult GetActiveBanners()
        {
            try
            {
                List<CBanner> allBanners = _bl.Banners.GetList_Full();
                DateTime currentDate = DateTime.Now.Date;

                List<CBanner> activeBanners = new List<CBanner>();
                foreach (CBanner item in allBanners)
                {
                    if (item.Date_Start <= currentDate && item.Date_End >= currentDate)
                    {
                        activeBanners.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CBanner item in activeBanners)
                {
                    string imageBigUrl = Images_BL.Get_Banner_Big(BannerID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");
                    string imageSmallUrl = Images_BL.Get_Banner_Small(BannerID: item.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.CityID,
                        CityName = item.City != null ? item.City.Name : string.Empty,
                        item.Category_ID,
                        item.Product_ID,
                        item.Date_Start,
                        item.Date_End,
                        item.OrderNum,
                        Image_Big_Url = imageBigUrl ?? string.Empty,
                        Image_Small_Url = imageSmallUrl ?? string.Empty
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

        public IActionResult Index()
        {

            List<CBanner>? Banners = _bl.Banners.GetList_Full();
            Banners.Reverse();

            Model_Banners model = new()
            {
                List_Cities = _bl.Cities.GetList(),
                List_Banners = [.. Banners]
            };

            return View(model: model);
        }

        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Part_Table()
        {
            return Index();
        }

        /// <summary>
        /// Получение Флага > Админ
        /// </summary>
        /// <returns></returns>
        public IActionResult Part_ListDishes(int DishCategoryID, int? Product_ID)
        {

            if (Product_ID == 0)
            {
                Product_ID = null;
            }

            Model_Banner_AddEdit model = new()
            {
                List_Product = _bl.Dishes.GetList(DishCategoryID),
                Product_ID = Product_ID
            };

            return View(model: model);

        }

        #region Добавление

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            Model_Banner_AddEdit model = new()
            {
                List_Cities = _bl.Cities.GetList(),
                List_Categories = _bl.Dish_Categories.GetList(Flag_IsParent: false),
            };

            return View(model: model);
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Banner_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                int banner_id = _bl.Banners.Add(Image_Big: null, Image_Small: null, Name: data.Name, CityID: data.CityID, Category_ID: data.Category_ID, Product_ID: data.Product_ID, Date_Start: data.Date_Start, Date_End: data.Date_End, OrderNum: int.Parse(data.OrderNum ?? "0"));

                if (banner_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = banner_id });
                }

                if (data.Image_Big != null)
                {
                    byte[] image_data = Helper_Image.Add_Banner_Big(BannerID: banner_id, CurrentFile: data.Image_Big, ContentRootPath: _env.ContentRootPath);

                    _bl.Banners.Edit_Image_Big(ID: banner_id, Banner_Image_Big: image_data);
                }

                if (data.Image_Small != null)
                {
                    byte[] image_data = Helper_Image.Add_Banner_Small(BannerID: banner_id, CurrentFile: data.Image_Small, ContentRootPath: _env.ContentRootPath);

                    _bl.Banners.Edit_Image_Small(ID: banner_id, Banner_Image_Small: image_data);
                }


                return RedirectToAction("Index");
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
            CBanner? banner = _bl.Banners.Get(ID: ID);

            if (banner != null)
            {



                Model_Banner_AddEdit data = new()
                {
                    ID = banner.ID,
                    Name = banner.Name,
                    Image_Big_Byte = banner.Image_Big,
                    Image_Small_Byte = banner.Image_Small,
                    CityID = banner.CityID,
                    Category_ID = banner.Category_ID,
                    Product_ID = banner.Product_ID,
                    Date_Start = banner.Date_Start,
                    Date_End = banner.Date_End,
                    Date_Create = banner.Date_Create,
                    OrderNum = banner.OrderNum.ToString(),
                    List_Cities = _bl.Cities.GetList(),
                    List_Categories = _bl.Dish_Categories.GetList(Flag_IsParent: false),

                };

                string? image_big_url = Images_BL.Get_Banner_Big(BannerID: data.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                string? image_small_url = Images_BL.Get_Banner_Small(BannerID: data.ID, ContentRootPath: _env.ContentRootPath, Url_Prefix: $"{Request.Scheme}://{Request.Host}");

                if (!string.IsNullOrWhiteSpace(image_big_url))
                {
                    data.Image_Big_Url = image_big_url;

                }
                if (!string.IsNullOrWhiteSpace(image_small_url))
                {

                    data.Image_Small_Url = image_small_url;
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
        public IActionResult Edit(Model_Banner_AddEdit data)
        {
            if (data != null)
            {
                // Преобразование из IFormFile в byte[]
                byte[]? img_big_Data = null;
                byte[]? img_small_Data = null;

                if (data.Image_Big != null)
                {
                    using var binaryReader = new BinaryReader(data.Image_Big.OpenReadStream());
                    img_big_Data = binaryReader.ReadBytes((int)data.Image_Big.Length);
                }
                else
                {
                    img_big_Data = data.Image_Big_Byte;
                }

                if (data.Image_Small != null)
                {
                    using var binaryReader = new BinaryReader(data.Image_Small.OpenReadStream());
                    img_small_Data = binaryReader.ReadBytes((int)data.Image_Small.Length);
                }
                else
                {
                    img_small_Data = data.Image_Small_Byte;
                }

                int banner_id = _bl.Banners.Edit(ID: data.ID, Name: data.Name, Image_Big: img_big_Data, Image_Small: img_small_Data, CityID: data.CityID, Category_ID: data.Category_ID, Product_ID: data.Product_ID, Date_Start: data.Date_Start,
                    Date_End: data.Date_End, OrderNum: int.Parse(data.OrderNum ?? "0"));

                if (banner_id > 0)
                {

                    if (data.Image_Big != null && img_big_Data != null)
                    {
                        Helper_Image.Del_Banner_Big(BannerID: banner_id, ContentRootPath: _env.ContentRootPath);
                        //byte[] image_data = Helper_Image.Add_Banner_Big(BannerID: banner_id, CurrentFile: data.Image_Big, ContentRootPath: _env.ContentRootPath);
                        _bl.Banners.Edit_Image_Big(ID: banner_id, Banner_Image_Big: img_big_Data);
                    }

                    if (data.Image_Small != null && img_small_Data != null)
                    {
                        Helper_Image.Del_Banner_Small(BannerID: banner_id, ContentRootPath: _env.ContentRootPath);
                        //byte[] image_data = Helper_Image.Add_Banner_Small(BannerID: banner_id, CurrentFile: data.Image_Small, ContentRootPath: _env.ContentRootPath);
                        _bl.Banners.Edit_Image_Small(ID: banner_id, Banner_Image_Small: img_small_Data);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model: data);
                }

            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err202 });
            }
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Изменение блюда
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Part_ModalDel(string ID)
        {
            CBanner? banner = _bl.Banners.Get(ID: int.Parse(ID));

            if (banner != null)
            {
                Model_Banners data = new()
                {
                    ID = banner.ID,
                    Name = banner.Name,

                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }
        }

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public string Del(int ID)
        {
            if (ID > 0)
            {
                int banners_id = _bl.Banners.Del(ID: ID, UserID: UserID);

                if (banners_id > 0)
                {
                    return "+";
                }
                else
                {
                    return "-";
                }
            }
            else
            {
                return "*";
            }
        }

        #endregion Удаление

        /// <summary>
        /// Удаление изображения
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>

        [HttpGet]
        public IActionResult Banners_DelImage(int ID, bool Big)
        {
            if (ID <= 0)
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }

            _bl.Banners.Del_Image(ID: ID, Big: Big);

            if (Big)
            {
                Helper_Image.Del_Banner_Big(BannerID: ID, ContentRootPath: _env.ContentRootPath);
            }
            else
            {
                Helper_Image.Del_Banner_Small(BannerID: ID, ContentRootPath: _env.ContentRootPath);
            }

            return RedirectToAction(controllerName: "Banners", actionName: "Edit", routeValues: new { ID });
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
