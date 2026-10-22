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
    public class Menu_Category
    {
        private readonly ILogger _log;
        private readonly Menu_Category_Oper _dl_Menu_Category = new();
        private readonly Setups _bl_Setup = new();

        public Menu_Category()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        private static CMenu_Category? Get(Menu_Category_Def? Elem)
        {
            if (Elem == null) return null;
            return new CMenu_Category
            {
                ID = Elem.ID,
                Name = Elem.Name,
                MenuID = Elem.MenuID,
                OrderNum = Elem.OrderNum
            };
        }

        public CMenu_Category? Get(int ID) => Get(Elem: _dl_Menu_Category.GetById(ID, false));

        public List<CMenu_Category> GetList()
        {
            List<CMenu_Category> listResult = new List<CMenu_Category>();
            foreach (Menu_Category_Def elem in _dl_Menu_Category.GetQuery().ToList())
            {
                CMenu_Category? item = Get(Elem: elem);
                if (item != null) listResult.Add(item);
            }
            return listResult;
        }

        public List<CMenu_Category> GetList_ByMenuID(int MenuID)
        {
            List<CMenu_Category> listResult = new List<CMenu_Category>();
            foreach (Menu_Category_Def elem in _dl_Menu_Category.GetQuery_ByMenuID(MenuID: MenuID).ToList())
            {
                CMenu_Category? item = Get(Elem: elem);
                if (item != null) listResult.Add(item);
            }
            return listResult;
        }

        public List<CMenu_Category> GetList_Deleted_ByMenuID(int MenuID)
        {
            List<CMenu_Category> listResult = new List<CMenu_Category>();
            foreach (Menu_Category_Def elem in _dl_Menu_Category.GetQuery_Deleted_ByMenuID(MenuID: MenuID).ToList())
            {
                CMenu_Category? item = Get(Elem: elem);
                if (item != null) listResult.Add(item);
            }
            return listResult;
        }

        public int Add(string Name, int MenuID, int OrderNum = 0)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Category.Add(Name: Name, MenuID: MenuID, OrderNum: OrderNum);
        }

        public int Edit(int ID, string Name, int MenuID, int OrderNum = 0)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Category.Edit(ID: ID, Name: Name, MenuID: MenuID, OrderNum: OrderNum);
        }

        public int Delete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Category.SoftDelete(ID: ID);
        }

        public int Restore(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Category.Restore(ID: ID);
        }

        public int HardDelete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Category.HardDelete(ID: ID);
        }

        /// <summary>
        /// Массовая перестановка порядка категорий.
        /// </summary>
        public int Reorder(List<int> orderedIds)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Category.Reorder(orderedIds: orderedIds);
        }
    }
}