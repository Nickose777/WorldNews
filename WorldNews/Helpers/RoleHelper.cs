using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Helpers
{
    public static class RoleHelper
    {
        public static MvcHtmlString RenderLogin(this HtmlHelper helper, IPrincipal user)
        {
            string partialViewName = user.Identity.IsAuthenticated
                ? RouteHelper.GetServerPath("Logout", "Shared")
                : RouteHelper.GetServerPath("Authorize", "Shared");

            return helper.Partial(partialViewName); ;
        }

        public static MvcHtmlString RenderGreeting(this HtmlHelper helper, IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return MvcHtmlString.Empty;
            }

            string name;

            if (user.IsInRole(Roles.AdminRole))
            {
                name = "admin";
            }
            else if (user.IsInRole(Roles.ModeratorRole))
            {
                name = "moderator";
            }
            else
            {
                name = user.Identity.Name;
            }

            TagBuilder builer = new TagBuilder("a");
            builer.Attributes.Add("href", "#");
            builer.SetInnerText(String.Format("Hello, {0}!", name));

            return MvcHtmlString.Create(builer.ToString(TagRenderMode.Normal));
        }
    }
}