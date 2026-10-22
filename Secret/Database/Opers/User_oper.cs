using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Пользователи
    /// </summary>
    public class User_oper
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public User_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public User? Get(int ID, bool? Flag_Del_val = false)
        {
            User? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение по Email и Password
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public User? Get(string Email, string Password, bool? Flag_Del_val = false) //TODO: MAE - GETQuery -  здесь и везде
        {
            User? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.Login == Email && p.Password == Password select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<User> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<User> query = from p in _db.Users.Include(x => x.Restaurant) select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Login"></param>
        /// <param name="Password"></param>
        /// <param name="Name"></param>
        /// <param name="Flag_Admin"></param>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        public int Add(string Login, string Password, string Name, bool Flag_Admin, bool Flag_Root, bool Flag_Oper, int? RestaurantID)
        {
            string loginfo = $"Name='{Name}' Login='{Login}' Password='{Password}' Flag_Admin='{Flag_Admin}' Flag_Root='{Flag_Root}' Flag_Oper='{Flag_Oper}'" +
                             $"RestaurantID='{RestaurantID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            User elem = new()
            {
                Name = Name,
                Login = Login,
                Password = Password,
                Flag_Admin = Flag_Admin,
                Flag_Root = Flag_Root,
                Flag_Oper = Flag_Oper,
                RestaurantID = RestaurantID
            };

            _db.Users.Add(elem);

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
        /// <param name="ID"></param>
        /// <param name="Login_New"></param>
        /// <param name="Password_New"></param>
        /// <param name="Name_New"></param>
        /// <param name="Flag_Admin_New"></param>
        /// <returns></returns>
        public int Edit(int ID, string Login_New, string Password_New, string Name_New, bool Flag_Admin_New, bool Flag_Root_New, bool Flag_Oper_New, int? RestaurantID_New)
        {
            string loginfo = $"ID='{ID}' Name_New='{Name_New}' Login_New='{Login_New}' Password_New='{Password_New}' Flag_Admin_New='{Flag_Admin_New}'" +
                             $"Flag_Root_New='{Flag_Root_New}' Flag_Oper_New='{Flag_Oper_New}' RestaurantID_New='{RestaurantID_New}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            User? elem = Get(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Name = Name_New;
            elem.Login = Login_New;
            elem.Password = Password_New;
            elem.Flag_Admin = Flag_Admin_New;
            elem.Flag_Root = Flag_Root_New;
            elem.Flag_Oper = Flag_Oper_New;
            elem.RestaurantID = RestaurantID_New;

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
        /// <param name="ID"></param>
        /// <param name="Flag_Del_New"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del_New, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del_New}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            User? elem = Get(ID: ID);

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