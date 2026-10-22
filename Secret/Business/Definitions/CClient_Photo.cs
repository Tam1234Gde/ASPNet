namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Клиент
    /// </summary>
    public class CClient_Photo
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Дата обновления фотографии
        /// </summary>
        public DateTime? Date_PhotoUpdate { get; set; }


        /// <summary>
        /// Аватар пользователя
        /// </summary>
        public byte[]? Photo { get; set; }


    }
}