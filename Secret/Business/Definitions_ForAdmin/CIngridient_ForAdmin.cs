using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Ингредиент
    /// </summary>
    public class CIngridient_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Наименование ингредиента
        /// </summary>
        [DisplayName("Наименование ингредиента")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Изображение ингредиента
        /// </summary>
        [DisplayName("Изображение")]
        public byte[]? Image { get; set; }


        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        [DisplayName("Ссылка на изображение")]
        public string? Image_Url { get; set; }


        /// <summary>
        /// Идентификатор категории
        /// </summary>
        [DisplayName("Код категории")]
        public int? Category_ID { get; set; }


        /// <summary>
        /// Категория
        /// </summary>
        [DisplayName("Категория")]
        public string? Category_Name { get; set; }


        /// <summary>
        /// Флаг: удалена ли категория
        /// </summary>
        [DisplayName("Категория удалена")]
        public bool Category_IsDeleted { get; set; }
    }
}