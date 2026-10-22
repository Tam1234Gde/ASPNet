using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin_ForMenu.Helpers;
using HIDE.RestAndRant.Web_Admin_ForMenu.Models.Ratings;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    /// <summary>
    /// Оценка приложения
    /// </summary>
    [Authorize]
    public class RatingsController : Controller
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
        /// Получить список всех оценок приложения
        /// GET: /Ratings
        /// </summary>
        [HttpGet]
        public IActionResult GetRatings()
        {
            try
            {
                List<CRating> ratings = _bl.Ratings.GetList();

                List<object> result = new List<object>();
                foreach (CRating item in ratings)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.ClientID,
                        ClientName = item.Client != null ? item.Client : string.Empty
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
        /// Получить оценку по ID
        /// GET: /Ratings/GetRating?ID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetRating(int ID)
        {
            try
            {
                if (ID <= 0)
                    return Json(new { success = false, message = "Неверный ID" });

                CRating rating = _bl.Ratings.Get(ID: ID);
                if (rating == null)
                    return Json(new { success = false, message = "Оценка не найдена" });

                object result = new
                {
                    rating.ID,
                    rating.ClientID,
                    ClientName = rating.Client != null ? rating.Client : string.Empty
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Получить список оценок по клиенту
        /// GET: /Ratings/GetRatingsByClient?ClientID={id}
        /// </summary>
        [HttpGet]
        public IActionResult GetRatingsByClient(int ClientID)
        {
            try
            {
                if (ClientID <= 0)
                    return Json(new { success = false, message = "Неверный ID клиента" });

                List<CRating> allRatings = _bl.Ratings.GetList();

                List<CRating> filteredRatings = new List<CRating>();
                foreach (CRating item in allRatings)
                {
                    if (item.ClientID == ClientID)
                    {
                        filteredRatings.Add(item);
                    }
                }

                List<object> result = new List<object>();
                foreach (CRating item in filteredRatings)
                {
                    result.Add(new
                    {
                        item.ID,
                        item.ClientID,
                        ClientName = item.Client != null ? item.Client : string.Empty
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
        /// Конструктор
        /// </summary>
        /// <param name="Logger"></param>
        /// <param name="BL"></param>
        public RatingsController(ILogger<RatingsController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogDebug("Constructor RatingsController");
            _bl = BL;
        }

        /// <summary>
        /// Список
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<CRating>? list_result = _bl.Ratings.GetList();
            return View(model: list_result);
        }


        /// <summary>
        /// Таблица > Список
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Ratings_Part_Table()
        {
            return Index();
        }


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
                CRating? elem = _bl.Ratings.Get(ID: ID);

                if (elem != null)
                {
                    Model_Ratings_AddEdit data = new()
                    {
                        Client = elem.Client,
                        ClientID = elem.ClientID,
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
        /// Удаление
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Del(Model_Ratings_AddEdit Data)
        {
            if (Data != null)
            {
                int result = _bl.Ratings.Del(ID: Data.ID, UserID: UserID);

                if (result > 0)
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