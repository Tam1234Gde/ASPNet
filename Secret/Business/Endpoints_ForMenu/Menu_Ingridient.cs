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
    public class Menu_Ingridient
    {
        private readonly ILogger _log;
        private readonly Menu_Ingridient_Oper _dl_Menu_Ingridient = new();
        private readonly Setups _bl_Setup = new();

        public Menu_Ingridient()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        private static CMenu_Ingridient? Get(Menu_Ingridient_Def? Elem)
        {
            if (Elem == null) return null;

            return new CMenu_Ingridient
            {
                ID = Elem.ID,
                Name = Elem.Name,
                Image = Elem.Image,
                Image_ChangeDate = Elem.Image_ChangeDate,
                Price = Elem.Price,
                Flag_Price_Overridden = Elem.Flag_Price_Overridden,
                Menu_Ingridient_GroupID = Elem.Menu_Ingridient_GroupID,
                OrderNum = Elem.OrderNum,
                Menu_OptionStateID = Elem.Menu_OptionStateID
            };
        }

        public CMenu_Ingridient? Get(int ID)
        {
            return Get(Elem: _dl_Menu_Ingridient.GetById(ID, false));
        }

        public List<CMenu_Ingridient> GetList()
        {
            List<CMenu_Ingridient> listResult = new List<CMenu_Ingridient>();
            foreach (Menu_Ingridient_Def elem in _dl_Menu_Ingridient.GetQuery().ToList())
            {
                CMenu_Ingridient? item = Get(Elem: elem);
                if (item != null) listResult.Add(item);
            }
            return listResult;
        }

        public List<CMenu_Ingridient> GetList_ByMenu_Ingridient_GroupID(int Menu_Ingridient_GroupID)
        {
            List<CMenu_Ingridient> listResult = new List<CMenu_Ingridient>();
            foreach (Menu_Ingridient_Def elem in _dl_Menu_Ingridient.GetQuery_ByMenu_Ingridient_GroupID(Menu_Ingridient_GroupID: Menu_Ingridient_GroupID).ToList())
            {
                CMenu_Ingridient? item = Get(Elem: elem);
                if (item != null) listResult.Add(item);
            }
            return listResult;
        }

        public List<CMenu_Ingridient> GetList_Deleted_ByMenu_Ingridient_GroupID(int Menu_Ingridient_GroupID)
        {
            List<CMenu_Ingridient> listResult = new List<CMenu_Ingridient>();
            foreach (Menu_Ingridient_Def elem in _dl_Menu_Ingridient.GetQuery_Deleted_ByMenu_Ingridient_GroupID(Menu_Ingridient_GroupID: Menu_Ingridient_GroupID).ToList())
            {
                CMenu_Ingridient? item = Get(Elem: elem);
                if (item != null) listResult.Add(item);
            }
            return listResult;
        }

        public int Add(string Name, byte[]? Image, int? Price, int Menu_Ingridient_GroupID, int Menu_OptionStateID, int OrderNum = 0)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient.Add(
                Name: Name,
                Image: Image,
                Price: Price,
                Menu_Ingridient_GroupID: Menu_Ingridient_GroupID,
                Menu_OptionStateID: Menu_OptionStateID,
                OrderNum: OrderNum);
        }

        public int Edit(int ID, string Name, byte[]? Image, int? Price, int Menu_Ingridient_GroupID, int Menu_OptionStateID, int OrderNum = 0)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient.Edit(
                ID: ID,
                Name: Name,
                Image: Image,
                Price: Price,
                Menu_Ingridient_GroupID: Menu_Ingridient_GroupID,
                Menu_OptionStateID: Menu_OptionStateID,
                OrderNum: OrderNum);
        }

        public int ApplyPriceToGroup(int GroupID, int? Price, bool SkipOverridden)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient.ApplyPriceToGroup(GroupID: GroupID, Price: Price, SkipOverridden: SkipOverridden);
        }

        public int Delete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient.SoftDelete(ID: ID);
        }

        public int Restore(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient.Restore(ID: ID);
        }

        public int HardDelete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient.HardDelete(ID: ID);
        }

        public int EditImage(int ID, byte[] Image)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient.Edit_Image(ID: ID, Image: Image);
        }

        public int DeleteImage(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient.Del_Image(ID: ID);
        }


        /// <summary>
        /// Массовая перестановка порядка ингредиентов.
        /// </summary>
        public int Reorder(List<int> orderedIds)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Ingridient.Reorder(orderedIds: orderedIds);
        }
    }
}