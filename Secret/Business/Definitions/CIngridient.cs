using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Ингридиент
    /// </summary>
    public class CIngridient
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование ингридиента
        /// </summary>
        [DisplayName("Наименование ингридиента")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Дата изменения изображения
        /// </summary>
        public DateTime? Image_ChangeDate { get; set; }

    }
}