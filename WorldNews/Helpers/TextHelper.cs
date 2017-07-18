using System;
using System.Web.Mvc;

namespace WorldNews.Helpers
{
    public static class TextHelper
    {
        public static MvcHtmlString TextToHtml(this HtmlHelper helper, string text)
        {
            return MvcHtmlString.Create(text.Replace(Environment.NewLine, "<br />"));
        }
    }
}