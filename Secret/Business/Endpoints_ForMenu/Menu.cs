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
    /// Бизнес-логика для меню
    /// </summary>
    public class Menu
    {
        private readonly ILogger _log;
        private readonly Menu_Oper _dl_Menu = new();
        private readonly Setups _bl_Setup = new();

        public Menu()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        private static CMenu? Get(Menu_Def? Elem)
        {
            if (Elem == null) return null;

            CMenu item = new()
            {
                ID = Elem.ID,
                Name = Elem.Name,
                Menu_OptionStateID = Elem.Menu_OptionStateID
            };
            return item;
        }

        public CMenu? Get(int ID)
        {
            return Get(Elem: _dl_Menu.GetById(ID, false));
        }

        public List<CMenu> GetList()
        {
            List<CMenu> list_result = [];
            foreach (Menu_Def elem in _dl_Menu.GetQuery().ToList())
            {
                CMenu? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Name)];
        }

        public List<CMenu> GetList_Deleted()
        {
            List<CMenu> list_result = [];
            foreach (Menu_Def elem in _dl_Menu.GetQuery_Deleted().ToList())
            {
                CMenu? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

        public int Add(string Name, int Menu_OptionStateID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu.Add(Name: Name, Menu_OptionStateID: Menu_OptionStateID);
        }

        public int Edit(int ID, string Name, int Menu_OptionStateID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu.Edit(ID: ID, Name: Name, Menu_OptionStateID: Menu_OptionStateID);
        }

        /// <summary>
        /// Мягкое удаление меню (установка DeletedAt)
        /// </summary>
        public int Delete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu.SoftDelete(ID: ID);
        }

        /// <summary>
        /// Восстановление удалённого меню
        /// </summary>
        public int Restore(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu.Restore(ID: ID);
        }

        /// <summary>
        /// Физическое удаление (использовать с осторожностью)
        /// </summary>
        public int HardDelete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu.HardDelete(ID: ID);
        }
    }
}