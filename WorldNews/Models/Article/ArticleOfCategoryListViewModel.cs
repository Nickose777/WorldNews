using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorldNews.Models.Article
{
    public class ArticleOfCategoryListViewModel
    {
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public IEnumerable<ArticleListViewModel> Articles { get; set; }
    }
}