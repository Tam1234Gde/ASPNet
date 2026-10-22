using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.RestaurantTables
{
    /// <summary>
    /// Добавление\Изменение столика в ресторане
    /// </summary>
    public class Model_RestaurantTables_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Ресторан - код
        /// </summary>
        [Required(ErrorMessage = "Поле 'Ресторан' обязательно к заполнению")]
        public int RestaurantID { get; set; }

        /// <summary>
        /// Ресторан
        /// </summary>
        public string? Restaurant { get; set; }

        /// <summary>
        /// Тип столика - код
        /// </summary>
        [DisplayName("Тип столика - код")]
        public ERestaurant_Table_Type? Restaurant_Table_TypeID { get; set; }

        /// <summary>
        /// Стоимость бронирования
        /// </summary>
        [Precision(8, 2)]
        [DisplayName("Стоимость бронирования")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Наименование стола
        /// </summary>
        [Required(ErrorMessage = "Поле 'Наименование стола' обязательно к заполнению")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Координаты стола для админки
        /// </summary>
        [DisplayName("Координаты стола для админки")]
        public string? Coordinates { get; set; }

        /// <summary>
        /// Код планировки
        /// </summary>
        [DisplayName("Код планировки")]
        public int? Restaurant_HallID { get; set; }

        /// <summary>
        /// Флаг доступности к заказу через приложение
        /// </summary>
        [DisplayName("Флаг доступности к заказу через приложение")]
        public bool Flag_IsAvailableApp { get; set; }

        /// <summary>
        /// Флаг доступности к заказу через официанта
        /// </summary>
        [DisplayName("Флаг доступности к заказу через официанта")]
        public bool Flag_IsAvailable { get; set; }

        /// <summary>
        /// Координаты X стола для приложения
        /// </summary>
        [Precision(8, 2)]
        [DisplayName("Координаты X стола для приложения")]
        public decimal? Coordinates_X { get; set; }


        /// <summary>
        /// Координаты Y стола для приложения
        /// </summary>
        [Precision(8, 2)]
        [DisplayName("Координаты Y стола для приложения")]
        public decimal? Coordinates_Y { get; set; }


        /// <summary>
        /// Ротация стола для приложения
        /// </summary>
        [Precision(8, 2)]
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
        public string? QR_Code { get; set; }


        /// <summary>
        /// Список ресторанов
        /// </summary>
        public List<CRestaurant>? List_Restaurants { get; set; }

        /// <summary>
        /// Типы столиков
        /// </summary>
        public List<CCommon>? List_RestaurantTableTypes { get; set; }

        /// <summary>
        /// Статус столика - код
        /// </summary>
        [DisplayName("Статус столика - код")]
        public ERestaurant_Table_Status? Restaurant_Table_StatusID { get; set; }

        /// <summary>
        /// Статусы столиков
        /// </summary>
        public List<CRestaurant_Table_Status>? List_RestaurantTableStatuses { get; set; }


        /// <summary>
        /// Список залов
        /// </summary>
        public List<CRestaurant_Hall>? List_Restaurant_Halls { get; set; }

    }
}
