using System.ComponentModel.DataAnnotations;

namespace WorldNews.Models.Account
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Compare("OldPassword")]
        [DataType(DataType.Password)]
        public string Submit { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}