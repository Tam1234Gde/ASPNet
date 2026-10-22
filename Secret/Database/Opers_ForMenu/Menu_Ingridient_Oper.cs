using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions_ForMenu;

namespace HIDE.RestAndRant.Database.Opers_ForMenu
{
    /// <summary>
    /// Класс для управления базой данных в части ингредиентов меню
    /// </summary>
    public class Menu_Ingridient_Oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        public Menu_Ingridient_Oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        public IQueryable<Menu_Ingridient_Def> GetQuery()
        {
            return from p in _db.Set<Menu_Ingridient_Def>().Include(x => x.Menu_Ingridient_Group).Include(x => x.Menu_OptionState).AsNoTracking()
                   where p.DeletedAt == null
                   orderby p.OrderNum, p.Name
                   select p;
        }

        public IQueryable<Menu_Ingridient_Def> GetQuery_Edit()
        {
            return from p in _db.Set<Menu_Ingridient_Def>().Include(x => x.Menu_Ingridient_Group).Include(x => x.Menu_OptionState)
                   where p.DeletedAt == null
                   orderby p.OrderNum, p.Name
                   select p;
        }

        public IQueryable<Menu_Ingridient_Def> GetQuery_Deleted()
        {
            return from p in _db.Set<Menu_Ingridient_Def>().Include(x => x.Menu_Ingridient_Group).Include(x => x.Menu_OptionState).AsNoTracking()
                   where p.DeletedAt != null
                   orderby p.DeletedAt descending
                   select p;
        }

        public Menu_Ingridient_Def? GetById(int ID, bool includeDeleted = false)
        {
            IQueryable<Menu_Ingridient_Def> query = _db.Set<Menu_Ingridient_Def>().Include(x => x.Menu_Ingridient_Group).Include(x => x.Menu_OptionState);
            if (!includeDeleted) query = query.Where(p => p.DeletedAt == null);
            return query.FirstOrDefault(p => p.ID == ID);
        }

        public Menu_Ingridient_Def? Get(int ID) => GetById(ID, false);
        public Menu_Ingridient_Def? GetEdit(int ID) => GetById(ID, false);

        public IQueryable<Menu_Ingridient_Def> GetQuery_ByMenu_Ingridient_GroupID(int Menu_Ingridient_GroupID)
        {
            return from p in GetQuery() where p.Menu_Ingridient_GroupID == Menu_Ingridient_GroupID select p;
        }

        public IQueryable<Menu_Ingridient_Def> GetQuery_Deleted_ByMenu_Ingridient_GroupID(int Menu_Ingridient_GroupID)
        {
            return from p in GetQuery_Deleted() where p.Menu_Ingridient_GroupID == Menu_Ingridient_GroupID select p;
        }

        public int Add(string Name, byte[]? Image, int? Price, int Menu_Ingridient_GroupID, int Menu_OptionStateID, int OrderNum)
        {
            string loginfo = $"Name='{Name}' Price='{Price}' Menu_Ingridient_GroupID='{Menu_Ingridient_GroupID}' OrderNum='{OrderNum}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            bool priceOverridden = Price.HasValue;

            if (!Price.HasValue)
            {
                Menu_Ingridient_Group_Def? group = _db.Set<Menu_Ingridient_Group_Def>()
                    .AsNoTracking()
                    .FirstOrDefault(g => g.ID == Menu_Ingridient_GroupID);

                if (group != null && group.Default_Price.HasValue)
                {
                    Price = group.Default_Price.Value;
                }
            }

            if (OrderNum <= 0)
            {
                int? maxOrder = _db.Set<Menu_Ingridient_Def>()
                    .Where(x => x.Menu_Ingridient_GroupID == Menu_Ingridient_GroupID && x.DeletedAt == null)
                    .Max(x => (int?)x.OrderNum);
                OrderNum = (maxOrder ?? 0) + 10;
            }

            Menu_Ingridient_Def elem = new Menu_Ingridient_Def
            {
                Name = Name,
                Image = Image,
                Image_ChangeDate = DateTime.Now,
                Price = Price,
                Flag_Price_Overridden = priceOverridden,
                Menu_Ingridient_GroupID = Menu_Ingridient_GroupID,
                Menu_OptionStateID = Menu_OptionStateID,
                OrderNum = OrderNum,
                DeletedAt = null
            };

            _db.Set<Menu_Ingridient_Def>().Add(elem);

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

        public int Edit(int ID, string Name, byte[]? Image, int? Price, int Menu_Ingridient_GroupID, int Menu_OptionStateID, int OrderNum)
        {
            string loginfo = $"ID='{ID}' Name='{Name}' Price='{Price}' Menu_Ingridient_GroupID='{Menu_Ingridient_GroupID}' OrderNum='{OrderNum}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Ingridient_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }

            elem.Name = Name;
            if (Image != null)
            {
                elem.Image = Image;
                elem.Image_ChangeDate = DateTime.Now;
            }
            elem.Price = Price;
            if (Price.HasValue) elem.Flag_Price_Overridden = true;
            elem.Menu_Ingridient_GroupID = Menu_Ingridient_GroupID;
            elem.Menu_OptionStateID = Menu_OptionStateID;
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

        public int ApplyPriceToGroup(int GroupID, int? Price, bool SkipOverridden)
        {
            string loginfo = $"GroupID='{GroupID}' Price='{Price}' SkipOverridden='{SkipOverridden}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            try
            {
                IQueryable<Menu_Ingridient_Def> query = _db.Set<Menu_Ingridient_Def>()
                    .Where(p => p.Menu_Ingridient_GroupID == GroupID && p.DeletedAt == null);

                if (SkipOverridden) query = query.Where(p => !p.Flag_Price_Overridden);

                var items = query.ToList();

                foreach (Menu_Ingridient_Def item in items)
                {
                    item.Price = Price;
                    item.Flag_Price_Overridden = false;
                }

                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: $"OK # Обновлено {items.Count} записей ## {loginfo}");
                return items.Count;
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

            Menu_Ingridient_Def? elem = GetById(ID, false);
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

            Menu_Ingridient_Def? elem = GetById(ID, true);
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

            Menu_Ingridient_Def? elem = GetById(ID, true);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }

            _db.Set<Menu_Ingridient_Def>().Remove(elem);

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

        public int Edit_Image(int ID, byte[] Image)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Ingridient_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }

            elem.Image = Image;
            elem.Image_ChangeDate = DateTime.Now;

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

        public int Del_Image(int ID)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Ingridient_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }

            elem.Image = null;
            elem.Image_ChangeDate = null;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(exception: ex, message: "Err # {0} ## {1}", args: new object[] { Errors.Err203_str, loginfo });
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Массовая перестановка порядка ингредиентов.
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
                List<Menu_Ingridient_Def> items = _db.Set<Menu_Ingridient_Def>()
                    .Where(x => orderedIds.Contains(x.ID) && x.DeletedAt == null)
                    .ToList();

                Dictionary<int, Menu_Ingridient_Def> map = items.ToDictionary(x => x.ID, x => x);

                int order = 10;
                int updated = 0;

                foreach (int id in orderedIds)
                {
                    if (map.TryGetValue(id, out Menu_Ingridient_Def? item))
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