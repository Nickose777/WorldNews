using System.Collections.Generic;

namespace WorldNews.Core.Entities
{
    public class ModeratorEntity
    {
        public string Id { get; set; }

        public string PhotoLink { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<ArticleEntity> Articles { get; set; }

        public virtual ICollection<CommentEntity> BannedComments { get; set; }
    }
}
