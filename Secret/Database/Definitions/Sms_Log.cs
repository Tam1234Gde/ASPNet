using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Логи отправки СМС
    /// </summary>
    [Table("Sms_Logs")]
    [Comment("Логи отправки СМС")]
    public class Sms_Log
    {
        /// <summary>
        /// Код
        /// </summary>
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Дата отправки
        /// </summary>
        [Comment("Дата отправки")]
        public DateTime Date_Act { get; set; }


        /// <summary>
        /// Клиент, которому отправлено СМС - код
        /// </summary>
        [Comment("Клиент, которому отправлено СМС - код")]
        public int ClientID { get; set; }


        /// <summary>
        /// Клиент, которому отправлено СМС
        /// </summary>
        [Comment("Клиент, которому отправлено СМС")]
        public Client Client { get; set; } = null!;


        /// <summary>
        /// Телефон на который отправили СМС
        /// </summary>
        [Comment("Телефон на который отправили СМС")]
        public string Phone { get; set; } = null!;


        /// <summary>
        /// Текст, который был отправлен в смс
        /// </summary>
        [Comment("Текст, который был отправлен в смс")]
        public string Text { get; set; } = null!;

    }
}