using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using HIDE.FirebaseLib.Data;
using HIDE.FirebaseLib.Data.Enums;
using HIDE.FirebaseLib.Data.Messaging;
using HIDE.FirebaseLib.Utils;
using HIDE.RestAndRant.App_Notify.Models;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.Models;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.App_Notify;

class Program
{
    private static int _millisSleep;
    private static int _maxLifeHourForNotification;
    private static int _maxTryCount_RefreshToken;

    private static FirebaseMessagingUtils? _firebaseMessagingUtils;
    private static readonly Client_Notifications_Planned _bl_Client_Notifications_Planned = new();
    private static readonly Client_Notifications _bl_Client_Notifications = new();

    private static readonly MemoryCache _memoryCache = new(new MemoryCacheOptions());

    static void Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        Init(args);

        WorkAll();
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ILogger log = AppLog.CreateLogger<Program>();

        log.LogCritical(exception: (Exception)e.ExceptionObject, message: "Необработанная ошибка");
    }

    private static void Init(string[] args)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureLogging(builder => builder.AddLog4Net("log4net.config"));

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

        AppConfig.Allowed_Activity_Days_Notification = config.GetSection("Allowed_Activity_Days_Notification").Get<int?>() ?? -1;
        if (AppConfig.Allowed_Activity_Days_Notification == -1)
        {
            throw new NullReferenceException("Allowed_Activity_Days_Notification не найден");
        }

        if (AppConfig.Allowed_Activity_Days_Notification < 1)
        {
            throw new NullReferenceException("Allowed_Activity_Days_Notification не может принимать отрицательное значение");
        }

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

    private static void PushNotifications()
    {
        ILogger log = AppLog.CreateLogger<Program>();

        Client_Notifications clientNotifications = new();
        Client_Notification_Tokens clientNotificationTokens = new();

        //Модуль отправки отложенных уведомлений
        int? planned_Result = _bl_Client_Notifications_Planned.Send();

        if (!planned_Result.HasValue)
        {
            if (log.IsEnabled(LogLevel.Information))
            {
                log.LogInformation(message: "{}", args: "Не найдено отложенных уведомлений для отправки");
            }
        }
        else if (planned_Result.Value < 0)
        {
            log.LogError(message: "{}", args: "Ошибка при отправке отложенных уведомлений");
        }

#if DEBUG
        SendTestNotification();
#endif

        List<CClient_Notification> notifications = clientNotifications.GetList_NeedPush(_maxLifeHourForNotification);
        if (notifications.Count == 0)
        {
            log.LogInformation("Нет уведомлений для отправки");
            return;
        }

        string listClientId = string.Join(',', notifications.Select(p => p.ClientID).ToList());
        if (log.IsEnabled(LogLevel.Information))
        {
            log.LogInformation("Найдено {} для отправки, clientIdList: [{}]", notifications.Count, listClientId);
        }

        List<string> sended = [];

        foreach (CClient_Notification notification in notifications)
        {
            string token = RefreshToken();
            if (string.IsNullOrWhiteSpace(token))
            {
                log.LogError("Ошибка создания токена");

                continue;
            }

            if (!notification.ClientID.HasValue)
            {
                //TODO: MAE - то что снизу закончено? или еще идет процесс?
                //Это готовый метод, то - просто мысли
                _bl_Client_Notifications.MarkAs_Sent(ID: notification.ID); //Отмечаем как отправленное, TODO: FUTURE - думаю регать ошибку в базе полем (создать и использовать Enum с ошибками)
                continue;
            }


            //Получение токенов для отправки пушей
            List<CClient_Notification_Token> tokensToSend = [.. clientNotificationTokens.GetList_ByClientId(notification.ClientID.Value).Distinct()];
            if (tokensToSend.Count == 0)
            {
                if (log.IsEnabled(LogLevel.Information))
                {
                    log.LogInformation("Нет токенов для отправки, ClientID: {}", notification.ClientID);
                }
                _bl_Client_Notifications.MarkAs_Sent(ID: notification.ID); //Отмечаем как отправленное, TODO: FUTURE - думаю регать ошибку в базе полем (создать и использовать Enum с ошибками)
                continue;
            }

            //Убираем дубли
            List<CClient_Notification_Token> distinctTokens = [.. tokensToSend.DistinctBy(x => x.Token)];

            

            int sentCount = 0;
            foreach (CClient_Notification_Token notificationToken in distinctTokens)
            {
                try
                {

                    if (!sended.Contains(notificationToken.Token))
                    {
                        SendResult? result = GetFirebaseMessagingUtils().SendMessageToUser(token, notificationToken.Token, notification.Name, notification.Content);

                        sended.Add(notificationToken.Token);

                        log.LogInformation("Отправлено, ClientID: {}, token: {}", notification.ClientID, notificationToken.Token);


                        if (result == null)
                        {
                            continue;
                        }


                        switch (result.FirebaseError)
                        {
                            case FirebaseErrors.None:
                                sentCount++;
                                break;
                        }

                        //int stateUpdate_result = UpdateTokenStats(TokenID: notificationToken.ID, State: result.FirebaseError);
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
                if (log.IsEnabled(logLevel: LogLevel.Critical))
                {
                    log.LogCritical(message: "{}", args: $"Ошибка изменения даты отправки уведомления ID: {notification.ID}");
                }
                //TODO: MAE - если changeSentResult < 0 только логи? или еще что-то нужно сделать?
                //Думаю только логи, потому что если не смогли изменить, то это ошибка записи в БД, можно конечно пробовать повторно изменить.
            }

            if (log.IsEnabled(logLevel: LogLevel.Information))
            {
                log.LogInformation("Успешно отправлено {} уведомлений", sentCount);
            }
        }

        sended = [];

    }


    /// <summary>
    /// Обновление статистики отправлений
    /// </summary>
    /// <returns></returns>
    //public static int UpdateTokenStats(int TokenID, FirebaseErrors State)
    //{
    //    int? Client_Notification_Stats_ID = _bl_Client_Notification_Statss.GetID_ByToken(TokenID: TokenID);

    //    if(Client_Notification_Stats_ID.HasValue && Client_Notification_Stats_ID > 0)
    //    {
    //        switch (State)
    //        {
    //            case FirebaseErrors.AlreadyExists:
    //                return _bl_Client_Notification_Statss.Edit_AddCounts(TokenID: TokenID, AlreadyExists: true);

    //            case FirebaseErrors.PermissionDenied:
    //                return _bl_Client_Notification_Statss.Edit_AddCounts(TokenID: TokenID, PermissionDenied: true);

    //            case FirebaseErrors.Unauthenticated:
    //                return _bl_Client_Notification_Statss.Edit_AddCounts(TokenID: TokenID, Unauthenticated: true);

    //            case FirebaseErrors.InvalidArgument:
    //                return _bl_Client_Notification_Statss.Edit_AddCounts(TokenID: TokenID, InvalidArgument: true);

    //            case FirebaseErrors.InvalidError:
    //                return _bl_Client_Notification_Statss.Edit_AddCounts(TokenID: TokenID, InvalidError: true);

    //            case FirebaseErrors.None:
    //                return _bl_Client_Notification_Statss.Edit_AddCounts(TokenID: TokenID, None: true);
    //        }
    //    }
    //    else
    //    {
    //        switch (State)
    //        {
    //            case FirebaseErrors.AlreadyExists:
    //                return _bl_Client_Notification_Statss.Add(TokenID: TokenID, AlreadyExists: true);

    //            case FirebaseErrors.PermissionDenied:
    //                return _bl_Client_Notification_Statss.Add(TokenID: TokenID, PermissionDenied: true);

    //            case FirebaseErrors.Unauthenticated:
    //                return _bl_Client_Notification_Statss.Add(TokenID: TokenID, Unauthenticated: true);

    //            case FirebaseErrors.InvalidArgument:
    //                return _bl_Client_Notification_Statss.Add(TokenID: TokenID, InvalidArgument: true);

    //            case FirebaseErrors.InvalidError:
    //                return _bl_Client_Notification_Statss.Add(TokenID: TokenID, InvalidError: true);

    //            case FirebaseErrors.None:
    //                return _bl_Client_Notification_Statss.Add(TokenID: TokenID, None: true);
    //        }
    //    }

    //    return 0;
    //}
}