using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Цвета
    /// </summary>
    public class Color_oper
    {
        /// <summary>
        /// Логер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Констекст базы данных
        /// </summary>
        private readonly DBContext _db;


        /// <summary>
        /// Конструктор
        /// </summary>
        public Color_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Иденетификатор записи</param>
        /// <returns></returns>
        public Color? Get(int ID)
        {
            Color? elem = (from p in _db.Colors.AsNoTracking() where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Иденетификатор записи</param>
        /// <returns></returns>
        public Color? GetEdit(int ID)
        {
            Color? elem = (from p in _db.Colors where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val">Значение флага удаления, если null - то игнорируется</param>
        /// <returns></returns>
        public IQueryable<Color> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Color> query = from p in _db.Colors select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Name_Eng">Имя англ</param>
        /// <param name="HexColor">HEX Цвет</param>
        /// <returns></returns>
        public int Add(string Name, string Name_Eng, string HexColor)
        {
            string loginfo = $"Name='{Name}'Name_Eng='{Name_Eng}'HexColor='{HexColor}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Color elem = new()
            {
                Name = Name,
                Name_Eng = Name_Eng,
                HexColor = HexColor
            };

            _db.Colors.Add(elem);
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
                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err202_str}' ## {loginfo}");
                return Errors.Err202;
            }
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">Идентфикатор записи</param>
        /// <param name="Name_New">Новое имя</param>
        /// <param name="Name_Eng_New">Новое англ имя</param>
        /// <param name="HexColor_New">Новый HEX Цвет</param>
        /// <returns></returns>
        public int Edit(int ID, string Name_New, string Name_Eng_New, string HexColor_New)
        {
            string loginfo = $"ID='{ID}' Name_New='{Name_New}' Name_Eng_New='{Name_Eng_New}' HexColor_New='{HexColor_New}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Color? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Name = Name_New;
            elem.Name_Eng = Name_Eng_New;
            elem.HexColor = HexColor_New;

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
        /// <param name="UserID">Кто изменил</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del_New, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Color? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");
                return Errors.Err200;
            }

            elem.Flag_Del = Flag_Del_New;
            if (Flag_Del_New)
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
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "{}", exception: ex, args: $"Err # {Errors.Err203_str} ## {loginfo}");
                }

                return Errors.Err203;
            }
        }
    }
}