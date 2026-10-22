// File: Business/Endpoints_ForMenu/Menu_Ingridient_Group.cs
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions_ForMenu;
using HIDE.RestAndRant.Database.Opers_ForMenu;

namespace HIDE.RestAndRant.Business.Endpoints_ForMenu
{
    /// <summary>
    /// Бизнес-логика для групп ингредиентов
    /// </summary>
    public class Menu_Ingridient_Group
    {
        private readonly ILogger _log;
        private readonly Menu_Ingridient_Group_Oper _dl_Menu_Ingridient_Group = new Menu_Ingridient_Group_Oper();
        private readonly Setups _bl_Setup = new Setups();

        public Menu_Ingridient_Group()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        private static CMenu_Ingridient_Group? Get(Menu_Ingridient_Group_Def? Elem)
        {
            if (Elem == null) return null;

            return new CMenu_Ingridient_Group
            {
                ID = Elem.ID,
                Name = Elem.Name,
                IsSelectable = Elem.IsSelectable,
                Default_Price = Elem.Default_Price,
                OrderNum = Elem.OrderNum,
                Menu_OptionStateID = Elem.Menu_OptionStateID
            };
        }

        public CMenu_Ingridient_Group? Get(int ID)
        {
            return Get(Elem: _dl_Menu_Ingridient_Group.Get(ID));
        }

        public List<CMenu_Ingridient_Group> GetList()
        {
            List<CMenu_Ingridient_Group> listResult = new List<CMenu_Ingridient_Group>();
            foreach (Menu_Ingridient_Group_Def elem in _dl_Menu_Ingridient_Group.GetQuery().ToList())
            {
                CMenu_Ingridient_Group? item = Get(Elem: elem);
                if (item != null) listResult.Add(item);
            }
            return listResult;
        }

        public int Add(string Name, bool IsSelectable, int Menu_OptionStateID, int? Default_Price, int OrderNum = 0)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient_Group.Add(
                Name: Name,
                IsSelectable: IsSelectable,
                Menu_OptionStateID: Menu_OptionStateID,
                Default_Price: Default_Price,
                OrderNum: OrderNum);
        }

        public int Edit(int ID, string Name, bool IsSelectable, int Menu_OptionStateID, int? Default_Price, int OrderNum = 0)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient_Group.Edit(
                ID: ID,
                Name: Name,
                IsSelectable: IsSelectable,
                Menu_OptionStateID: Menu_OptionStateID,
                Default_Price: Default_Price,
                OrderNum: OrderNum);
        }

        public int Delete(int ID, int? MigrateToGroupID = null)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient_Group.Delete(ID: ID, MigrateToGroupID: MigrateToGroupID);
        }


        /// <summary>
        /// Массовая перестановка порядка групп ингредиентов.
        /// </summary>
        public int Reorder(List<int> orderedIds)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient_Group.Reorder(orderedIds: orderedIds);
        }
    }
}