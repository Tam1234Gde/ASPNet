using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    /// <summary>
    /// Данные заказа - БД
    /// </summary>
    public class Order_Data_oper
    {
        private readonly ILogger _log;
        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Order_Data_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public int Add(int OrderID, int DishID, int? Dish_ColorID, decimal Dish_Price, decimal Dish_Count, decimal Dish_Summ, string Dish_Name,
                       int? Dish_VolumeID, decimal? Dish_VolumeValue, decimal? Dish_VolumePrice, decimal? Bonuses_Minus, decimal? Bonuses_Plus)
        {
            string loginfo = $"OrderID='{OrderID}' DishID='{DishID}' Dish_ColorID='{Dish_ColorID}' Dish_Price='{Dish_Price}' Dish_Count='{Dish_Count}' " +
                             $"Dish_Summ='{Dish_Summ}' Dish_Name='{Dish_Name}' Dish_VolumeID='{Dish_VolumeID}' Dish_VolumeValue='{Dish_VolumeValue}' " +
                             $"Dish_VolumePrice='{Dish_VolumePrice}' Bonuses_Minus='{Bonuses_Minus}' Bonuses_Plus='{Bonuses_Plus}' ";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order_Data elem = new()
            {
                OrderID = OrderID,
                DishID = DishID,
                Dish_ColorID = Dish_ColorID,
                Dish_Price = Dish_Price,
                Dish_Count = Dish_Count,
                Dish_Summ = Dish_Summ,
                Dish_Name = Dish_Name,
                Dish_VolumeID = Dish_VolumeID,
                Dish_VolumeValue = Dish_VolumeValue,
                Dish_VolumePrice = Dish_VolumePrice,
                Bonuses_Plus = Bonuses_Plus,
                Bonuses_Minus = Bonuses_Minus,
            };

            _db.Order_Datas.Add(elem);

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
    }
}
