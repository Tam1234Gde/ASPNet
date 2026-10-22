using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Цвет
    /// </summary>
    public class CColor
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Наименование цвета
        /// </summary>
        [DisplayName("Наименование цвета")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Наименование цвета - англ
        /// </summary>
        [DisplayName("Наименование цвета - EN")]
        public string Name_Eng { get; set; } = null!;


        /// <summary>
        /// Цвет стакана
        /// </summary>
        [DisplayName("Цвет стакана")]
        public string HexColor { get; set; } = null!;

    }
}