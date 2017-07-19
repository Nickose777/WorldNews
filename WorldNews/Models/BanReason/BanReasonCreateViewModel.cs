using System.ComponentModel.DataAnnotations;

namespace WorldNews.Models.BanReason
{
    public class BanReasonCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}