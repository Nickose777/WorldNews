namespace WorldNews.Logic.DTO.Profile
{
    public class ModeratorListDTO : ProfileBaseDTO
    {
        public string PhotoLink { get; set; }

        public bool IsBanned { get; set; }
    }
}
