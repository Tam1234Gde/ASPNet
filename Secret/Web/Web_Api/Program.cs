using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Web_Api.Extensions;
using HIDE.RestAndRant.Web_Api.Helpers;
using HIDE.Units_Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<BL_Context>();

builder.Services.AddMemoryCache();

builder.Services.AddResponseCaching(); // регистрация служб





#region log4net
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddLog4Net("log4net.config");
});
#endregion log4net

#region Swagger - часть 1

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>(); //Подключение опций Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwaggerGen(options =>
{
    string basePath = AppContext.BaseDirectory;  //Определяет базовый путь приложения

    string xmlPath = Path.Combine(path1: basePath, path2: $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");     //Устанавливает путь к XML файлам для чтения документации
    options.IncludeXmlComments(filePath: xmlPath);

    string xmlPath2 = Path.Combine(path1: basePath, path2: BL_Context.GetAssemblyPath);     //Подключение комментария для бизнеса
    options.IncludeXmlComments(filePath: xmlPath2);

    options.EnableAnnotations(); //Включает параметр Descritions
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

#endregion Swagger - часть 1


WebApplication app = builder.Build();


#region Обработка исключений и ошибочного вызова страниц

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(Extension_ExceptionHandler.GetExceptionHandler(ReturnJsonResponse: false)); //Подключение обработчика ошибок, который пишет в логи все необработанные исключения
    });
}

app.UseStatusCodePages(Extension_StatusCodeHandle.GetStatusCodeHandler(ReturnJsonResponse: false)); // Обработка статус-кодов http кода 400 и выше, которые не имеют тела ответа (необработанные)

#endregion Обработка исключений
app.UseResponseCaching(); // добавление в конвейер

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


#region Подключаем логи

ILoggerFactory? logger_factory = app.Services.GetService<ILoggerFactory>();
if (logger_factory != null)
{
    AppLog.LoggerFactory = logger_factory;
}
ILogger<Program> log = AppLog.CreateLogger<Program>();

#endregion Подключаем логи

ConfigurationManager config = builder.Configuration; //было раньше IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

string? config_val_str;
int? config_val_int;
bool? config_val_bool;

#if !DEBUG
config_val_str = config.GetSection("ConnectionStrings")["Connection_Base"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->ConnectionStrings:Connection_Base");
}
AppConfig.ConnectionString = config_val_str;
#else
AppConfig.ConnectionString = Consts_Debug.DB_ConnectionString;
#endif

config_val_str = config.GetSection("MainSettings")["Guid_Script"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:Guid_Script");
}
AppConfig.Guid_Script = config_val_str;

