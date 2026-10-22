using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Banners
{
    public class Model_Banners
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Список банеров
        /// </summary>
        public List<CBanner>? List_Banners { get; set; }


        /// <summary>
        /// Список городов
        /// </summary>
        public List<CCity>? List_Cities { get; set; }

    }
}
