using System;

namespace WorldNews.Logic.DTO.Article
{
    public class ArticleAuthorListDTO
    {
        public string Id { get; set; }

        public string Header { get; set; }

        public string AuthorFullName { get; set; }

        public string AuthorLogin { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastModified { get; set; }

        public int CommentsCount { get; set; }
    }
}