config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("MainSettings")["Allow_Script"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:Allow_Script");

AppConfig.Allow_Script = config_val_bool.Value;

AppConfig.Testers_Code_ClientIDs = config.GetSection("Testers_Code_ClientIDs").Get<int[]>()?.ToList();
config_val_str = config.GetSection("Testers_Code")["Val"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Testers_Code:Val");
}
AppConfig.Testers_Code_Val = config_val_str;

config_val_int = HIDE_Convert.ToInt_Nullable(config.GetSection("MainSettings")["RangeBooking"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:RangeBooking");

AppConfig.RangeOrder = config_val_int.Value;

config_val_int = HIDE_Convert.ToInt_Nullable(config.GetSection("MainSettings")["RangeBookingVIP"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:RangeBookingVIP");

AppConfig.RangeBookingVIP = config_val_int.Value;

#region MainSettings

config_val_str = config.GetSection("MainSettings")["Excel_CommercialPersonal"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->YooKassa:Login");
}
AppConfig.Excel_CommercialPersonal = config_val_str;

#endregion MainSettings


#region Настройки SMS

config_val_str = config.GetSection("SMS_Settings")["SMS_Token"];
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

config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("SMS_Settings")["SMS_Send"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->SMS_Settings:SMS_Send");

AppConfig.SMS_Send = config_val_bool.Value;

#endregion Настройки SMS


#region Настройки почты

config_val_str = config.GetSection("Mail_Settings")["SMTP_Host"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Mail_Settings:SMTP_Host");
}
AppConfig.Mail_SMTP_Host = config_val_str;


config_val_int = HIDE_Convert.ToInt_Nullable(config.GetSection("Mail_Settings")["SMTP_Port"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Mail_Settings:SMTP_Port");

AppConfig.Mail_SMTP_Port = config_val_int.Value;


config_val_str = config.GetSection("Mail_Settings")["SMTP_User"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Mail_Settings:SMTP_User");
}
AppConfig.Mail_SMTP_User = config_val_str;

config_val_str = config.GetSection("Mail_Settings")["SMTP_Passwd"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Mail_Settings:SMTP_Passwd");
}
AppConfig.Mail_SMTP_Passwd = config_val_str;


config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("Mail_Settings")["Flag_EnableSSL"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Mail_Settings:Flag_EnableSSL");

AppConfig.Mail_SMTP_FlagEnableSSL = config_val_bool.Value;


config_val_str = config.GetSection("Mail_Settings")["Mail_From"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Mail_Settings:Mail_From");
}
AppConfig.Mail_MailFrom = config_val_str;


AppConfig.Mail_MailReply = config.GetSection("Mail_Settings")["Mail_Reply"];


config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("Mail_Settings")["Mail_Send"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Mail_Settings:Mail_Send");

AppConfig.Mail_Send = config_val_bool.Value;

#endregion Настройки почты


#region Настройки YooKassa

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

config_val_str = config.GetSection("YooKassa")["Url"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->YooKassa:Url");
}
AppConfig.Yookassa_Url = config_val_str;

#endregion Настройки YooKassa


#region Редирект на HTTPS

config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("MainSettings")["UseHttpsRedirection"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:UseHttpsRedirection");

if (config_val_bool.Value)
{
    app.UseHttpsRedirection();
}

#endregion Редирект на HTTPS


#region TelegramBot

config_val_str = config.GetSection("Telegram_Settings")["TG_Token"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Telegram_Settings:TG_Token");
}
AppConfig.TG_Token = config_val_str;

config_val_str = config.GetSection("Telegram_Settings")["TG_ProxyUrl"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Telegram_Settings:TG_ProxyUrl");
}
AppConfig.TG_ProxyUrl = config_val_str;

config_val_str = config.GetSection("Telegram_Settings")["TG_ChatID"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Telegram_Settings:TG_ChatID");
}
AppConfig.TG_ChatID = config_val_str;

config_val_str = config.GetSection("Telegram_Settings")["TG_ChatDeliveryID"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Telegram_Settings:TG_ChatDeliveryID");
}
AppConfig.TG_ChatDeliveryID = config_val_str;

config_val_str = config.GetSection("Telegram_Settings")["TG_ChatBookingID"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Telegram_Settings:TG_ChatBookingID");
}
AppConfig.TG_ChatBookingID = config_val_str;

config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("Telegram_Settings")["TG_Active"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Telegram_Settings:TG_Active");

AppConfig.TG_Active = config_val_bool.Value;

#endregion TelegramBot



#region swagger 

config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("Swagger")["Enabled"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Swagger:Enabled");

if (config_val_bool.Value)
{
    config_val_str = config.GetSection("Swagger")["Url_Prefix"];
    if (string.IsNullOrWhiteSpace(config_val_str))
    {
        log.LogCritical("Не удалось считать параметр в appsettings.json ->Swagger:Url_Prefix");

        throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->Swagger:Url_Prefix");
    }
    app.UseSwagger(option =>
    {
        option.RouteTemplate = config_val_str + "/{documentName}/swagger.json";
    });

    IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

    app.UseSwaggerUI(option =>
    {
        option.DocumentTitle = $"Api_RestAndRant";
        option.RoutePrefix = config_val_str;

        foreach (ApiVersionDescription description in descriptions)
        {
            option.SwaggerEndpoint($"/{config_val_str}/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

#endregion swagger 

app.Run();