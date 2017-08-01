using System;
using System.Web.Mvc;

namespace WorldNews.Helpers
{
    public static class TextHelper
    {
        public static MvcHtmlString MultilineText(this HtmlHelper helper, string text)
        {
            return MvcHtmlString.Create(text.Replace(Environment.NewLine, "<br />"));
        }

        public static string MaxString(string text, int maxLength)
        {
            return text.Length > maxLength
                ? text.Substring(0, maxLength) + "..."
                : text;
        }
    }
}