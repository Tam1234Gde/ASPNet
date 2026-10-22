using Microsoft.Extensions.Logging;
using HIDE.Crypto_Core;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business.Endpoints
{
    /// <summary>
    /// Пользователи
    /// </summary>
    public class Users
    {
        private readonly ILogger _log;
        private readonly User_oper _dl_User = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Users()
        {
            _log = AppLog.CreateLogger(GetType());
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CUser? Get(User? Elem)
        {
            if (Elem != null)
            {
                CUser item = new()
                {
                    ID = Elem.ID,
                    Name = Elem.Name,
                    Login = Elem.Login,
                    Password = Elem.Password,
                    Flag_Admin = Elem.Flag_Admin,
                    Flag_Root = Elem.Flag_Root,
                    Flag_Oper = Elem.Flag_Oper,
                    RestaurantID = Elem.RestaurantID,
                    Restaurant = Elem.Restaurant?.Adress
                };
                return item;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CUser? Get(int ID)
        {
            return Get(Elem: _dl_User.Get(ID));
        }

        /// <summary>
        /// Получение данных категории в коротком виде
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CCommon? Get_Short(User? Elem)
        {
            if (Elem != null)
            {
                CCommon item = new()
                {
                    ID = Elem.ID,
                    Name = Elem.Name,
                };

                return item;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Логин в систему
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns>Возвращает код пользователя</returns>
        public CUser? Login(string Email, string Password)
        {
            string? loginfo = $"Email='{Email}' Password='{Password}' ";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            User? elem = _dl_User.Get(Email: Email, Password: HIDE_SHA1.GetSHA1(Password));
            if (elem != null)
            {
                return Get(elem);
            }
            else
            {

                return null;
            }
        }

        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <returns></returns>
        public List<CUser> GetList(bool? Flag_Del_val = false)
        {
            List<CUser> list_result = [];
            foreach (User elem in _dl_User.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CUser? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Name)];
        }

        /// <summary>
        /// Возвращает список в коротком виде
        /// </summary>
        /// <returns></returns>
        public List<CCommon> GetList_Short()
        {
            List<CCommon> list_result = [];
            foreach (User elem in _dl_User.GetQuery().ToList())
            {
                CCommon? item = Get_Short(Elem: elem);
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
        /// <param name="Name"></param>
        /// <param name="Login"></param>
        /// <param name="Password"></param>
        /// <param name="Flag_Admin"></param>
        /// <param name="Flag_Root"></param>
        /// <param name="Flag_Oper"></param>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        public int Add(string Name, string Login, string Password, bool Flag_Admin, bool Flag_Root, bool Flag_Oper, int? RestaurantID)
        {
            return _dl_User.Add(Name: Name, Login: Login, Password: HIDE_SHA1.GetSHA1(Password), Flag_Admin: Flag_Admin, Flag_Root: Flag_Root, Flag_Oper: Flag_Oper, RestaurantID: RestaurantID);
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name_New"></param>
        /// <param name="Login_New"></param>
        /// <param name="Password_New"></param>
        /// <param name="Flag_Admin_New"></param>
        /// <param name="Flag_Root_New"></param>
        /// <param name="Flag_Oper_New"></param>
        /// <param name="RestaurantID_New"></param>
        /// <returns></returns>
        public int Edit(int ID, string Name_New, string Login_New, string Password_New, bool Flag_Admin_New, bool Flag_Root_New, bool Flag_Oper_New, int? RestaurantID_New)
        {
            return _dl_User.Edit(ID: ID, Name_New: Name_New, Login_New: Login_New, Password_New: HIDE_SHA1.GetSHA1(Password_New), Flag_Admin_New: Flag_Admin_New, Flag_Root_New: Flag_Root_New,
                                 Flag_Oper_New: Flag_Oper_New, RestaurantID_New: RestaurantID_New);
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            return _dl_User.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }

    }
}