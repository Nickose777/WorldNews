using AutoMapper;
using WorldNews.Logic.DTO.BanReason;
using WorldNews.Models.BanReason;

namespace WorldNews.Mappings
{
    class BanReasonProfile : Profile
    {
        public BanReasonProfile()
        {
            this.CreateMap<BanReasonCreateViewModel, BanReasonCreateDTO>();
            this.CreateMap<BanReasonListDTO, BanReasonListViewModel>();
            this.CreateMap<BanReasonEditViewModel, BanReasonEditDTO>().ReverseMap();
        }
    }
}