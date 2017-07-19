using System.ComponentModel.DataAnnotations;

namespace WorldNews.Models.Category
{
    public class CategoryListViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Articles count")]
        public int NewsCount { get; set; }

        public bool IsDisabled { get; set; }
    }
}