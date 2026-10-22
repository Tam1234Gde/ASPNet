using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Нотификация клиента
    /// </summary>
    public class CClient_Notification
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Клиент, для которого нотификация - код
        /// </summary>
        public int? ClientID { get; set; }


        /// <summary>
        /// Дата генерации нотификации
        /// </summary>
        public DateTime Date_Act { get; set; }


        /// <summary>
        /// Наименование (заголовок) нотификации
        /// </summary>
        public string Name { get; set; } = null!;


        /// <summary>
        /// Содержимое нотификации
        /// </summary>
        public string Content { get; set; } = null!;


        /// <summary>
        /// Флаг прочтения этого уведомления
        /// </summary>
        public bool Flag_Read { get; set; }


        /// <summary>
        /// Заказ - Код
        /// </summary>
        [Comment("Заказ - Код")]
        [JsonIgnore]
        public int? OrderID { get; set; }


        /// <summary>
        /// Код шаблона
        /// </summary>
        [Comment("Код шаблона")]
        [JsonIgnore]
        public EClient_Notification_Shablon? Client_Notification_ShablonID { get; set; }

    }
}