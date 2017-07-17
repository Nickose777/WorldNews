namespace WorldNews.Logic.DTO.Registration
{
    public abstract class RegisterBaseDTO
    {
        public string Email { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
