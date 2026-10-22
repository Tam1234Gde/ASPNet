using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class CUser
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [StringLength(50)]
        [DisplayName("Логин")]
        public string Login { get; set; } = null!;

        /// <summary>
        /// Пароль
        /// </summary>
        [StringLength(50)]
        [DisplayName("Пароль")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// ФИО пользователя
        /// </summary>
        [StringLength(100)]
        [DisplayName("ФИО пользователя")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Флаг суперадмин
        /// </summary>
        [DisplayName("Суперадмин")]
        public bool Flag_Root { get; set; }


        /// <summary>
        /// Флаг админ
        /// </summary>
        [DisplayName("Админ")]
        public bool Flag_Admin { get; set; }


        /// <summary>
        /// Флаг оператор
        /// </summary>
        [DisplayName("Оператор")]
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
        public string? Restaurant { get; set; }

    }
}