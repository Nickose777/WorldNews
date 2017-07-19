using System;
using WorldNews.Logic.DTO.ReasonOfBan;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts.Services
{
    public interface IBanReasonService : IDisposable
    {
        ServiceMessage Create(BanReasonCreateDTO banReasonDTO);
    }
}
