using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
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
    options.Cookie.MaxAge = options.ExpireTimeSpan;
    options.LoginPath = "/Auth/Login";
    options.Cookie.Name = "Web_Oper";
});
builder.Services.AddTransient<BL_Context>(); //Добавление Dependency Injection BL_Context
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + "/Temp")).SetDefaultKeyLifetime(TimeSpan.FromDays(30)); //https://stackoverflow.com/questions/75952374/asp-net-core-6-project-auto-logout-after-1020-minutes

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

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
    pattern: "{controller=Orders}/{action=Index}/{id?}");
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

int? config_val_int;

config_val_int = HIDE_Convert.ToInt_Nullable(config.GetSection("MainSettings")["RangeOrder"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:RangeOrder");

AppConfig.RangeOrder = config_val_int.Value;

config_val_int = HIDE_Convert.ToInt_Nullable(config.GetSection("MainSettings")["SystemClient"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:SystemClient");

AppConfig.SystemClient = config_val_int.Value;

config_val_int = HIDE_Convert.ToInt_Nullable(config.GetSection("MainSettings")["Restaurant_Default"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:Restaurant_Default");

AppConfig.Restaurant_Default = config_val_int.Value;

#region Редирект на HTTPS

bool? config_val_bool = HIDE_Convert.ToBool_Nullable(config.GetSection("MainSettings")["UseHttpsRedirection"]) ?? throw new NullReferenceException("Не удалось считать параметр в appsettings.json ->MainSettings:UseHttpsRedirection");

if (config_val_bool.Value)
{
    app.UseHttpsRedirection();
}

#endregion Редирект на HTTPS

app.Run();