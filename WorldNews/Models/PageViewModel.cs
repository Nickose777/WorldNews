using System.Collections.Generic;

namespace WorldNews.Models
{
    public class PageViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public string Action { get; set; }

        public string ControllerName { get; set; }

        public IDictionary<string, object> RouteValues { get; set; }

        public PageViewModel()
        {
            RouteValues = new Dictionary<string, object>();
        }
    }
}