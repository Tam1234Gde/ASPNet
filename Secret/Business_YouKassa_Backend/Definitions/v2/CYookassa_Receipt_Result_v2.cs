using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// Объект чека - результат
    /// </summary>
    public class CYookassa_Receipt_Result_v2
    {
        /// <summary>
        /// Идентификатор чека в ЮKassa
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; } = null!;


        /// <summary>
        /// Тип чека в онлайн-кассе
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = null!;


        /// <summary>
        /// Статус доставки данных для чека в онлайн-кассу
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; } = null!;


        /// <summary>
        /// Список товаров в чеке
        /// </summary>
        [JsonProperty("items")]
        public List<CYookassa_Receipt_Item_v2> Items { get; set; } = null!;


        /// <summary>
        /// Перечень совершенных расчетов
        /// </summary>
        [JsonProperty("settlements")]
        public List<CYookassa_Receipt_Settlement_v2>? Settlements { get; set; }
    }

}
