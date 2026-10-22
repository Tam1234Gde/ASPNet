// File: Menu_Restaurant_Option.cs
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
    /// Бизнес-логика для опций меню ресторана
    /// </summary>
    public class Menu_Restaurant_Option
    {
        private readonly ILogger _log;
        private readonly Menu_Restaurant_Option_Oper _dl_Menu_Restaurant_Option = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Menu_Restaurant_Option()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        /// <summary>
        /// Получение данных
        /// </summary>
        /// <param name="Elem">Объект</param>
        /// <returns></returns>
        private static CMenu_Restaurant_Option? Get(Menu_Restaurant_Option_Def? Elem)
        {
            if (Elem == null) return null;

            CMenu_Restaurant_Option item = new()
            {
                ID = Elem.ID,
                Menu_RestaurantID = Elem.Menu_RestaurantID,
                Menu_DishID = Elem.Menu_DishID,
                Menu_IngridientID = Elem.Menu_IngridientID,
                Menu_Ingridient_GroupID = Elem.Menu_Ingridient_GroupID,
                MenuID = Elem.MenuID
            };
            return item;
        }

        /// <summary>
        /// Получение данных по ID
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        public CMenu_Restaurant_Option? Get(int ID)
        {
            return Get(Elem: _dl_Menu_Restaurant_Option.Get(ID));
        }

        /// <summary>
        /// Возвращает список по Menu_RestaurantID
        /// </summary>
        /// <param name="Menu_RestaurantID">Код привязки меню к ресторану</param>
        /// <returns></returns>
        public List<CMenu_Restaurant_Option> GetList_ByMenu_RestaurantID(int Menu_RestaurantID)
        {
            List<CMenu_Restaurant_Option> list_result = [];
            foreach (Menu_Restaurant_Option_Def elem in _dl_Menu_Restaurant_Option.GetQuery_ByMenu_RestaurantID(Menu_RestaurantID: Menu_RestaurantID).ToList())
            {
                CMenu_Restaurant_Option? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

        /// <summary>
        /// Добавление опции для блюда
        /// </summary>
        /// <param name="Menu_RestaurantID">Код привязки меню к ресторану</param>
        /// <param name="Menu_DishID">Код блюда</param>
        /// <param name="MenuID">Код меню</param>
        /// <returns></returns>
        public int AddDish(int Menu_RestaurantID, int Menu_DishID, int MenuID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Restaurant_Option.Add_Dish(Menu_RestaurantID: Menu_RestaurantID, Menu_DishID: Menu_DishID,
                                                       MenuID: MenuID);
        }

        /// <summary>
        /// Добавление опции для ингредиента
        /// </summary>
        /// <param name="Menu_RestaurantID">Код привязки меню к ресторану</param>
        /// <param name="Menu_IngridientID">Код ингредиента</param>
        /// <param name="MenuID">Код меню</param>
        /// <returns></returns>
        public int AddIngridient(int Menu_RestaurantID, int Menu_IngridientID, int MenuID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Restaurant_Option.Add_Ingridient(Menu_RestaurantID: Menu_RestaurantID,
                                                             Menu_IngridientID: Menu_IngridientID, MenuID: MenuID);
        }

        /// <summary>
        /// Добавление опции для группы ингредиентов
        /// </summary>
        /// <param name="Menu_RestaurantID">Код привязки меню к ресторану</param>
        /// <param name="Menu_Ingridient_GroupID">Код группы ингредиентов</param>
        /// <param name="MenuID">Код меню</param>
        /// <returns></returns>
        public int AddIngridientGroup(int Menu_RestaurantID, int Menu_Ingridient_GroupID, int MenuID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Restaurant_Option.Add_Ingridient_Group(Menu_RestaurantID: Menu_RestaurantID,
                                                                   Menu_Ingridient_GroupID: Menu_Ingridient_GroupID,
                                                                   MenuID: MenuID);
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        public int Delete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Restaurant_Option.Delete(ID: ID);
        }

        /// <summary>
        /// Удалить все опции по Menu_RestaurantID
        /// </summary>
        /// <param name="Menu_RestaurantID">Код привязки меню к ресторану</param>
        /// <returns></returns>
        public bool Delete_ByMenu_RestaurantID(int Menu_RestaurantID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Restaurant_Option.Delete_ByMenu_RestaurantID(Menu_RestaurantID: Menu_RestaurantID);
        }
    }
}