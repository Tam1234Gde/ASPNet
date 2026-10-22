using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json.Serialization;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Столик в ресторане
    /// </summary>
    public class CRestaurant_Table
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Ресторан - код
        /// </summary>
        [DisplayName("Ресторан - код")]
        public int RestaurantID { get; set; }

        /// <summary>
        /// Ресторан
        /// </summary>
        [DisplayName("Ресторан")]
        public string? Restaurant { get; set; }

        /// <summary>
        /// Тип столика - код
        /// </summary>
        [DisplayName("Тип столика - код")]
        public ERestaurant_Table_Type? Restaurant_Table_TypeID { get; set; }

        /// <summary>
        /// Тип столика
        /// </summary>
        [DisplayName("Тип столика")]
        public string? Restaurant_Table_Type { get; set; }


        /// <summary>
        /// Код планировки
        /// </summary>
        [DisplayName("Код планировки")]
        public int? Restaurant_HallID { get; set; }


        /// <summary>
        /// Планировка
        /// </summary>
        [DisplayName("Код планировки")]
        public string? Restaurant_Hall { get; set; }


        /// <summary>
        /// Стоимость бронирования
        /// </summary>
        [Precision(8, 2)]
        [DisplayName("Стоимость бронирования")]
        public decimal? Price { get; set; }


        /// <summary>
        /// Наименование стола
        /// </summary>
        [DisplayName("Наименование стола")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Координаты стола для админки
        /// </summary>
        [DisplayName("Координаты стола")]
        public string? Coordinates { get; set; }

        /// <summary>
        /// Флаг доступности к заказу через приложение
        /// </summary>
        [DisplayName("Доступен к заказу через приложение")]
        public bool Flag_IsAvailableApp { get; set; }

        /// <summary>
        /// Флаг доступности к заказу через официанта
        /// </summary>
        [DisplayName("Доступен к заказу через официанта")]
        public bool Flag_IsAvailable { get; set; }


        /// <summary>
        /// Статус столика(Не забронирован\Забронирован\Недоступен) - код
        /// </summary>
        [DisplayName("Статус столика - код")]
        public ERestaurant_Table_Status? Restaurant_Table_StatusID { get; set; }


        /// <summary>
        /// Координаты X стола для приложения
        /// </summary>
        [DisplayName("Координаты X стола для приложения")]
        public decimal? Coordinates_X { get; set; }


        /// <summary>
        /// Координаты Y стола для приложения
        /// </summary>
        [DisplayName("Координаты Y стола для приложения")]
        public decimal? Coordinates_Y { get; set; }


        /// <summary>
        /// Ротация стола для приложения
        /// </summary>
        [DisplayName("Ротация стола для приложения")]
        public decimal? Coordinates_Rotation { get; set; }


        /// <summary>
        /// Дополнительный текст
        /// </summary>
        [DisplayName("Дополнительный текст")]
        public string? Dop_Text { get; set; }


        /// <summary>
        /// Уникальный код столика для QR-кода
        /// </summary>
        [Comment("Уникальный код столика для QR-кода")]
        [JsonIgnore]
        public string? QR_Code { get; set; }

    }
}