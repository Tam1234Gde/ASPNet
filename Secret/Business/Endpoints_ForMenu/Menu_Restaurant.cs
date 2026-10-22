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
    public class Menu_Restaurant
    {
        private readonly ILogger _log;
        private readonly Menu_Restaurant_Oper _dl_Menu_Restaurant = new();
        private readonly Setups _bl_Setup = new();

        public Menu_Restaurant()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        private static CMenu_Restaurant? Get(Menu_Restaurant_Def? Elem)
        {
            if (Elem == null) return null;

            CMenu_Restaurant item = new()
            {
                ID = Elem.ID,
                MenuID = Elem.MenuID,
                RestaurantID = Elem.RestaurantID,
                DateFrom = Elem.DateFrom,
                DateTo = Elem.DateTo,
                Menu_OptionStateID = Elem.Menu_OptionStateID
            };
            return item;
        }

        public CMenu_Restaurant? Get(int ID)
        {
            return Get(Elem: _dl_Menu_Restaurant.GetById(ID, false));
        }

        public List<CMenu_Restaurant> GetList()
        {
            List<CMenu_Restaurant> list_result = new List<CMenu_Restaurant>();
            foreach (Menu_Restaurant_Def elem in _dl_Menu_Restaurant.GetQuery().ToList())
            {
                CMenu_Restaurant? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

        public List<CMenu_Restaurant> GetList_ByMenuID(int MenuID)
        {
            List<CMenu_Restaurant> list_result = new List<CMenu_Restaurant>();
            foreach (Menu_Restaurant_Def elem in _dl_Menu_Restaurant.GetQuery_ByMenuID(MenuID: MenuID).ToList())
            {
                CMenu_Restaurant? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

        public List<CMenu_Restaurant> GetList_Deleted_ByMenuID(int MenuID)
        {
            List<CMenu_Restaurant> list_result = new List<CMenu_Restaurant>();
            foreach (Menu_Restaurant_Def elem in _dl_Menu_Restaurant.GetQuery_Deleted_ByMenuID(MenuID: MenuID).ToList())
            {
                CMenu_Restaurant? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

        public List<CMenu_Restaurant> GetList_ByRestaurantID(int RestaurantID)
        {
            List<CMenu_Restaurant> list_result = new List<CMenu_Restaurant>();
            foreach (Menu_Restaurant_Def elem in _dl_Menu_Restaurant.GetQuery_ByRestaurantID(RestaurantID: RestaurantID).ToList())
            {
                CMenu_Restaurant? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

        public int Add(int MenuID, int RestaurantID, DateTime? DateFrom, DateTime? DateTo, int Menu_OptionStateID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Restaurant.Add(MenuID: MenuID, RestaurantID: RestaurantID, DateFrom: DateFrom,
                                           DateTo: DateTo, Menu_OptionStateID: Menu_OptionStateID);
        }

        public int Edit(int ID, int MenuID, int RestaurantID, DateTime? DateFrom, DateTime? DateTo, int Menu_OptionStateID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Restaurant.Edit(ID: ID, MenuID: MenuID, RestaurantID: RestaurantID, DateFrom: DateFrom,
                                            DateTo: DateTo, Menu_OptionStateID: Menu_OptionStateID);
        }

        public int Delete(int ID)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Restaurant.SoftDelete(ID: ID);
        }

        public int Restore(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Restaurant.Restore(ID: ID);
        }

        public int HardDelete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Restaurant.HardDelete(ID: ID);
        }
    }
}