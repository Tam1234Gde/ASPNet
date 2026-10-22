using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Объём блюда
    /// </summary>
    public class Dish_Volume_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Dish_Volume_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Dish_Volume? Get(int ID)
        {
            Dish_Volume? elem = (from p in _db.Dish_Volumes.Include(x => x.Dish).Include(x => x.Volume) where p.ID == ID select p).FirstOrDefault();
                                 
            return elem;
        }

        /// <summary>
        /// Получение запроса по DishID
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Dish_Volume> GetQuery_ByDishID(int DishID, bool? Flag_Del_val = false)
        {
            IQueryable<Dish_Volume> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.DishID == DishID select p;  //TODO: MAE - GetQuery

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Dish_Volume> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Dish_Volume> query = from p in _db.Dish_Volumes.Include(x => x.Dish).Include(x => x.Volume).AsNoTracking() select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="VolumeID"></param>
        /// <param name="Price"></param>
        /// <param name="OrderNum"></param>
        /// <returns></returns>
        public int Add(int DishID, int VolumeID, decimal Price, int OrderNum)
        {
            string loginfo = $"DishID='{DishID}' VolumeID='{VolumeID}' Price='{Price}' OrderNum='{OrderNum}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Dish_Volume elem = new()
            {
                DishID = DishID,
                VolumeID = VolumeID,
                Price = Price,
                OrderNum = OrderNum
            };

            _db.Dish_Volumes.Add(elem);
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
        /// <param name="DishID_New"></param>
        /// <param name="VolumeID_New"></param>
        /// <param name="Price_New"></param>
        /// <param name="OrderNum_New"></param>
        /// <returns></returns>
        public int Edit(int ID, int DishID_New, int VolumeID_New, decimal Price_New, int OrderNum_New)
        {
            string loginfo = $"ID='{ID}' DishID_New='{DishID_New}' VolumeID_New='{VolumeID_New}' Price_New='{Price_New}' OrderNum_New='{OrderNum_New}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Dish_Volume? elem = Get(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.DishID = DishID_New;
            elem.VolumeID = VolumeID_New;
            elem.Price = Price_New;
            elem.OrderNum = OrderNum_New;

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


            Dish_Volume? elem = Get(ID: ID);

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