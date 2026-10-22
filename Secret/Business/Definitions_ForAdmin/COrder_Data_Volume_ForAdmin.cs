using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Объем позиции заказа
    /// </summary>
    public class COrder_Data_Volume_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [JsonIgnore]
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Ссылка на позицию заказа - код
        /// </summary>
        [JsonIgnore]
        [DisplayName("Код позиции заказа")]
        public int Order_DataID { get; set; }


        /// <summary>
        /// Объём - код
        /// </summary>
        [DisplayName("Код объёма")]
        public int VolumeID { get; set; }


        /// <summary>
        /// Цена объёма
        /// </summary>
        [DisplayName("Цена объёма")]
        public decimal Volume_Price { get; set; }


        /// <summary>
        /// Объём в момент оформления
        /// </summary>
        [DisplayName("Объём")]
        public decimal Volume_Name { get; set; }


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        [DisplayName("Удален")]
        public bool Flag_Del { get; set; }
    }
}