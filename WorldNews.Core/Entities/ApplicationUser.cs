using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace WorldNews.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual ModeratorEntity Moderator { get; set; }

        public virtual ICollection<CommentEntity> Comments { get; set; }
    }
}
