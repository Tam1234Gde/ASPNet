using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.RestaurantTables;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Столики в ресторане
    /// </summary>
    [Authorize]
    public class RestaurantTablesController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public RestaurantTablesController(ILogger<RestaurantTablesController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor RestaurantTablesController");
            _bl = BL;
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех столиков
        /// GET: /RestaurantTables
        /// </summary>
        [HttpGet]
        public IActionResult GetRestaurantTables(int? RestaurantID)
        {
            try
            {
                List<CRestaurant_Table> tables;
                if (RestaurantID.HasValue && RestaurantID.Value > 0)
                {
                    tables = _bl.Restaurant_Tables.GetList(RestaurantID.Value);
                }
                else
                {
                    tables = _bl.Restaurant_Tables.GetListAll();
                }

                List<object> result = new List<object>();
                foreach (CRestaurant_Table item in tables)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.RestaurantID,
                        RestaurantName = item.Restaurant != null ? item.Restaurant : string.Empty,
                        item.Coordinates,
                        item.Coordinates_X,
                        item.Coordinates_Y,
                        item.Coordinates_Rotation,
                        item.Flag_IsAvailable,
                        item.Flag_IsAvailableApp,
                        item.Restaurant_Table_TypeID,
                        TableTypeName = item.Restaurant_Table_Type != null ? item.Restaurant_Table_Type : string.Empty,
                        item.Restaurant_Table_StatusID,
                        item.Restaurant_HallID,
                        HallName = item.Restaurant_Hall != null ? item.Restaurant_Hall : string.Empty,
                        item.Price,
                        item.Dop_Text,
                        item.QR_Code
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
        /// Получить столик по ID
        /// GET: /RestaurantTables/GetRestaurantTable?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetRestaurantTable(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CRestaurant_Table table = _bl.Restaurant_Tables.Get(ID: ID);
                if (table == null)
                    return Json(new { success = false, message = "Столик не найден" });

                object result = new
                {
                    table.ID,
                    table.Name,
                    table.RestaurantID,
                    RestaurantName = table.Restaurant != null ? table.Restaurant : string.Empty,
                    table.Coordinates,
                    table.Coordinates_X,
                    table.Coordinates_Y,
                    table.Coordinates_Rotation,
                    table.Flag_IsAvailable,
                    table.Flag_IsAvailableApp,
                    table.Restaurant_Table_TypeID,
                    TableTypeName = table.Restaurant_Table_Type != null ? table.Restaurant_Table_Type : string.Empty,
                    table.Restaurant_Table_StatusID,
                    table.Restaurant_HallID,
                    HallName = table.Restaurant_Hall != null ? table.Restaurant_Hall : string.Empty,
                    table.Price,
                    table.Dop_Text,
                    table.QR_Code
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список столиков по ресторану
        /// GET: /RestaurantTables/GetRestaurantTablesByRestaurant?RestaurantID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetRestaurantTablesByRestaurant(int RestaurantID)
        {
            try
            {
                if (RestaurantID <= 0)
                    return Json(new { success = false, message = "Неверный ID ресторана" });

                List<CRestaurant_Table> tables = _bl.Restaurant_Tables.GetList(RestaurantID);

                List<object> result = new List<object>();
                foreach (CRestaurant_Table item in tables)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.RestaurantID,
                        RestaurantName = item.Restaurant != null ? item.Restaurant : string.Empty,
                        item.Coordinates,
                        item.Coordinates_X,
                        item.Coordinates_Y,
                        item.Coordinates_Rotation,
                        item.Flag_IsAvailable,
                        item.Flag_IsAvailableApp,
                        item.Restaurant_Table_TypeID,
                        TableTypeName = item.Restaurant_Table_Type != null ? item.Restaurant_Table_Type : string.Empty,
                        item.Restaurant_Table_StatusID,
                        item.Restaurant_HallID,
                        HallName = item.Restaurant_Hall != null ? item.Restaurant_Hall : string.Empty,
                        item.Price,
                        item.Dop_Text,
                        item.QR_Code
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
        /// Получить список доступных столиков
        /// GET: /RestaurantTables/GetAvailableRestaurantTables?RestaurantID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetAvailableRestaurantTables(int? RestaurantID)
        {
            try
            {
                List<CRestaurant_Table> allTables;
                if (RestaurantID.HasValue && RestaurantID.Value > 0)
                {
                    allTables = _bl.Restaurant_Tables.GetList(RestaurantID.Value);
                }
                else
                {
                    allTables = _bl.Restaurant_Tables.GetListAll();
                }

                List<CRestaurant_Table> availableTables = new List<CRestaurant_Table>();
                foreach (CRestaurant_Table item in allTables)
                {
                    if (item.Flag_IsAvailable && item.Flag_IsAvailableApp)
                    {
                        availableTables.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CRestaurant_Table item in availableTables)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.RestaurantID,
                        RestaurantName = item.Restaurant != null ? item.Restaurant : string.Empty,
                        item.Coordinates,
                        item.Coordinates_X,
                        item.Coordinates_Y,
                        item.Coordinates_Rotation,
                        item.Restaurant_Table_TypeID,
                        TableTypeName = item.Restaurant_Table_Type != null ? item.Restaurant_Table_Type : string.Empty,
                        item.Restaurant_Table_StatusID,
                        item.Restaurant_HallID,
                        HallName = item.Restaurant_Hall != null ? item.Restaurant_Hall : string.Empty,
                        item.Price,
                        item.Dop_Text,
                        item.QR_Code
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
        /// Список столиков в ресторане
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int? RestaurantID)
        {
            List<CRestaurant_Table>? list_restauranttables;
            if (RestaurantID.HasValue)
            {
                list_restauranttables = _bl.Restaurant_Tables.GetList(HIDE_Convert.ToInt(RestaurantID))?.OrderBy(p => p.RestaurantID).ThenBy(p => p.Restaurant_Table_TypeID).ToList();
            }
            else
            {
                list_restauranttables = _bl.Restaurant_Tables.GetListAll()?.OrderBy(p => p.RestaurantID).ThenBy(p => p.Restaurant_Table_TypeID).ToList();
            }

            return View(model: list_restauranttables);
        }

        /// <summary>
        /// Таблица > Список столиков в ресторане
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RestaurantTables_Part_Table(int? RestaurantID)
        {
            return Index(RestaurantID: RestaurantID);
        }

        #region Добавление

        /// <summary>
        /// Добавление столика в ресторане
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            Model_RestaurantTables_AddEdit model = new()
            {
                List_Restaurants = _bl.Restaurants.GetList(),
                List_RestaurantTableTypes = _bl.Restaurant_Table_Types.GetList(),
                List_RestaurantTableStatuses = _bl.Restaurant_Table_Statuses.GetList()?.Where(p => p.ID != (int)ERestaurant_Table_Status.Unavailable).ToList(),
                //List_Restaurant_Halls = _bl.Restaurant_Halls.GetListByRestaurantID()
            };
            return View(model: model);
        }

        /// <summary>
        /// Добавление столика в ресторане
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_RestaurantTables_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                int restaurantеable_id = _bl.Restaurant_Tables.Add(Name: data.Name, RestaurantID: data.RestaurantID, Coordinates: data.Coordinates,
                                                                   Flag_IsAvailableApp: data.Flag_IsAvailableApp, Flag_IsAvailable: data.Flag_IsAvailable,
                                                                   Restaurant_Table_TypeID: data.Restaurant_Table_TypeID, Coordinates_X: data.Coordinates_X,
                                                                   Coordinates_Y: data.Coordinates_Y, Coordinates_Rotation: data.Coordinates_Rotation,
                                                                   Dop_Text: data.Dop_Text, Price: data.Price, Restaurant_Table_StatusID: data.Restaurant_Table_StatusID,
                                                                   QR_Code: data.QR_Code);

                if (restaurantеable_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = restaurantеable_id });
                }

                return RedirectToAction("Index"); //Возвращаем к списку столов в ресторане
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение столика в ресторане
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CRestaurant_Table? restaurantеable = _bl.Restaurant_Tables.Get(ID: ID);
            if (restaurantеable != null)
            {
                Model_RestaurantTables_AddEdit data = new()
                {
                    ID = restaurantеable.ID,
                    Name = restaurantеable.Name,
                    Coordinates = restaurantеable.Coordinates,
                    Flag_IsAvailable = restaurantеable.Flag_IsAvailable,
                    Flag_IsAvailableApp = restaurantеable.Flag_IsAvailableApp,
                    RestaurantID = restaurantеable.RestaurantID,
                    List_Restaurants = _bl.Restaurants.GetList(),
                    Restaurant_Table_TypeID = restaurantеable.Restaurant_Table_TypeID,
                    Restaurant_HallID = restaurantеable.Restaurant_HallID,
                    Price = restaurantеable.Price,
                    Dop_Text = restaurantеable.Dop_Text,
                    QR_Code = restaurantеable.QR_Code,
                    Coordinates_Rotation = restaurantеable.Coordinates_Rotation,
                    Coordinates_Y = restaurantеable.Coordinates_Y,
                    Coordinates_X = restaurantеable.Coordinates_X,
                    Restaurant_Table_StatusID = restaurantеable.Restaurant_Table_StatusID,
                    List_RestaurantTableTypes = _bl.Restaurant_Table_Types.GetList(),
                    List_RestaurantTableStatuses = _bl.Restaurant_Table_Statuses.GetList()?.Where(p => p.ID != (int)ERestaurant_Table_Status.Unavailable).ToList(),
                    List_Restaurant_Halls = _bl.Restaurant_Halls.GetListByRestaurantID(RestaurantID: restaurantеable.RestaurantID, Restaurant_Table_TypeID: restaurantеable.Restaurant_Table_TypeID)
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }
        }

        /// <summary>
        /// Изменение столика в ресторане
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_RestaurantTables_AddEdit data)
        {
            if (data != null)
            {
                int restaurantеable_id = _bl.Restaurant_Tables.Edit(ID: data.ID, Name: data.Name, RestaurantID: data.RestaurantID, Coordinates: data.Coordinates,
                                                                    Flag_IsAvailableApp: data.Flag_IsAvailableApp, Flag_IsAvailable: data.Flag_IsAvailable,
                                                                    Restaurant_Table_TypeID: data.Restaurant_Table_TypeID, Coordinates_X: data.Coordinates_X,
                                                                    Coordinates_Y: data.Coordinates_Y, Coordinates_Rotation: data.Coordinates_Rotation,
                                                                    Dop_Text: data.Dop_Text, Price: data.Price, Restaurant_Table_StatusID: data.Restaurant_Table_StatusID,
                                                                    QR_Code: data.QR_Code, Restaurant_HallID: data.Restaurant_HallID);

                if (restaurantеable_id > 0)
                {
                    return RedirectToAction("Index");
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
        /// Удаление столика в ресторане
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CRestaurant_Table? restaurantеable = _bl.Restaurant_Tables.Get(ID: ID);

                if (restaurantеable != null)
                {
                    Model_RestaurantTables_AddEdit data = new()
                    {
                        Name = restaurantеable.Name
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
        /// Удаление столика в ресторане
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_RestaurantTables_AddEdit Data)
        {
            if (Data != null)
            {
                int restaurantеable_id = _bl.Restaurant_Tables.Del(ID: Data.ID, UserID: Helper_User.GetCurrentUserID(HttpContext.User));

                if (restaurantеable_id > 0)
                {
                    return RedirectToAction("Index");
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