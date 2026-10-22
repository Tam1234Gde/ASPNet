using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.Volumes;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Объёмы
    /// </summary>
    [Authorize]
    public class VolumesController : Controller
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


        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех объемов
        /// GET: /Volumes
        /// </summary>
        [HttpGet]
        public IActionResult GetVolumes()
        {
            try
            {
                List<CVolume_ForAdmin> volumes = _bl.Volumes.GetList_ForAdmin();

                List<object> result = new List<object>();
                foreach (CVolume_ForAdmin item in volumes)
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
        /// Получить объем по ID
        /// GET: /Volumes/GetVolume?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetVolume(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CVolume_ForAdmin volume = _bl.Volumes.Get_ForAdmin(ID: ID);
                if (volume == null)
                    return Json(new { success = false, message = "Объем не найден" });

                object result = new
                {
                    volume.ID,
                    volume.Value,
                    volume.Name
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
        /// Конструктор
        /// </summary>
        /// <param name="BL"></param>
        public VolumesController(ILogger<VolumesController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor VolumesController");
            _bl = BL;
        }

        /// <summary>
        /// Список
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<CVolume_ForAdmin>? list_ingridients = _bl.Volumes.GetList_ForAdmin();
            return View(model: list_ingridients);
        }

        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Volumes_Part_Table()
        {
            return Index();
        }

        #region Добавление

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Volumes_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                int volume_id = _bl.Volumes.Add(Value: data.Value, Name: data.Name);

                if (volume_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index");
                }

                return RedirectToAction(controllerName: "Volumes", actionName: "Index");
            }
            else
            {
                return View();
            }
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
            CVolume_ForAdmin? volume = _bl.Volumes.Get_ForAdmin(ID: ID);
            if (volume != null)
            {
                Model_Volumes_AddEdit data = new()
                {
                    ID = volume.ID,
                    Value = volume.Value,
                    Name = volume.Name,
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index");
            }
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Volumes_AddEdit data)
        {
            if (data != null)
            {
                int volume_id = _bl.Volumes.Edit(ID: data.ID, Value_New: data.Value, Name_New: data.Name);

                if (volume_id > 0)
                {
                    return RedirectToAction(controllerName: "Volumes", actionName: "Index");
                }
                else
                {
                    return View(model: data);
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
        /// Удаление
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CVolume_ForAdmin? volume = _bl.Volumes.Get_ForAdmin(ID: ID);

                if (volume != null)
                {
                    Model_Volumes_AddEdit data = new()
                    {
                        Name = volume.Name
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
        /// Удаление
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Volumes_AddEdit Data)
        {
            if (Data != null)
            {
                int volume_id = _bl.Volumes.Del(ID: Data.ID, UserID: UserID);

                if (volume_id > 0)
                {
                    return RedirectToAction(controllerName: "Volumes", actionName: "Index");
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