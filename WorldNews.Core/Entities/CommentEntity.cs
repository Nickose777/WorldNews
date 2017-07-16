using System;

namespace WorldNews.Core.Entities
{
    public class CommentEntity
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateBanned { get; set; }

        public string AuthorId { get; set; }

        public virtual UserEntity Author { get; set; }

        public int ArticleId { get; set; }

        public virtual ArticleEntity Article { get; set; }

        public string ModeratorWhoBannedId { get; set; }

        public virtual ModeratorEntity ModeratorWhoBanned { get; set; }

        public int? BanReasonId { get; set; }

        public virtual BanReasonEntity BanReason { get; set; }
    }
}
