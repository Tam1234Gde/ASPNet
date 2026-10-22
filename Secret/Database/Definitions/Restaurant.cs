using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Ресторан
    /// </summary>
    [Table("Restaurants")]
    [Comment("Рестораны")]
    public class Restaurant
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Город - Код
        /// </summary>
        [Comment("Город - Код")]
        public int? CityID { get; set; }


        /// <summary>
        /// Город
        /// </summary>
        [Comment("Город")]
        public City? City { get; set; }


        /// <summary>
        /// Адрес
        /// </summary>
        [Comment("Адрес")]
        public string Adress { get; set; } = null!;


        /// <summary>
        /// Широта
        /// </summary>
        [Comment("Широта")]
        public string Latitude { get; set; } = null!;


        /// <summary>
        /// Долгота
        /// </summary>
        [Comment("Долгота")]
        public string Longitude { get; set; } = null!;


        /// <summary>
        /// Telegram - Обычный заказ - Код чата
        /// </summary>
        [Comment("Telegram - Код чата")]
        public string? TgChatID { get; set; }


        /// <summary>
        /// Telegram - Бронирование - Код чата
        /// </summary>
        [Comment("Telegram - Бронирование - Код чата")]
        public string? TgChatBookingID { get; set; }


        /// <summary>
        /// Telegram - Qr - Код чата
        /// </summary>
        [Comment("Telegram - Qr - Код чата")]
        public string? TgChatQrID { get; set; }


        /// <summary>
        /// Часы работы ресторана через ;
        /// </summary>
        [Comment("Часы работы ресторана через ;")]
        public string? Hours_Work { get; set; }


        /// <summary>
        /// Часы работы ресторана для заказа в приложении через ;
        /// </summary>
        [Comment("Часы работы ресторана для заказа в приложении через ;")]
        public string? Hours_WorkApp { get; set; }


        /// <summary>
        /// Часы загрузки работы ресторана через ;
        /// </summary>
        [Comment("Часы загрузки работы ресторана через ;")]
        public string? Hours_HardWork { get; set; }


        /// <summary>
        /// Часы работы ресторана для заказа бронирования через ;
        /// </summary>
        [Comment("Часы работы ресторана для заказа бронирования через ;")]
        public string? Hours_BookingWork { get; set; }


        /// <summary>
        /// На сколько можно забронировать VIP столик(в часах)
        /// </summary>
        [Comment("На сколько можно забронировать VIP столик(в часах)")]
        public int? BookingHour { get; set; }


        /// <summary>
        /// Телефон для переноса времени брони
        /// </summary>
        [Comment("Телефон для переноса времени брони")]
        public string? BookingPhone { get; set; }


        /// <summary>
        /// Время доставки в минутах
        /// </summary>
        [Comment("Время доставки в минутах")]
        public int? Delivery_Time { get; set; }


        /// <summary>
        /// Время срочной доставки в минутах
        /// </summary>
        [Comment("Время срочной доставки в минутах")]
        public int? Express_Delivery_Time { get; set; }


        /// <summary>
        /// Стоимость доставки
        /// </summary>
        [Comment("Стоимость доставки")]
        [Precision(8, 2)]
        public decimal? Delivery_Price { get; set; }


        /// <summary>
        /// True - Есть возможность поесть в кафе
        /// </summary>
        [Comment("Есть возможность поесть в кафе")]
        public bool Flag_Incafe { get; set; }


        /// <summary>
        /// True - Есть возможность забрать с собой
        /// </summary>
        [Comment("Есть возможность забрать с собой")]
        public bool Flag_Myself { get; set; }


        /// <summary>
        /// True - Возможна доставка
        /// </summary>
        [Comment("Возможна доставка")]
        public bool Flag_Delivery { get; set; }


        /// <summary>
        /// True - Есть возможность вынести к машине
        /// </summary>
        [Comment("Есть возможность вынести к машине")]
        public bool Flag_Car { get; set; }


        /// <summary>
        /// Системный клиент(заказы с сайта) - Код
        /// </summary>
        [Comment("Системный клиент(заказы с сайта) - Код")]
        public int? ClientID { get; set; }


        /// <summary>
        /// Системный клиент(заказы с сайта)
        /// </summary>
        [Comment("Системный клиент(заказы с сайта)")]
        public Client? Client { get; set; }


        /// <summary>
        /// True - Отключен
        /// </summary>
        [Comment("Отключен")]
        public bool Flag_Hidden { get; set; }

        /// <summary>
        /// True - Скрыт
        /// </summary>
        [Comment("Скрыт")]
        public bool Flag_Hide { get; set; }


        /// <summary>
        /// Заголовок сообщения при отключенном ресторане
        /// </summary>
        [Comment("Заголовок сообщения при отключенном ресторане")]
        public string? HiddenTitle { get; set; }


        /// <summary>
        /// Тело сообщения при отключенном ресторане
        /// </summary>
        [Comment("Тело сообщения при отключенном ресторане")]
        public string? HiddenText { get; set; }


        /// <summary>
        /// Юкасса > Логин
        /// </summary>
        [Comment("Юкасса > Логин")]
        public string? YouKassaLogin { get; set; }


        /// <summary>
        /// Юкасса > Пароль
        /// </summary>
        [Comment("Юкасса > Пароль")]
        public string? YouKassaPassword { get; set; }


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