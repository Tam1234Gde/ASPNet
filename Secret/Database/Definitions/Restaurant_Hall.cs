using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Зал в ресторане
    /// </summary>
    [Table("Restaurant_Halls")]
    [Comment("Залы в ресторане")]
    public class Restaurant_Hall
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Код ресторана
        /// </summary>
        [Comment("Код ресторана")]
        public int RestaurantID { get; set; }


        /// <summary>
        /// Ресторан
        /// </summary>
        [Comment("Ресторан")]
        public Restaurant Restaurant { get; set; } = null!;


        /// <summary>
        /// Название
        /// </summary>
        [Comment("Название")]
        public string? Name { get; set; }


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
        /// Планировка
        /// </summary>
        [Comment("Планировка")]
        public byte[]? Restaurant_Image { get; set; }


        /// <summary>
        /// True - Активен
        /// </summary>
        [Comment("Активен")]
        public bool Flag_Active { get; set; }


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