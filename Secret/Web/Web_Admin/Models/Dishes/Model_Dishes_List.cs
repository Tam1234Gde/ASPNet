using Microsoft.AspNetCore.Mvc.Rendering;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;

namespace HIDE.RestAndRant.Web_Admin.Models.Dishes
{
    /// <summary>
    /// Список блюд
    /// </summary>
    public class Model_Dishes_List
    {
        /// <summary>
        /// Список категорий блюд
        /// </summary>
        public List<CCommon>? List_Categories { get; set; }

        /// <summary>
        /// Список категорий блюд - SelectListItem
        /// </summary>
        public List<SelectListItem>? List_Categories_ForSelect { get; set; }

        /// <summary>
        /// Список блюд
        /// </summary>
        public List<CDish_ForAdmin>? List_Dishes { get; set; }

        /// <summary>
        /// Код выбранной категории
        /// </summary>
        public int? Dish_CategoryID { get; set; }

        /// <summary>
        /// Кол-во объёмов
        /// </summary>
        public int? Count_Volumes { get; set; }

        /// <summary>
        /// Кол-во ингридиентов
        /// </summary>
        public int? Count_Ingridients { get; set; }

        /// <summary>
        /// Кол-во цветов
        /// </summary>
        public int? Count_Colors { get; set; }


        /// <summary>
        /// Дата окончания действия флага новинка
        /// </summary>
        public int? New_DateFrom { get; set; }
    }
}