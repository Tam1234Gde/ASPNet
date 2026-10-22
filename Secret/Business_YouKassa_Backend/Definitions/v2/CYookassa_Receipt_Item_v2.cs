using Newtonsoft.Json;

namespace HIDE.Ithelper.Web.Business_YooKassa_Backend.Definitions.v2
{
    /// <summary>
    /// Список товаров в чеке
    /// </summary>
    public class CYookassa_Receipt_Item_v2
    {
        /// <summary>
        /// Название товара (от 1 до 128 символов)
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; } = null!;


        /// <summary>
        /// Цена товара
        /// </summary>
        [JsonProperty("amount")]
        public CYookassa_Amount_v2 Amount { get; set; } = null!;


        /// <summary>
        /// Ставка НДС
        /// </summary>
        [JsonProperty("vat_code")]
        public int Vat_Code { get; set; }


        /// <summary>
        /// Количество товара
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }


        /// <summary>
        /// Признак предмета расчета
        /// </summary>
        [JsonProperty("payment_subject")]
        public string? Payment_Subject { get; set; }


        /// <summary>
        /// Признак способа расчета
        /// </summary>
        [JsonProperty("payment_mode")]
        public string? Payment_Mode { get; set; }
    }

}
