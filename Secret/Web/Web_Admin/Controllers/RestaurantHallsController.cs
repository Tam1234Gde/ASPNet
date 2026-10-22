using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.RestaurantHalls;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Залы в ресторане
    /// </summary>
    [Authorize]
    public class RestaurantHallsController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public RestaurantHallsController(ILogger<RestaurantHallsController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor RestaurantHallsController");
            _bl = BL;
        }

        /// <summary>
        /// Список
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int RestaurantID)
        {
            Model_RestaurantHalls_Index model = new()
            {
                List_Restaurant_Halls = _bl.Restaurant_Halls.GetListByRestaurantID(RestaurantID),
                Restaurant = _bl.Restaurants.Get(RestaurantID)
            };

            return View(model: model);
        }

        /// <summary>
        /// Таблица > Частичное представление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantHalls_Part_Table(int RestaurantID)
        {
            return Index(RestaurantID);
        }

        #region Добавление

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add(int? RestaurantID)
        {
            Model_RestaurantHalls_AddEdit model = new()
            {
                List_Restaurants = _bl.Restaurants.GetList(),
                List_Restaurant_Table_Types = _bl.Restaurant_Table_Types.GetList(),
            };

            if (RestaurantID.HasValue)
            {
                model.RestaurantID = RestaurantID.Value;
            }

            return View(model: model);
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_RestaurantHalls_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                // Преобразование из IFormFile в byte[]
                byte[]? icon_Data = null;
                if (data.UploadImge != null)
                {
                    using var binaryReader = new BinaryReader(data.UploadImge.OpenReadStream());
                    icon_Data = binaryReader.ReadBytes((int)data.UploadImge.Length);
                }

                int restaurant_hall_id = _bl.Restaurant_Halls.Add(RestaurantID: data.RestaurantID, Restaurant_Table_TypeID: data.Restaurant_Table_TypeID,
                                                                  Restaurant_Image: icon_Data, Flag_Active: data.Flag_Active, Name: data.Name);

                if (restaurant_hall_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = restaurant_hall_id });
                }

                return RedirectToAction(controllerName: "RestaurantHalls", actionName: "Index", routeValues: new { data.RestaurantID });
            }

            return View();
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CRestaurant_Hall? restaurant_hall = _bl.Restaurant_Halls.Get(ID: ID);
            if (restaurant_hall != null)
            {
                Model_RestaurantHalls_AddEdit data = new()
                {
                    ID = restaurant_hall.ID,
                    Restaurant_Image = restaurant_hall.Restaurant_Image,
                    Restaurant_Table_TypeID = restaurant_hall.Restaurant_Table_TypeID,
                    List_Restaurant_Table_Types = _bl.Restaurant_Table_Types.GetList(),
                    RestaurantID = restaurant_hall.RestaurantID,
                    Flag_Active = restaurant_hall.Flag_Active,
                    Name = restaurant_hall.Name,
                    List_Restaurants = _bl.Restaurants.GetList()
                };

                return View(model: data);
            }

            return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_RestaurantHalls_AddEdit data)
        {
            if (data != null)
            {
                // Преобразование из IFormFile в byte[]
                byte[]? icon_Data = null;
                if (data.UploadImge != null)
                {
                    using var binaryReader = new BinaryReader(data.UploadImge.OpenReadStream());
                    icon_Data = binaryReader.ReadBytes((int)data.UploadImge.Length);
                }
                else
                {
                    icon_Data = data.Restaurant_Image;
                }

                int restaurant_hall_id = _bl.Restaurant_Halls.Edit(ID: data.ID, RestaurantID: data.RestaurantID, Restaurant_Table_TypeID: data.Restaurant_Table_TypeID,
                                                                   Restaurant_Image: icon_Data, Flag_Active: data.Flag_Active, Name: data.Name);

                if (restaurant_hall_id < 0)
                {
                    data.List_Restaurants = _bl.Restaurants.GetList();
                    return View(model: data);
                }

                return RedirectToAction(controllerName: "RestaurantHalls", actionName: "Index", routeValues: new { data.RestaurantID });
            }

            return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удаление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CRestaurant_Hall? restaurant_hall = _bl.Restaurant_Halls.Get(ID: ID);

                if (restaurant_hall != null)
                {
                    Model_RestaurantHalls_AddEdit data = new()
                    {

                    };

                    return View(model: data);
                }
            }

            return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_RestaurantHalls_AddEdit Data)
        {
            if (Data != null)
            {
                int restaurant_hall_id = _bl.Restaurant_Halls.Del(ID: Data.ID, UserID: Helper_User.GetCurrentUserID(HttpContext.User));

                if (restaurant_hall_id < 0)
                {
                    return View(model: Data);
                }

                return RedirectToAction(controllerName: "RestaurantHalls", actionName: "Index", routeValues: new { Data.RestaurantID });
            }

            return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
        }

        #endregion Удаление

        /// <summary>
        /// Получить изображение
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult GetImage(int ID)
        {
            CRestaurant_Hall? result = _bl.Restaurant_Halls.Get(ID: ID);

            if (result == null || result.Restaurant_Image == null)
            {
                return NotFound();
            }

            return File(result.Restaurant_Image, "image/png", result.Restaurant);
        }

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