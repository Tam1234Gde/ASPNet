using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Пользователь в системе
    /// </summary>
    [Table("Users")]
    [Comment("Пользователи")]
    public class User
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Логин
        /// </summary>
        [StringLength(50)]
        [Comment("Логин")]
        public string Login { get; set; } = null!;


        /// <summary>
        /// Пароль
        /// </summary>
        [StringLength(50)]
        [Comment("Пароль")]
        public string Password { get; set; } = null!;


        /// <summary>
        /// ФИО пользователя
        /// </summary>
        [StringLength(100)]
        [Comment("ФИО пользователя")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Флаг суперадмин
        /// </summary>
        [Comment("Флаг суперадмин")]
        public bool Flag_Root { get; set; }


        /// <summary>
        /// Флаг админ
        /// </summary>
        [Comment("Флаг админ")]
        public bool Flag_Admin { get; set; }


        /// <summary>
        /// Флаг оператор
        /// </summary>
        [Comment("Флаг оператор")]
        public bool Flag_Oper { get; set; }


        /// <summary>
        /// Код ресторана
        /// </summary>
        [Comment("Код ресторана")]
        public int? RestaurantID { get; set; }


        /// <summary>
        /// Ресторан
        /// </summary>
        [Comment("Ресторан")]
        public Restaurant? Restaurant { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        [Comment("Дата пометки на удаление")]
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