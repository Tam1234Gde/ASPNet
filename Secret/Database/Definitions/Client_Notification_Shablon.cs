using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Шаблон уведомления
    /// </summary>
    [Table("Client_Notification_Shablons")]
    [Comment("Шаблоны уведомлений")]
    public class Client_Notification_Shablon
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Код")]
        public EClient_Notification_Shablon ID { get; set; }


        /// <summary>
        /// Клиент - Код
        /// </summary>
        [Comment("Клиент - Код")]
        public int ClientID { get; set; }


        /// <summary>
        /// Дата генерации нотификации
        /// </summary>
        [Comment("Дата генерации нотификации")]
        public DateTime Date_Act { get; set; }


        /// <summary>
        /// Наименование (заголовок) нотификации
        /// </summary>
        [Comment("Наименование нотификации")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Содержимое нотификации
        /// </summary>
        [Comment("Содержимое нотификации")]
        public string Content { get; set; } = null!;


        /// <summary>
        /// Дата прочтения нотификации
        /// </summary>
        [Comment("Дата прочтения нотификации")]
        public DateTime? Date_Read { get; set; }


        /// <summary>
        /// Пуш-уведомления: Флаг того, что нужно отправлять пуш
        /// </summary>
        [Comment("Пуш-уведомления: Флаг того, что нужно отправлять пуш")]
        public bool Push_Need { get; set; }


        /// <summary>
        /// Пуш-уведомления: Дата отправки 
        /// </summary>
        [Comment("Пуш-уведомления: Дата отправки")]
        public DateTime? Push_DateSend { get; set; }


        /// <summary>
        /// Флаг помеки на удаление
        /// </summary>
        [Comment("Флаг помеки на удаление")]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        [Comment("Дата пометки на удаление")]
        public DateTime? Date_Del { get; set; }

    }
}