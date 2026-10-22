// File: Database/Opers_ForMenu/Menu_Ingridient_Group_Oper.cs
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
    /// Класс для управления базой данных в части групп ингредиентов меню
    /// </summary>
    public class Menu_Ingridient_Group_Oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        public Menu_Ingridient_Group_Oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        public IQueryable<Menu_Ingridient_Group_Def> GetQuery()
        {
            return from p in _db.Set<Menu_Ingridient_Group_Def>().Include(x => x.Menu_OptionState).AsNoTracking()
                   orderby p.OrderNum, p.Name
                   select p;
        }

        public IQueryable<Menu_Ingridient_Group_Def> GetQuery_Edit()
        {
            return from p in _db.Set<Menu_Ingridient_Group_Def>().Include(x => x.Menu_OptionState)
                   orderby p.OrderNum, p.Name
                   select p;
        }

        public Menu_Ingridient_Group_Def? Get(int ID)
        {
            return (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();
        }

        public Menu_Ingridient_Group_Def? GetEdit(int ID)
        {
            return (from p in GetQuery_Edit() where p.ID == ID select p).FirstOrDefault();
        }

        public int Add(string Name, bool IsSelectable, int Menu_OptionStateID, int? Default_Price, int OrderNum)
        {
            string loginfo = $"Name='{Name}' IsSelectable='{IsSelectable}' Menu_OptionStateID='{Menu_OptionStateID}' Default_Price='{Default_Price}' OrderNum='{OrderNum}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            if (OrderNum <= 0)
            {
                int? maxOrder = _db.Set<Menu_Ingridient_Group_Def>()
                    .Max(x => (int?)x.OrderNum);
                OrderNum = (maxOrder ?? 0) + 10;
            }

            Menu_Ingridient_Group_Def elem = new Menu_Ingridient_Group_Def
            {
                Name = Name,
                IsSelectable = IsSelectable,
                Menu_OptionStateID = Menu_OptionStateID,
                Default_Price = Default_Price,
                OrderNum = OrderNum
            };

            _db.Set<Menu_Ingridient_Group_Def>().Add(elem);

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

        public int Edit(int ID, string Name, bool IsSelectable, int Menu_OptionStateID, int? Default_Price, int OrderNum)
        {
            string loginfo = $"ID='{ID}' Name='{Name}' IsSelectable='{IsSelectable}' Menu_OptionStateID='{Menu_OptionStateID}' Default_Price='{Default_Price}' OrderNum='{OrderNum}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Ingridient_Group_Def? elem = GetEdit(ID: ID);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}", args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }

            elem.Name = Name;
            elem.IsSelectable = IsSelectable;
            elem.Menu_OptionStateID = Menu_OptionStateID;
            elem.Default_Price = Default_Price;
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

        /// <summary>
        /// Удаление группы ингредиентов.
        /// </summary>
        /// <param name="ID">Код группы</param>
        /// <param name="MigrateToGroupID">
        /// Код группы, в которую переносятся ингредиенты.
        /// NULL — ингредиенты помечаются удалёнными (soft-delete) вместе с группой.
        /// </param>
        public int Delete(int ID, int? MigrateToGroupID = null)
        {
            string loginfo = $"ID='{ID}' MigrateToGroupID='{MigrateToGroupID}'";
            if (_log.IsEnabled(LogLevel.Debug)) _log.LogDebug(message: "{}", args: loginfo);

            Menu_Ingridient_Group_Def? elem = GetEdit(ID: ID);
            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                    _log.LogError(message: "Err # ErrorCode={0} ~ Error={1} ### {2}",
                        args: new object[] { Errors.Err200, Errors.Err200_str, loginfo });
                return Errors.Err200;
            }

            List<Menu_Ingridient_Def> ingredients = _db.Set<Menu_Ingridient_Def>()
                .Where(x => x.Menu_Ingridient_GroupID == ID)
                .ToList();

            if (MigrateToGroupID.HasValue && MigrateToGroupID.Value > 0)
            {
                Menu_Ingridient_Group_Def? targetGroup = _db.Set<Menu_Ingridient_Group_Def>()
                    .AsNoTracking()
                    .FirstOrDefault(g => g.ID == MigrateToGroupID.Value);

                if (targetGroup == null || targetGroup.ID == ID)
                {
                    if (_log.IsEnabled(LogLevel.Error))
                        _log.LogError(message: "Err # Целевая группа невалидна ### {0}", args: new object[] { loginfo });
                    return Errors.Err200;
                }

                foreach (Menu_Ingridient_Def ing in ingredients)
                    ing.Menu_Ingridient_GroupID = MigrateToGroupID.Value;
            }
            else
            {
                if (ingredients.Count > 0)
                {
                    List<int> ingredientIds = ingredients.Select(x => x.ID).ToList();

                    // 1. Удаляем связи ингредиентов с блюдами (FK constraint)
                    List<Menu_Dish_Ingridient_Def> links = _db.Set<Menu_Dish_Ingridient_Def>()
                        .Where(x => ingredientIds.Contains(x.Menu_IngridientID))
                        .ToList();

                    if (links.Count > 0)
                        _db.Set<Menu_Dish_Ingridient_Def>().RemoveRange(links);

                    // 2. Удаляем сами ингредиенты (чтобы EF не ругался на severed relationship)
                    _db.Set<Menu_Ingridient_Def>().RemoveRange(ingredients);
                }
            }

            _db.Set<Menu_Ingridient_Group_Def>().Remove(elem);

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
        /// Массовая перестановка порядка групп ингредиентов.
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
                List<Menu_Ingridient_Group_Def> items = _db.Set<Menu_Ingridient_Group_Def>()
                    .Where(x => orderedIds.Contains(x.ID))
                    .ToList();

                Dictionary<int, Menu_Ingridient_Group_Def> map = items.ToDictionary(x => x.ID, x => x);

                int order = 10;
                int updated = 0;

                foreach (int id in orderedIds)
                {
                    if (map.TryGetValue(id, out Menu_Ingridient_Group_Def? item))
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