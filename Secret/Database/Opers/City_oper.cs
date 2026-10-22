using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Города
    /// </summary>
    public class City_oper
    {
        /// <summary>
        /// Логер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Констекст БД 
        /// </summary>
        private readonly DBContext _db;


        /// <summary>
        /// Конструктор
        /// </summary>
        public City_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="Flag_Del_val">Флаг удаленности</param>
        /// <returns></returns>
        public IQueryable<City> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<City> query = from p in _db.Cities.AsNoTracking() orderby p.OrderNum select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение списка для редактирования
        /// </summary>
        /// <param name="Flag_Del_val">Флаг удаленности</param>
        /// <returns></returns>
        public IQueryable<City> GetQuery_Edit(bool? Flag_Del_val = false)
        {
            IQueryable<City> query = from p in _db.Cities orderby p.OrderNum select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public City? Get(int ID, bool? Flag_Del_val = false)
        {
            City? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по RestaurantID
        /// </summary>
        /// <param name="RestaurantID">Идентификатор ресторана</param>
        /// <returns></returns>
        public City? GetByRestaurantID(int RestaurantID)
        {
            Restaurant? elem = (from p in _db.Restaurants.Include(x => x.City).AsNoTracking() where p.ID == RestaurantID select p).FirstOrDefault();
            return elem?.City;
        }


        /// <summary>
        /// Получение по ID для редактирования
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns></returns>
        private City? Get_Edit(int ID, bool? Flag_Del_val = false)
        {
            City? elem = (from p in GetQuery_Edit(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Name">Название города</param>
        /// <param name="NameDeclension">Название города(склонение)</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <returns></returns>
        public int Add(string Name, string? NameDeclension, int OrderNum, bool Flag_Hidden, string? HiddenTitle, string? HiddenText)
        {
            string loginfo = $"Name='{Name}' OrderNum='{OrderNum}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
           
            City elem = new()
            {
                Name = Name,
                NameDeclension = NameDeclension,
                OrderNum = OrderNum,
                Flag_Hidden = Flag_Hidden,
                HiddenTitle = HiddenTitle,
                HiddenText = HiddenText,
            };

            _db.Cities.Add(elem);

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
                _log.LogError(exception: ex, "Err # ErrorCode='{}' ## {}", Errors.Err202_str, loginfo);
                return Errors.Err202;
            }
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="Name">Название города</param>
        /// <param name="NameDeclension">Название города(склонение)</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <returns></returns>
        public int Edit(int ID, string Name, string? NameDeclension, int OrderNum, bool Flag_Hidden, string? HiddenTitle, string? HiddenText)
        {
            string loginfo = $"ID='{ID}' Name='{Name}' NameDeclension='{NameDeclension}' OrderNum='{OrderNum}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            

            City? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError("Err # ErrorCode='{}' ## {}", Errors.Err200_str, loginfo);
                return Errors.Err200;
            }

            elem.Name = Name;
            elem.NameDeclension = NameDeclension;
            elem.OrderNum = OrderNum;
            elem.Flag_Hidden = Flag_Hidden;
            elem.HiddenTitle = HiddenTitle;
            elem.HiddenText = HiddenText;

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
                _log.LogError(exception: ex, "Err # ErrorCode='{}' ## {}", Errors.Err203_str, loginfo);
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="Flag_Hidden">Скрыт</param>
        /// <returns></returns>
        public int EditFlag_Hidden(int ID, bool Flag_Hidden)
        {
            string loginfo = $"ID='{ID}' Flag_Hidden='{Flag_Hidden}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            City? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError("Err # ErrorCode='{}' ## {}", Errors.Err200_str, loginfo);
                return Errors.Err200;
            }

            elem.Flag_Hidden = Flag_Hidden;

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
                _log.LogError(exception: ex, "Err # ErrorCode='{}' ## {}", Errors.Err203_str, loginfo);
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение пометки на удаление
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Flag_Del">Удален</param>
        /// <param name="UserID">Идентификато пользователя</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del}' UserID='{UserID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            
            City? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError("Err # ErrorCode='{}' ## {}", Errors.Err200_str, loginfo);
                return Errors.Err200;
            }

            elem.Flag_Del = Flag_Del;
            if (Flag_Del)
            {
                elem.Date_Del = DateTime.Now;
                elem.UserID_Del = UserID;
            }
            else
            {
                elem.Date_Del = null;
                elem.UserID_Del = null;
            }

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
                _log.LogError(exception: ex, "Err # ErrorCode='{}' ## {}", Errors.Err203_str, loginfo);
                return Errors.Err203;
            }
        }

    }
}