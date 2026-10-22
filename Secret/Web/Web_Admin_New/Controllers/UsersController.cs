using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin_ForMenu.Helpers;
using HIDE.RestAndRant.Web_Admin_ForMenu.Models.Users;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    /// <summary>
    /// Пользователи 
    /// </summary>
    [Authorize]
    public class UsersController : Controller
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
        public UsersController(ILogger<UsersController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor UsersController");
            _bl = BL;
        }


        #region API Methods (JSON)

        /// <summary>
        /// Получить список всех пользователей
        /// GET: /Users
        /// </summary>
        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                List<CUser> users = _bl.Users.GetList();

                List<object> result = new List<object>();
                foreach (CUser item in users)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Login,
                        item.Flag_Admin,
                        item.Flag_Root,
                        item.Flag_Oper,
                        item.RestaurantID,
                        RestaurantName = item.Restaurant != null ? item.Restaurant : string.Empty
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
        /// Получить пользователя по ID
        /// GET: /Users/GetUser?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetUser(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CUser user = _bl.Users.Get(ID: ID);
                if (user == null)
                    return Json(new { success = false, message = "Пользователь не найден" });

                object result = new
                {
                    user.ID,
                    user.Name,
                    user.Login,
                    user.Flag_Admin,
                    user.Flag_Root,
                    user.Flag_Oper,
                    user.RestaurantID,
                    RestaurantName = user.Restaurant != null ? user.Restaurant : string.Empty
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список администраторов
        /// GET: /Users/GetAdmins
        /// </summary>
        [HttpGet]
        public IActionResult GetAdmins()
        {
            try
            {
                List<CUser> allUsers = _bl.Users.GetList();

                List<CUser> admins = new List<CUser>();
                foreach (CUser item in allUsers)
                {
                    if (item.Flag_Admin)
                    {
                        admins.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CUser item in admins)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Login,
                        item.Flag_Admin,
                        item.Flag_Root,
                        item.Flag_Oper,
                        item.RestaurantID,
                        RestaurantName = item.Restaurant != null ? item.Restaurant : string.Empty
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
        /// Получить список операторов
        /// GET: /Users/GetOperators
        /// </summary>
        [HttpGet]
        public IActionResult GetOperators()
        {
            try
            {
                List<CUser> allUsers = _bl.Users.GetList();

                List<CUser> operators = new List<CUser>();
                foreach (CUser item in allUsers)
                {
                    if (item.Flag_Oper)
                    {
                        operators.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CUser item in operators)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.Name,
                        item.Login,
                        item.Flag_Admin,
                        item.Flag_Root,
                        item.Flag_Oper,
                        item.RestaurantID,
                        RestaurantName = item.Restaurant != null ? item.Restaurant : string.Empty
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
        /// Список пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            CUser? user = _bl.Users.Get(ID: Helper_User.GetCurrentUserID(HttpContext.User));

            if (user == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "Auth");
            }

            List<CUser> list_users = _bl.Users.GetList();

            Model_Users_Table table_Users = new()
            {
                User = user,
                List_Users = list_users
            };

            return View(model: table_Users);
        }

        /// <summary>
        /// Таблица > Список пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Users_Part_Table()
        {

            CUser? user = _bl.Users.Get(ID: Helper_User.GetCurrentUserID(HttpContext.User));

            if (user == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "Auth");
            }

            List<CUser> list_users = _bl.Users.GetList();

            Model_Users_Table table_Users = new()
            {
                User = user,
                List_Users = list_users
            };

            return View(model: table_Users);
        }

        #region Добавление

        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            Model_Users_AddEdit model = new()
            {
                List_Restaurants = _bl.Restaurants.GetList()
            };

            return View(model: model);
        }

        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Model_Users_AddEdit data)
        {
            if (ModelState.IsValid && data != null)
            {
                int user_id = _bl.Users.Add(Name: data.Name ?? "", Login: data.Login, Password: data.Password, Flag_Admin: data.Flag_Admin, Flag_Root: data.Flag_Root,
                                            Flag_Oper: data.Flag_Oper, RestaurantID: data.RestaurantID > 0 ? data.RestaurantID : null);

                if (user_id < 0)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = user_id });
                }

                return RedirectToAction("Index"); //Возвращаем к списку пользователей
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region Изменение

        /// <summary>
        /// Изменение пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            CUser? user = _bl.Users.Get(ID: ID);
            if (user != null)
            {
                Model_Users_AddEdit data = new()
                {
                    ID = user.ID,
                    Name = user.Name,
                    Login = user.Login,
                    Password = user.Password,
                    Flag_Admin = user.Flag_Admin,
                    Flag_Root = user.Flag_Root,
                    Flag_Oper = user.Flag_Oper,
                    RestaurantID = user.RestaurantID,
                    List_Restaurants = _bl.Restaurants.GetList()
                };

                return View(model: data);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "Index", routeValues: new { code = Errors.Err200 });
            }
        }

        /// <summary>
        /// Изменение пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Model_Users_AddEdit Data)
        {
            if (Data != null)
            {
                int user_id = _bl.Users.Edit(ID: Data.ID, Name_New: Data.Name ?? "", Login_New: Data.Login, Password_New: Data.Password, Flag_Admin_New: Data.Flag_Admin,
                                             Flag_Root_New: Data.Flag_Root, Flag_Oper_New: Data.Flag_Oper, RestaurantID_New: Data.RestaurantID > 0 ? Data.RestaurantID : null);

                if (user_id > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Data.List_Restaurants = _bl.Restaurants.GetList();
                    return View(model: Data);
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
        /// Удаление пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID > 0)
            {
                CUser? user = _bl.Users.Get(ID: ID);

                if (user != null)
                {
                    Model_Users_AddEdit data = new()
                    {
                        Name = user.Name
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
        /// Удаление пользователя
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Users_AddEdit Data)
        {
            if (Data != null)
            {
                int user_id = _bl.Users.Del(ID: Data.ID, UserID: UserID);

                if (user_id > 0)
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