using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Установки
    /// </summary>
    public class Setup_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Setup_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        private IQueryable<Setup> GetQuery()
        {
            IQueryable<Setup> query = from p in _db.Setups.AsNoTracking() select p;
            return query;
        }

        /// <summary>
        /// Получение по Key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public Setup? Get(string Key)
        {
            Setup? elem = (from p in _db.Setups where p.Key == Key.Trim() select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <returns></returns>
        public IQueryable<Setup> GetQueryAll()
        {
            IQueryable<Setup> query = from p in GetQuery() select p;

            return query;
        }

        /// <summary>
        /// Изменение флага удаления
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Val_New"></param>
        /// <returns></returns>
        public bool Edit(string Key, string? Val_New)
        {
            string loginfo = $"Key='{Key}' Val_New='{Val_New}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Setup? elem = Get(Key: Key);

            if (elem == null)
            {
                Setup item = new()
                {
                    Key = Key,
                    Val = Val_New?.Trim()
                };

                _db.Setups.Add(item);
            }
            else
            {
                elem.Val = Val_New?.Trim();
            }

            try
            {
                _db.SaveChanges();

                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return true;
            }
            catch (Exception ex)
            {

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return false;
            }
        }

        /// <summary>
        /// Изменение в админке
        /// </summary>
        /// <param name="Key">Ключ</param>
        /// <param name="Val">Значение 1</param>
        /// <param name="Val_Comment">Значение 1 - Комментарий</param>
        /// <param name="Val2">Значение 2</param>
        /// <param name="Val2_Comment">Значение 2 - Комментарий</param>
        /// <param name="Val3">Значение 3</param>
        /// <param name="Val3_Comment">Значение 3 - Комментарий</param>
        /// <param name="Val4">Значение 4</param>
        /// <param name="Val4_Comment">Значение 4 - Комментарий</param>
        /// <returns></returns>
        public bool Edit_ForAdmin(string Key, string? Val, string? Val_Comment, string? Val2, string? Val2_Comment, string? Val3, string? Val3_Comment, string? Val4,
                                  string? Val4_Comment)
        {
            string loginfo = $"Key='{Key}' Val='{Val}' Val_Comment='{Val_Comment}' Val2='{Val2}' Val2_Comment='{Val2_Comment}' " +
                             $"Val3='{Val3}' Val3_Comment='{Val3_Comment}' Val4='{Val4}' Val4_Comment='{Val4_Comment}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Setup? elem = Get(Key: Key);

            if (elem == null)
            {
                Setup item = new()
                {
                    Key = Key,
                    Val = Val,
                    Val_Comment = Val_Comment,
                    Val2 = Val2,
                    Val2_Comment = Val2_Comment,
                    Val3 = Val3,
                    Val3_Comment = Val3_Comment,
                    Val4 = Val4,
                    Val4_Comment = Val4_Comment
                };

                _db.Setups.Add(item);
            }
            else
            {
                elem.Val = Val;
                elem.Val_Comment = Val_Comment;
                elem.Val2 = Val2;
                elem.Val2_Comment = Val2_Comment;
                elem.Val3 = Val3;
                elem.Val3_Comment = Val3_Comment;
                elem.Val4 = Val4;
                elem.Val4_Comment = Val4_Comment;
            }

            try
            {
                _db.SaveChanges();

                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }

                return true;
            }
            catch (Exception ex)
            {

                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return false;
            }
        }

    }
}