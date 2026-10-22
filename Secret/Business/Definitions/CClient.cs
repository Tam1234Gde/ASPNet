using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Клиент
    /// </summary>
    public class CClient
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Телефон - используется как логин
        /// </summary>
        [DisplayName("Телефон")]
        public string Phone { get; set; } = null!;


        /// <summary>
        /// Guid пользователя
        /// </summary>
        [DisplayName("Guid пользователя")]
        public string Guid { get; set; } = null!;


        /// <summary>
        /// Имя клиента
        /// </summary>
        [DisplayName("Имя")]
        public string? Name { get; set; } = null!;


        /// <summary>
        /// Дата рождения
        /// </summary>
        [DisplayName("Дата рождения")]
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
        /// Ссылка на изображение
        /// </summary>
        public byte[]? Photo { get; set; }


        /// <summary>
        /// Политика принята
        /// </summary>
        [DisplayName("Политика принята")]
        public bool Flag_IsPolicy { get; set; }


        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string? Image_Url { get; set; }


        /// <summary>
        /// Дата обновления фотографии
        /// </summary>
        public DateTime? Date_PhotoUpdate { get; set; }


        /// <summary>
        /// Данные банковской карты
        /// </summary>
        [DisplayName("Данные банковской карты")]
        public string? CardData { get; set; }


        /// <summary>
        /// Смс код
        /// </summary>
        [DisplayName("Смс код")]
        public string SmsCode { get; set; } = null!;


        /// <summary>
        /// Город
        /// </summary>
        [DisplayName("Город")]
        public string? City { get; set; }


        /// <summary>
        /// Общая сумма заказов
        /// </summary>
        [DisplayName("Общая сумма заказов")]
        public decimal? OrdersSum { get; set; }


        /// <summary>
        /// Дата регистрации
        /// </summary>
        [DisplayName("Дата регистрации")]
        public DateTime? Date_Registration { get; set; }


        /// <summary>
        /// Дата последнего входа в приложение
        /// </summary>
        [DisplayName("Дата последнего входа в приложение")]
        public DateTime Date_LastAction { get; set; }


        /// <summary>
        /// Дата последнего заказа
        /// </summary>
        [DisplayName("Дата последнего заказа")]
        public DateTime? Date_LastZakaz { get; set; }


        /// <summary>
        /// Дата последнего изменения информации клиента
        /// </summary>
        public DateTime Date_LastUpdate { get; set; }


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        public DateTime? Date_Del { get; set; }


        /// <summary>
        /// Пользователь, который поставил запись на удаление
        /// </summary>
        public int? UserID_Del { get; set; }

    }
}