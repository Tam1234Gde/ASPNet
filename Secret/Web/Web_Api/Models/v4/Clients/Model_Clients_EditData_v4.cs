using Newtonsoft.Json;
using HIDE.RestAndRant.Web_Api.Helpers;

namespace HIDE.RestAndRant.Web_Api.Models.v4.Clients
{
    /// <summary>
    /// Редактирование
    /// </summary>
    public class Model_Clients_EditData_v4
    {
        /// <summary>
        /// Имя клиента
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        [JsonConverter(typeof(Helper_CustomDateTimeConverter))]
        public DateTime? Date_Birth { get; set; }

    }
}