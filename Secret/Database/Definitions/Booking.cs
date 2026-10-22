using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Бронирование
    /// </summary>
    [Table("Bookings")]
    [Comment("Бронирование")]
    public class Booking
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Дата добавления записи
        /// </summary>
        [Comment("Дата добавления записи")]
        public DateTime Date_Action { get; set; }


        /// <summary>
        /// Клиент - Код
        /// </summary>
        [Comment("Клиент - Код")]
        public int? ClientID { get; set; }


        /// <summary>
        /// Клиент
        /// </summary>
        [Comment("Клиент")]
        public Client? Client { get; set; }


        /// <summary>
        /// Телефон
        /// </summary>
        [Comment("Телефон")]
        public string? Phone { get; set; }


        /// <summary>
        /// Имя
        /// </summary>
        [Comment("Имя")]
        public string? Name { get; set; }


        /// <summary>
        /// Количество гостей
        /// </summary>
        [Comment("Количество гостей")]
        public int? NumberQuests { get; set; }


        /// <summary>
        /// Стол - Код
        /// </summary>
        [Comment("Стол - Код")]
        public int Restaurant_TableID { get; set; }


        /// <summary>
        /// Стол
        /// </summary>
        [Comment("Стол")]
        public Restaurant_Table? Restaurant_Table { get; set; }


        /// <summary>
        /// Дата начала
        /// </summary>
        [Comment("Дата начала")]
        public DateTime Date_Start { get; set; }


        /// <summary>
        /// Дата окончания
        /// </summary>
        [Comment("Дата окончания")]
        public DateTime Date_End { get; set; }


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        [Comment("Флаг пометки на удаление")]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        [Comment("Дата пометки на удаление")]
        public DateTime? Date_Del { get; set; }


        /// <summary>
        /// Пользователь, который поставил запись на удаление
        /// </summary>
        [Comment("Пользователь, который поставил запись на удаление")]
        public int? UserID_Del { get; set; }

    }
}