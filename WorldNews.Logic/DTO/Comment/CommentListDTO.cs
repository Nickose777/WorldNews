﻿using System;

namespace WorldNews.Logic.DTO.Comment
{
    public class CommentListDTO
    {
        public string Id { get; set; }

        public string ArticleId { get; set; }

        public string Content { get; set; }

        public string AuthorDisplayFullName { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsBanned { get; set; }
    }
}
