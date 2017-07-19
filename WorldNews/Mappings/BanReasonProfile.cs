using AutoMapper;
using WorldNews.Logic.DTO.ReasonOfBan;
using WorldNews.Models.BanReason;

namespace WorldNews.Mappings
{
    class BanReasonProfile : Profile
    {
        public BanReasonProfile()
        {
            this.CreateMap<BanReasonCreateViewModel, BanReasonCreateDTO>();
        }
    }
}