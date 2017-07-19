using System;
using System.Collections.Generic;
using WorldNews.Logic.DTO.Comment;

namespace WorldNews.Logic.DTO.Article
{
    public class ArticleDetailsDTO
    {
        public string Id { get; set; }

        public string CategoryName { get; set; }

        public DateTime DateCreated { get; set; }

        public string Header { get; set; }

        public string PhotoLink { get; set; }

        public string Text { get; set; }

        public List<CommentListDTO> Comments { get; set; }
    }
}
