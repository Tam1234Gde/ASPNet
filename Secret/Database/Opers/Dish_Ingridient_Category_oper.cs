using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{

    /// <summary>
    /// Класс управления БД категорий ингридиентов.
    /// </summary>
    public class Dish_Ingridient_Category_oper
    {

        /// <summary>
        /// Логер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст Базы данных
        /// </summary>
        private readonly DBContext _db = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Dish_Ingridient_Category_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Dish_Ingridient_Category? Get(int ID, bool? Flag_Del_val = false)
        {
            Dish_Ingridient_Category? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Dish_Ingridient_Category? GetEdit(int ID, bool? Flag_Del_val = false)
        {
            Dish_Ingridient_Category? elem = (from p in GetQuery_Edit(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Dish_Ingridient_Category> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Dish_Ingridient_Category> query = from p in _db.Dish_Ingridient_Categories.AsNoTracking() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Dish_Ingridient_Category> GetQuery_Edit(bool? Flag_Del_val = false)
        {
            IQueryable<Dish_Ingridient_Category> query = from p in _db.Dish_Ingridient_Categories select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public int Add(string Name)
        {
            string loginfo = $"Name='{Name}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Ingridient_Category elem = new()
            {
                Name = Name,
            };

            _db.Dish_Ingridient_Categories.Add(elem);
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
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err202_str}' ## {loginfo}");
                }
                return Errors.Err202;
            }
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public int Edit(int ID, string Name_New)
        {
            string loginfo = $"ID='{ID}' Name_New='{Name_New}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Ingridient_Category? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Name = Name_New;

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
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(exception: ex, message: "{}", args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }
                return Errors.Err203;
            }
        }

        /// <summary>
        /// Изменение пометки на удаление
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Flag_Del_New">Удалено</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del_New, int UserID, bool? Flag_Del_val = false)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Dish_Ingridient_Category? elem = GetEdit(ID: ID, Flag_Del_val: Flag_Del_val);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Flag_Del = Flag_Del_New;
            elem.Date_Del = DateTime.Now;
            elem.UserID_Del = UserID;

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
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", exception: ex, args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }
                return Errors.Err203;
            }
        }

    }
}
