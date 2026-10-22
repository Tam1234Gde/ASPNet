using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Clients
{
    /// <summary>
    /// Добавление\Изменение клиента
    /// </summary>
    public class Model_Clients_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Телефон - используется как логин
        /// </summary>
        [Required(ErrorMessage = "Поле 'Телефон' обязательно к заполнению")]
        [RegularExpression(@"^\(?([7]{1})\)?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Поле 'Телефон' указано неверно. Пример записи: 79001234567")]
        public string Phone { get; set; } = null!;

        /// <summary>
        /// Имя
        /// </summary>
        public string? Name { get; set; } = null!;


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
        /// Код подъезда
        /// </summary>
        [Comment("Код подъезда")]
        public string? Delivery_EntranceCode { get; set; }


        /// <summary>
        /// Домофон
        /// </summary>
        [Comment("Домофон")]
        public string? Delivery_EntranceNumber { get; set; }


        /// <summary>
        /// Номер этажа
        /// </summary>
        [Comment("Номер этажа")]
        public string? Delivery_Floor { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public string? Date_Birth { get; set; }

        /// <summary>
        /// Количество текущих бонусов у клиента (каждая 10 кружка в подарок)
        /// </summary>
        public int? Bonuses { get; set; }

        /// <summary>
        /// True - Политика принята, False - Политика не принята
        /// </summary>
        public bool Flag_IsPolicy { get; set; }

        /// <summary>
        /// Смс код
        /// </summary>
        public string? SmsCode { get; set; } = null!;

        /// <summary>
        /// Guid пользователя
        /// </summary>
        public string? Guid { get; set; } = null!;

        /// <summary>
        /// Дата последнего входа в приложение
        /// </summary>
        public DateTime Date_LastAction { get; set; }

        /// <summary>
        /// Дата последнего заказа
        /// </summary>
        public DateTime? Date_LastZakaz { get; set; }

        /// <summary>
        /// Изображение
        /// </summary>
        public IFormFile? UploadImge { get; set; }

        /// <summary>
        /// Изображение товара
        /// </summary>
        public byte[]? Image { get; set; }

        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string? Image_Url { get; set; }

    }
}
