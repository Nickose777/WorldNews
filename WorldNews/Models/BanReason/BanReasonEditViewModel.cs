using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WorldNews.Models.BanReason
{
    public class BanReasonEditViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }
}