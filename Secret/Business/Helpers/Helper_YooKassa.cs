using HIDE.RestAndRant.Business_YooKassa;
using HIDE.RestAndRant.Common;

namespace HIDE.RestAndRant.Business.Helpers
{
    /// <summary>
    /// Помощник с Юкассой
    /// </summary>
    public static class Helper_YooKassa
    {
        /// <summary>
        /// Объект для работы с Юкассой
        /// </summary>
        public static Yookassa YooKassa { get; set; } = new Yookassa(
            Login: AppConfig.Yookassa_Login,
            Password: AppConfig.Yookassa_Password);
    }
}