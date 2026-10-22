using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Oper.Models.Auth;

namespace HIDE.RestAndRant.Web_Oper.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _log;

        private readonly BL_Context _bl;

        public AuthController(ILogger<AuthController> Logger, BL_Context BL)
        {
            _log = Logger;
            _log.LogTrace("Constructor");
            _bl = BL;
        }

        /// <summary>
        /// Служебная функция для входа в систему
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Flag_Remember"></param>
        private void LoginSystem(int UserID, string Email, bool Flag_Remember)
        {
            // https://docs.microsoft.com/ru-ru/aspnet/core/security/authentication/cookie?view=aspnetcore-6.0

            int? client_id = _bl.Users.Get(ID: UserID)?.ID;
            if (!client_id.HasValue)
            {
                //Если клиента определить не удалось, то выходим
                return;
            }

            var claims = new List<Claim> { new(ClaimTypes.Sid, UserID.ToString()),
                                           new(ClaimTypes.Name, Email),
                                           new(ClaimTypes.Role, "Administrator"),
                                           new(ClaimTypes.GroupSid, client_id.Value.ToString())
                                          };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Flag_Remember,
                ExpiresUtc = DateTime.Now.AddDays(30)
            };

            HttpContext.SignInAsync(scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                                    principal: new ClaimsPrincipal(claimsIdentity),
                                    properties: authProperties);
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            Model_Auth_Login model = new();
            if (!string.IsNullOrWhiteSpace(ReturnUrl))
            {
                model.ReturnUrl = ReturnUrl;
#if DEBUG
                model.Flag_Debug = true;
#endif
            }
            return View(model: model);
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(Model_Auth_Login data)
        {
            if (data != null)
            {
#if DEBUG
                data.Flag_Debug = true;
#endif
            }

            if (ModelState.IsValid && data != null && data.EMail != null && data.Password != null)
            {
                CUser? user = _bl.Users.Login(Email: data.EMail, Password: data.Password);
                if (user != null && user.ID > 0)
                {
                    if (user.Flag_Oper || user.Flag_Root)
                    {
                        if (!user.RestaurantID.HasValue && !user.Flag_Root)
                        {
                            ModelState.AddModelError("Info", "Доступ ограничен. Для пользователя не указан ресторан");
                            return View(model: data);
                        }

                        LoginSystem(UserID: user.ID, Email: data.EMail, Flag_Remember: data.Flag_Remember);

                        if (!string.IsNullOrWhiteSpace(data.ReturnUrl))
                        {
                            string decodedUrl = WebUtility.UrlDecode(data.ReturnUrl);

                            if (Url.IsLocalUrl(decodedUrl))
                            {
                                return LocalRedirect(localUrl: decodedUrl);
                            }
                            else
                            {
                                return RedirectToAction(actionName: "Index", controllerName: "Orders");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Index", controllerName: "Orders");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Info", "Доступ ограничен");
                        return View(model: data);
                    }
                }
                else
                {
                    ModelState.AddModelError("Info", "Не найден такой пользователь");
                    return View(model: data);
                }
            }
            else
            {
                return View(model: data);
            }
        }

#if DEBUG

        /// <summary>
        /// Автоматический вход (только для Debug)
        /// </summary>
        /// <returns>Редирект на Блюда</returns>
        public IActionResult DebugLogin(string? ReturnUrl)
        {
            CUser? user = _bl.Users.Get(ID: 1);
            if (user != null)
            {
                this.LoginSystem(UserID: user.ID, Email: user.Login, Flag_Remember: true);
            }

            if (!string.IsNullOrWhiteSpace(ReturnUrl))
            {
                string decodedUrl = WebUtility.UrlDecode(ReturnUrl);

                if (Url.IsLocalUrl(decodedUrl))
                {
                    return LocalRedirect(localUrl: decodedUrl);
                }
                else
                {
                    return RedirectToAction(actionName: "Index", controllerName: "Orders");
                }
            }

            return RedirectToAction(controllerName: "Orders", actionName: "Index");
        }

#endif

        /// <summary>
        /// Выход из системы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }

    }
}
