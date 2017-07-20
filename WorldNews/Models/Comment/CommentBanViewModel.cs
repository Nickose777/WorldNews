using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WorldNews.Models.Comment
{
    public class CommentBanViewModel
    {
        [Required]
        [HiddenInput]
        public string Id { get; set; }

        [Required(ErrorMessage = "You must select the reason")]
        [Display(Name = "Select the reason")]
        public string BanReasonId { get; set; }

        public IEnumerable<SelectListItem> BanReasons { get; set; }
    }
}