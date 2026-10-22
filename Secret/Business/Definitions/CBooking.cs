using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Бронирование
    /// </summary>
    public class CBooking
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Дата добавления записи
        /// </summary>
        [DisplayName("Дата добавления записи")]
        [JsonIgnore]
        public DateTime Date_Action { get; set; }


        /// <summary>
        /// Код клиента
        /// </summary>
        [DisplayName("Код клиента")]
        [JsonIgnore]
        public int? ClientID { get; set; }


        /// <summary>
        /// Клиент
        /// </summary>
        [DisplayName("Клиент")]
        [JsonIgnore]
        public string? Client { get; set; }


        /// <summary>
        /// Телефон(в ресторане)
        /// </summary>
        [DisplayName("Телефон")]
        [JsonIgnore]
        public string? Phone { get; set; }


        /// <summary>
        /// Имя(в ресторане)
        /// </summary>
        [DisplayName("Имя")]
        [JsonIgnore]
        public string? Name { get; set; }


        /// <summary>
        /// Количество гостей
        /// </summary>
        [DisplayName("Количество гостей")]
        [JsonIgnore]
        public int? NumberQuests { get; set; }


        /// <summary>
        /// Код стола
        /// </summary>
        [DisplayName("Код стола")]
        public int Restaurant_TableID { get; set; }


        /// <summary>
        /// Стол
        /// </summary>
        [DisplayName("Стол")]
        public string? Restaurant_Table { get; set; }


        /// <summary>
        /// Тип столика
        /// </summary>
        [DisplayName("Тип столика")]
        public string? Restaurant_Table_Type { get; set; }


        /// <summary>
        /// Дата начала
        /// </summary>
        [DisplayName("Дата начала")]
        public DateTime Date_Start { get; set; }


        /// <summary>
        /// Дата окончания
        /// </summary>
        [DisplayName("Дата окончания")]
        public DateTime Date_End { get; set; }


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        [JsonIgnore]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        [JsonIgnore]
        public DateTime? Date_Del { get; set; }

    }
}