using System;
using System.ComponentModel.DataAnnotations;

namespace WorldNews.Models.Comment
{
    public class CommentBanDetailsViewModel
    {
        public string Id { get; set; }

        public string BanReason { get; set; }

        public string Text { get; set; }

        public string AuthorFullName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime DateBanned { get; set; }
    }
}