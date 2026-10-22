using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Класс для управления базой данных в части баннеров
    /// </summary>
    public class Banner_oper
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
        public Banner_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="Flag_Del_val">Флаг удаленные</param>
        /// <returns></returns>
        public IQueryable<Banner> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Banner> query = from p in _db.Banners.Include(x => x.City).AsNoTracking() orderby p.Date_Create select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Получение списка для редактирования
        /// </summary>
        /// <param name="Flag_Del_val">Флаг удаленные</param>
        /// <returns></returns>
        public IQueryable<Banner> GetQuery_Edit(bool? Flag_Del_val = false)
        {
            IQueryable<Banner> query = from p in _db.Banners.Include(x => x.City) orderby p.Date_Create select p;

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
        public Banner? Get(int ID, bool? Flag_Del_val = false)
        {
            Banner? elem = (from p in GetQuery(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение по ID для редактирования
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <returns></returns>
        public Banner? GetEdit(int ID, bool? Flag_Del_val = false)
        {
            Banner? elem = (from p in GetQuery_Edit(Flag_Del_val: Flag_Del_val) where p.ID == ID select p).FirstOrDefault();
            return elem;
        }


        /// <summary>
        /// Получение списка
        /// </summary>
        /// <param name="Flag_Del_val">Флаг удаленные</param>
        /// <returns></returns>
        public IQueryable<Banner> GetQuery_ByCity(int CityID, bool? Flag_Del_val = false)
        {
            IQueryable<Banner> query = from p in GetQuery(Flag_Del_val: Flag_Del_val) orderby p.Date_Create where p.CityID == CityID select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Изменение изображения большого 
        /// </summary>
        /// <param name="ID">Идентификатор запии</param>
        /// <param name="Banner_Image_Big">Контент</param>
        /// <returns></returns>
        public int Edit_Image_Big(int ID, byte[] Banner_Image_Big)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            
            Banner? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={} ~ Error={} ### {}", args: [Errors.Err200, Errors.Err200_str, loginfo]);
                }
                return Errors.Err200;
            }

            elem.Image_Big = Banner_Image_Big;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "OK # {}", args: loginfo);
                }
                
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "Err # {} ## {}", args: [Errors.Err203_str, loginfo], exception: ex);
                }
                
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Изменение изображения
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Banner_Image_Small">Контент</param>
        /// <returns></returns>
        public int Edit_Image_Small(int ID, byte[] Banner_Image_Small)
        {
            string loginfo = $"ID='{ID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Banner? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={} ~ Error={} ### {}", args: [Errors.Err200, Errors.Err200_str, loginfo]);
                }
                
                return Errors.Err200;
            }

            elem.Image_Small = Banner_Image_Small;

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "OK # {}", args: [loginfo]);
                }
                
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Critical))
                {
                    _log.LogCritical(message: "Err # {} ## {}", args: [Errors.Err203_str, loginfo], exception: ex);
                }
                
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Удалить изображение
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Big">True, если большое изображение</param>
        /// <returns></returns>
        public int Del_Image(int ID, bool Big)
        {
            string loginfo = $"ID='{ID}' ";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            Banner? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={} ~ Error={} ### {}", args: [Errors.Err200, Errors.Err200_str, loginfo]);
                }
                
                return Errors.Err200;
            }

            if (Big)
            {
                elem.Image_Big = null;
                elem.Date_Edit = DateTime.Now;
            }
            else
            {
                elem.Image_Small = null;
                elem.Date_Edit = DateTime.Now;
            }

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "OK # {}", args: loginfo);
                }
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(exception: ex, message: "Err # Error='{}' ## {}", args: [Errors.Err203_str, loginfo]);
                }
                
                return Errors.Err203;
            }
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Image_Big">Банер большрой</param>
        /// <param name="Image_Small">Банер маленький></param>
        /// <param name="CityID">Город</param>
        /// <param name="Category_ID">Категория</param>
        /// <param name="Product_ID">Продукт</param>
        /// <param name="Date_Start">Дата начала показа</param>
        /// <param name="Date_End">Дата окончания показа</param>
        /// <param name="OrderNum">Номер сортировки</param>
        /// <returns></returns>
        public int Add(string? Name, byte[]? Image_Big, byte[]? Image_Small, int CityID, int? Category_ID, int? Product_ID, DateTime? Date_Start, DateTime? Date_End, int OrderNum)
        {
            string loginfo = $"Image_Big='{Image_Big}'  Image_Small='{Image_Small}'  CityID='{CityID}' Category_ID='{Category_ID}' Product_ID='{Product_ID}' Date_Start='{Date_Start}' Date_End='{Date_End} OrderNum={OrderNum}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            Banner elem = new()
            {
                Name = Name,
                Image_Big = Image_Big,
                Image_Small = Image_Small,
                CityID = CityID,
                Category_ID = Category_ID,
                Product_ID = Product_ID,
                Date_Start = Date_Start,
                Date_End = Date_End,
                Date_Create = DateTime.Now,
                OrderNum = OrderNum,
                Date_Edit = DateTime.Now,
            };

            _db.Banners.Add(elem);

            try
            {
                _db.SaveChanges();
                if (_log.IsEnabled(LogLevel.Debug))
                {
                    _log.LogDebug(message: "OK # {}", args: loginfo);
                }
                return elem.ID;
            }
            catch (Exception ex)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(exception: ex, message: "Err # ErrorCode='{}' ## {}", args: [Errors.Err202_str, loginfo]);
                }
                return Errors.Err202;
            }
        }


        /// <summary>
        /// Редактирование
        /// </summary>
        /// <param name="Image_Big">Банер большрой</param>
        /// <param name="Image_Small">Банер маленький></param>
        /// <param name="CityID">Город</param>
        /// <param name="Category_ID">Категория</param>
        /// <param name="Product_ID">Продукт</param>
        /// <param name="Date_Start">Дата начала показа</param>
        /// <param name="Date_End">Дата окончания показа</param>
        /// <param name="OrderNum">Номер сортировки</param>
        /// <returns></returns>
        public int Edit(int ID, string? Name, byte[]? Image_Big, byte[]? Image_Small, int CityID, int? Category_ID, int? Product_ID, DateTime? Date_Start, DateTime? Date_End, int OrderNum)
        {
            string loginfo = $"ID='{ID}' Image_Big='{Image_Big}'  Image_Small='{Image_Small}'  City_ID='{CityID}' Category_ID='{Category_ID}' Product_ID='{Product_ID}' Date_Start='{Date_Start}' Date_End='{Date_End} OrderNum = {OrderNum}'";
            
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }

            Banner? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode={} ~ Error={} ### {}", args: [Errors.Err200, Errors.Err200_str, loginfo]);
                }
                return Errors.Err200;
            }

            elem.Name = Name;

            if (Image_Big != null) //required
            {
                elem.Image_Big = Image_Big;
            }
            if (Image_Small != null)//required
            {
                elem.Image_Small = Image_Small;
            }
            elem.CityID = CityID;
            elem.Category_ID = Category_ID;
            elem.Product_ID = Product_ID;
            if (Date_Start != null) //required
            {
                elem.Date_Start = Date_Start;
            }
            elem.Date_End = Date_End;
            elem.OrderNum = OrderNum;
            elem.Date_Edit = DateTime.Now;
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
                    _log.LogCritical(exception: ex, message: "Err # {} ## {}", args: [Errors.Err203_str, loginfo]);
                }
                return Errors.Err203;
            }

        }


        /// <summary>
        /// Изменение пометки на удаление
        /// </summary>
        /// <param name="ID">Идентификатор записи</param>
        /// <param name="Flag_Del">Флаг удалено или нет</param>
        /// <param name="UserID">номер пользователя</param>
        /// <returns></returns>
        public int Edit_FlagDel(int ID, bool Flag_Del, int UserID)
        {
            string loginfo = $"ID='{ID}' FlagDel_New='{Flag_Del}' UserID='{UserID}'";
            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }
            Banner? elem = GetEdit(ID: ID);

            if (elem == null)
            {
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(message: "Err # ErrorCode='{}' ## {}", args: [Errors.Err200_str, loginfo]);
                }
                return Errors.Err200;
            }
            elem.Date_Edit = DateTime.Now;
            elem.Flag_Del = Flag_Del;

            if (Flag_Del)
            {
                elem.Date_Del = DateTime.Now;
                elem.UserID_Del = UserID;
                elem.Image_Big = null;
                elem.Image_Small = null;
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
                if (_log.IsEnabled(LogLevel.Error))
                {
                    _log.LogError(exception: ex, message: "Err # ErrorCode='{}' ## {}", args: [Errors.Err203_str, loginfo]);
                }
                return Errors.Err203;
            }
        }
    }
}
