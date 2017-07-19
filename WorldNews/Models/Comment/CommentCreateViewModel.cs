using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WorldNews.Models.Comment
{
    public class CommentCreateViewModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string ArticleId { get; set; }
    }
}