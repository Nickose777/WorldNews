using System;
using System.Linq;
using System.Web;

namespace WorldNews.Helpers
{
    public static class RequestHelper
    {
        private static string jsonMime = "application/json";

        public static bool IsJsonRequest(this HttpRequestBase request)
        {
            bool isJsonRequest = false;

            if (request.AcceptTypes != null)
            {
                isJsonRequest = request.AcceptTypes.Any(t => t.Equals(jsonMime, StringComparison.OrdinalIgnoreCase));
            }

            return
                isJsonRequest || 
                request.ContentType.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Any(t => t.Equals(jsonMime, StringComparison.OrdinalIgnoreCase));
        }
    }
}