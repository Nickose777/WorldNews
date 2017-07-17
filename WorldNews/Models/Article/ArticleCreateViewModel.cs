using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorldNews.Models.Article
{
    public class ArticleCreateViewModel
    {
        [Required]
        public string Header { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string ShortDescription { get; set; }

        [Required]
        [DataType(DataType.Upload | DataType.ImageUrl)]
        public HttpPostedFileBase Photo { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}