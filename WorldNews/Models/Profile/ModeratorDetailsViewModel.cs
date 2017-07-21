using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorldNews.Models.Comment;

namespace WorldNews.Models.Profile
{
    public class ModeratorDetailsViewModel
    {
        [Display(Name = "First name:")]
        public string FirstName { get; set; }

        [Display(Name = "Last name:")]
        public string LastName { get; set; }

        public string PhotoLink { get; set; }

        [Display(Name = "Login:")]
        public string Login { get; set; }

        [Display(Name = "E-Mail:")]
        public string Email { get; set; }

        public IEnumerable<CommentBanDetailsViewModel> BannedComments { get; set; }
    }
}