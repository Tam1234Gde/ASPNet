using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.RegularExpressions;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;

namespace HIDE.RestAndRant.Web_Api_Test;

public static partial class TestHelper
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static string DefaultPhone = "79282000000";
    public static string ClientID = "1";
    public static string ClientGuid = "395df047-314d-4111-8532-be3bcbba6e99";
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public const string TestCardId = "2cd40127-000f-5000-9000-1323a33426b3";
    public const string DefaultCardNumber = "6759649826438453";

    [GeneratedRegex("^.{8}-.{4}-.{4}-.{4}-.{12}$")]
    public static partial Regex IdRegex();

    /// <summary>
    /// Инициализирует WebApplication
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public static void Init()
    {
#if DEBUG
        AppConfig.ConnectionString = Consts_Debug.DB_ConnectionString;
#endif
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Services.AddLogging(b => b.AddLog4Net("log4net.config"));
        builder.Services.AddTransient<BL_Context>();
        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();
        WebApplication app = builder.Build();

        app.MapControllers();

        ILoggerFactory? logger_factory = app.Services.GetService<ILoggerFactory>();
        if (logger_factory != null)
        {
            AppLog.LoggerFactory = logger_factory;
        }

        IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        string? config_val_str = config.GetSection("SMS_Settings")["SMS_Token"];
        if (string.IsNullOrWhiteSpace(config_val_str))
        {
            throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->SMS_Settings:SMS_Token");
        }

        AppConfig.SMS_Token = config_val_str;

        config_val_str = config.GetSection("SMS_Settings")["SMS_Shablon"];
        if (string.IsNullOrWhiteSpace(config_val_str))
        {
            throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->SMS_Settings:SMS_Shablon");
        }

        AppConfig.SMS_Shablon = config_val_str;

        AppConfig.SMS_Send = false;
        AppConfig.Mail_Send = false;

#if DEBUG
        config_val_str = config.GetSection("YooKassa")["Login"];
        if (string.IsNullOrWhiteSpace(config_val_str))
        {
            throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->YooKassa:Login");
        }

        AppConfig.Yookassa_Login = config_val_str;


        config_val_str = config.GetSection("YooKassa")["Password"];
        if (string.IsNullOrWhiteSpace(config_val_str))
        {
            throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->YooKassa:Password");
        }

        AppConfig.Yookassa_Password = config_val_str;
#else
        AppConfig.Yookassa_Password = config_val_str;
        //AppConfig.Yookassa_Login = "217079";
        //AppConfig.Yookassa_Password = "live_bUNFw0HIPb7Sy0V40fszlBG0JUQnSNzj3zGx8wbNN4k";
#endif
    }

    /// <summary>
    /// <para>Подставляет данные в контроллер</para>
    /// <para>Во время юнит тестирования HttpContext принимает значение NULL, этот метод служит для подставки нового HttpContext в контроллер</para>
    /// </summary>
    /// <param name="controller">Контроллер для привязки</param>
    /// <param name="Login">Логин для авторизации Basic, по умолчанию пустота</param>
    /// <param name="Password">Пароль для авторизации Basic, по умолчанию пустота</param>
    public static void MockController(this ControllerBase controller, string Login = "", string Password = "")
    {
        // https://stackoverflow.com/a/41400246/10240327
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        if (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password))
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Sid, Login),
                new Claim(ClaimTypes.SerialNumber, Password)
            ];

            ClaimsPrincipal principal = new(new ClaimsIdentity(claims, "Basic "));
            controller.ControllerContext.HttpContext.User = principal;
        }
    }

    /// <summary>
    /// Проверяет данные пользователя
    /// </summary>
    public static void CheckUserData()
    {
        Clients clients = new();
        CClient? client = clients.GetList().FirstOrDefault();
        if (client == null)
        {
            return;
        }

        ClientID = client.ID.ToString();

        if (!string.IsNullOrWhiteSpace(client.Phone))
        {
            DefaultPhone = client.Phone;
        }
    }

}