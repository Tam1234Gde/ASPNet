using Newtonsoft.Json.Converters;

namespace HIDE.RestAndRant.Web_Api.Helpers
{
    /// <summary>
    /// Helper_CustomDateTimeConverter
    /// </summary>
    public class Helper_CustomDateTimeConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Helper_CustomDateTimeConverter
        /// </summary>
        public Helper_CustomDateTimeConverter()
        {
            base.DateTimeFormat = "dd.MM.yyyy";
        }
    }
}