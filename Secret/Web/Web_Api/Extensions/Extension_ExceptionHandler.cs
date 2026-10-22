using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text;
using HIDE.RestAndRant.Common;

namespace HIDE.RestAndRant.Web_Api.Extensions
{
    /// <summary>
    /// Глобальный обработчик исключений для всего приложения
    /// </summary>
    public static class Extension_ExceptionHandler //TODO: Применить в других наших проектах
    {
        private static readonly ILogger log = AppLog.CreateLogger(typeof(Extension_ExceptionHandler)); //Логер


        /// <summary>
        /// Получает обработчик исключений для UseExceptionHandler
        /// </summary>
        /// <param name="ReturnJsonResponse">Определяет, возвращать ли JSON ответ (true) или использовать стандартный (false). Логирование работает всегда.</param>
        public static RequestDelegate GetExceptionHandler(bool ReturnJsonResponse = true)
        {
            if (ReturnJsonResponse)
            {
                return HandleExceptionWithJsonAsync;
            }
            else
            {
                return HandleExceptionWithoutJsonAsync;
            }
        }


        /// <summary>
        /// Обработчик исключений без JSON ответа
        /// </summary>
        private static Task HandleExceptionWithoutJsonAsync(HttpContext context)
        {
            IExceptionHandlerPathFeature? exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            Exception? ex = exceptionFeature?.Error;

            if (ex != null)
            {
                LogException(ex, context);
            }

            return Task.CompletedTask;
        }


        /// <summary>
        /// Обработчик исключений с JSON ответом
        /// </summary>
        private static async Task HandleExceptionWithJsonAsync(HttpContext context)
        {
            IExceptionHandlerPathFeature? exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            Exception? ex = exceptionFeature?.Error;

            if (ex != null)
            {
                LogException(ex, context);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Internal Server Error",
                    message = "Произошла внутренняя ошибка сервера. Детали записаны в лог.",
                    timestamp = DateTime.Now,
                    path = context.Request.Path.ToString(),
#if DEBUG
                    details = ex.Message,
                    exceptionType = ex.GetType().Name,
                    stackTrace = ex.StackTrace,
                    innerException = GetInnerExceptionInfo(ex.InnerException)
#endif
                });
            }
        }



        /// <summary>
        /// Логирует исключение с учетом всех внутренних исключений
        /// </summary>
        private static void LogException(Exception Ex, HttpContext Context)
        {
            if (!log.IsEnabled(LogLevel.Error))
            {
                return;
            }

            StringBuilder logMessage = new();
            logMessage.AppendLine($"Необработанное исключение в запросе {Context.Request.Method} {Context.Request.Path}");
            logMessage.AppendLine();

            // Логируем цепочку исключений
            Exception? currentException = Ex;
            int level = 0;

            while (currentException != null)
            {
                string prefix = level == 0 ? "Исключение" : $"Inner Exception (уровень {level})";

                logMessage.AppendLine($"{prefix}:");
                logMessage.AppendLine($"  Тип: {currentException.GetType().Name}");
                logMessage.AppendLine($"  Сообщение: {currentException.Message}");

                if (!string.IsNullOrWhiteSpace(currentException.StackTrace))
                {
                    logMessage.AppendLine($"  StackTrace:");
                    logMessage.AppendLine(currentException.StackTrace);
                }

                currentException = currentException.InnerException;
                level++;
            }

            if (log.IsEnabled(logLevel: LogLevel.Error))
            {
                log.LogError(exception: Ex, message: "{}", args: logMessage.ToString());
            }
        }


        /// <summary>
        /// Получает информацию о внутренних исключениях для DEBUG режима
        /// </summary>
        private static List<object>? GetInnerExceptionInfo(Exception? innerException)
        {
            if (innerException == null)
            {
                return null;
            }

            List<object> innerExceptions = [];
            Exception? currentException = innerException;
            int loglevel = 1;

            while (currentException != null && loglevel <= 5) // Ограничение глубины вложенности
            {
                innerExceptions.Add(new
                {
                    level = loglevel,
                    exceptionType = currentException.GetType().Name,
                    message = currentException.Message,
                    stackTrace = currentException.StackTrace
                });

                currentException = currentException.InnerException;
                loglevel++;
            }

            return innerExceptions;
        }

    }
}
