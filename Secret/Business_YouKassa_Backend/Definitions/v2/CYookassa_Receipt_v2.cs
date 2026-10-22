using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// Объект чека (для создания объекта чека)
    /// </summary>
    public class CYookassa_Receipt_v2
    {
        /// <summary>
        /// Информация о пользователе
        /// </summary>
        [JsonProperty("customer")]
        public CYookassa_Receipt_Customer_v2 Customer { get; set; } = null!;


        /// <summary>
        /// Список товаров в чеке
        /// </summary>
        [JsonProperty("items")]
        public List<CYookassa_Receipt_Item_v2> Items { get; set; } = null!;
    }

}
