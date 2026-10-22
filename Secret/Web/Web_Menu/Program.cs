using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.Units_Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(configure => configure.AddLog4Net("log4net.config"));
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.Cookie.Name = "Web_Menu";
});
builder.Services.AddTransient<BL_Context>();
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>(); //TODO: MAE - понять будет ли работать без этого, это нужно для Html_Helper, а он не используется помоему (Он используется в _Layout.cshtml) - не уверен что это правильно, посоветуйтесь с DVV и SVA
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null); // Настройка форматирования json

WebApplication app = builder.Build();
IWebHostEnvironment env = app.Environment;

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Index/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

#if DEBUG
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
#else
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
#endif

// Обработка ошибки 404 - https://joonasw.net/view/custom-error-pages
app.Use(async (ctx, next) =>
{
    await next();

    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
    {
        ctx.Request.Path = "/error/404";
        await next();
    }
});

// Обработка ошибки 500 - https://joonasw.net/view/custom-error-pages
#if !DEBUG
app.UseExceptionHandler("/error/500");
#endif

app.UseStaticFiles();

// Изображения
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "_Images")),
    RequestPath = "/_Images",
    OnPrepareResponse = ctx =>
    { //https://stackoverflow.com/questions/61152499/dotnet-core-3-1-cors-issue-when-serving-static-image-files
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers",
          "Origin, X-Requested-With, Content-Type, Accept");
    },
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

ILoggerFactory? logger_factory = app.Services.GetService<ILoggerFactory>();
if (logger_factory != null)
{
    AppLog.LoggerFactory = logger_factory;
}

IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

#if !DEBUG
AppConfig.ConnectionString = config.GetSection("ConnectionStrings")["Connection_Base"];
#else
AppConfig.ConnectionString = Consts_Debug.DB_ConnectionString;
#endif

string? config_val_str;
int? config_val_int;
bool? config_val_bool;

#region Настройки почты
config_val_str = config.GetSection("MailSettings")["SMTP_Host"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MailSettings:SMTP_Host");
}
AppConfig.Mail_SMTP_Host = config_val_str;

config_val_int = HIDE_Convert.ToInt_Nullable(config.GetSection("MailSettings")["SMTP_Port"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MailSettings:SMTP_Port");
AppConfig.Mail_SMTP_Port = config_val_int.Value;

config_val_str = config.GetSection("MailSettings")["SMTP_User"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MailSettings:SMTP_User");
}
AppConfig.Mail_SMTP_User = config_val_str;

config_val_str = config.GetSection("MailSettings")["SMTP_Passwd"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MailSettings:SMTP_Passwd");
}
AppConfig.Mail_SMTP_Passwd = config_val_str;

config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("MailSettings")["Flag_EnableSSL"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MailSettings:Flag_EnableSSL");
AppConfig.Mail_SMTP_FlagEnableSSL = config_val_bool.Value;

config_val_str = config.GetSection("MailSettings")["Mail_From"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MailSettings:Mail_From");
}
AppConfig.Mail_MailFrom = config_val_str;

AppConfig.Mail_MailReply = config.GetSection("MailSettings")["Mail_Reply"];

config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("MailSettings")["Mail_Send"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MailSettings:Mail_Send");
AppConfig.Mail_Send = config_val_bool.Value;
#endregion Настройки почты

#region Настройки СМС
config_val_str = config.GetSection("MainSettings")["SMS_Token"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:SMS_Token");
}
AppConfig.SMS_Token = config_val_str;

config_val_str = config.GetSection("MainSettings")["SMS_Shablon"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:SMS_Shablon");
}
AppConfig.SMS_Shablon = config_val_str;

config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("MainSettings")["SMS_Send"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:SMS_Send");
AppConfig.SMS_Send = config_val_bool.Value;
#endregion Настройки СМС

#region TelegramBot
config_val_str = config.GetSection("Telegram_Settings")["TG_Token"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Telegram_Settings:TG_Token");
}
AppConfig.TG_Token = config_val_str;

config_val_str = config.GetSection("Telegram_Settings")["TG_ChatID"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Telegram_Settings:TG_ChatID");
}
AppConfig.TG_ChatID = config_val_str;

config_val_str = config.GetSection("Telegram_Settings")["TG_ChatBookingID"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Telegram_Settings:TG_ChatBookingID");
}
AppConfig.TG_ChatBookingID = config_val_str;

config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("Telegram_Settings")["TG_Active"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Telegram_Settings:TG_Active");
AppConfig.TG_Active = config_val_bool.Value;
#endregion TelegramBot

#region Редирект на HTTPS
config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("MainSettings")["UseHttpsRedirection"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:UseHttpsRedirection");
if (config_val_bool.Value)
{
    app.UseHttpsRedirection();
}
#endregion Редирект на HTTPS

app.Run();