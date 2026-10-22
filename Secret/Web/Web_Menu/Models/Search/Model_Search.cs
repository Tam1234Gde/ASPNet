using HIDE.RestAndRant.Business.Definitions_ForAdmin;

namespace HIDE.RestAndRant.Web_Site_Menu.Models.Search
{
    /// <summary>
    /// Поиск
    /// </summary>
    public class Model_Search
    {
        /// <summary>
        /// Искомое блюдо
        /// </summary>
        public string? Search { get; set; }


        /// <summary>
        /// Список найденных товаров
        /// </summary>
        public List<CDish_ForAdmin>? List_Dishes { get; set; }

    }
}