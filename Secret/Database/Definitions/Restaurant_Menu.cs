using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Меню ресторана
    /// </summary>
    [Table("Restaurant_Menus")]
    [Comment("Меню ресторана>")]
    public class Restaurant_Menu
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
        /// Название меню
        /// </summary>
        [Comment("Название меню")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Активен
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