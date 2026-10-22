using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    public class Order_Data_Ingridient_oper
    {
        private readonly ILogger _log;

        private readonly DBContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Order_Data_Ingridient_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _db = new DBContext();
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public int Add(int Order_DataID, int IngridientID, decimal Ingridient_Price, decimal Ingridient_Count, decimal Ingridient_Summ, string Ingridient_Name)
        {
            string loginfo = $"Order_DataID='{Order_DataID}' IngridientID='{IngridientID}' Ingridient_Price='{Ingridient_Price}' Ingridient_Count='{Ingridient_Count}' " +
                             $"Ingridient_Summ='{Ingridient_Summ}' Ingridient_Name='{Ingridient_Name}' ";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }


            Order_Data_Ingridient elem = new()
            {
                Order_DataID = Order_DataID,
                IngridientID = IngridientID,
                Ingridient_Price = Ingridient_Price,
                Ingridient_Count = Ingridient_Count,
                Ingridient_Summ = Ingridient_Summ,
                Ingridient_Name = Ingridient_Name,
            };

            _db.Order_Data_Ingridients.Add(elem);

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
