using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions_ForMenu;

namespace HIDE.RestAndRant.Database.Opers_ForMenu
{
    public class Menu_Dish_Oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        public Menu_Dish_Oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        public IQueryable<Menu_Dish_Def> GetQuery()
        {
            return from p in _db.Set<Menu_Dish_Def>().Include(x => x.Menu_Subcategory).Include(x => x.Menu_OptionState).AsNoTracking()
                   where p.DeletedAt == null
                   orderby p.OrderNum, p.Name
                   select p;
        }

        public IQueryable<Menu_Dish_Def> GetQuery_Edit()
        {
            return from p in _db.Set<Menu_Dish_Def>().Include(x => x.Menu_Subcategory).Include(x => x.Menu_OptionState)
                   where p.DeletedAt == null
                   orderby p.OrderNum, p.Name
                   select p;
        }

        public IQueryable<Menu_Dish_Def> GetQuery_Deleted()
        {
            return from p in _db.Set<Menu_Dish_Def>().Include(x => x.Menu_Subcategory).Include(x => x.Menu_OptionState).AsNoTracking()
                   where p.DeletedAt != null
                   orderby p.DeletedAt descending
                   select p;
        }

        public Menu_Dish_Def? GetById(int ID, bool includeDeleted = false)
        {
            IQueryable<Menu_Dish_Def> query = _db.Set<Menu_Dish_Def>().Include(x => x.Menu_Subcategory).Include(x => x.Menu_OptionState);
            if (!includeDeleted)
                query = query.Where(p => p.DeletedAt == null);
            return query.FirstOrDefault(p => p.ID == ID);
        }

        public Menu_Dish_Def? Get(int ID)
        {
            return GetById(ID, false);
        }

        public Menu_Dish_Def? GetEdit(int ID)
        {
            return GetById(ID, false);
        }

        public IQueryable<Menu_Dish_Def> GetQuery_ByMenu_SubcategoryID(int Menu_SubcategoryID)
        {
            return from p in GetQuery() where p.Menu_SubcategoryID == Menu_SubcategoryID select p;
        }

        public IQueryable<Menu_Dish_Def> GetQuery_Deleted_ByMenu_SubcategoryID(int Menu_SubcategoryID)
        {
            return from p in GetQuery_Deleted() where p.Menu_SubcategoryID == Menu_SubcategoryID select p;
        }

