using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Auth
{
    /// <summary>
    /// Авторизация
    /// </summary>
    public class Model_Auth_Login
    {
        /// <summary>
        /// EMail
        /// </summary>
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Это поле обязательно к заполнению")]
        public string EMail { get; set; } = "";

        /// <summary>
        /// Password
        /// </summary>
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Это поле обязательно к заполнению")]
        public string Password { get; set; } = "";

        /// <summary>
        /// Запомнить пользователя
        /// </summary>
        [Display(Name = "Flag_Remember")]
        [Required(ErrorMessage = "Это поле обязательно к заполнению")]
        public bool Flag_Remember { get; set; }

        /// <summary>
        /// Url, куда возвращаться после успешного логина
        /// </summary>
        public string? ReturnUrl { get; set; }

        /// <summary>
        /// Флаг для отладки
        /// </summary>
        public bool Flag_Debug { get; set; }

    }
}
