using Microsoft.AspNetCore.Diagnostics;
using HIDE.RestAndRant.Common;

namespace HIDE.RestAndRant.Web_Api.Extensions
{
    /// <summary>
    /// Глобальный обработчик исключений для всего приложения
    /// </summary>
    public static class Extension_StatusCodeHandle //TODO: Применить в других наших проекта
    {
        private static readonly ILogger log = AppLog.CreateLogger(typeof(Extension_StatusCodeHandle)); //Логер


        /// <summary>
        /// Получает обработчик статус кодов для UseStatusCodePages
        /// </summary>
        /// <param name="ReturnJsonResponse">Определяет, возвращать ли JSON ответ (true) или использовать стандартный (false). Логирование работает всегда.</param>
        public static Func<StatusCodeContext, Task> GetStatusCodeHandler(bool ReturnJsonResponse = true)
        {
            if (ReturnJsonResponse)
            {
                return HandleStatusCodeWithJsonAsync;
            }
            else
            {
                return HandleStatusCodeWithoutJsonAsync;
            }
        }



        /// <summary>
        /// Обработчик статус кодов с JSON ответом
        /// </summary>
        private static async Task HandleStatusCodeWithJsonAsync(StatusCodeContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            HttpRequest request = context.HttpContext.Request;

            if (response.StatusCode >= 400)
            {
                LogStatusCode(response.StatusCode, request.Method, request.Path);

                response.ContentType = "application/json";
                await response.WriteAsJsonAsync(new
                {
                    error = GetStatusCodeTitle(response.StatusCode),
                    message = GetStatusCodeMessage(response.StatusCode),
                    path = request.Path.ToString(),
                    statusCode = response.StatusCode,
                    timestamp = DateTime.Now
                });
            }
        }


        /// <summary>
        /// Обработчик статус кодов без JSON ответа
        /// </summary>
        private static Task HandleStatusCodeWithoutJsonAsync(StatusCodeContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            HttpRequest request = context.HttpContext.Request;

            if (response.StatusCode >= 400)
            {
                LogStatusCode(response.StatusCode, request.Method, request.Path);
            }

            return Task.CompletedTask;
        }



        /// <summary>
        /// Логирует HTTP статус код
        /// </summary>
        private static void LogStatusCode(int StatusCode, string Method, PathString Path)
        {
            if (log.IsEnabled(LogLevel.Warning))
            {
                log.LogWarning(message: "{}", args: $"HTTP {StatusCode}: {Method} {Path}");
            }
        }


        private static string GetStatusCodeTitle(int statusCode)
        {
            string result = statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                405 => "Method Not Allowed",
                _ => $"HTTP Error {statusCode}"
            };

            return result;
        }


        private static string GetStatusCodeMessage(int statusCode)
        {

            string result = statusCode switch
            {

                400 => "Некорректный запрос",
                401 => "Требуется авторизация",
                403 => "Доступ запрещен",
                404 => "Ресурс не найден",
                405 => "Метод не разрешен",
                _ => $"HTTP ошибка {statusCode}"
            };
            return result;
        }


    }
}
