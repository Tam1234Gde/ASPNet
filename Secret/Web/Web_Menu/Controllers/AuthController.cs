using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Site_Menu.Models.Auth;

namespace HIDE.RestAndRant.Web_Site_Menu.Controllers
{
    /// <summary>
    /// Авторизация
    /// </summary>
    /// <param name="_bl"></param>
    public class AuthController(BL_Context _bl, ILogger<AuthController> _log) : Controller
    {
        /// <summary>
        /// Авторизация\Регистрация через телефон > Ввод номера
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// Авторизация\Регистрация через телефон > Ввод номера
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Login(string phone)
        {
            if (!string.IsNullOrWhiteSpace(phone))
            {
                CClient? client = _bl.Clients.GetOrAdd(Phone: new([.. phone.Where(char.IsDigit)]), Flag_SendSms: true);

                if (client != null)
                {
                    return "+";
                }
            }

            return "-";
        }


        /// <summary>
        /// Получение кода с телефона > Ввод кода из смс
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login_Code()
        {
            return View();
        }


        /// <summary>
        /// Получение кода с телефона > Ввод кода из смс
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login_Code(string phone, string code)
        {
            Model_SendCode model = new();

            if (!string.IsNullOrWhiteSpace(code))
            {
                CClient? client = _bl.Clients.GetClient_ByPhone_AndSms(Phone: new([.. phone.Where(char.IsDigit)]), SmsCode: code);

                if (client != null)
                {
                    // Авторизация
                    LoginSystem(ClientID: client.ID, ClientGuid: client.Guid, Flag_Remember: true);

                    model.Info = "+";
                    return new JsonResult(model);
                }
            }

            model.Info = "-";
            return new JsonResult(model);
        }


        /// <summary>
        /// Получить код еще раз через Телефон
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Login_Retry(string phone)
        {
            if (!string.IsNullOrWhiteSpace(phone))
            {
                CClient? client = _bl.Clients.GetOrAdd(Phone: new([.. phone.Where(char.IsDigit)]), Flag_SendSms: true);

                if (client != null)
                {
                    return "+";
                }
            }

            return "-";
        }


        /// <summary>
        /// Служебная функция для входа в систему
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="ClientGuid"></param>
        /// <param name="Flag_Remember"></param>
        private void LoginSystem(int ClientID, string ClientGuid, bool Flag_Remember)
        {
            List<Claim> claims = [
                new(ClaimTypes.Sid, ClientID.ToString()),
                new(ClaimTypes.SerialNumber, ClientGuid),
                new(ClaimTypes.Role, "User"),
                new(ClaimTypes.GroupSid, ClientID.ToString())
            ];

            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties authProperties = new()
            {
                IsPersistent = true
            };


            if (_log.IsEnabled(LogLevel.Information))
            {
                _log.LogInformation(message: "{}", args: $"OK # Login ClientID='{ClientID}' ClientGuid='{ClientGuid}' Flag_Remember='{Flag_Remember}'");
            }


            HttpContext.SignInAsync(scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                                    principal: new ClaimsPrincipal(claimsIdentity),
                                    properties: authProperties);
        }


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