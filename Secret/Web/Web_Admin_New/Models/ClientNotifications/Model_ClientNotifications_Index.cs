using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.ClientNotifications
{
    /// <summary>
    /// Уведомления клиента
    /// </summary>
    public class Model_ClientNotifications_Index
    {
        /// <summary>
        /// Клиент
        /// </summary>
        public CClient? Client { get; set; }

        /// <summary>
        /// Список уведомлений
        /// </summary>
        public List<CClient_Notification>? List_ClientNotifications { get; set; }

    }
}