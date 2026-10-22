using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions_ForMenu;

namespace HIDE.RestAndRant.Database.Opers_ForMenu
{
    public class Menu_Category_Oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        public Menu_Category_Oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        public IQueryable<Menu_Category_Def> GetQuery()
        {
            return from p in _db.Set<Menu_Category_Def>().Include(x => x.Menu).AsNoTracking()
                   where p.DeletedAt == null
                   orderby p.OrderNum, p.Name
                   select p;
        }

        public IQueryable<Menu_Category_Def> GetQuery_Edit()
        {
            return from p in _db.Set<Menu_Category_Def>().Include(x => x.Menu)
                   where p.DeletedAt == null
                   orderby p.OrderNum, p.Name
                   select p;
        }

        public IQueryable<Menu_Category_Def> GetQuery_Deleted()
        {
            return from p in _db.Set<Menu_Category_Def>().Include(x => x.Menu).AsNoTracking()
                   where p.DeletedAt != null
                   orderby p.DeletedAt descending
                   select p;
        }

        public Menu_Category_Def? GetById(int ID, bool includeDeleted = false)
        {
            IQueryable<Menu_Category_Def> query = _db.Set<Menu_Category_Def>().Include(x => x.Menu);
            if (!includeDeleted)
                query = query.Where(p => p.DeletedAt == null);
            return query.FirstOrDefault(p => p.ID == ID);
        }

        public Menu_Category_Def? Get(int ID) => GetById(ID, false);
        public Menu_Category_Def? GetEdit(int ID) => GetById(ID, false);

        public IQueryable<Menu_Category_Def> GetQuery_ByMenuID(int MenuID)
        {
            return from p in GetQuery() where p.MenuID == MenuID select p;
        }

        public IQueryable<Menu_Category_Def> GetQuery_Deleted_ByMenuID(int MenuID)
        {
            return from p in GetQuery_Deleted() where p.MenuID == MenuID select p;
        }

        public int Add(string Name, int MenuID, int OrderNum)
        {
            string loginfo = $"Name='{Name}' MenuID='{MenuID}' OrderNum='{OrderNum}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            if (OrderNum <= 0)
            {
                int? maxOrder = _db.Set<Menu_Category_Def>()
                    .Where(x => x.MenuID == MenuID && x.DeletedAt == null)
                    .Max(x => (int?)x.OrderNum);
                OrderNum = (maxOrder ?? 0) + 10;
            }

            Menu_Category_Def elem = new Menu_Category_Def
            {
                Name = Name,
                MenuID = MenuID,
                OrderNum = OrderNum,
                DeletedAt = null
            };

            _db.Set<Menu_Category_Def>().Add(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(exception: ex, message: "Err # Error='{0}' ## {1}",
                        args: new object[] { Errors.Err202_str, loginfo });
                return Errors.Err202;
            }
        }

        public int Edit(int ID, string Name, int MenuID, int OrderNum)
        {
            string loginfo = $"ID='{ID}' Name='{Name}' MenuID='{MenuID}' OrderNum='{OrderNum}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Category_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}",
                        args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }

            elem.Name = Name;
            elem.MenuID = MenuID;
            if (OrderNum > 0) elem.OrderNum = OrderNum;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}",
                        args: new object[] { Errors.Err203_str, loginfo });
                return Errors.Err203;
            }
        }

        /// <summary>
        /// Массовая перестановка порядка.
        /// Принимает список ID в новом порядке и проставляет OrderNum = 10, 20, 30, ...
        /// </summary>
        /// <param name="orderedIds">ID в новом порядке (первый — наверху)</param>
        /// <returns>Количество обновлённых записей, либо отрицательный код ошибки</returns>
        public int Reorder(List<int> orderedIds)
        {
            string loginfo = $"Count='{orderedIds?.Count ?? 0}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            if (orderedIds == null || orderedIds.Count == 0)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # Пустой список ID ### {0}", args: new object[] { loginfo });
                return Errors.Err200;
            }

            try
            {
                // Загружаем все нужные категории
                List<Menu_Category_Def> categories = _db.Set<Menu_Category_Def>()
                    .Where(x => orderedIds.Contains(x.ID) && x.DeletedAt == null)
                    .ToList();

                // Строим карту ID → Entity
                Dictionary<int, Menu_Category_Def> map = categories.ToDictionary(x => x.ID, x => x);

                int order = 10;
                int updated = 0;

                foreach (int id in orderedIds)
                {
                    if (map.TryGetValue(id, out Menu_Category_Def? category))
                    {
                        category.OrderNum = order;
                        order += 10;
                        updated++;
                    }
                }

                _db.SaveChanges();

                if (_log.IsEnabled(LogLevel.Debug))
                    _log.LogDebug(message: "{}", args: $"OK # Обновлено {updated} записей ## {loginfo}");

                return updated;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}",
                        args: new object[] { Errors.Err203_str, loginfo });
                return Errors.Err203;
            }
        }

        public int SoftDelete(int ID)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Category_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}",
                        args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }

            elem.DeletedAt = DateTime.Now;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}",
                        args: new object[] { Errors.Err203_str, loginfo });
                return Errors.Err203;
            }
        }

        public int Restore(int ID)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Category_Def? elem = GetById(ID, true);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}",
                        args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }
            if (elem.DeletedAt == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # Record is not deleted ### {0}", args: new object[] { loginfo });
                return Errors.Err200;
            }

            elem.DeletedAt = null;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}",
                        args: new object[] { Errors.Err203_str, loginfo });
                return Errors.Err203;
            }
        }

        public int HardDelete(int ID)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Category_Def? elem = GetById(ID, true);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}",
                        args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }

            _db.Set<Menu_Category_Def>().Remove(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}",
                        args: new object[] { Errors.Err203_str, loginfo });
                return Errors.Err203;
            }
        }
    }
}