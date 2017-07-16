using System.Collections.Generic;

namespace WorldNews.Core.Entities
{
    public class BanReasonEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<CommentEntity> Comments { get; set; }
    }
}
