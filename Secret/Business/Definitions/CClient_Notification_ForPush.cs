namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Нотификация клиента
    /// </summary>
    public class CClient_Notification_ForPush
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Клиент, для которого нотификация - код
        /// </summary>
        public int? ClientID { get; set; }


        /// <summary>
        /// Наименование (заголовок) нотификации
        /// </summary>
        public string Name { get; set; } = null!;


    }
}