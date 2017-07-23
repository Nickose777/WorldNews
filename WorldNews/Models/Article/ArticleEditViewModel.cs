using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WorldNews.Models.Article
{
    public class ArticleEditViewModel
    {
        [HiddenInput]
        public string Id { get; set; }

        [Required]
        public string PhotoLink { get; set; }

        [Required]
        public string Header { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string ShortDescription { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required(ErrorMessage = "Select the category")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}