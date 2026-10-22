using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions_ForMenu;

namespace HIDE.RestAndRant.Database.Opers_ForMenu
{
    public class Menu_Restaurant_Oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        public Menu_Restaurant_Oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        public IQueryable<Menu_Restaurant_Def> GetQuery()
        {
            return from p in _db.Set<Menu_Restaurant_Def>().Include(x => x.Menu).Include(x => x.Restaurant).Include(x => x.Menu_OptionState).AsNoTracking()
                   where p.DeletedAt == null
                   select p;
        }

        public IQueryable<Menu_Restaurant_Def> GetQuery_Edit()
        {
            return from p in _db.Set<Menu_Restaurant_Def>().Include(x => x.Menu).Include(x => x.Restaurant).Include(x => x.Menu_OptionState)
                   where p.DeletedAt == null
                   select p;
        }

        public IQueryable<Menu_Restaurant_Def> GetQuery_Deleted()
        {
            return from p in _db.Set<Menu_Restaurant_Def>().Include(x => x.Menu).Include(x => x.Restaurant).Include(x => x.Menu_OptionState).AsNoTracking()
                   where p.DeletedAt != null
                   orderby p.DeletedAt descending
                   select p;
        }

        public Menu_Restaurant_Def? GetById(int ID, bool includeDeleted = false)
        {
            IQueryable<Menu_Restaurant_Def> query = _db.Set<Menu_Restaurant_Def>().Include(x => x.Menu).Include(x => x.Restaurant).Include(x => x.Menu_OptionState);
            if (!includeDeleted)
                query = query.Where(p => p.DeletedAt == null);
            return query.FirstOrDefault(p => p.ID == ID);
        }

        public Menu_Restaurant_Def? Get(int ID)
        {
            return GetById(ID, false);
        }

        public Menu_Restaurant_Def? GetEdit(int ID)
        {
            return GetById(ID, false);
        }

        public IQueryable<Menu_Restaurant_Def> GetQuery_ByMenuID(int MenuID)
        {
            return from p in GetQuery() where p.MenuID == MenuID select p;
        }

        public IQueryable<Menu_Restaurant_Def> GetQuery_Deleted_ByMenuID(int MenuID)
        {
            return from p in GetQuery_Deleted() where p.MenuID == MenuID select p;
        }

        public IQueryable<Menu_Restaurant_Def> GetQuery_ByRestaurantID(int RestaurantID)
        {
            return from p in GetQuery() where p.RestaurantID == RestaurantID select p;
        }

        public int Add(int MenuID, int RestaurantID, DateTime? DateFrom, DateTime? DateTo, int Menu_OptionStateID)
        {
            string loginfo = $"MenuID='{MenuID}' RestaurantID='{RestaurantID}' DateFrom='{DateFrom}' DateTo='{DateTo}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Restaurant_Def elem = new Menu_Restaurant_Def
            {
                MenuID = MenuID,
                RestaurantID = RestaurantID,
                DateFrom = DateFrom,
                DateTo = DateTo,
                Menu_OptionStateID = Menu_OptionStateID,
                DeletedAt = null
            };

            _db.Set<Menu_Restaurant_Def>().Add(elem);

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

        public int Edit(int ID, int MenuID, int RestaurantID, DateTime? DateFrom, DateTime? DateTo, int Menu_OptionStateID)
        {
            string loginfo = $"ID='{ID}' MenuID='{MenuID}' RestaurantID='{RestaurantID}' DateFrom='{DateFrom}' DateTo='{DateTo}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Restaurant_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                }
                return Errors.Err200;
            }

            elem.MenuID = MenuID;
            elem.RestaurantID = RestaurantID;
            elem.DateFrom = DateFrom;
            elem.DateTo = DateTo;
            elem.Menu_OptionStateID = Menu_OptionStateID;

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

        public int SoftDelete(int ID)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Restaurant_Def? elem = GetById(ID, false);
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

        public int Restore(int ID)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Restaurant_Def? elem = GetById(ID, true);
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

        public int HardDelete(int ID)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Restaurant_Def? elem = GetById(ID, true);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                }
                return Errors.Err200;
            }

            _db.Set<Menu_Restaurant_Def>().Remove(elem);

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