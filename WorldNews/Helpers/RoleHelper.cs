using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models.Comment;
using WorldNews.Models.Profile;

namespace WorldNews.Helpers
{
    public static class RoleHelper
    {
        //TODO - naming convenstions (Render - void, Partial - MvcHtmlString)
        //TODO - RoleHelper for roles, RenderHelper for render etc.
        public static void RenderSidebar(this HtmlHelper helper, IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                object model;

                if (user.IsInRole(Roles.AdminRole))
                {
                    model = RouteHelper.GetServerPath("Admin", "Shared", "Sidebar");
                }
                else if (user.IsInRole(Roles.ModeratorRole))
                {
                    model = RouteHelper.GetServerPath("Moderator", "Shared", "Sidebar");
                }
                else
                {
                    model = RouteHelper.GetServerPath("User", "Shared", "Sidebar");
                }

                string partialViewName = RouteHelper.GetServerPath("Sidebar", "Shared", "Sidebar");
                helper.RenderPartial(partialViewName, model);
            }
        }

        public static MvcHtmlString RenderLogin(this HtmlHelper helper, IPrincipal user)
        {
            string partialViewName = user.Identity.IsAuthenticated
                ? RouteHelper.GetServerPath("Logout", "Shared")
                : RouteHelper.GetServerPath("Authorize", "Shared");

            return helper.Partial(partialViewName);
        }

        public static MvcHtmlString CommentsPartial(this HtmlHelper helper, IPrincipal user, object model)
        {
            bool withBanOption = user.Identity.IsAuthenticated && user.IsInRole(Roles.ModeratorRole);
            string partialViewName = withBanOption
                ? RouteHelper.GetServerPath("ListWithBanOption", "Comment")
                : RouteHelper.GetServerPath("ListWithoutBanOption", "Comment");

            return helper.Partial(partialViewName, model);
        }

        public static void RenderCreateCommentPartial(this HtmlHelper helper, IPrincipal user, string articleId)
        {
            if (user.Identity.IsAuthenticated)
            {
                string partialViewName = RouteHelper.GetServerPath("Create", "Comment");
                helper.RenderPartial(partialViewName, new CommentCreateViewModel { ArticleId = articleId });
            }
        }

        public static MvcHtmlString RenderModeratorAction(this HtmlHelper helper, ModeratorListViewModel model)
        {
            bool isBanned = model.IsBanned;
            string action = isBanned
                ? "Unban"
                : "Ban";

            return helper.ActionLink(action, action, "Account", new { login = model.Login }, new { @class = "dropdown-item" });
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

            return MvcHtmlString.Create(h4.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString RenderGreeting(this HtmlHelper helper, IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return MvcHtmlString.Create("Account");
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