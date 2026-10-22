// File: Menu_OptionState_Oper.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Database.Definitions_ForMenu;

namespace HIDE.RestAndRant.Database.Opers_ForMenu
{
    /// <summary>
    /// Класс для управления базой данных в части состояний активности меню
    /// </summary>
    public class Menu_OptionState_Oper
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;

        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Menu_OptionState_Oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Получение списка
        /// </summary>
        /// <returns></returns>
        public IQueryable<Menu_OptionState_Def> GetQuery()
        {
            IQueryable<Menu_OptionState_Def> query = from p in _db.Set<Menu_OptionState_Def>().AsNoTracking()
                                                 orderby p.ID
                                                 select p;

            return query;
        }

        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Menu_OptionState_Def? Get(int ID)
        {
            Menu_OptionState_Def? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение по значению
        /// </summary>
        /// <param name="Value">Значение состояния</param>
        /// <returns></returns>
        public Menu_OptionState_Def? Get(EMenu_OptionStates Value)
        {
            Menu_OptionState_Def? elem = (from p in GetQuery() where p.Value == Value select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Value">Значение состояния</param>
        /// <returns></returns>
        public int Add(EMenu_OptionStates Value)
        {
            string loginfo = $"Value='{Value}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Menu_OptionState_Def elem = new()
            {
                Value = Value
            };

            _db.Set<Menu_OptionState_Def>().Add(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "{}", args: $"OK # {loginfo}");
                }
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(exception: ex, message: "Err # Error='{0}' ## {1}", args: [Errors.Err202_str, loginfo]);
                }
                return Errors.Err202;
            }
        }
    }
}