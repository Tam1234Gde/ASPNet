using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Restaurants
{
    /// <summary>
    /// Добавление\Изменение ресторана
    /// </summary>
    public class Model_Restaurants_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [Required(ErrorMessage = "Поле 'Адрес' обязательно к заполнению")]
        public string Adress { get; set; } = null!;

        /// <summary>
        /// Широта
        /// </summary>
        [Required(ErrorMessage = "Поле 'Широта' обязательно к заполнению")]
        public string Latitude { get; set; } = null!;


        /// <summary>
        /// Долгота
        /// </summary>
        [Required(ErrorMessage = "Поле 'Долгота' обязательно к заполнению")]
        public string Longitude { get; set; } = null!;


        /// <summary>
        /// Telegram - Обычный заказ - Код чата
        /// </summary>
        [DisplayName("Telegram - Код чата")]
        public string? TgChatID { get; set; }


        /// <summary>
        /// Telegram - Бронирование - Код чата
        /// </summary>
        [DisplayName("Telegram - Бронирование - Код чата")]
        public string? TgChatBookingID { get; set; }


        /// <summary>
        /// Telegram - Qr - Код чата
        /// </summary>
        [DisplayName("Telegram - Qr - Код чата")]
        public string? TgChatQrID { get; set; }


        /// <summary>
        /// Часы работы ресторана через ;
        /// </summary>
        [DisplayName("Часы работы ресторана через ;")]
        public string? Hours_Work { get; set; }


        /// <summary>
        /// Часы работы ресторана для заказа в приложении через ;
        /// </summary>
        [DisplayName("Часы работы ресторана для заказа в приложении через ;")]
        public string? Hours_WorkApp { get; set; }


        /// <summary>
        /// Часы загрузки работы ресторана через ;
        /// </summary>
        [DisplayName("Часы загрузки работы ресторана через ;")]
        public string? Hours_HardWork { get; set; }


        /// <summary>
        /// Часы работы ресторана для заказа бронирования через ;
        /// </summary>
        [DisplayName("Часы работы ресторана для заказа бронирования через ;")]
        public string? Hours_BookingWork { get; set; }


        /// <summary>
        /// На сколько можно забронировать VIP столик(в часах)
        /// </summary>
        [DisplayName("На сколько можно забронировать VIP столик(в часах)")]
        public int? BookingHour { get; set; }


        /// <summary>
        /// Телефон для переноса времени брони
        /// </summary>
        [DisplayName("Телефон для переноса времени брони")]
        public string? BookingPhone { get; set; }


        /// <summary>
        /// Время доставки в минутах
        /// </summary>
        [DisplayName("Время доставки в минутах")]
        public int? Delivery_Time { get; set; }


        /// <summary>
        /// Время срочной доставки в минутах
        /// </summary>
        [DisplayName("Время срочной доставки в минутах")]
        public int? Express_Delivery_Time { get; set; }


        /// <summary>
        /// Сумма доставки
        /// </summary>
        [DisplayName("Сумма доставки")]
        [Precision(8, 2)]
        public decimal? Delivery_Price { get; set; }


        /// <summary>
        /// True - Есть возможность поесть в кафе
        /// </summary>
        [DisplayName("Есть возможность поесть в кафе")]
        public bool Flag_Incafe { get; set; }


        /// <summary>
        /// True - Есть возможность забрать с собой
        /// </summary>
        [DisplayName("Есть возможность забрать с собой")]
        public bool Flag_Myself { get; set; }


        /// <summary>
        /// True - Возможна доставка
        /// </summary>
        [DisplayName("Возможна доставка")]
        public bool Flag_Delivery { get; set; }


        /// <summary>
        /// True - Есть возможность вынести к машине
        /// </summary>
        [DisplayName("Есть возможность вынести к машине")]
        public bool Flag_Car { get; set; }


        /// <summary>
        /// True - Активен
        /// </summary>
        [DisplayName("Активен")]
        public bool Flag_Active { get; set; }


        /// <summary>
        /// True - Отключен
        /// </summary>
        [DisplayName("Отключен")]
        public bool Flag_Hidden { get; set; }

        /// <summary>
        /// True - Скрыт
        /// </summary>
        [DisplayName("Скрыть")]
        public bool Flag_Hide { get; set; }


        /// <summary>
        /// Заголовок сообщения при отключенном ресторане
        /// </summary>
        [DisplayName("Заголовок сообщения при отключенном ресторане")]
        public string? HiddenTitle { get; set; }


        /// <summary>
        /// Тело сообщения при отключенном ресторане
        /// </summary>
        [DisplayName("Тело сообщения при отключенном ресторане")]
        public string? HiddenText { get; set; }


        /// <summary>
        /// Системный клиент(заказы с сайта) - Код
        /// </summary>
        [DisplayName("Системный клиент(заказы с сайта) - Код")]
        public int? ClientID { get; set; }


        /// <summary>
        /// Список городов
        /// </summary>
        public List<CCity>? List_Cities { get; set; }


        /// <summary>
        /// Город
        /// </summary>
        public int? CityID { get; set; }


        /// <summary>
        /// Юкасса > Логин
        /// </summary>
        [DisplayName("Юкасса > Логин")]
        public string? YouKassaLogin { get; set; }


        /// <summary>
        /// Юкасса > Пароль
        /// </summary>
        [DisplayName("Юкасса > Пароль")]
        public string? YouKassaPassword { get; set; }

    }
}