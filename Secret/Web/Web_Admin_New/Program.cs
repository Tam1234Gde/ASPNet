using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.Units_Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(builder => builder.AddLog4Net("log4net.config"));
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.Cookie.Name = "Web_Admin_ForMenu";
});
builder.Services.AddTransient<BL_Context>(); //Добавление Dependency Injection BL_Context
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + "/Temp")).SetDefaultKeyLifetime(TimeSpan.FromDays(30)); //https://stackoverflow.com/questions/75952374/asp-net-core-6-project-auto-logout-after-1020-minutes

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = int.MaxValue;
});
WebApplication app = builder.Build();

IWebHostEnvironment env = app.Environment;

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

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

#if DEBUG
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Debug}/{action=Index}/{id?}");
#else
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
#endif


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


#region MainSettings

config_val_str = config.GetSection("MainSettings")["Excel_CommercialPersonal"];
if (string.IsNullOrWhiteSpace(config_val_str))
{
    throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->YooKassa:Login");
}
AppConfig.Excel_CommercialPersonal = config_val_str;

#endregion MainSettings


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

bool? config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("MainSettings")["UseHttpsRedirection"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:UseHttpsRedirection");

if (config_val_bool.Value)
{
    app.UseHttpsRedirection();
}

#endregion Редирект на HTTPS



app.Run();
