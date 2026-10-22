using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Объем блюда в меню ресторана
    /// </summary>
    public class Restaurant_Menu_Dish_Volume_oper
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст БД
        /// </summary>
        private readonly DBContext _db = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Restaurant_Menu_Dish_Volume_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Restaurant_Menu_Dish_Volume? Get(int ID, bool? Flag_Del_val = false)
        {
            Restaurant_Menu_Dish_Volume? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="Restaurant_Menu_DishID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu_Dish_Volume> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Menu_Dish_Volume> query = from p in _db.Restaurant_Menu_Dish_Volumes.Include(x => x.Restaurant_Menu_Dish).Include(x => x.Volume)
                                                            select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="Restaurant_Menu_DishID"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Restaurant_Menu_Dish_Volume> GetQuery_ByRestaurantID(int Restaurant_Menu_DishID, bool? Flag_Del_val = false)
        {
            IQueryable<Restaurant_Menu_Dish_Volume> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.Restaurant_Menu_DishID == Restaurant_Menu_DishID select p;

            return query;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID">Код записи, которую нужно получить</param>
        /// <returns></returns>
        private Restaurant_Menu_Dish_Volume? Get_Edit(int ID, bool? Flag_Del_val = false)
        {
            Restaurant_Menu_Dish_Volume? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Restaurant_Menu_DishID">Код блюда ресторана</param>
        /// <param name="VolumeID">Код объема</param>
        /// <param name="Price">Стоимость</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <returns></returns>
        public int Add(int Restaurant_Menu_DishID, int VolumeID, decimal Price, int OrderNum)
        {
            string loginfo = $"Restaurant_Menu_DishID='{Restaurant_Menu_DishID}' VolumeID='{VolumeID}' Price='{Price}' OrderNum='{OrderNum}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_Menu_Dish_Volume elem = new()
            {
                VolumeID = VolumeID,
                Restaurant_Menu_DishID = Restaurant_Menu_DishID,
                Price = Price,
                OrderNum = OrderNum
            };

            _db.Restaurant_Menu_Dish_Volumes.Add(elem);

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
        /// <param name="Restaurant_Menu_DishID">Код блюда ресторана</param>
        /// <param name="VolumeID">Код объема</param>
        /// <param name="Price">Стоимость</param>
        /// <param name="OrderNum">Порядок отображения</param>
        /// <returns></returns>
        public int Edit(int ID, int Restaurant_Menu_DishID, int VolumeID, decimal Price, int OrderNum)
        {
            string loginfo = $"ID='{ID}' Restaurant_Menu_DishID='{Restaurant_Menu_DishID}' VolumeID='{VolumeID}' Price='{Price}' OrderNum='{OrderNum}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_Menu_Dish_Volume? elem = Get_Edit(ID: ID);

            if (elem == null)
            {
                _log.LogError("Err # ErrorCode='{}' ## {}", Errors.Err200_str, loginfo);
                return Errors.Err200;
            }

            elem.Restaurant_Menu_DishID = Restaurant_Menu_DishID;
            elem.VolumeID = VolumeID;
            elem.Price = Price;
            elem.OrderNum = OrderNum;

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
        /// <param name="ID"></param>
        /// <param name="Flag_Del"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del}' UserID='{UserID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Restaurant_Menu_Dish_Volume? elem = Get_Edit(ID: ID);

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