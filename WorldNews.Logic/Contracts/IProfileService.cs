using System;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts
{
    public interface IProfileService : IDisposable
    {
        DataServiceMessage<ProfileBaseDTO> GetAdminProfile(string login);

        ServiceMessage UpdateAdminProfile(ProfileBaseDTO profileDTO);
    }
}
