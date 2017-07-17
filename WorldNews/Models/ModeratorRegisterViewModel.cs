using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WorldNews.Models
{
    public class ModeratorRegisterViewModel : RegisterViewModel
    {
        [Required]
        public HttpPostedFileBase Photo { get; set; }
    }
}