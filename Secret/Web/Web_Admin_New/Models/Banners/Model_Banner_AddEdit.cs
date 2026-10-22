using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Banners
{
    public class Model_Banner_AddEdit
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
        public IFormFile? Image_Big { get; set; }

        /// <summary>
        /// Баннер большой byte
        /// </summary>
        public byte[]? Image_Big_Byte { get; set; }

        /// <summary>
        /// Ссылка на баннер большой
        /// </summary>
        public string? Image_Big_Url { get; set; }

        /// <summary>
        /// Баннер маленький
        /// </summary>
        public IFormFile? Image_Small { get; set; }

        /// <summary>
        /// Баннер маленький byte
        /// </summary>
        public byte[]? Image_Small_Byte { get; set; }

        /// <summary>
        /// Ссылка на баннер маленький
        /// </summary>
        public string? Image_Small_Url { get; set; }

        /// <summary>
        /// Город - код
        /// </summary>
        public int CityID { get; set; }

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
        public string? OrderNum { get; set; }

        /// <summary>
        /// Список городов
        /// </summary>
        public List<CCity>? List_Cities { get; set; }

        /// <summary>
        /// Список категорий
        /// </summary>
        public List<CDish_Category>? List_Categories { get; set; }


        /// <summary>
        /// Список продуктов
        /// </summary>
        public List<CDish>? List_Product { get; set; }
    }
}
