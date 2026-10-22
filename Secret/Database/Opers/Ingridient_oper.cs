using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Ингридиент
    /// </summary>
    public class Ingridient_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Ingridient_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Ingridient? Get(int ID)
        {
            Ingridient? elem = (from p in _db.Ingridients where p.ID == ID select p).FirstOrDefault();
            return elem;
        }

        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Ingridient> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Ingridient> query = from p in _db.Ingridients select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Image"></param>
        /// <returns></returns>
        public int Add(string Name, byte[]? Image, int? Category_ID)
        {
            string loginfo = $"Name='{Name}' Image='{Image}' ";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Ingridient elem = new()
            {
                Name = Name,
                Image = Image,
                Category_ID = Category_ID
            };

            _db.Ingridients.Add(elem);
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
        /// <param name="Name_New"></param>
        /// <param name="Image_New"></param>
        /// <returns></returns>
        public int Edit(int ID, string Name_New, byte[]? Image_New, int? Category_ID_New)
        {
            string loginfo = $"ID='{ID}' Name_New='{Name_New}' Image_New='{Image_New}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Ingridient? elem = Get(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Name = Name_New;
            elem.Image = Image_New ?? elem.Image;
            elem.Category_ID = Category_ID_New;



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


            Ingridient? elem = Get(ID: ID);

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

        /// <summary>
        /// Удалить изображение
        /// </summary>
        /// <returns></returns>
        public int Del_Image(int ID)
        {
            string loginfo = $"ID='{ID}' ";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Ingridient? elem = Get(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Image = null;
            elem.Image_ChangeDate = DateTime.Now;

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

                _log.LogError(exception: ex, message: "{}", args: $"Err # Error='{Errors.Err203_str}' ## {loginfo}");

                return Errors.Err203;
            }
        }

        /// <summary>
        /// Изменение даты изменения изображения
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Edit_Image_ChangeData(int ID)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Ingridient? elem = Get(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Image_ChangeDate = DateTime.Now;

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

        /// <summary>
        /// Изменение изображения
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Edit_Image(int ID, byte[] Ingridient_Image)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Ingridient? elem = Get(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Image = Ingridient_Image;
            elem.Image_ChangeDate = DateTime.Now;

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