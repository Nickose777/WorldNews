using System;

namespace WorldNews.Logic.DTO.Comment
{
    public class CommentBanDetailsDTO
    {
        public string Id { get; set; }

        public string BanReason { get; set; }

        public string Text { get; set; }

        public string AuthorFullName { get; set; }

        public DateTime DateBanned { get; set; }
    }
}
