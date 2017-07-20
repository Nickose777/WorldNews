using System;
using System.Collections.Generic;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts.Services
{
    public interface IModeratorService : IDisposable
    {
        DataServiceMessage<ModeratorDetailsDTO> Get(string login);

        DataServiceMessage<IEnumerable<ModeratorListDTO>> GetAll();
    }
}
