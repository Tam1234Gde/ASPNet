using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Баннер
    /// </summary>
    public class CBanner
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Баннер большой
        /// </summary>
        public byte[]? Image_Big { get; set; }

        /// <summary>
        /// Баннер маленький
        /// </summary>
        public byte[]? Image_Small { get; set; }

        /// <summary>
        /// Город - код
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        public virtual City? City { get; set; }

        /// <summary>
        /// Категория - ID
        /// </summary>
        public int? Category_ID { get; set; }

        /// <summary>
        /// Продукт - ID
        /// </summary>
        public int? Product_ID { get; set; }

        /// <summary>
        /// Дата начала показа
        /// </summary>
        public DateTime? Date_Start { get; set; }

        /// <summary>
        /// Дата окончания показа
        /// </summary>
        public DateTime? Date_End { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime? Date_Create { get; set; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public int OrderNum { get; set; }

    }
}
