using System;
using System.ComponentModel.DataAnnotations;

namespace WorldNews.Models.Comment
{
    public class CommentListViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string AuthorDisplayFullName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateCreated { get; set; }
    }
}