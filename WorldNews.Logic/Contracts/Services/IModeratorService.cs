using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts.Services
{
    public interface IModeratorService : IDisposable
    {
        DataServiceMessage<IEnumerable<ModeratorListDTO>> GetAll();
    }
}
