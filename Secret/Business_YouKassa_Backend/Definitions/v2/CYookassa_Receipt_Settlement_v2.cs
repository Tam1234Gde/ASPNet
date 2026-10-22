using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// Перечень совершенных расчетов
    /// </summary>
    public class CYookassa_Receipt_Settlement_v2
    {
        /// <summary>
        /// Тип расчета
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = null!;


        /// <summary>
        /// Цена товара
        /// </summary>
        [JsonProperty("amount")]
        public CYookassa_Amount_v2 Amount { get; set; } = null!;
    }

}
