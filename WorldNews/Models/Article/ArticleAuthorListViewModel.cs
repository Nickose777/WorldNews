using System;
using System.ComponentModel.DataAnnotations;

namespace WorldNews.Models.Article
{
    public class ArticleAuthorListViewModel
    {
        public string Id { get; set; }

        public string Header { get; set; }

        [Display(Name = "Author's name")]
        public string AuthorFullName { get; set; }

        [Display(Name = "Author's login")]
        public string AuthorLogin { get; set; }

        [Display(Name = "Created: ")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Last modified: ")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime DateLastModified { get; set; }

        [Display(Name = "Comments: ")]
        public int CommentsCount { get; set; }
    }
}