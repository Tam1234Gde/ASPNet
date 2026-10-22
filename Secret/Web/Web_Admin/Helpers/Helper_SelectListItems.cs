using Microsoft.AspNetCore.Mvc.Rendering;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Helpers
{
    public static class Helper_SelectListItems
    {
        public static List<SelectListItem> GetList(List<CCommon> List_source)
        {
            List<SelectListItem> list_result = [];

            foreach (CCommon item in List_source)
            {
                SelectListItem elem = new() { Value = item.ID.ToString(), Text = item.Name };
                list_result.Add(elem);
            }

            return list_result;
        }
    }
}