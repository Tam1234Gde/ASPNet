using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Объём
    /// </summary>
    public class CVolume_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Объём
        /// </summary>
        [DisplayName("Объём")]
        public decimal Value { get; set; }


        /// <summary>
        /// Наименование объёма
        /// </summary>
        [DisplayName("Наименование")]
        public string? Name { get; set; }
    }
}