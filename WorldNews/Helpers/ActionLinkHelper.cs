﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Mvc.Ajax;

namespace WorldNews.Helpers
{
    public static class ActionLinkHelper
    {
        public static MvcHtmlString ActionLinkWithRouteValues(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, IDictionary<string, object> routeValues, params string[] htmlClasses)
        {
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("class", String.Join(" ", htmlClasses));

            return ajaxHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new AjaxOptions { OnSuccess = "onSuccess" }, htmlAttributes);
        }
    }
}