using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.Dish_Volumes;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Объемы
    /// </summary>
    [Authorize]
    public class DishVolumesController : Controller
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
                    user_id = Helper_User.GetCurrentUserID(HttpContext.User);
                }
                return user_id;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public DishVolumesController(ILogger<DishVolumesController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor DishVolumesController");
            _bl = BL;
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список объемов для блюда
        /// GET: /DishVolumes?DishID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetDishVolumes(int? DishID)
        {
            try
            {
                List<CDish_Volume> volumes = new List<CDish_Volume>();

                if (DishID > 0)
                {
                    volumes = _bl.Dish_Volumes.GetList(DishID: (int)DishID);
                }
                else
                {
                    volumes = _bl.Dish_Volumes.GetList(DishID: 0);
                }

                List<object> result = new List<object>();
                foreach (CDish_Volume item in volumes)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.DishID,
                        item.VolumeID,
                        item.VolumePrice,
                        item.OrderNum
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
        /// Получить объем блюда по ID
        /// GET: /DishVolumes/GetDishVolume?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetDishVolume(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CDish_Volume volume = _bl.Dish_Volumes.Get(ID: ID);
                if (volume == null)
                    return Json(new { success = false, message = "Объем не найден" });

                object result = new
                {
                    volume.ID,
                    volume.DishID,
                    volume.VolumeID,
                    volume.VolumePrice,
                    volume.OrderNum
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список всех доступных объемов
        /// GET: /DishVolumes/GetAllVolumes
        /// </summary>
        [HttpGet]
        public IActionResult GetAllVolumes()
        {
            try
            {
                List<CVolume> volumes = _bl.Volumes.GetList();

                List<object> result = new List<object>();
                foreach (CVolume item in volumes)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Value,
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
        /// Получить количество объемов для блюда
        /// GET: /DishVolumes/GetDishVolumesCount?DishID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetDishVolumesCount(int DishID)
        {
            try
            {
                if (DishID <= 0)
                    return Json(new { success = false, message = "Неверный ID блюда" });

                List<CDish_Volume> volumes = _bl.Dish_Volumes.GetList(DishID: DishID);
                int count = volumes != null ? volumes.Count : 0;

                return Json(new { success = true, data = new { DishID, Count = count } });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion


        /// <summary>
        /// Список объёмов
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int? DishID)
        {
            Model_Dish_Volumes_List data = new()
            {
                List_Volumes = Dish_Volumes.GetList(List_from: null),
            };

            if (DishID > 0)
            {
                data.DishID = DishID;
                data.DishName = _bl.Dishes.Get(ID: (int)DishID)?.Name;
                data.Dish_CategoryID = _bl.Dishes.Get(ID: (int)DishID)?.Dish_CategoryID;
                data.List_Volumes = _bl.Dish_Volumes.GetList(DishID: (int)DishID);
            }

            return View(model: data);
        }

        /// <summary>
        /// Таблица > Список объёмов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Dish_Volumes_Part_Table(int? DishID)
        {
            return Index(DishID);
        }

        #region Добавление

        /// <summary>
        /// Добавление объёма
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add(int DishID)
        {
            Model_Dish_Volumes_AddEdit model = new()
            {
                DishID = DishID,
                List_Volumes = _bl.Volumes.GetList()
            };

            return View(model);
        }

        /// <summary>
        /// Добавление объёма
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Dish_Volumes_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                int? volumes_count = _bl.Dish_Volumes.GetList(DishID: data.DishID)?.Count;
                if (volumes_count != null && volumes_count >= 3)
                {
                    ModelState.AddModelError(key: "VolumeID", errorMessage: "Для товара доступны только 3 объёма");
                    data.List_Volumes = _bl.Volumes.GetList();
                    return View(model: data);
                }

                int dish_volume_id = _bl.Dish_Volumes.Add(DishID: data.DishID, VolumeID: data.VolumeID, Price: data.Price, OrderNum: data.OrderNum);
                if (dish_volume_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index");
                }

                return RedirectToAction(controllerName: "DishVolumes", actionName: "Index", routeValues: new { data.DishID });
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение объёма
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CDish_Volume? dish_volume = _bl.Dish_Volumes.Get(ID: ID);
            if (dish_volume != null)
            {
                Model_Dish_Volumes_AddEdit data = new()
                {
                    ID = dish_volume.ID,
                    Price = dish_volume.VolumePrice,
                    List_Volumes = _bl.Volumes.GetList(),
                    OrderNum = dish_volume.OrderNum,
                    DishID = dish_volume.DishID
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }
        }

        /// <summary>
        /// Изменение объёма
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Dish_Volumes_AddEdit Data)
        {
            if (Data != null)
            {
                int dish_volume_id = _bl.Dish_Volumes.Edit(ID: Data.ID, DishID_New: Data.DishID, VolumeID_New: Data.VolumeID, Price_New: Data.Price,
                                                           OrderNum_New: Data.OrderNum);

                if (dish_volume_id > 0)
                {
                    return RedirectToAction(controllerName: "DishVolumes", actionName: "Index", routeValues: new { Data.DishID });
                }
                else
                {
                    return View(model: Data);
                }
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удаление объёма
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CDish_Volume? dish_volume = _bl.Dish_Volumes.Get(ID: ID);

                if (dish_volume != null)
                {
                    Model_Dish_Volumes_AddEdit data = new()
                    {
                        VolumeID = dish_volume.VolumeID,
                        DishID = dish_volume.DishID
                    };

                    return View(model: data);
                }
                else
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index");
                }
            }
            else
            {
                return RedirectToAction(controllerName: "Admin", actionName: "Error");
            }
        }

        /// <summary>
        /// Удаление объёма
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Dish_Volumes_AddEdit Data)
        {
            if (Data != null)
            {
                int dish_volume_id = _bl.Dish_Volumes.Del(ID: Data.ID, UserID: UserID);

                if (dish_volume_id > 0)
                {
                    return RedirectToAction(controllerName: "DishVolumes", actionName: "Index", routeValues: new { Data.DishID });
                }
                else
                {
                    return View(model: Data);
                }
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }
        }

        #endregion Удаление

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