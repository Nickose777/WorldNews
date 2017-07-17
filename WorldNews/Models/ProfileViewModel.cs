using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorldNews.Models
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "E-Mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}