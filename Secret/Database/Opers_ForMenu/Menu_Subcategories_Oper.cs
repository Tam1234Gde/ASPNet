using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions_ForMenu;

namespace HIDE.RestAndRant.Database.Opers_ForMenu
{
    public class Menu_Subcategories_Oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        public Menu_Subcategories_Oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        public IQueryable<Menu_Subcategory_Def> GetQuery()
        {
            return from p in _db.Set<Menu_Subcategory_Def>().Include(x => x.Menu_Category).Include(x => x.Parent).AsNoTracking()
                   where p.DeletedAt == null
                   orderby p.OrderNum, p.Name
                   select p;
        }

        public IQueryable<Menu_Subcategory_Def> GetQuery_Edit()
        {
            return from p in _db.Set<Menu_Subcategory_Def>().Include(x => x.Menu_Category).Include(x => x.Parent)
                   where p.DeletedAt == null
                   orderby p.OrderNum, p.Name
                   select p;
        }

        public IQueryable<Menu_Subcategory_Def> GetQuery_Deleted()
        {
            return from p in _db.Set<Menu_Subcategory_Def>().Include(x => x.Menu_Category).Include(x => x.Parent).AsNoTracking()
                   where p.DeletedAt != null
                   orderby p.DeletedAt descending
                   select p;
        }

        public Menu_Subcategory_Def? GetById(int ID, bool includeDeleted = false)
        {
            IQueryable<Menu_Subcategory_Def> query = _db.Set<Menu_Subcategory_Def>().Include(x => x.Menu_Category).Include(x => x.Parent);
            if (!includeDeleted)
                query = query.Where(p => p.DeletedAt == null);
            return query.FirstOrDefault(p => p.ID == ID);
        }

        public Menu_Subcategory_Def? Get(int ID) => GetById(ID, false);
        public Menu_Subcategory_Def? GetEdit(int ID) => GetById(ID, false);

        public IQueryable<Menu_Subcategory_Def> GetQuery_ByMenu_CategoryID(int Menu_CategoryID)
        {
            return from p in GetQuery() where p.Menu_CategoryID == Menu_CategoryID select p;
        }

        public IQueryable<Menu_Subcategory_Def> GetQuery_Deleted_ByMenu_CategoryID(int Menu_CategoryID)
        {
            return from p in GetQuery_Deleted() where p.Menu_CategoryID == Menu_CategoryID select p;
        }

        public int Add(string Name, int Menu_CategoryID, int? ParentID, int OrderNum)
        {
            string loginfo = $"Name='{Name}' Menu_CategoryID='{Menu_CategoryID}' ParentID='{ParentID}' OrderNum='{OrderNum}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            if (OrderNum <= 0)
            {
                int? maxOrder = _db.Set<Menu_Subcategory_Def>()
                    .Where(x => x.Menu_CategoryID == Menu_CategoryID && x.DeletedAt == null)
                    .Max(x => (int?)x.OrderNum);
                OrderNum = (maxOrder ?? 0) + 10;
            }

            Menu_Subcategory_Def elem = new Menu_Subcategory_Def
            {
                Name = Name,
                Menu_CategoryID = Menu_CategoryID,
                ParentID = ParentID,
                OrderNum = OrderNum,
                DeletedAt = null
            };

            _db.Set<Menu_Subcategory_Def>().Add(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(exception: ex, message: "Err # Error='{0}' ## {1}", args: new object[] { Errors.Err202_str, loginfo });
                return Errors.Err202;
            }
        }

        public int Edit(int ID, string Name, int Menu_CategoryID, int? ParentID, int OrderNum)
        {
            string loginfo = $"ID='{ID}' Name='{Name}' Menu_CategoryID='{Menu_CategoryID}' ParentID='{ParentID}' OrderNum='{OrderNum}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Subcategory_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }

            elem.Name = Name;
            elem.Menu_CategoryID = Menu_CategoryID;
            elem.ParentID = ParentID;
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
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}", args: new object[] { Errors.Err203_str, loginfo });
                return Errors.Err203;
            }
        }

        public int SoftDelete(int ID)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Subcategory_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
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
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}", args: new object[] { Errors.Err203_str, loginfo });
                return Errors.Err203;
            }
        }

        public int Restore(int ID)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Subcategory_Def? elem = GetById(ID, true);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
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
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}", args: new object[] { Errors.Err203_str, loginfo });
                return Errors.Err203;
            }
        }

        public int HardDelete(int ID)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Subcategory_Def? elem = GetById(ID, true);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }

            _db.Set<Menu_Subcategory_Def>().Remove(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                    _log.LogCritical(exception: ex, message: "Err # {0} ## {1}", args: new object[] { Errors.Err203_str, loginfo });
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Массовая перестановка порядка подкатегорий.
        /// Принимает список ID в новом порядке и проставляет OrderNum = 10, 20, 30, ...
        /// </summary>
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
                List<Menu_Subcategory_Def> items = _db.Set<Menu_Subcategory_Def>()
                    .Where(x => orderedIds.Contains(x.ID) && x.DeletedAt == null)
                    .ToList();

                Dictionary<int, Menu_Subcategory_Def> map = items.ToDictionary(x => x.ID, x => x);

                int order = 10;
                int updated = 0;

                foreach (int id in orderedIds)
                {
                    if (map.TryGetValue(id, out Menu_Subcategory_Def? item))
                    {
                        item.OrderNum = order;
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



    }
}