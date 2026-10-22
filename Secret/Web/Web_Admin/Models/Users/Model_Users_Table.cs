using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Users
{

    /// <summary>
    /// Модель для отображения таблицы пользователей
    /// </summary>
    public class Model_Users_Table
    {
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public CUser User { get; set; } = null!;


        /// <summary>
        /// Список пользователей
        /// </summary>
        public List<CUser> List_Users { get; set; } = [];
    }
}
