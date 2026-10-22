using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Opers
{
    public class Sms_Log_oper
    {
        private readonly ILogger _log;

        private readonly DBContext _db;

        public Sms_Log_oper()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
            _db = new();
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public int Add(int ClientID, string Phone, string Text)
        {
            string loginfo = $"ClientID='{ClientID}' Phone='{Phone}' Text='{Text}' ";


            if (_log.IsEnabled(LogLevel.Debug))
            {
                _log.LogDebug(message: "{}", args: loginfo);
            }



            Sms_Log elem = new()
            {
                Date_Act = DateTime.Now,
                ClientID = ClientID,
                Phone = Phone,
                Text = Text,
            };

            _db.Sms_Logs.Add(elem);

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
