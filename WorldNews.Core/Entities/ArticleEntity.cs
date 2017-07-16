using System;
using System.Collections.Generic;

namespace WorldNews.Core.Entities
{
    public class ArticleEntity
    {
        public int Id { get; set; }

        public string Header { get; set; }

        public string ShortDescription { get; set; }

        public string PhotoLink { get; set; }

        public string Text { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastModified { get; set; }

        public int CategoryId { get; set; }

        public virtual CategoryEntity Category { get; set; }

        public string AuthorId { get; set; }

        public virtual ModeratorEntity Author { get; set; }

        public virtual ICollection<CommentEntity> Comments { get; set; }
    }
}
