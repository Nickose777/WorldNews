using System;
using System.ComponentModel.DataAnnotations;

namespace WorldNews.Models.Article
{
    public class ArticleDetailsViewModel
    {
        public string Id { get; set; }

        public string CategoryName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateCreated { get; set; }

        public string Header { get; set; }

        public string PhotoLink { get; set; }

        public string Text { get; set; }
    }
}