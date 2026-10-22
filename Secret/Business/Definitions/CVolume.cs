using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Объём
    /// </summary>
    public class CVolume
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        [DisplayName("Значение")]
        public decimal Value { get; set; }


        /// <summary>
        /// Дополнительное описание
        /// </summary>
        [DisplayName("Дополнительное описание")]
        public string? Name { get; set; }

    }
}