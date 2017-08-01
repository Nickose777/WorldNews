using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace WorldNews.Helpers
{
    public static class AjaxRequestHelper
    {
        public static MvcHtmlString ActionLinkWithRouteValues(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, IDictionary<string, object> routeValues, params string[] htmlClasses)
        {
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("class", String.Join(" ", htmlClasses));

            return ajaxHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new AjaxOptions { OnSuccess = "onGetRequestSuccess" }, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkGet(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null)
        {
            AjaxOptions ajaxOptions = new AjaxOptions
            {
                HttpMethod = "GET",
                OnSuccess = "onGetRequestSuccess"
            };
            return ajaxHelper.ActionLink(linkText, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkPost(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, string urlOnSuccess, object routeValues = null, object htmlAttributes = null)
        {
            AjaxOptions ajaxOptions = new AjaxOptions
            {
                HttpMethod = "POST",
                OnSuccess = String.Format("onActionPostRequestSuccess(data, '{0}')", urlOnSuccess)
            };
            return ajaxHelper.ActionLink(linkText, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkCloseSidebar(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null)
        {
            AjaxOptions ajaxOptions = new AjaxOptions
            {
                HttpMethod = "GET",
                OnBegin = "closeSidebar",
                OnSuccess = "onGetRequestSuccess"
            };
            return ajaxHelper.ActionLink(linkText, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
        }

        public static MvcForm BeginForm(this AjaxHelper ajaxHelper, string action, string controller, string urlOnSuccess, object routeValues = null)
        {
            AjaxOptions ajaxOptions = new AjaxOptions
            {
                HttpMethod = "POST",
                OnSuccess = String.Format("onFormPostRequestSuccess(data, '{0}')", urlOnSuccess)
            };
            return ajaxHelper.BeginForm(action, controller, routeValues, ajaxOptions);
        }
    }
}