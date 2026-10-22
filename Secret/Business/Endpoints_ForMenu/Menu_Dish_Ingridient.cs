// File: Menu_Dish_Ingridient.cs
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
    /// Бизнес-логика для ингредиентов блюд меню
    /// </summary>
    public class Menu_Dish_Ingridient
    {
        private readonly ILogger _log;
        private readonly Menu_Dish_Ingridient_Oper _dl_Menu_Dish_Ingridient = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Menu_Dish_Ingridient()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        /// <summary>
        /// Получение данных
        /// </summary>
        /// <param name="Elem">Объект</param>
        /// <returns></returns>
        private static CMenu_Dish_Ingridient? Get(Menu_Dish_Ingridient_Def? Elem)
        {
            if (Elem == null) return null;

            CMenu_Dish_Ingridient item = new()
            {
                ID = Elem.ID,
                Menu_DishID = Elem.Menu_DishID,
                Menu_IngridientID = Elem.Menu_IngridientID
            };
            return item;
        }

        /// <summary>
        /// Получение данных по ID
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        public CMenu_Dish_Ingridient? Get(int ID)
        {
            return Get(Elem: _dl_Menu_Dish_Ingridient.Get(ID));
        }

        /// <summary>
        /// Возвращает список по Menu_DishID
        /// </summary>
        /// <param name="Menu_DishID">Код блюда</param>
        /// <returns></returns>
        public List<CMenu_Dish_Ingridient> GetList_ByMenu_DishID(int Menu_DishID)
        {
            List<CMenu_Dish_Ingridient> list_result = [];
            foreach (Menu_Dish_Ingridient_Def elem in _dl_Menu_Dish_Ingridient.GetQuery_ByMenu_DishID(Menu_DishID: Menu_DishID).ToList())
            {
                CMenu_Dish_Ingridient? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Menu_DishID">Код блюда</param>
        /// <param name="Menu_IngridientID">Код ингредиента</param>
        /// <returns></returns>
        public int Add(int Menu_DishID, int Menu_IngridientID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Dish_Ingridient.Add(Menu_DishID: Menu_DishID, Menu_IngridientID: Menu_IngridientID);
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        public int Delete(int ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Dish_Ingridient.Delete(ID: ID);
        }

        /// <summary>
        /// Удалить все ингредиенты блюда
        /// </summary>
        /// <param name="Menu_DishID">Код блюда</param>
        /// <returns></returns>
        public bool Delete_ByMenu_DishID(int Menu_DishID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Menu_DateChange, Val_New: DateTime.Now.ToString());
            return _dl_Menu_Dish_Ingridient.Delete_ByMenu_DishID(Menu_DishID: Menu_DishID);
        }
    }
}