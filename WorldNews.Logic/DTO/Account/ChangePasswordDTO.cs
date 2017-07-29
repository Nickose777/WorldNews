namespace WorldNews.Logic.DTO.Account
{
    public class ChangePasswordDTO
    {
        public string Login { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
