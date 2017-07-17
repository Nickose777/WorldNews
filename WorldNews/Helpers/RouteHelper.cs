using System.Text;

namespace WorldNews.Helpers
{
    public static class RouteHelper
    {
        public static string GetServerPath(string viewName, params string[] subDirectories)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("~/Views/");

            foreach (string directory in subDirectories)
            {
                builder.AppendFormat("{0}/", directory);
            }

            builder.AppendFormat("{0}.cshtml", viewName);

            return builder.ToString();
        }
    }
}