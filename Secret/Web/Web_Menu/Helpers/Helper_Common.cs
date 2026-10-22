using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Web_Site_Menu.Models.Cart;
using HIDE.RestAndRant.Web_Site_Menu.Models.Restaurants;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Site_Menu.Helpers
{
    /// <summary>
    /// Helper
    /// </summary>
    public static class Helper_Common
    {
        #region Получение выбранного ресторана

        /// <summary>
        /// Возвращает выбранный ресторан
        /// </summary>
        /// <returns></returns>
        public static Model_Restaurant? GetRestaurant(string? Restaurant_Cookies)
        {
            CRestaurant? restaurant;

            if (string.IsNullOrWhiteSpace(Restaurant_Cookies))
            {
                return null;
            }

            restaurant = new Restaurants().Get(HIDE_Convert.ToInt(Restaurant_Cookies));

            if (restaurant == null)
            {
                return null;
            }

            Model_Restaurant item = new()
            {
                RestaurantID = restaurant.ID,
                Restaurant_Address = restaurant.Address
            };

            return item;
        }

        #endregion Получение выбранного ресторана

        #region Возвращает кол-во блюд в корзине

        /// <summary>
        /// Возвращает кол-во блюд в корзине
        /// </summary>
        /// <returns></returns>
        public static decimal GetDishesCount(string? Cookies_Cart)
        {
            decimal dish_count = 0;
            List<Model_CartItem_Add>? list_result = new List<Model_CartItem_Add>();
            if (!string.IsNullOrWhiteSpace(Cookies_Cart))
            {
                list_result = JsonConvert.DeserializeObject<List<Model_CartItem_Add>>(Cookies_Cart);

                if (list_result != null && list_result.Count != 0)
                {
                    dish_count = list_result.Select(p => p.Dish_Count).Sum();
                }
            }

            return dish_count;
        }

        #endregion Возвращает кол-во блюд в корзине

        #region Вывод скриптов в частичном представлении

        private const string ScriptsKey = "scripts"; // скрипты ниже bundle.min.js
        private const string ScriptsKeyTop = "scripts_top"; // скрипты выше bundle.min.js

        /// <summary>
        /// Функция для вызова в частичном представлении(Ниже bundle.min.js)
        /// </summary>
        /// <param name="helper"></param>
        /// Пример использования:
        /// @using (Html.BeginScripts())
        /// {
        ///   <script>
        ///     alert('hello');
        ///   </script>
        /// }
        /// <returns></returns>
        public static IDisposable BeginScripts(this IHtmlHelper helper)
        {
            return new ScriptBlock(helper.ViewContext);
        }

        /// <summary>
        /// Функция для вызова в частичном представлении(Выше bundle.min.js)
        /// </summary>
        /// <param name="helper"></param>
        /// Пример использования:
        /// @using (Html.BeginScripts_Top())
        /// {
        ///   <script>
        ///     alert('hello');
        ///   </script>
        /// }
        /// <returns></returns>
        public static IDisposable BeginScripts_Top(this IHtmlHelper helper)
        {
            return new ScriptBlock_Top(helper.ViewContext);
        }

        /// <summary>
        /// Функция для вызова в Layout(Ниже bundle.min.js)
        /// </summary>
        /// <param name="helper"></param>
        /// Пример использования:
        /// @Html.PageScripts()
        /// <returns></returns>
        public static HtmlString PageScripts(this IHtmlHelper helper)
        {
            return new HtmlString(string.Join(Environment.NewLine, GetPageScriptsList(helper.ViewContext.HttpContext)));
        }

        /// <summary>
        /// Функция для вызова в Layout(Выше bundle.min.js)
        /// </summary>
        /// <param name="helper"></param>
        /// Пример использования:
        /// @Html.PageScripts_Top()
        /// <returns></returns>
        public static HtmlString PageScripts_Top(this IHtmlHelper helper)
        {
            return new HtmlString(string.Join(Environment.NewLine, GetPageScriptsList_Top(helper.ViewContext.HttpContext)));
        }

        /// <summary>
        /// Получение данных для скриптов ниже bundle.min.js
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private static List<string> GetPageScriptsList(HttpContext httpContext)
        {
            if (httpContext.Items[key: ScriptsKey] is not List<string> pageScripts)
            {
                pageScripts = new List<string>();
                httpContext.Items[key: ScriptsKey] = pageScripts;
            }
            return pageScripts;
        }

        /// <summary>
        /// Получение данных для скриптов выше bundle.min.js
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private static List<string> GetPageScriptsList_Top(HttpContext httpContext)
        {
            if (httpContext.Items[ScriptsKeyTop] is not List<string> pageScripts)
            {
                pageScripts = new List<string>();
                httpContext.Items[ScriptsKeyTop] = pageScripts;
            }
            return pageScripts;
        }

        /// <summary>
        /// Для скриптов ниже bundle.min.js
        /// </summary>
        private class ScriptBlock : IDisposable
        {
            private readonly TextWriter _originalWriter;
            private readonly StringWriter _scriptsWriter;
            private readonly ViewContext _viewContext;

            public ScriptBlock(ViewContext viewContext)
            {
                _viewContext = viewContext;
                _originalWriter = _viewContext.Writer;
                _viewContext.Writer = _scriptsWriter = new StringWriter();
            }

            public void Dispose()
            {
                _viewContext.Writer = _originalWriter;
                List<string> pageScripts = GetPageScriptsList(_viewContext.HttpContext);
                pageScripts.Add(_scriptsWriter.ToString());
            }
        }

        /// <summary>
        /// Для скриптов выше bundle.min.js
        /// </summary>
        private class ScriptBlock_Top : IDisposable
        {
            private readonly TextWriter _originalWriter;
            private readonly StringWriter _scriptsWriter;
            private readonly ViewContext _viewContext;

            public ScriptBlock_Top(ViewContext viewContext)
            {
                _viewContext = viewContext;
                _originalWriter = _viewContext.Writer;
                _viewContext.Writer = _scriptsWriter = new StringWriter();
            }

            public void Dispose()
            {
                _viewContext.Writer = _originalWriter;
                List<string> pageScripts = GetPageScriptsList_Top(_viewContext.HttpContext);
                pageScripts.Add(_scriptsWriter.ToString());
            }
        }

        #endregion Вывод скриптов в частичном представлении
    }
}