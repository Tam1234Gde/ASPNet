using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Helpers;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Business.Endpoints
{

    /// <summary>
    /// Класс реализующий сервисные функции
    /// </summary>
    public class ZDeveloper
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly DBContext _DB_Context = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public ZDeveloper()
        {
            _log = AppLog.CreateLogger(GetType());
        }


        /// <summary>
        /// Получение названия последней миграции
        /// </summary>
        /// <returns></returns>
        public async Task<string?> GetLastMigration()
        {
            IEnumerable<string> appliedMigration = await _DB_Context.Database.GetAppliedMigrationsAsync();

            return appliedMigration.ToList().LastOrDefault()?.ToString();
        }
    }
}
