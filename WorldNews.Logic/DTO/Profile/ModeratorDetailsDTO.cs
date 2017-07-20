using System.Collections.Generic;
using WorldNews.Logic.DTO.Comment;

namespace WorldNews.Logic.DTO.Profile
{
    public class ModeratorDetailsDTO : ProfileBaseDTO
    {
        public string Id { get; set; }

        public string PhotoLink { get; set; }

        public List<CommentBanDetailsDTO> BannedComments { get; set; }
    }
}
