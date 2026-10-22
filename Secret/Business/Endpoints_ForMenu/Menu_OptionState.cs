// File: Menu_OptionState.cs
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions_ForMenu;
using HIDE.RestAndRant.Database.Opers_ForMenu;

namespace HIDE.RestAndRant.Business.Endpoints_ForMenu
{
    /// <summary>
    /// Бизнес-логика для состояний активности меню
    /// </summary>
    public class Menu_OptionState
    {
        private readonly ILogger _log;
        private readonly Menu_OptionState_Oper _dl_Menu_OptionState = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Menu_OptionState()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        /// <summary>
        /// Получение данных
        /// </summary>
        /// <param name="Elem">Объект</param>
        /// <returns></returns>
        private static CMenu_OptionState? Get(Menu_OptionState_Def? Elem)
        {
            if (Elem == null) return null;

            CMenu_OptionState item = new()
            {
                ID = Elem.ID,
                Value = Elem.Value
            };
            return item;
        }

        /// <summary>
        /// Получение данных по ID
        /// </summary>
        /// <param name="ID">Код</param>
        /// <returns></returns>
        public CMenu_OptionState? Get(int ID)
        {
            return Get(Elem: _dl_Menu_OptionState.Get(ID));
        }

        /// <summary>
        /// Получение данных по значению
        /// </summary>
        /// <param name="Value">Значение состояния</param>
        /// <returns></returns>
        public CMenu_OptionState? Get(EMenu_OptionStates Value)
        {
            return Get(Elem: _dl_Menu_OptionState.Get(Value));
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CMenu_OptionState> GetList()
        {
            List<CMenu_OptionState> list_result = [];
            foreach (Menu_OptionState_Def elem in _dl_Menu_OptionState.GetQuery().ToList())
            {
                CMenu_OptionState? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }
    }
}