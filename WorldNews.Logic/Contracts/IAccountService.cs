using System;
using WorldNews.Logic.DTO.Registration;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts
{
    public interface IAccountService : IDisposable
    {
        ServiceMessage InitializeRoles();

        ServiceMessage RegisterUser(UserRegisterDTO userDTO);

        ServiceMessage RegisterModerator(ModeratorRegisterDTO moderatorDTO);

        ServiceMessage LogIn(string login, string password);

        void LogOff();
    }
}
