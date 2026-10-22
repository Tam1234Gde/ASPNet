using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Стол в ресторане
    /// </summary>
    [Table("Restaurant_Tables")]
    [Comment("Столы в ресторане")]
    public class Restaurant_Table
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Ресторан - код
        /// </summary>
        [Comment("Ресторан - код")]
        public int RestaurantID { get; set; }


        /// <summary>
        /// Ресторан
        /// </summary>
        [Comment("Ресторан")]
        public Restaurant Restaurant { get; set; } = null!;


        /// <summary>
        /// Тип столика - код
        /// </summary>
        [Comment("Тип столика - код")]
        public ERestaurant_Table_Type? Restaurant_Table_TypeID { get; set; }


        /// <summary>
        /// Тип столика
        /// </summary>
        [Comment("Тип столика")]
        public Restaurant_Table_Type? Restaurant_Table_Type { get; set; }


        /// <summary>
        /// Статус столика - код
        /// </summary>
        [Comment("Статус столика - код")]
        public ERestaurant_Table_Status? Restaurant_Table_StatusID { get; set; }


        /// <summary>
        /// Статус столика
        /// </summary>
        [Comment("Статус столика")]
        public Restaurant_Table_Status? Restaurant_Table_Status { get; set; }


        /// <summary>
        /// Код планировки
        /// </summary>
        [Comment("Код планировки")]
        public int? Restaurant_HallID { get; set; }


        /// <summary>
        /// Планировка
        /// </summary>
        [Comment("Планировка")]
        public Restaurant_Hall? Restaurant_Hall { get; set; }


        /// <summary>
        /// Стоимость бронирования
        /// </summary>
        [Precision(8, 2)]
        [Comment("Стоимость бронирования")]
        public decimal? Price { get; set; }


        /// <summary>
        /// Наименование стола
        /// </summary>
        [Comment("Наименование стола")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Координаты стола для админки
        /// </summary>
        [Comment("Координаты стола для админки")]
        public string? Coordinates { get; set; }


        /// <summary>
        /// Координаты X стола для приложения
        /// </summary>
        [Precision(8, 2)]
        [Comment("Координаты X стола для приложения")]
        public decimal? Coordinates_X { get; set; }


        /// <summary>
        /// Координаты Y стола для приложения
        /// </summary>
        [Precision(8, 2)]
        [Comment("Координаты Y стола для приложения")]
        public decimal? Coordinates_Y { get; set; }


        /// <summary>
        /// Ротация стола для приложения
        /// </summary>
        [Precision(8, 2)]
        [Comment("Ротация стола для приложения")]
        public decimal? Coordinates_Rotation { get; set; }


        /// <summary>
        /// Дополнительный текст
        /// </summary>
        [Comment("Ротация стола для приложения")]
        public string? Dop_Text { get; set; }


        /// <summary>
        /// Флаг доступности к заказу через приложение
        /// </summary>
        [Comment("Флаг доступности к заказу через приложение")]
        public bool Flag_IsAvailableApp { get; set; }


        /// <summary>
        /// Флаг доступности к заказу через официанта
        /// </summary>
        [Comment("Флаг доступности к заказу через официанта")]
        public bool Flag_IsAvailable { get; set; }


        /// <summary>
        /// Уникальный код столика для QR-кода
        /// </summary>
        [Comment("Уникальный код столика для QR-кода")]
        public string? QR_Code { get; set; }


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