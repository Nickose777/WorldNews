using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models.Comment;
using WorldNews.Models.Profile;

namespace WorldNews.Helpers
{
    public static class RenderHelper
    {
        public static void RenderSidebarMenu(this HtmlHelper helper, IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                string partialViewName;

                if (user.IsInRole(Roles.AdminRole))
                {
                    partialViewName = RouteHelper.GetServerPath("Admin", "Shared", "Sidebar");
                }
                else if (user.IsInRole(Roles.ModeratorRole))
                {
                    partialViewName = RouteHelper.GetServerPath("Moderator", "Shared", "Sidebar");
                }
                else
                {
                    partialViewName = RouteHelper.GetServerPath("User", "Shared", "Sidebar");
                }

                helper.RenderPartial(partialViewName);
            }
        }

        public static MvcHtmlString RenderModeratorLogin(this HtmlHelper helper, ModeratorListViewModel model)
        {
            TagBuilder h4 = new TagBuilder("h4");

            bool isBanned = model.IsBanned;
            string innerText = !isBanned
                ? model.Login
                : model.Login + " - [Banned]";
            if (isBanned)
            {
                h4.AddCssClass("text-danger");
            }

            h4.SetInnerText(innerText);
            h4.AddCssClass("card-title");

            string value = h4.ToString();
            return MvcHtmlString.Create(value);
        }

        public static MvcHtmlString RenderGreeting(this HtmlHelper helper, IPrincipal user)
        {
            string value;

            if (user.Identity.IsAuthenticated)
            {
                value = user.Identity.Name;

                if (user.IsInRole(Roles.ModeratorRole))
                {
                    value += " (moderator)";
                }

                value = String.Format("Hello, {0}!", value);
            }
            else
            {
                value = "Account";
            }

            return MvcHtmlString.Create(value);
        }
    }
}