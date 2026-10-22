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
    public class Menu_Dish
    {
        private readonly ILogger _log;
        private readonly Menu_Dish_Oper _dl_Menu_Dish = new();
        private readonly Setups _bl_Setup = new();

        public Menu_Dish()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        private static CMenu_Dish? Get(Menu_Dish_Def? Elem)
        {
            if (Elem == null) return null;

            CMenu_Dish item = new()
            {
                ID = Elem.ID,
                Menu_SubcategoryID = Elem.Menu_SubcategoryID,
                Name = Elem.Name,
                Teaser = Elem.Teaser,
                Text = Elem.Text,
                Cooking_Minutes = Elem.Cooking_Minutes,
                Price = Elem.Price,
                Flag_AllowColors = Elem.Flag_AllowColors,
                Flag_Bonus = Elem.Flag_Bonus,
                Bonuses = Elem.Bonuses,
                OrderNum = Elem.OrderNum,
                FlagNew = Elem.FlagNew,
                Image = Elem.Image,
                Image_ChangeDate = Elem.Image_ChangeDate,
                RecommendDishIDs = Elem.RecommendDishIDs,
                FlagNoActive = Elem.FlagNoActive,
                New_DateFrom = Elem.New_DateFrom,
                Vol1 = Elem.Vol1,
                Vol1_Price = Elem.Vol1_Price,
                Vol2 = Elem.Vol2,
                Vol2_Price = Elem.Vol2_Price,
                Vol3 = Elem.Vol3,
                Vol3_Price = Elem.Vol3_Price,
                Vol4 = Elem.Vol4,
                Vol4_Price = Elem.Vol4_Price,
                Vol5 = Elem.Vol5,
                Vol5_Price = Elem.Vol5_Price,
                Menu_OptionStateID = Elem.Menu_OptionStateID
            };
            return item;
        }

        public CMenu_Dish? Get(int ID)
        {
            return Get(Elem: _dl_Menu_Dish.GetById(ID, false));
        }

        public List<CMenu_Dish> GetList()
        {
            List<CMenu_Dish> list_result = [];
            foreach (Menu_Dish_Def elem in _dl_Menu_Dish.GetQuery().ToList())
            {
                CMenu_Dish? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }

        public List<CMenu_Dish> GetList_ByMenu_SubcategoryID(int Menu_SubcategoryID)
        {
            List<CMenu_Dish> list_result = [];
            foreach (Menu_Dish_Def elem in _dl_Menu_Dish.GetQuery_ByMenu_SubcategoryID(Menu_SubcategoryID: Menu_SubcategoryID).ToList())
            {
                CMenu_Dish? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.OrderNum).ThenBy(x => x.Name)];
        }

        public List<CMenu_Dish> GetList_Deleted_ByMenu_SubcategoryID(int Menu_SubcategoryID)
        {
            List<CMenu_Dish> list_result = [];
            foreach (Menu_Dish_Def elem in _dl_Menu_Dish.GetQuery_Deleted_ByMenu_SubcategoryID(Menu_SubcategoryID: Menu_SubcategoryID).ToList())
            {
                CMenu_Dish? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
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
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Dish.Add(
                Menu_SubcategoryID: Menu_SubcategoryID,
                Name: Name,
                Teaser: Teaser,
                Text: Text,
                Cooking_Minutes: Cooking_Minutes,
                Price: Price,
                Flag_AllowColors: Flag_AllowColors,
                Flag_Bonus: Flag_Bonus,
                Bonuses: Bonuses,
                OrderNum: OrderNum,
                FlagNew: FlagNew,
                Image: Image,
                Image_ChangeDate: Image_ChangeDate,
                RecommendDishIDs: RecommendDishIDs,
                FlagNoActive: FlagNoActive,
                New_DateFrom: New_DateFrom,
                Vol1: Vol1,
                Vol1_Price: Vol1_Price,
                Vol2: Vol2,
                Vol2_Price: Vol2_Price,
                Vol3: Vol3,
                Vol3_Price: Vol3_Price,
                Vol4: Vol4,
                Vol4_Price: Vol4_Price,
                Vol5: Vol5,
                Vol5_Price: Vol5_Price,
                Menu_OptionStateID: Menu_OptionStateID
            );
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
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Dish.Edit(
                ID: ID,
                Menu_SubcategoryID: Menu_SubcategoryID,
                Name: Name,
                Teaser: Teaser,
                Text: Text,
                Cooking_Minutes: Cooking_Minutes,
                Price: Price,
                Flag_AllowColors: Flag_AllowColors,
                Flag_Bonus: Flag_Bonus,
                Bonuses: Bonuses,
                OrderNum: OrderNum,
                FlagNew: FlagNew,
                Image: Image,
                Image_ChangeDate: Image_ChangeDate,
                RecommendDishIDs: RecommendDishIDs,
                FlagNoActive: FlagNoActive,
                New_DateFrom: New_DateFrom,
                Vol1: Vol1,
                Vol1_Price: Vol1_Price,
                Vol2: Vol2,
                Vol2_Price: Vol2_Price,
                Vol3: Vol3,
                Vol3_Price: Vol3_Price,
                Vol4: Vol4,
                Vol4_Price: Vol4_Price,
                Vol5: Vol5,
                Vol5_Price: Vol5_Price,
                Menu_OptionStateID: Menu_OptionStateID
            );
        }

        public int EditImage(int ID, byte[] Image)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Dish.Edit_Image(ID: ID, Image: Image);
        }

        public int DeleteImage(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Dish.Del_Image(ID: ID);
        }

        public int Delete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Dish.SoftDelete(ID: ID);
        }

        public int Restore(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Dish.Restore(ID: ID);
        }

        public int HardDelete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Dish.HardDelete(ID: ID);
        }


        /// <summary>
        /// Массовая перестановка порядка блюд.
        /// </summary>
        public int Reorder(List<int> orderedIds)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Dish.Reorder(orderedIds: orderedIds);
        }

    }
}