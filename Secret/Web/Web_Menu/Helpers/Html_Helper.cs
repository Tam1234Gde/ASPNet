using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HIDE.RestAndRant.Web_Site_Menu.Helpers
{
    /// <summary>
    /// Html помощник
    /// </summary>
    public static class Html_Helper //TODO: MAE - не используется?
    {
        #region HtmlAction

        public static IHtmlContent Action(this IHtmlHelper helper, string action, object? parameters = null)
        {
            string? controller = helper.ViewContext.RouteData.Values["controller"]?.ToString(); ;

            return Action(helper, action, controller ?? "", parameters);
        }

        public static IHtmlContent Action(this IHtmlHelper helper, string action, string controller, object? parameters = null)
        {
            string? area = helper.ViewContext.RouteData.Values["area"]?.ToString();

            return Action(helper, action, controller, area ?? "", parameters);
        }

        public static IHtmlContent Action(this IHtmlHelper helper, string action, string controller, string area, object? parameters = null)
        {
            ArgumentNullException.ThrowIfNull(action);

            ArgumentNullException.ThrowIfNull(controller);

            var task = RenderActionAsync(helper, action, controller, area, parameters);

            return task.Result;
        }

        private static async Task<IHtmlContent> RenderActionAsync(this IHtmlHelper helper, string action, string controller, string area, object? parameters = null)
        {
            // fetching required services for invocation
            var serviceProvider = helper.ViewContext.HttpContext.RequestServices;
            var actionContextAccessor = helper.ViewContext.HttpContext.RequestServices.GetRequiredService<IActionContextAccessor>();
            var httpContextAccessor = helper.ViewContext.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
            var actionSelector = serviceProvider.GetRequiredService<IActionSelector>();

            // creating new action invocation context
            var routeData = new RouteData();
            foreach (var router in helper.ViewContext.RouteData.Routers)
            {
                routeData.PushState(router, null, null);
            }
            routeData.PushState(null, new RouteValueDictionary(new { controller, action, area }), null);
            routeData.PushState(null, new RouteValueDictionary(parameters ?? new { }), null);

            //get the actiondescriptor
            RouteContext routeContext = new(helper.ViewContext.HttpContext) { RouteData = routeData };
            IReadOnlyList<ActionDescriptor>? candidates = actionSelector.SelectCandidates(routeContext);
            ActionDescriptor? actionDescriptor = new();
            if (candidates != null)
            {
                actionDescriptor = actionSelector.SelectBestCandidate(routeContext, candidates);
            }

            ActionContext? originalActionContext = actionContextAccessor.ActionContext;
            var originalhttpContext = httpContextAccessor.HttpContext;
            try
            {
                var newHttpContext = serviceProvider.GetRequiredService<IHttpContextFactory>().Create(helper.ViewContext.HttpContext.Features);
                if (newHttpContext.Items.ContainsKey(typeof(IUrlHelper)))
                {
                    newHttpContext.Items.Remove(typeof(IUrlHelper));
                }
                newHttpContext.Response.Body = new MemoryStream();
                ActionContext? actionContext = new();
                if (actionDescriptor != null)
                {
                    actionContext = new ActionContext(newHttpContext, routeData, actionDescriptor);
                }
                actionContextAccessor.ActionContext = actionContext;
                var invoker = serviceProvider.GetRequiredService<IActionInvokerFactory>().CreateInvoker(actionContext);
                if (invoker != null)
                {
                    await invoker.InvokeAsync();
                }
                newHttpContext.Response.Body.Position = 0;
                using var reader = new StreamReader(newHttpContext.Response.Body);
                return new HtmlString(reader.ReadToEnd());
            }
            catch (Exception ex)
            {
                return new HtmlString(ex.Message);
            }
            finally
            {
                if (originalActionContext != null)
                {
                    actionContextAccessor.ActionContext = originalActionContext;
                }
                httpContextAccessor.HttpContext = originalhttpContext;
                if (helper.ViewContext.HttpContext.Items.ContainsKey(typeof(IUrlHelper)))
                {
                    helper.ViewContext.HttpContext.Items.Remove(typeof(IUrlHelper));
                }
            }
        }

        #endregion
    }
}