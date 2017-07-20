using System;
using System.Collections.Generic;
using WorldNews.Logic.DTO.BanReason;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts.Services
{
    public interface IBanReasonService : IDisposable
    {
        ServiceMessage Create(BanReasonCreateDTO banReasonDTO);

        ServiceMessage Edit(BanReasonEditDTO banReasonDTO);

        DataServiceMessage<BanReasonEditDTO> Get(string id);

        DataServiceMessage<IEnumerable<BanReasonListDTO>> GetEnabled();

        DataServiceMessage<IEnumerable<BanReasonListDTO>> GetAll();
    }
}
