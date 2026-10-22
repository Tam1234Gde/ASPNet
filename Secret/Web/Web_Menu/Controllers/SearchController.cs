using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Web_Site_Menu.Models.Search;
using static HIDE.RestAndRant.Web_Site_Menu.Helpers.Helper_Client;

namespace HIDE.RestAndRant.Web_Site_Menu.Controllers
{
    /// <summary>
    /// Поиск по блюдам
    /// </summary>
    public class SearchController(BL_Context _bl) : BaseControllerWithUser
    {
        /// <summary>
        /// Страница поиска
        /// </summary>
        /// <param name="Search">Искомое блюдо</param>
        /// <returns></returns>
        public IActionResult Search_Index(string? Search)
        {
            Model_Search model = new();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                model.Search = Search;
                model.List_Dishes = _bl.Dishes.Search(Search, Include_VolumesList: true);
            }

            return View(model);
        }

    }
}