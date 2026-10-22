using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Клиент
    /// </summary>
    [Table("Clients")]
    [Comment("Клиенты системы")]
    [Index(nameof(Phone), IsUnique = true)]
    public class Client
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Guid пользователя
        /// </summary>
        [StringLength(40)]
        [Comment("Guid")]
        public string Guid { get; set; } = null!;


        /// <summary>
        /// Телефон - используется как логин
        /// </summary>
        [Comment("Телефон")]
        public string Phone { get; set; } = null!;


        /// <summary>
        /// Смс код
        /// </summary>
        [Comment("Смс код")]
        public string Code_Phone { get; set; } = null!;


        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Comment("Имя пользователя")]
        public string? Name { get; set; }


        /// <summary>
        /// Дата рождения
        /// </summary>
        [Comment("Дата рождения")]
        public DateTime? Date_Birth { get; set; }


        /// <summary>
        /// Имя - Другому человеку
        /// </summary>
        [Comment("Имя - Другому человеку")]
        public string? Name_Another { get; set; }


        /// <summary>
        /// Телефон - Другому человеку
        /// </summary>
        [Comment("Телефон - Другому человеку")]
        public string? Phone_Another { get; set; }


        /// <summary>
        /// Марка машины
        /// </summary>
        [Comment("Марка машины")]
        public string? Car_Brand { get; set; }


        /// <summary>
        /// Госномера машины
        /// </summary>
        [Comment("Госномера машины")]
        public string? Car_RegNumber { get; set; }


        /// <summary>
        /// Цвет машины
        /// </summary>
        [Comment("Цвет машины")]
        public string? Car_Color { get; set; }


        /// <summary>
        /// Адрес доставки
        /// </summary>
        [Comment("Адрес доставки")]
        public string? Delivery_Address { get; set; }


        /// <summary>
        /// Квартира
        /// </summary>
        [Comment("Квартира")]
        public string? Delivery_Apartment { get; set; }


        /// <summary>
        /// Домофон
        /// </summary>
        [Comment("Домофон")]
        public string? Delivery_EntranceCode { get; set; }


        /// <summary>
        /// Номер подъезда
        /// </summary>
        [Comment("Номер подъезда")]
        public string? Delivery_EntranceNumber { get; set; }


        /// <summary>
        /// Номер этажа
        /// </summary>
        [Comment("Номер этажа")]
        public string? Delivery_Floor { get; set; }


        /// <summary>
        /// Аватар пользователя
        /// </summary>
        [Comment("Аватар пользователя")]
        public byte[]? Photo { get; set; }


        /// <summary>
        /// Дата обновления фотографии
        /// </summary>
        [Comment("Дата обновления фотографии")]
        public DateTime? Date_PhotoUpdate { get; set; }


        /// <summary>
        /// True - Политика принята, False - Политика не принята
        /// </summary>
        [Comment("Политика принята")]
        public bool Flag_IsPolicy { get; set; }


        /// <summary>
        /// Дата регистрации
        /// </summary>
        [Comment("Дата регистрации")]
        public DateTime? Date_Registration { get; set; }


        /// <summary>
        /// Дата последнего входа в приложение
        /// </summary>
        [Comment("Дата последнего входа в приложение")]
        public DateTime Date_LastAction { get; set; }


        /// <summary>
        /// Дата последнего заказа
        /// </summary>
        [Comment("Дата последнего заказа")]
        public DateTime? Date_LastZakaz { get; set; }


        /// <summary>
        /// Дата последнего изменения информации клиента
        /// </summary>
        [Comment("Дата последнего изменения информации клиента")]
        public DateTime Date_LastUpdate { get; set; }


        /// <summary>
        /// Общая сумма заказов
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Comment("Общая сумма заказов")]
        public decimal? OrdersSum
        {
            get
            {
                return List_Orders?.Sum(p => p.Order_Sum);
            }
        }


        /// <summary>
        /// Город
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Comment("Последняя дата заказа")]
        public string? OrdersCity
        {
            get
            {
                return List_Orders?.OrderByDescending(p => p.Date_Act).FirstOrDefault()?.Restaurant?.City?.Name;
            }
        }


        /// <summary>
        /// Последняя дата заказа
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Comment("Последняя дата заказа")]
        public DateTime? OrdersLastDate
        {
            get
            {
                return List_Orders?.OrderByDescending(p => p.Date_Act).FirstOrDefault()?.Date_Act;
            }
        }


        /// <summary>
        /// Заказы
        /// </summary>
        [Comment("Заказы")]
        public List<Order>? List_Orders { get; set; } //TODO: MAE - тут точно нужен Nullable?


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