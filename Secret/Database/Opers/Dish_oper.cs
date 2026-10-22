using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Блюдо
    /// </summary>
    public class Dish_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Dish_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Dish? Get_Edit(int ID)
        {
            Dish? elem = (from p in GetQuery_Edit() where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Dish? Get(int ID)
        {
            Dish? elem = (from p in GetQuery() where p.ID == ID select p).FirstOrDefault();

            return elem;
        }


        /// <summary>
        /// Получение запроса
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Dish> GetQuery(bool? Flag_Del_val = false)
        {
            IQueryable<Dish> query = from p in _db.Dishes.Include(x => x.Dish_Category).Include(x => x.Dish_Volumes).Include(x => x.Dish_Ingridients).ThenInclude(x => x.Dish_Ingridient_Category).Include(x => x.Dish_Category).AsNoTracking() select p;


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
        public IQueryable<Dish> GetQuery_Edit(bool? Flag_Del_val = false)
        {
            IQueryable<Dish> query = from p in _db.Dishes.Include(x => x.Dish_Ingridients).ThenInclude(x => x.Dish_Ingridient_Category).Include(x => x.Dish_Ingridients).ThenInclude(x => x.Ingridient).Include(x => x.Dish_Volumes) where p.Dish_Category != null && p.Dish_Category.Flag_Del == false select p; //TODO: MAE - GetQuery    from MAE - ??????, если поставлю GetQuery() - получится рекурсия


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
        public IQueryable<Dish> GetQueryByIDs_Edit(List<int> IDs, bool? Flag_Del_val = false)
        {
            IQueryable<Dish> query = from p in GetQuery_Edit() where IDs.Contains(p.ID) select p;

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
        public IQueryable<Dish> GetQueryByIDs(List<int> IDs, bool? Flag_Del_val = false)
        {
            IQueryable<Dish> query = from p in GetQuery() where IDs.Contains(p.ID) select p;

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
        public IQueryable<Dish> GetQueryForAdmin(int RestaurantMenuID)
        { 
            IQueryable<Dish> query = from p in GetQuery() where p.Dish_Category != null && p.Dish_Category.Flag_Del == false && p.Flag_Del == false select p;
             
            IQueryable<Restaurant_Menu_Dish> query_restaurant = from p in _db.Restaurant_Menu_Dishes where p.Restaurant_MenuID == RestaurantMenuID &&
                                                                !p.Flag_Del select p;
                                                                
            IQueryable<int> result = query.Select(p => p.ID).Except(query_restaurant.Select(p => p.DishID));
            query = from p in GetQuery() where result.Contains(p.ID) select p;

            return query;
        }


        /// <summary>
        /// Поиск по блюдам
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public IQueryable<Dish> GetQuerySearch(string Name, bool? Flag_Del_val = false)
        {
            IQueryable<Dish> query = from p in GetQuery() where p.Name.Contains(Name) && p.Dish_Category != null && !p.Dish_Category.Flag_Del select p;

            if (Flag_Del_val.HasValue)
            {
                query = from p in query where p.Flag_Del == Flag_Del_val select p;
            }

            return query;
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Dish_CategoryID"></param>
        /// <param name="Name"></param>
        /// <param name="Price"></param>
        /// <param name="Flag_AllowColors"></param>
        /// <param name="OrderNum"></param>
        /// <param name="Teaser"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public int Add(int Dish_CategoryID, string Name, decimal Price, bool Flag_AllowColors, int OrderNum, byte[]? Image, string? Teaser, string? Text,
                       decimal? Bonuses, bool Flag_Bonus, int? Cooking_Minutes, string? RecommendDishIDs, bool FlagNew, bool FlagNoActive, DateTime? New_DateFrom)
        {
            string loginfo = $"Dish_CategoryID='{Dish_CategoryID}' Name='{Name}' Price='{Price}' Flag_AllowColors='{Flag_AllowColors}' OrderNum='{OrderNum}'" +
                             $"Image='{Image}' Teaser='{Teaser}' Text='{Text}' Bonuses='{Bonuses}' Flag_Bonus='{Flag_Bonus}' Cooking_Minutes='{Cooking_Minutes}'" +
                             $"RecommendDishIDs='{RecommendDishIDs}' FlagNew='{FlagNew}' FlagNoActive='{FlagNoActive}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Dish elem = new()
            {
                Dish_CategoryID = Dish_CategoryID,
                Name = Name,
                Price = Price,
                Flag_AllowColors = Flag_AllowColors,
                OrderNum = OrderNum,
                Image = Image,
                Teaser = Teaser,
                Text = Text,
                Bonuses = Bonuses,
                Flag_Bonus = Flag_Bonus,
                Cooking_Minutes = Cooking_Minutes,
                RecommendDishIDs = RecommendDishIDs,
                FlagNew = FlagNew,
                FlagNoActive = FlagNoActive,
                New_DateFrom = New_DateFrom,
            };

            _db.Dishes.Add(elem);
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
        /// <param name="Dish_CategoryID_New"></param>
        /// <param name="Name_New"></param>
        /// <param name="Price_New"></param>
        /// <param name="Flag_AllowColors_New"></param>
        /// <param name="OrderNum_New"></param>
        /// <param name="Teaser_New"></param>
        /// <param name="Text_New"></param>
        /// <param name="Bonuses_New"></param>
        /// <param name="Flag_Bonus_New"></param>
        /// <returns></returns>
        public int Edit(int ID, int Dish_CategoryID_New, string Name_New, decimal Price_New, bool Flag_AllowColors_New, int OrderNum_New, byte[]? Image_New,
                        string? Teaser_New, string? Text_New, decimal? Bonuses_New, bool Flag_Bonus_New, int? Cooking_Minutes_New, string? RecommendDishIDs_New,
                        bool FlagNew_New, bool FlagNoActive_New, DateTime? New_DateFrom_New)
        {
            string loginfo = $"ID='{ID}' Dish_CategoryID_New='{Dish_CategoryID_New}' Name_New='{Name_New}' Price_New='{Price_New}' " +
                             $"Flag_AllowColors_New='{Flag_AllowColors_New}' OrderNum_New='{OrderNum_New}' Image_New='{Image_New}' " +
                             $"Teaser_New='{Teaser_New}' Text_New='{Text_New}' Bonuses_New='{Bonuses_New}' Flag_Bonus_New='{Flag_Bonus_New}' Cooking_Minutes_New='{Cooking_Minutes_New}' RecommendDishIDs_New='{RecommendDishIDs_New}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Dish? elem = Get_Edit(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Dish_CategoryID = Dish_CategoryID_New;
            elem.Name = Name_New;
            elem.Price = Price_New;
            elem.Flag_AllowColors = Flag_AllowColors_New;
            elem.OrderNum = OrderNum_New;
            elem.Image = Image_New;
            elem.Teaser = Teaser_New;
            elem.Text = Text_New;
            elem.Bonuses = Bonuses_New;
            elem.Flag_Bonus = Flag_Bonus_New;
            elem.Cooking_Minutes = Cooking_Minutes_New;
            elem.RecommendDishIDs = RecommendDishIDs_New;
            elem.FlagNew = FlagNew_New;
            elem.FlagNoActive = FlagNoActive_New;
            elem.New_DateFrom = New_DateFrom_New;

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


            Dish? elem = Get_Edit(ID: ID);

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


            Dish? elem = Get_Edit(ID: ID);

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


            Dish? elem = Get_Edit(ID: ID);

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
        public int Edit_Image(int ID, byte[] Dish_Image)
        {
            string loginfo = $"ID='{ID}'";

            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Dish? elem = Get_Edit(ID: ID);

            if (elem == null)
            {

                _log.LogError(message: "{}", args: $"Err # ErrorCode={Errors.Err200} ~ Error={Errors.Err200_str} ### {loginfo}");

                return Errors.Err200;
            }

            elem.Image = Dish_Image;
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