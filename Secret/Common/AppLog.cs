using Microsoft.Extensions.Logging;

namespace HIDE.RestAndRant.Common
{
    /// <summary>
    /// Класс для хранения логгера приложения
    /// </summary>
    public static class AppLog
    {
        public static ILoggerFactory LoggerFactory { get; set; } = null!;


        public static ILogger<T> CreateLogger<T>() => LoggerFactory.CreateLogger<T>();


        public static ILogger CreateLogger(Type type) => LoggerFactory.CreateLogger(type);
    }
}