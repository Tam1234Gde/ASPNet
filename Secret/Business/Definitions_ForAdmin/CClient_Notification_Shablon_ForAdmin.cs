using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Шаблон уведомления для клиента
    /// </summary>
    public class CClient_Notification_Shablon_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Клиент, для которого нотификация - код
        /// </summary>
        [DisplayName("Код клиента")]
        public int ClientID { get; set; }


        /// <summary>
        /// Дата генерации нотификации
        /// </summary>
        [DisplayName("Дата генерации")]
        public DateTime Date_Act { get; set; }


        /// <summary>
        /// Наименование (заголовок) нотификации
        /// </summary>
        [DisplayName("Заголовок")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Содержимое нотификации
        /// </summary>
        [DisplayName("Содержимое")]
        public string Content { get; set; } = null!;


        /// <summary>
        /// Флаг прочтения этого уведомления
        /// </summary>
        [DisplayName("Прочитано")]
        public bool Flag_Read { get; set; }


        /// <summary>
        /// Пуш-уведомления: Флаг того, что нужно отправлять пуш
        /// </summary>
        [DisplayName("Отправить пуш")]
        public bool Push_Need { get; set; }
    }
}