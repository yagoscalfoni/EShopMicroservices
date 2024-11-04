using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping.Web.Pages.Filters
{
    public class AuthenticationPageFilter : IPageFilter
    {
        public void OnPageHandlerSelected(PageHandlerSelectedContext context) { }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var pageModel = context.HandlerInstance as PageModel;
            if (pageModel != null)
            {
                var isAuthenticated = context.HttpContext.Session.GetString("IsAuthenticated") == "true";
                pageModel.ViewData["IsLoggedIn"] = isAuthenticated;
                pageModel.ViewData["UserName"] = isAuthenticated
                    ? context.HttpContext.Session.GetString("UserName")
                    : null;
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
    }
}
