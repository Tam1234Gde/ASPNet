using FirebaseLib.Data;
using FirebaseLib.Data.Enums;
using FirebaseLib.Data.Messaging;
using FirebaseLib.Utils;
using log4net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO.Pipelines;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;

namespace HIDE.RestAndRant.App_Notify_Booking;

/// <summary>
/// Консольное приложение - Отправка уведомлений за N-ое время
/// </summary>
class Program
{
    private static int _millisSleep;
    private static int _maxLifeHourForNotification;
    private static int _maxTryCount_RefreshToken;
    private static FirebaseMessagingUtils? _firebaseMessagingUtils;
    private static readonly MemoryCache _memoryCache = new(new MemoryCacheOptions());

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        Init(args);
        WorkAll();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ILogger log = AppLog.CreateLogger<Program>();
        log.LogCritical(exception: (Exception)e.ExceptionObject, message: "Необработанная ошибка");
    }


    /// <summary>
    /// Инициализация
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="NullReferenceException"></exception>
    private static void Init(string[] args)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureLogging(configure => configure.AddLog4Net("log4net.config"));

        IHost app = builder.Build();

        ILoggerFactory? logger_factory = app.Services.GetService<ILoggerFactory>();
        if (logger_factory != null)
        {
            AppLog.LoggerFactory = logger_factory;
        }

        IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

#if !DEBUG
        AppConfig.ConnectionString = config.GetSection("ConnectionStrings")["Connection_Base"];
        _millisSleep = config.GetValue("SleepMillis", 10_000);
#else
        AppConfig.ConnectionString = Consts_Debug.DB_ConnectionString;
        _millisSleep = 5_000;
#endif

        _maxLifeHourForNotification = config.GetSection("Notification_MaxLifeHour").Get<int?>() ?? -1;
        if (_maxLifeHourForNotification == -1)
        {
            throw new NullReferenceException("Notification_MaxLifeHour не найден");
        }

        if (_maxLifeHourForNotification < 1)
        {
            throw new NullReferenceException("Notification_MaxLifeHour не может принимать отрицательное значение");
        }

        AppConfig_FirebaseAuthParams.PrivateKey = config.GetSection("FireBase_Settings")["private_key"] ?? "";
        AppConfig_FirebaseAuthParams.ClientData = config.GetSection("FireBase_Settings")["client_email"] ?? "";
        AppConfig_FirebaseAuthParams.TokenUri = config.GetSection("FireBase_Settings")["token_uri"] ?? "";
        AppConfig_FirebaseAuthParams.ProjectId = config.GetSection("FireBase_Settings")["project_id"] ?? "";

        _maxTryCount_RefreshToken = config.GetValue("MaxTryCount_RefreshToken", 5);

        if (string.IsNullOrEmpty(AppConfig_FirebaseAuthParams.PrivateKey))
        {
            throw new NullReferenceException("PrivateKey is empty");
        }

        if (string.IsNullOrEmpty(AppConfig_FirebaseAuthParams.ClientData))
        {
            throw new NullReferenceException("ClientData is empty");
        }

        if (string.IsNullOrEmpty(AppConfig_FirebaseAuthParams.TokenUri))
        {
            throw new NullReferenceException("TokenUri is empty");
        }

        if (string.IsNullOrEmpty(AppConfig_FirebaseAuthParams.ProjectId))
        {
            throw new NullReferenceException("ProjectId is empty");
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private static FirebaseMessagingUtils GetFirebaseMessagingUtils()
    {
        if (_firebaseMessagingUtils != null)
        {
            return _firebaseMessagingUtils;
        }

        FirebaseAuthParams authParams = new()
        {
            PrivateKey = AppConfig_FirebaseAuthParams.PrivateKey,
            ClientEmail = AppConfig_FirebaseAuthParams.ClientData,
            ProjectId = AppConfig_FirebaseAuthParams.ProjectId,
            TokenUri = AppConfig_FirebaseAuthParams.TokenUri
        };
        _firebaseMessagingUtils = new FirebaseMessagingUtils(authParams);
        return _firebaseMessagingUtils;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private static string RefreshToken()
    {
        string key = AppConfig_FirebaseAuthParams.ProjectId;

        if (_memoryCache.TryGetValue(key, out string? token))
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                return token;
            }
        }

        try
        {
            int tryCount = 0;
            while (tryCount < _maxTryCount_RefreshToken)
            {
                token = GetFirebaseMessagingUtils().CreateToken();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    break;
                }

                tryCount++;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return "";
            }

            _memoryCache.Set(key, token, DateTimeOffset.Now.AddMinutes(50));
            return token;
        }
        catch (Exception)
        {
            // dummy
        }

        return "";
    }


    /// <summary>
    /// Основной метод
    /// </summary>
    private static void WorkAll()
    {
        ILogger log = AppLog.CreateLogger<Program>();

        FirebaseAuthParams authParams = new()
        {
            PrivateKey = AppConfig_FirebaseAuthParams.PrivateKey,
            ClientEmail = AppConfig_FirebaseAuthParams.ClientData,
            ProjectId = AppConfig_FirebaseAuthParams.ProjectId,
            TokenUri = AppConfig_FirebaseAuthParams.TokenUri
        };
        _firebaseMessagingUtils = new FirebaseMessagingUtils(authParams);

        while (true)
        {
            if (ContainsAppOffline())
            {
                break;
            }

            try
            {
                PushNotifications();
            }
            catch (Exception e)
            {
                log.LogError(e, "Error");
            }

            Thread.Sleep(_millisSleep);

            if (ContainsAppOffline())
            {
                break;
            }
        }

        log.LogInformation("Сервис остановлен потому что найден app_offline.htm");
    }

    private static bool ContainsAppOffline()
    {
        return File.Exists("app_offline.htm");
    }

