using System.Collections.Generic;

namespace WorldNews.Core.Entities
{
    public class UserEntity
    {
        public string Id { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<CommentEntity> Comments { get; set; }
    }
}