        public int Add(
            int Menu_SubcategoryID,
            string Name,
            string? Teaser,
            string? Text,
            int? Cooking_Minutes,
            decimal Price,
            bool Flag_AllowColors,
            bool Flag_Bonus,
            decimal? Bonuses,
            int OrderNum,
            bool FlagNew,
            byte[]? Image,
            DateTime? Image_ChangeDate,
            string? RecommendDishIDs,
            bool FlagNoActive,
            DateTime? New_DateFrom,
            int? Vol1,
            int? Vol1_Price,
            int? Vol2,
            int? Vol2_Price,
            int? Vol3,
            int? Vol3_Price,
            int? Vol4,
            int? Vol4_Price,
            int? Vol5,
            int? Vol5_Price,
            int Menu_OptionStateID)
        {
            string loginfo = $"Menu_SubcategoryID='{Menu_SubcategoryID}' Name='{Name}' Price='{Price}' OrderNum='{OrderNum}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Dish_Def elem = new Menu_Dish_Def
            {
                Menu_SubcategoryID = Menu_SubcategoryID,
                Name = Name,
                Teaser = Teaser,
                Text = Text,
                Cooking_Minutes = Cooking_Minutes,
                Price = Price,
                Flag_AllowColors = Flag_AllowColors,
                Flag_Bonus = Flag_Bonus,
                Bonuses = Bonuses,
                OrderNum = OrderNum,
                FlagNew = FlagNew,
                Image = Image,
                Image_ChangeDate = Image_ChangeDate,
                RecommendDishIDs = RecommendDishIDs,
                FlagNoActive = FlagNoActive,
                New_DateFrom = New_DateFrom,
                Vol1 = Vol1,
                Vol1_Price = Vol1_Price,
                Vol2 = Vol2,
                Vol2_Price = Vol2_Price,
                Vol3 = Vol3,
                Vol3_Price = Vol3_Price,
                Vol4 = Vol4,
                Vol4_Price = Vol4_Price,
                Vol5 = Vol5,
                Vol5_Price = Vol5_Price,
                Menu_OptionStateID = Menu_OptionStateID,
                DeletedAt = null
            };

            _db.Set<Menu_Dish_Def>().Add(elem);

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

        public int Edit(
            int ID,
            int Menu_SubcategoryID,
            string Name,
            string? Teaser,
            string? Text,
            int? Cooking_Minutes,
            decimal Price,
            bool Flag_AllowColors,
            bool Flag_Bonus,
            decimal? Bonuses,
            int OrderNum,
            bool FlagNew,
            byte[]? Image,
            DateTime? Image_ChangeDate,
            string? RecommendDishIDs,
            bool FlagNoActive,
            DateTime? New_DateFrom,
            int? Vol1,
            int? Vol1_Price,
            int? Vol2,
            int? Vol2_Price,
            int? Vol3,
            int? Vol3_Price,
            int? Vol4,
            int? Vol4_Price,
            int? Vol5,
            int? Vol5_Price,
            int Menu_OptionStateID)
        {
            string loginfo = $"ID='{ID}' Menu_SubcategoryID='{Menu_SubcategoryID}' Name='{Name}' Price='{Price}' OrderNum='{OrderNum}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Dish_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                }
                return Errors.Err200;
            }

            elem.Menu_SubcategoryID = Menu_SubcategoryID;
            elem.Name = Name;
            elem.Teaser = Teaser;
            elem.Text = Text;
            elem.Cooking_Minutes = Cooking_Minutes;
            elem.Price = Price;
            elem.Flag_AllowColors = Flag_AllowColors;
            elem.Flag_Bonus = Flag_Bonus;
            elem.Bonuses = Bonuses;
            elem.OrderNum = OrderNum;
            elem.FlagNew = FlagNew;
            if (Image != null)
            {
                elem.Image = Image;
                elem.Image_ChangeDate = DateTime.Now;
            }
            elem.RecommendDishIDs = RecommendDishIDs;
            elem.FlagNoActive = FlagNoActive;
            elem.New_DateFrom = New_DateFrom;
            elem.Vol1 = Vol1;
            elem.Vol1_Price = Vol1_Price;
            elem.Vol2 = Vol2;
            elem.Vol2_Price = Vol2_Price;
            elem.Vol3 = Vol3;
            elem.Vol3_Price = Vol3_Price;
            elem.Vol4 = Vol4;
            elem.Vol4_Price = Vol4_Price;
            elem.Vol5 = Vol5;
            elem.Vol5_Price = Vol5_Price;
            elem.Menu_OptionStateID = Menu_OptionStateID;
            // DeletedAt остаётся NULL

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
        /// Массовая перестановка порядка блюд.
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
                List<Menu_Dish_Def> items = _db.Set<Menu_Dish_Def>()
                    .Where(x => orderedIds.Contains(x.ID) && x.DeletedAt == null)
                    .ToList();

                Dictionary<int, Menu_Dish_Def> map = items.ToDictionary(x => x.ID, x => x);

                int order = 10;
                int updated = 0;

                foreach (int id in orderedIds)
                {
                    if (map.TryGetValue(id, out Menu_Dish_Def? item))
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


        public int SoftDelete(int ID)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Dish_Def? elem = GetById(ID, false);
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

            Menu_Dish_Def? elem = GetById(ID, true);
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

            Menu_Dish_Def? elem = GetById(ID, true);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                }
                return Errors.Err200;
            }

            _db.Set<Menu_Dish_Def>().Remove(elem);

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

        public int Edit_Image(int ID, byte[] Image)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Dish_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                }
                return Errors.Err200;
            }

            elem.Image = Image;
            elem.Image_ChangeDate = DateTime.Now;

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

        public int Del_Image(int ID)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_Dish_Def? elem = GetById(ID, false);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                }
                return Errors.Err200;
            }

            elem.Image = null;
            elem.Image_ChangeDate = null;

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
                    _log.LogError(exception: ex, message: "Err # {0} ## {1}", args: new object[] { Errors.Err203_str, loginfo });
                }
                return Errors.Err203;
            }
        }
    }
}