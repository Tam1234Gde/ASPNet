using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions_ForMenu;

namespace HIDE.RestAndRant.Database.Opers_ForMenu
{
    /// <summary>
    /// Класс для управления базой данных в части меню
    /// </summary>
    public class Menu_Oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        public Menu_Oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Получение списка активных записей (не удалённых)
        /// </summary>
        public IQueryable<Menu_Def> GetQuery()
        {
            IQueryable<Menu_Def> query = from p in _db.Set<Menu_Def>().Include(x => x.Menu_OptionState).AsNoTracking()
                                         where p.DeletedAt == null
                                         orderby p.Name
                                         select p;
            return query;
        }

        /// <summary>
        /// Получение списка активных записей для редактирования (с отслеживанием)
        /// </summary>
        public IQueryable<Menu_Def> GetQuery_Edit()
        {
            IQueryable<Menu_Def> query = from p in _db.Set<Menu_Def>().Include(x => x.Menu_OptionState)
                                         where p.DeletedAt == null
                                         orderby p.Name
                                         select p;
            return query;
        }

        /// <summary>
        /// Получение списка удалённых записей
        /// </summary>
        public IQueryable<Menu_Def> GetQuery_Deleted()
        {
            IQueryable<Menu_Def> query = from p in _db.Set<Menu_Def>().Include(x => x.Menu_OptionState).AsNoTracking()
                                         where p.DeletedAt != null
                                         orderby p.DeletedAt descending
                                         select p;
            return query;
        }

        /// <summary>
        /// Получение записи по ID с возможностью включить удалённые
        /// </summary>
        public Menu_Def? GetById(int ID, bool includeDeleted = false)
        {
            IQueryable<Menu_Def> query = _db.Set<Menu_Def>().Include(x => x.Menu_OptionState);
            if (!includeDeleted)
                query = query.Where(p => p.DeletedAt == null);
            return query.FirstOrDefault(p => p.ID == ID);
        }

        /// <summary>
        /// Получение активной записи по ID (для совместимости)
        /// </summary>
        public Menu_Def? Get(int ID)
        {
            return GetById(ID, false);
        }

        /// <summary>
        /// Получение записи для редактирования (активной)
        /// </summary>
        public Menu_Def? GetEdit(int ID)
        {
            return GetById(ID, false);
        }

        /// <summary>
        /// Добавление новой записи
        /// </summary>
        public int Add(string Name, int Menu_OptionStateID)
        {
            string loginfo = $"Name='{Name}' Menu_OptionStateID='{Menu_OptionStateID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Def elem = new Menu_Def
            {
                Name = Name,
                Menu_OptionStateID = Menu_OptionStateID,
                DeletedAt = null
            };

            _db.Set<Menu_Def>().Add(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(exception: ex, message: "Err # Error='{0}' ## {1}", args: new object[] { Errors.Err202_str, loginfo });
                }
                return Errors.Err202;
            }
        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        public int Edit(int ID, string Name, int Menu_OptionStateID)
        {
            string loginfo = $"ID='{ID}' Name='{Name}' Menu_OptionStateID='{Menu_OptionStateID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                }
                return Errors.Err200;
            }

            elem.Name = Name;
            elem.Menu_OptionStateID = Menu_OptionStateID;
            // DeletedAt не меняется, остаётся NULL

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}", args: new object[] { Errors.Err203_str, loginfo });
                }
                return Errors.Err203;
            }
        }

        /// <summary>
        /// Мягкое удаление записи (установка DeletedAt)
        /// </summary>
        public int SoftDelete(int ID)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                }
                return Errors.Err200;
            }

            elem.DeletedAt = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}", args: new object[] { Errors.Err203_str, loginfo });
                }
                return Errors.Err203;
            }
        }

        /// <summary>
        /// Восстановление удалённой записи (сброс DeletedAt в NULL)
        /// </summary>
        public int Restore(int ID)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Def? elem = GetById(ID, true);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                }
                return Errors.Err200;
            }

            if (elem.DeletedAt == null)
            {
                // Запись не была удалена, ошибка
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # Record is not deleted ### {0}", args: new object[] { loginfo });
                }
                return Errors.Err200;
            }

            elem.DeletedAt = null;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}", args: new object[] { Errors.Err203_str, loginfo });
                }
                return Errors.Err203;
            }
        }

        /// <summary>
        /// Физическое удаление записи (вызывать с осторожностью)
        /// </summary>
        public int HardDelete(int ID)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Def? elem = GetById(ID, true);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                }
                return Errors.Err200;
            }

            _db.Set<Menu_Def>().Remove(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}", args: new object[] { Errors.Err203_str, loginfo });
                }
                return Errors.Err203;
            }
        }
    }
}