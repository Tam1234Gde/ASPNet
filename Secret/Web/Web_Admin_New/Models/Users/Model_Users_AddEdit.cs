using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Users
{
    /// <summary>
    /// Добавление\Изменение пользователя
    /// </summary>
    public class Model_Users_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [DisplayName("Логин")]
        [StringLength(50)]
        [Required(ErrorMessage = "Поле 'Логин' обязательно к заполнению")]
        public string Login { get; set; } = null!;

        /// <summary>
        /// Пароль
        /// </summary>
        [DisplayName("Пароль")]
        [StringLength(50)]
        [Required(ErrorMessage = "Поле 'Пароль' обязательно к заполнению")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// ФИО пользователя
        /// </summary>
        [DisplayName("ФИО пользователя")]
        [StringLength(100)]
        public string? Name { get; set; } = null!;

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
        /// Список ресторанов
        /// </summary>
        [Comment("Список ресторанов")]
        public List<CRestaurant>? List_Restaurants { get; set; }

    }
}
