using System;

namespace WorldNews.Logic.DTO.Article
{
    public class ArticleListDTO
    {
        public string Id { get; set; }

        public string PhotoLink { get; set; }

        public string Header { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
