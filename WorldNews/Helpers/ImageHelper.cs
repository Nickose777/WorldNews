using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace WorldNews.Helpers
{
    public static class ImageHelper
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText)
        {
            return Image(helper, src, altText, null);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, object htmlAttributes)
        {
            TagBuilder builder = new TagBuilder("img");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("alt", altText);

            if (htmlAttributes != null)
            {
                RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                foreach (var attribute in attributes)
                {
                    builder.Attributes.Add(attribute.Key, attribute.Value.ToString());
                }
            }

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}