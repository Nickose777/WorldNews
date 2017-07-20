using System;
using System.Collections.Generic;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts.Services
{
    public interface IModeratorService : IDisposable
    {
        ServiceMessage Edit(ModeratorEditDTO moderatorDTO);

        DataServiceMessage<ModeratorEditDTO> Get(string login);

        DataServiceMessage<ModeratorDetailsDTO> GetDetails(string login);

        DataServiceMessage<IEnumerable<ModeratorListDTO>> GetAll();
    }
}