#if DEBUG

    private static void SendTestNotification()
    {
        string oauthToken = RefreshToken();

        const string token = "dAdiv9XWQEnYtxctNlldGg:APA91bHnB38xXKeME-lg9dyvROhq_wxNLCrgRfopO8Gw4_6EuA5af61ZNbsmBujNunU2ypZojl72_LZXvxPwgCcDY6R1VlnpK9yKifjfiZ8Dsikf7oUluUE";
        GetFirebaseMessagingUtils().SendMessageToUser(oauthToken, token, "Test notification", "Test notification body");
    }

#endif


    /// <summary>
    /// Создание уведомлений и отправка пушей
    /// </summary>
    private static void PushNotifications()
    {
        ILogger log = AppLog.CreateLogger<Program>();

        Client_Notifications clientNotifications = new();
        Client_Notification_Tokens clientNotificationTokens = new();

#if DEBUG
        SendTestNotification();
#endif

        // Создание уведомлений для отправки
        List<CClient_Notification>? notifications = clientNotifications.AddNotifications(_maxLifeHourForNotification);
        if (notifications != null && notifications.Count == 0 || notifications == null)
        {
            log.LogInformation("Нет уведомлений для отправки");
            return;
        }

        string listClientId = string.Join(',', notifications.Select(p => p.ClientID).ToList());
        if (log.IsEnabled(LogLevel.Information))
        {
            log.LogInformation("Найдено {} для отправки, clientIdList: [{}]", notifications.Count, listClientId);
        }


        foreach (CClient_Notification notification in notifications)
        {
            string token = RefreshToken();
            if (string.IsNullOrWhiteSpace(token))
            {
                log.LogError("Ошибка создания токена");

                continue;
            }

            if (notification.ClientID.HasValue)
            {
                List<CClient_Notification_Token> tokensToSend = clientNotificationTokens.GetList_ByClientId(notification.ClientID.Value);
                if (tokensToSend.Count == 0)
                {
                    if (log.IsEnabled(LogLevel.Information))
                    {
                        log.LogInformation("Нет токенов для отправки, ClientID: {}", notification.ClientID);
                    }

                    continue;
                }

                int sentCount = 0;
                foreach (CClient_Notification_Token notificationToken in tokensToSend)
                {
                    try
                    {
                        SendResult? result = GetFirebaseMessagingUtils().SendMessageToUser(token, notificationToken.Token, notification.Name, notification.Content);
                        if (result == null)
                        {
                            continue;
                        }

                        if (result.FirebaseError == FirebaseErrors.None)
                        {
                            sentCount++;
                        }
                    }
                    catch (Exception e)
                    {
                        log.LogError(e, "Error");
                    }
                }

                int changeSentResult = clientNotifications.MarkAs_Sent(notification.ID);
                if (changeSentResult < 0)
                {
                    if (log.IsEnabled(LogLevel.Critical))
                    {
                        log.LogCritical("Ошибка изменения даты отправки уведомления ID: {}", notification.ID);
                    }
                }

                if (log.IsEnabled(LogLevel.Information))
                {
                    log.LogInformation("Успешно отправлено {} уведомлений", sentCount);
                }
            }
        }
    }
}