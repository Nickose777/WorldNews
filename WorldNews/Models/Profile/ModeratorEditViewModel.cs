using System.ComponentModel.DataAnnotations;
namespace WorldNews.Models.Profile
{
    public class ModeratorEditViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhotoLink { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Email { get; set; }
    }
}