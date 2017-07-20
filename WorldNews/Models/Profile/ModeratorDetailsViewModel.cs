using System.Collections.Generic;
using WorldNews.Models.Comment;

namespace WorldNews.Models.Profile
{
    public class ModeratorDetailsViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhotoLink { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public IEnumerable<CommentBanDetailsViewModel> BannedComments { get; set; }
    }
}