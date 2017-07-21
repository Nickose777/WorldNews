using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models.Profile;

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

        public static MvcHtmlString RenderComments(this HtmlHelper helper, IPrincipal user, object model)
        {
            bool withBanOption = user.Identity.IsAuthenticated && user.IsInRole(Roles.ModeratorRole);
            string partialViewName = withBanOption
                ? RouteHelper.GetServerPath("ListWithBanOption", "Comment")
                : RouteHelper.GetServerPath("ListWithoutBanOption", "Comment");

            return helper.Partial(partialViewName);
        }

        public static MvcHtmlString RenderModeratorAction(this HtmlHelper helper, ModeratorListViewModel model)
        {
            bool isBanned = model.IsBanned;
            string action = isBanned
                ? "Unban"
                : "Ban";

            return helper.ActionLink(action, action, "Account", new { login = model.Login }, new { @class = "dropdown-item" });
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

            return MvcHtmlString.Create(String.Format("Hello, {0}!", name));
        }
    }
}