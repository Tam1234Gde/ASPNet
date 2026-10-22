using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin_ForMenu.Helpers;
using HIDE.RestAndRant.Web_Admin_ForMenu.Models.RestaurantTableStatuses;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    /// <summary>
    /// Статусы столика
    /// </summary>
    [Authorize]
    public class RestaurantTableStatusesController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public RestaurantTableStatusesController(ILogger<RestaurantTableStatusesController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor RestaurantStatusesController");
            _bl = BL;
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех статусов столиков
        /// GET: /RestaurantTableStatuses
        /// </summary>
        [HttpGet]
        public IActionResult GetRestaurantTableStatuses()
        {
            try
            {
                List<CRestaurant_Table_Status> statuses = _bl.Restaurant_Table_Statuses.GetList();

                List<object> result = new List<object>();
                foreach (CRestaurant_Table_Status item in statuses)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name
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
        /// Получить статус столика по ID
        /// GET: /RestaurantTableStatuses/GetRestaurantTableStatus?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetRestaurantTableStatus(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CRestaurant_Table_Status status = _bl.Restaurant_Table_Statuses.Get(ID: ID);
                if (status == null)
                    return Json(new { success = false, message = "Статус не найден" });

                object result = new
                {
                    status.ID,
                    status.Name
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion


        /// <summary>
        /// Список
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<CRestaurant_Table_Status>? list_result = _bl.Restaurant_Table_Statuses.GetList();
            return View(list_result);
        }

        /// <summary>
        /// Таблица > Частичное представление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantTableStatuses_Part_Table()
        {
            return Index();
        }

        #region Изменение

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CRestaurant_Table_Status? elem = _bl.Restaurant_Table_Statuses.Get(ID: ID);
            if (elem != null)
            {
                Model_RestaurantTableStatuses_AddEdit data = new()
                {
                    ID = elem.ID,
                    Name = elem.Name,
                    RestaurantTable_Image = elem.RestaurantTable_Image,
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
        public IActionResult Edit(Model_RestaurantTableStatuses_AddEdit data)
        {
            if (data != null)
            {
                // Преобразование из IFormFile в byte[]
                byte[]? image = null;
                if (data.UploadImge != null)
                {
                    using var binaryReader = new BinaryReader(data.UploadImge.OpenReadStream());
                    image = binaryReader.ReadBytes((int)data.UploadImge.Length);
                }
                else
                {
                    image = data.RestaurantTable_Image;
                }

                int result = _bl.Restaurant_Table_Statuses.Edit(ID: data.ID, Name: data.Name, RestaurantTable_Image: image);

                if (result < 0)
                {
                    return View(model: data);
                }

                return RedirectToAction(controllerName: "RestaurantTableStatuses", actionName: "Index");
            }

            return RedirectToError(ErrCode: Errors.Err202, Url: HttpContext.Request.Path.ToString());
        }

        #endregion

        /// <summary>
        /// Получить изображение
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult GetImage(int ID)
        {
            CRestaurant_Table_Status? result = _bl.Restaurant_Table_Statuses.Get(ID: ID);

            if (result == null || result.RestaurantTable_Image == null)
            {
                return NotFound();
            }

            return File(result.RestaurantTable_Image, "image/png", result.Name);
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