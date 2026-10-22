using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Admin.Helpers;
using HIDE.RestAndRant.Web_Admin.Models.Pushes;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    /// <summary>
    /// Отправка пушей
    /// </summary>
    [Authorize]
    public class PushesController : Controller
    {
        private readonly ILogger _log;
        readonly BL_Context _bl;
        readonly IWebHostEnvironment _env;
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
        public PushesController(ILogger<PushesController> Logger, BL_Context BL, IWebHostEnvironment env)
        {
            _log = Logger;
            _log.LogDebug("Constructor PushesController");
            _bl = BL;
            _env = env;
        }


        /// <summary>
        /// Главная страница
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Pushes_Index()
        {

            return View();
        }


        /// <summary>
        /// Отправка пушей
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Pushes_Index(Model_Push_Send data)
        {
            if (ModelState.IsValid && data != null)
            {
                string? filePath = Path.Combine($"{_env.ContentRootPath}\\{Consts.FileFolder}", Path.GetRandomFileName());


                List<int>? list_result = null;

                if (data.File != null)
                {
                    using (FileStream fileStream = new(filePath, FileMode.Create))
                    {
                        data.File.CopyTo(fileStream);
                    }

                    list_result = Client_Notifications_Excel.GetFirstColumnData(filePath);
                }


                if (list_result != null && list_result.Count != 0)
                {
                    int result = _bl.ClientNotificationsExcel.Add_ClientNotifications(Name: data.Title, Content: data.Text, List_Clients: list_result, Date_Send: data.Date);

                    if (result < 0)
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = result });
                    }

                    if (data.Date != null)
                    {
                        ModelState.AddModelError("Info", $"В очередь добавлено {list_result.Count} уведомлений");

                    }
                    else
                    {
                        ModelState.AddModelError("Info", $"Успешно отправлено {list_result.Count} уведомлений");

                    }

                    if(data.File != null)
                    {
                        // Преобразование из IFormFile в byte[]
                        using BinaryReader binaryReader = new(data.File.OpenReadStream());
                        byte[] fileData = binaryReader.ReadBytes((int)data.File.Length);
                    }
                }
                else
                {
                    int result = _bl.ClientNotificationsExcel.Add_ClientNotifications(Name: data.Title, Content: data.Text, List_Clients: [], Date_Send: data.Date);

                    if (result < 0)
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = result });
                    }

                    if (data.Date != null)
                    {
                        ModelState.AddModelError("Info", $"В очередь добавлено уведомление для всех активных пользователей");

                    }
                    else
                    {
                        ModelState.AddModelError("Info", $"Успешно отправлено уведомление всем активным пользователям");

                    }
                }

                // Удаление файла - используйте System.IO.File вHIDE File
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                return View(model: data);
            }
            else
            {
                return View(model: data);
            }
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

        #region Получение имени пользователя

        /// <summary>
        /// Получение имени пользователя
        /// </summary>
        /// <returns></returns>
        public string? Get_UserName()
        {
            CUser? user = _bl.Users.Get(ID: Helper_User.GetCurrentUserID(HttpContext.User));
            if (user != null)
            {
                return user.Name;
            }
            return "";
        }

        #endregion Получение имени пользователя

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