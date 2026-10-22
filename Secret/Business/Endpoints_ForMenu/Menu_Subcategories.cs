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
    public class Menu_Subcategories
    {
        private readonly ILogger _log;
        private readonly Menu_Subcategories_Oper _dl_Menu_Subcategories = new();
        private readonly Setups _bl_Setup = new();

        public Menu_Subcategories()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        private static CMenu_Subcategory? Get(Menu_Subcategory_Def? Elem)
        {
            if (Elem == null) return null;

            return new CMenu_Subcategory
            {
                ID = Elem.ID,
                Name = Elem.Name,
                Menu_CategoryID = Elem.Menu_CategoryID,
                ParentID = Elem.ParentID,
                OrderNum = Elem.OrderNum
            };
        }

        public CMenu_Subcategory? Get(int ID)
        {
            return Get(Elem: _dl_Menu_Subcategories.GetById(ID, false));
        }

        public List<CMenu_Subcategory> GetList()
        {
            List<CMenu_Subcategory> list_result = [];
            foreach (Menu_Subcategory_Def elem in _dl_Menu_Subcategories.GetQuery().ToList())
            {
                CMenu_Subcategory? item = Get(Elem: elem);
                if (item != null) list_result.Add(item);
            }
            return list_result;
        }

        public List<CMenu_Subcategory> GetList_ByMenu_CategoryID(int Menu_CategoryID)
        {
            List<CMenu_Subcategory> list_result = [];
            foreach (Menu_Subcategory_Def elem in _dl_Menu_Subcategories.GetQuery_ByMenu_CategoryID(Menu_CategoryID: Menu_CategoryID).ToList())
            {
                CMenu_Subcategory? item = Get(Elem: elem);
                if (item != null) list_result.Add(item);
            }
            return list_result;
        }

        public List<CMenu_Subcategory> GetList_Deleted_ByMenu_CategoryID(int Menu_CategoryID)
        {
            List<CMenu_Subcategory> list_result = [];
            foreach (Menu_Subcategory_Def elem in _dl_Menu_Subcategories.GetQuery_Deleted_ByMenu_CategoryID(Menu_CategoryID: Menu_CategoryID).ToList())
            {
                CMenu_Subcategory? item = Get(Elem: elem);
                if (item != null) list_result.Add(item);
            }
            return list_result;
        }

        public int Add(string Name, int Menu_CategoryID, int? ParentID = null, int OrderNum = 0)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Subcategories.Add(
                Name: Name,
                Menu_CategoryID: Menu_CategoryID,
                ParentID: ParentID,
                OrderNum: OrderNum);
        }

        public int Edit(int ID, string Name, int Menu_CategoryID, int? ParentID = null, int OrderNum = 0)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Subcategories.Edit(
                ID: ID,
                Name: Name,
                Menu_CategoryID: Menu_CategoryID,
                ParentID: ParentID,
                OrderNum: OrderNum);
        }

        public int Delete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Subcategories.SoftDelete(ID: ID);
        }

        public int Restore(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Subcategories.Restore(ID: ID);
        }

        public int HardDelete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Subcategories.HardDelete(ID: ID);
        }


        /// <summary>
        /// Массовая перестановка порядка подкатегорий.
        /// </summary>
        public int Reorder(List<int> orderedIds)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Subcategories.Reorder(orderedIds: orderedIds);
        }
    }
}