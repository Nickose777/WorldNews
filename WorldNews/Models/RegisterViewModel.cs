using System.ComponentModel.DataAnnotations;

namespace WorldNews.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string EMail { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords are different")]
        public string ConfirmPassword { get; set; }
    }
}