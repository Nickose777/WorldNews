using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models.Comment;

namespace WorldNews.Helpers
{
    public static class AuthorizeHelper
    {
        public static void RenderLoginForUnauthorized(this HtmlHelper helper, IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                string partialViewName = RouteHelper.GetServerPath("LogIn", "Shared");
                helper.RenderPartial(partialViewName);
            }
        }

        public static MvcHtmlString BanButtonForModerator(this HtmlHelper helper, IPrincipal user, CommentListViewModel model)
        {
            string value = "";
            bool canBan = user.Identity.IsAuthenticated && user.IsInRole(Roles.ModeratorRole);
            if (canBan)
            {
                value = model.IsBanned
                    ? "<button class=\"btn btn-danger disabled\">Banned</button>"
                    : "<button class=\"btn btn-danger\" onclick=\"displayBanModal('" + model.Id + "', '" + model.ArticleId + "')\">Ban</button>";
            }

            return MvcHtmlString.Create(value);
        }

        public static void RenderCreateCommentForAuthorized(this HtmlHelper helper, IPrincipal user, string articleId)
        {
            if (user.Identity.IsAuthenticated)
            {
                string partialViewName = RouteHelper.GetServerPath("Create", "Comment");
                helper.RenderPartial(partialViewName, new CommentCreateViewModel { ArticleId = articleId });
            }
        }

        public static MvcHtmlString PartialEditArticleForModerator(this AjaxHelper ajaxHelper, IPrincipal user, object routeValues, object htmlAttributes)
        {
            return user.Identity.IsAuthenticated && user.IsInRole(Roles.ModeratorRole)
                ? ajaxHelper.ActionLinkGet("Edit", "Edit", "Article", routeValues, htmlAttributes)
                : MvcHtmlString.Empty;
        }
    }
}