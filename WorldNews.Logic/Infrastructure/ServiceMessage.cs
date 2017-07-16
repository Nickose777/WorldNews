using System.Collections.Generic;

namespace WorldNews.Logic.Infrastructure
{
    public class ServiceMessage
    {
        public bool Succeeded { get; set; }

        public List<string> Errors { get; set; }
    }
}
